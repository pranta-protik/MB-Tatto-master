using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessHandler : MonoBehaviour
{
    private PostProcessProfile _postProcessProfile;

    private void Start()
    {
        _postProcessProfile = GetComponent<PostProcessVolume>().profile;
    }

    public void AddGrayscaleFilter()
    {
        RemoveAllFilters();
        
        _postProcessProfile.AddSettings<ColorGrading>();
        ColorGrading colorGrading = _postProcessProfile.GetSetting<ColorGrading>();
        
        // Grading Mode
        colorGrading.gradingMode.overrideState = true;
        colorGrading.gradingMode.value = GradingMode.LowDefinitionRange;
        
        // Saturation
        colorGrading.saturation.overrideState = true;
        colorGrading.saturation.value = -100;
    }

    public void RemoveAllFilters()
    {
        if (_postProcessProfile.HasSettings<ColorGrading>())
        {
            _postProcessProfile.RemoveSettings<ColorGrading>();   
        }
    }
}
