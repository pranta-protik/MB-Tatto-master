using UnityEngine;

public class CanvasLookAt : MonoBehaviour
{
    private Camera _camera;
    public float offset;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(new Vector3(0f, 0f, _camera.transform.position.z - offset));
    }
}