#if HOMA_BELLY_EDITOR_ANALYTICS_ENABLED
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HomaGames.HomaBelly;
using Unity.Collections;
using UnityEditor;
using UnityEditor.Profiling;
using UnityEditorInternal;
using UnityEngine;
#if UNITY_2020_3_OR_NEWER
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Profiling;
#endif

public class PerformanceReporter : ReporterBase, IReportingService
{
    public event Action<EventApiQueryModel> OnDataReported;

    protected override long MinTimeInSecondsBetweenReports => 24 * 60 * 60;


    private bool _profilerWasEnabled = false;
    private float _playSessionStartTime;

    private bool IsPlaySessionLongEnough => Time.realtimeSinceStartup - _playSessionStartTime > 5;

#if UNITY_2020_3_OR_NEWER

    // Max number of frames analysed on a session
    private const int MAX_NUMBER_OF_FRAMES = 500;

    // Number of timings to send (only top contributing ones)
    private const int ANALYSED_MARKER_NUMBER = 50;

    // Frame data cache
    private static List<RawFrameDataView> _rawFrameDataViews = new List<RawFrameDataView>(MAX_NUMBER_OF_FRAMES);

    /// <summary>
    /// Hand picked memory markers to analyse
    /// </summary>
    private static HashSet<string> _memoryMarkers = new HashSet<string>()
    {
        "Total Used Memory",
        "Texture Memory",
        "Mesh Memory",
        "Material Count",
        "Object Count",
        "GC Used Memory",
        "Asset Count",
        "Game Object Count",
        "Scene Object Count"
    };

    /// <summary>
    /// Hand picked rendering markers to analyse
    /// </summary>
    private static HashSet<string> _renderingMarkers = new HashSet<string>()
    {
        "Batches Count",
        "SetPass Calls Count",
        "Triangles Count",
        "Vertices Count"
    };

    /// <summary>
    /// Preloading frame data before processing, this is somewhat expensive
    /// </summary>
    private static void PreloadFrameData()
    {
        _rawFrameDataViews.Clear();
        int frameCount = ProfilerDriver.lastFrameIndex - ProfilerDriver.firstFrameIndex;
        int step = 1;
        if (frameCount > MAX_NUMBER_OF_FRAMES)
        {
            step = frameCount / MAX_NUMBER_OF_FRAMES;
        }

        for (int i = ProfilerDriver.firstFrameIndex; i < ProfilerDriver.lastFrameIndex; i += step)
        {
            _rawFrameDataViews.Add(ProfilerDriver.GetRawFrameDataView(i, 0));
        }
    }

    public PerformanceReporter()
    {
        EditorApplication.playModeStateChanged -= PlayModeChanged;
        EditorApplication.playModeStateChanged += PlayModeChanged;
    }

    private void PlayModeChanged(PlayModeStateChange state)
    {
        if (CanReport && state == PlayModeStateChange.EnteredPlayMode)
        {
            StartProfiling();
        }

        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            EndProfiling();
        }
    }

    private void StartProfiling()
    {
        _profilerWasEnabled = Profiler.enabled;
        Profiler.enabled = true;
        _playSessionStartTime = Time.realtimeSinceStartup;
    }

    private void EndProfiling()
    {
        Profiler.enabled = _profilerWasEnabled;
        if (!IsPlaySessionLongEnough || !CanReport)
            return;

        PreloadFrameData();
        Dictionary<string, float> performanceData = new Dictionary<string, float>();
        Dictionary<string, float> topContributors = new Dictionary<string, float>();

        var availableStatHandles = new List<ProfilerRecorderHandle>();
        ProfilerRecorderHandle.GetAvailable(availableStatHandles);

        foreach (var marker in _memoryMarkers)
        {
            performanceData.Add(marker, GetMetadataMean(marker));
        }

        foreach (var marker in _renderingMarkers)
        {
            performanceData.Add(marker, GetCounterMean(marker));
        }

        foreach (var wildcard in ComputeMostExpensiveOperations())
        {
            topContributors.Add(wildcard.Value, wildcard.Key);
        }

        LastTimeReported = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        OnDataReported?.Invoke(new PerformanceReportQueryModel("performance_report_session",
            Time.realtimeSinceStartup - _playSessionStartTime, MAX_NUMBER_OF_FRAMES,
            performanceData, topContributors));
    }

    static float GetMetadataMean(string marker, int metadataIndex = 0)
    {
        float sum = 0;
        foreach (var frame in _rawFrameDataViews)
        {
            sum += GetMetadata(frame, marker, metadataIndex);
        }

        return sum / _rawFrameDataViews.Count;
    }

    static float GetMetadata(RawFrameDataView rawFrameDataView, string marker, int metadataIndex)
    {
        float totalTime = 0;
        int markerId = rawFrameDataView.GetMarkerId(marker);
        if (markerId == FrameDataView.invalidMarkerId)
            return 0;

        int sampleCount = rawFrameDataView.sampleCount;
        for (int i = 0; i < sampleCount; ++i)
        {
            if (markerId != rawFrameDataView.GetSampleMarkerId(i))
                continue;

            totalTime += rawFrameDataView.GetSampleMetadataAsFloat(i, metadataIndex);
        }

        return totalTime;
    }

    static float GetCounterMean(string marker)
    {
        float sum = 0;
        foreach (var frame in _rawFrameDataViews)
        {
            sum += GetCounterData(frame, marker);
        }

        return sum / _rawFrameDataViews.Count;
    }

    static int GetCounterData(RawFrameDataView rawFrameDataView, string marker)
    {
        int markerId = rawFrameDataView.GetMarkerId(marker);
        if (markerId == FrameDataView.invalidMarkerId)
            return 0;
        return rawFrameDataView.HasCounterValue(markerId) ? rawFrameDataView.GetCounterValueAsInt(markerId) : 0;
    }

    static SortedList<float, string> ComputeMostExpensiveOperations()
    {
        Dictionary<string, float> accumulatedTimes = new Dictionary<string, float>();

        foreach (var frame in _rawFrameDataViews)
        {
            int sampleCount = frame.sampleCount;
            for (int y = 0; y < sampleCount; ++y)
            {
                int markerID = frame.GetSampleMarkerId(y);
                if (markerID != FrameDataView.invalidMarkerId)
                {
                    float sampleTime = frame.GetSampleTimeMs(y);
                    if (sampleTime > 0)
                    {
                        string sampleName = frame.GetSampleName(y);
                        if (accumulatedTimes.ContainsKey(sampleName))
                            accumulatedTimes[sampleName] += sampleTime;
                        else
                            accumulatedTimes.Add(sampleName, sampleTime);
                    }
                }
            }
        }

        SortedList<float, string> sorted = new SortedList<float, string>();
        foreach (var markerToTime in accumulatedTimes)
        {
            sorted.Add(markerToTime.Value / _rawFrameDataViews.Count, markerToTime.Key);
            if (sorted.Count > ANALYSED_MARKER_NUMBER)
                sorted.RemoveAt(0);
        }

        return sorted;
    }
#endif
}
#endif