using UnityEngine;

public class PoseManager : MonoBehaviour
{
    public int currentPoseButtonId;

    public void HandPose1(GameObject poseButtonObj)
    {
        SetCurrentPoseButtonId(poseButtonObj);
        GameManager.Instance.ResetPose();
    }

    public void HandPose2(GameObject poseButtonObj)
    {
        SetCurrentPoseButtonId(poseButtonObj);
        GameManager.Instance.PlayPoseAnimation(0);
    }
    
    public void HandPose3(GameObject poseButtonObj)
    {
        SetCurrentPoseButtonId(poseButtonObj);
        GameManager.Instance.PlayPoseAnimation(1);
    }
    
    public void HandPose4(GameObject poseButtonObj)
    {
        SetCurrentPoseButtonId(poseButtonObj);
        GameManager.Instance.PlayPoseAnimation(2);
    }
    
    public void HandPose5(GameObject poseButtonObj)
    {
        SetCurrentPoseButtonId(poseButtonObj);
        GameManager.Instance.PlayPoseAnimation(3);
    }

    private void SetCurrentPoseButtonId(GameObject currentPoseButtonObj)
    {
        PoseButton poseButton = currentPoseButtonObj.GetComponent<PoseButton>();
        currentPoseButtonId = poseButton.buttonId;
    }
}
