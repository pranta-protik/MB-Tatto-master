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
        ColorGrading colorGrading = _postProcessProfile.GetSetting<ColorGrading>();
        colorGrading.saturation.overrideState = true;
        colorGrading.saturation.value = -100;
    }
}
