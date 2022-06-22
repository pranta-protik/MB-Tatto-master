using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessHandler : MonoBehaviour
{
    [SerializeField] private PostProcessProfile _postProcessProfile;

    private void Start()
    {
        _postProcessProfile = GetComponent<PostProcessVolume>().profile;
    }

    public void GrayscaleEffect()
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
}
