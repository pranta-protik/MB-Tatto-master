using UnityEngine;
using HomaGames.HomaBelly;
using HomaGames.HomaConsole.Core.Attributes;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    [DebuggableField("Camera", CustomName = "offsetX", LayoutOption = LayoutOption.Slider, Min = -0.95f, Max = 1.5f)]
    public float offsetX = -0.95f;

    void LateUpdate()
    {
        Transform cameraTransform = transform;
        Vector3 cameraPosition = cameraTransform.position;
            
        Vector3 playerPosition = player.transform.position;
            
        cameraPosition.z = playerPosition.z ; 
        cameraPosition.x = playerPosition.x + offsetX;
        cameraTransform.position = cameraPosition;
    }
}