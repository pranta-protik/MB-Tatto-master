using DG.Tweening;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private float moveAmount;
    [SerializeField] private float moveDuration;
    private float _initialYPosition;
    
    private void Start()
    {
        _initialYPosition = transform.position.y;

        transform.DOMoveY(_initialYPosition + moveAmount, moveDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public void DestroyPointer()
    {
        transform.DOKill();
        gameObject.SetActive(false);
    }
}
