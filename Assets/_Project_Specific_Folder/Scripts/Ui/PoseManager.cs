using UnityEngine;

public class PoseManager : MonoBehaviour
{
    [SerializeField] private int _currentPoseButtonId;
    private PoseButton _poseButton;
    
    private void WatchAd()
    {
        Debug.Log("Ad Watched");
        PlayerPrefs.SetInt("PoseAdWatched" + _currentPoseButtonId, 1);
        _poseButton.buttonImage.sprite = _poseButton.normalIcon; 
        GameManager.Instance.PlayPoseAnimation(_poseButton.animationId);
    }

    public void ResetPose()
    {
        GameManager.Instance.ResetPose();
    }

    public void SetPose(GameObject poseButtonObj)
    {
        SetCurrentPoseButtonId(poseButtonObj);
        
        if (_poseButton.watchAdRequired)
        {
            WatchAdPoseButton(_poseButton.animationId);
        }
        else
        {
            NormalPoseButton(_poseButton.animationId);
        }
    }

    private void SetCurrentPoseButtonId(GameObject currentPoseButtonObj)
    {
        _poseButton = currentPoseButtonObj.GetComponent<PoseButton>();
        _currentPoseButtonId = _poseButton.buttonId;
    }

    private void NormalPoseButton(int animationIndex)
    {
        GameManager.Instance.PlayPoseAnimation(animationIndex);
    }
    
    private void WatchAdPoseButton(int animationIndex)
    {
        if (PlayerPrefs.GetInt("PoseAdWatched" + _currentPoseButtonId, 0) == 0)
        {
            WatchAd();
        }
        else
        {
            GameManager.Instance.PlayPoseAnimation(animationIndex);
        }
    }
}
