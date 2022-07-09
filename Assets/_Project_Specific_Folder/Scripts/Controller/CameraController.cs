using UnityEngine;
using HomaGames.HomaBelly;
using HomaGames.HomaConsole.Core.Attributes;

public class CameraController : MonoBehaviour
{
    public GameObject player;

   
    [DebuggableField("Camera", CustomName = "offsetX", LayoutOption = LayoutOption.Slider, Min = -0.95f, Max = 1.5f)]
    public float offsetX = -0.95f;
    [DebuggableField("Camera", CustomName = "offsetY", LayoutOption = LayoutOption.Slider, Min = -.56f, Max = 1.5f)]
    public float offsetY = .56f;
    [DebuggableField("Camera", CustomName = "offsetZ", LayoutOption = LayoutOption.Slider, Min = -2f, Max = 2f)]
    public float offsetZ = 0f;
    [DebuggableField("Camera", Order = 10)]
    public Vector3 targetRotation;

    public float smoothTimeLook;

    public float InitialY, InitialZ;
    private void Start()
    {
        // InitialY = transform.position.y;
        // InitialZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (!UAManager.Instance.isEndReached && UAManager.Instance.EnableUA)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), smoothTimeLook * Time.deltaTime);    
        }
        
        Transform cameraTransform = transform;
        Vector3 cameraPosition = cameraTransform.position;         
        Vector3 playerPosition = player.transform.position;
        
        cameraPosition.y = playerPosition.y + offsetY;
        cameraPosition.z = playerPosition.z + offsetZ;
        cameraPosition.x = playerPosition.x + offsetX;
        
        cameraTransform.position = cameraPosition;
    }

    // public void ResetCamPosYZ()
    // {
    //     // transform.position = new Vector3(transform.position.x, InitialY, InitialZ);
    // }
}