using DG.Tweening;
using UnityEngine;

public class ScoreMultiplier : MonoBehaviour
{
    [SerializeField] private GameObject multiplierPointer;

    private void Start()
    {
        multiplierPointer.transform.DORotate(new Vector3(0f, 0f, 140f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    public void OnWatchAdButtonClick()
    {
        Debug.Log("Ad Watched");

        multiplierPointer.transform.DOKill();
        Debug.Log(multiplierPointer.transform.localEulerAngles.z);
    }

    public void OnSkipButtonClick()
    {
        
    }
}
