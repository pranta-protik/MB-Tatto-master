using UnityEngine;

public class TattooArtist : MonoBehaviour
{
    private Animator _animator;
    private static readonly int IsDrawing = Animator.StringToHash("IsDrawing");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartDrawingTattoo()
    {
        transform.eulerAngles = new Vector3(0f, 90f, 0f);
        _animator.SetBool(IsDrawing, true);
    }

    public void StopDrawingTattoo()
    {
        transform.eulerAngles = new Vector3(0f, 180f, 0f);
        _animator.SetBool(IsDrawing, false);
    }
}