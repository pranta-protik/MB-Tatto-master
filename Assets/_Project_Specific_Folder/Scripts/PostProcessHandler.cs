using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessHandler : MonoBehaviour
{
    [SerializeField] private PostProcessProfile _postProcessProfile;
    
    private bool _isGrayscale;
    
    private void Start()
    {
        _postProcessProfile = GetComponent<PostProcessVolume>().profile;
    }

    public void AddGrayscaleEffect()
    {
        if (_isGrayscale)
        {
            _isGrayscale = false;
            RemoveGrayscaleEffect();
        }
        else
        {
            _isGrayscale = true;
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

    private void RemoveGrayscaleEffect()
    {
        _postProcessProfile.RemoveSettings<ColorGrading>();
    }
}
