using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessHandler : MonoBehaviour
{
    private PostProcessProfile _postProcessProfile;

    private void Start()
    {
        _postProcessProfile = GetComponent<PostProcessVolume>().profile;
    }

    public void AddGrayscaleEffect()
    {
        RemoveAllEffects();
        
        _postProcessProfile.AddSettings<ColorGrading>();
        ColorGrading colorGrading = _postProcessProfile.GetSetting<ColorGrading>();
        
        // Grading Mode
        colorGrading.gradingMode.overrideState = true;
        colorGrading.gradingMode.value = GradingMode.LowDefinitionRange;
        
        // Saturation
        colorGrading.saturation.overrideState = true;
        colorGrading.saturation.value = -100;
    }

    public void AddDistortedEffect()
    {
        RemoveAllEffects();
        
        // Chromatic Aberration
        _postProcessProfile.AddSettings<ChromaticAberration>();
        ChromaticAberration chromaticAberration = _postProcessProfile.GetSetting<ChromaticAberration>();

        // Intensity
        chromaticAberration.intensity.overrideState = true;
        chromaticAberration.intensity.value = 1;
        
        // Fast Mode
        chromaticAberration.fastMode.overrideState = true;
        chromaticAberration.fastMode.value = true;
        
        // Depth of Field
        _postProcessProfile.AddSettings<DepthOfField>();
        DepthOfField depthOfField = _postProcessProfile.GetSetting<DepthOfField>();
        
        // Focus Distance
        depthOfField.focusDistance.overrideState = true;
        depthOfField.focusDistance.value = 0.88f;
        
        // Aperture
        depthOfField.aperture.overrideState = true;
        depthOfField.aperture.value = 5.6f;
    }

    public void AddVintageEffect()
    {
        RemoveAllEffects();
        
        // Grain
        _postProcessProfile.AddSettings<Grain>();
        Grain grain = _postProcessProfile.GetSetting<Grain>();
        
        // Colored
        grain.colored.overrideState = true;
        grain.colored.value = true;
        
        // Intensity
        grain.intensity.overrideState = true;
        grain.intensity.value = 0.563f;
        
        // Size
        grain.size.overrideState = true;
        grain.size.value = 3;
        
        // Vignette
        _postProcessProfile.AddSettings<Vignette>();
        Vignette vignette = _postProcessProfile.GetSetting<Vignette>();
        
        // Mode
        vignette.mode.overrideState = true;
        vignette.mode.value = VignetteMode.Classic;
        
        // Center
        vignette.center.overrideState = true;
        vignette.center.value = new Vector2(0.5f, 0.5f);
        
        // Intensity
        vignette.intensity.overrideState = true;
        vignette.intensity.value = 0.564f;
        
        // Smoothness
        vignette.smoothness.overrideState = true;
        vignette.smoothness.value = 0.252f;
        
        // Roundness
        vignette.roundness.overrideState = true;
        vignette.roundness.value = 0.954f;
    }
    
    public void AddBloomEffect()
    {
        RemoveAllEffects();
        
        // Bloom
        _postProcessProfile.AddSettings<Bloom>();
        Bloom bloom = _postProcessProfile.GetSetting<Bloom>();
        
        // Intensity
        bloom.intensity.overrideState = true;
        bloom.intensity.value = 1.2f;
        
        // Threshold
        bloom.threshold.overrideState = true;
        bloom.threshold.value = 0.72f;
        
        // Soft Knee
        bloom.softKnee.overrideState = true;
        bloom.softKnee.value = 0.254f;
        
        // Color
        bloom.color.overrideState = true;
        bloom.color.value = Color.white;
    }

    public void RemoveAllEffects()
    {
        if (_postProcessProfile.HasSettings<ColorGrading>())
        {
            _postProcessProfile.RemoveSettings<ColorGrading>();   
        }

        if (_postProcessProfile.HasSettings<ChromaticAberration>())
        {
            _postProcessProfile.RemoveSettings<ChromaticAberration>();
        }

        if (_postProcessProfile.HasSettings<DepthOfField>())
        {
            _postProcessProfile.RemoveSettings<DepthOfField>();
        }

        if (_postProcessProfile.HasSettings<Grain>())
        {
            _postProcessProfile.RemoveSettings<Grain>();
        }

        if (_postProcessProfile.HasSettings<Vignette>())
        {
            _postProcessProfile.RemoveSettings<Vignette>();
        }

        if (_postProcessProfile.HasSettings<Bloom>())
        {
            _postProcessProfile.RemoveSettings<Bloom>();
        }
    }
}
