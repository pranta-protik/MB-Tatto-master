using Singleton;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessHandler : Singleton<PostProcessHandler>
{
    [SerializeField] private PostProcessProfile _postProcessProfile;

    public override void Start()
    {
        _postProcessProfile = GetComponent<PostProcessVolume>().profile;
    }

    public void AddGrayscaleEffect()
    {
        _postProcessProfile.AddSettings<ColorGrading>();
        ColorGrading colorGrading = _postProcessProfile.GetSetting<ColorGrading>();
        
        // Grading Mode
        colorGrading.gradingMode.overrideState = true;
        colorGrading.gradingMode.value = GradingMode.LowDefinitionRange;
        
        // Saturation
        colorGrading.saturation.overrideState = true;
        colorGrading.saturation.value = -100;
    }

    public void RemoveGrayscaleEffect()
    {
        _postProcessProfile.RemoveSettings<ColorGrading>();
    }
}
