using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float offset = -0.95f;

    void LateUpdate()
    {
        Transform cameraTransform = transform;
        Vector3 cameraPosition = cameraTransform.position;
            
        Vector3 playerPosition = player.transform.position;
            
        cameraPosition.z = playerPosition.z; 
        cameraPosition.x = playerPosition.x + offset;
        cameraTransform.position = cameraPosition;
    }
}