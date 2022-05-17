#if HOMA_BELLY_EDITOR_ANALYTICS_ENABLED
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HomaGames.HomaBelly;
using UnityEditor;
using UnityEngine;
#if UNITY_2020_3_OR_NEWER
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Profiling;
#endif

/// <summary>
/// Reports usage of different dll / namespaces / functions in the project.
/// Uses the profiler API to make sure we are reporting functions and namespace signatures that are actually used at runtime.
/// </summary>
public class UsageReporter : ReporterBase, IReportingService
{
    public event Action<EventApiQueryModel> OnDataReported;
    
    protected override long MinTimeInSecondsBetweenReports => 24 * 60 * 60;


    private bool _profilerWasEnabled = false;
    private float _playSessionStartTime;

    private bool IsPlaySessionLongEnough => Time.realtimeSinceStartup - _playSessionStartTime > 5;

#if UNITY_2020_3_OR_NEWER

    public UsageReporter()
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

    /// <summary>
    /// Builds a list of dll + namespace combo looking like this :
    /// "HomaGames.HomaConsole.dll!HomaGames.HomaConsole.Performance"
    /// "HomaGames.HomaConsole.dll!HomaGames.HomaConsole.Performance.Audio"
    /// "UnityEngine.SubsystemsModule.dll!UnityEngine"
    /// ...
    /// </summary>
    private void EndProfiling()
    {
        Profiler.enabled = _profilerWasEnabled;
        
        if (!IsPlaySessionLongEnough || !CanReport)
            return;

        var availableStatHandles = new List<ProfilerRecorderHandle>();
        ProfilerRecorderHandle.GetAvailable(availableStatHandles);

        HashSet<string> usedFunctionsData = new HashSet<string>();

        foreach (var h in availableStatHandles)
        {
            var statDesc = ProfilerRecorderHandle.GetDescription(h);
            int subIndex = statDesc.Name.IndexOf(':');
            if (subIndex != -1)
            {
                string dllWithNamespace = statDesc.Name.Substring(0,subIndex);
                if ( dllWithNamespace.Contains("dll") && statDesc.Category == ProfilerCategory.Scripts)
                {
                    usedFunctionsData.Add(dllWithNamespace);
                }
            }
        }

        LastTimeReported = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        OnDataReported?.Invoke(new UsageReportQueryModel("editor_play_session_ended", usedFunctionsData.ToList()));
    }
#endif
}
#endif