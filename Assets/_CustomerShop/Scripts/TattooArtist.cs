using UnityEngine;

public class TattooArtist : MonoBehaviour
{
    private Animator _animator;
    private Vector3 _initialPosition;
    private Vector3 _initialRotation;
    private static readonly int IsDrawing = Animator.StringToHash("IsDrawing");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _initialPosition = transform.position;
        _initialRotation = transform.eulerAngles;
    }

    public void StartDrawingTattoo()
    {
        transform.eulerAngles = new Vector3(0f, 90f, 0f);
        _animator.SetBool(IsDrawing, true);
    }

    public void StopDrawingTattoo()
    {
        transform.position = _initialPosition;
        transform.eulerAngles = _initialRotation;
        _animator.SetBool(IsDrawing, false);
    }
}