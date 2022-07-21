using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    public void Move(Transform[] exitPositions)
    {
        transform.LookAt(exitPositions[0]);
        transform.GetChild(0).LookAt(exitPositions[0]);
        
        _animator.SetBool(IsWalking, true);
        transform.DOMove(exitPositions[0].position, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.LookAt(exitPositions[1]);
            transform.GetChild(0).LookAt(exitPositions[1]);
            transform.DOMove(exitPositions[1].position, 3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}