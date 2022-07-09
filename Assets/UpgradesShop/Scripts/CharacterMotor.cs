using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform characterModelTransform;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float animatorMovementSpeed = 2f;

    private Vector3 dirVector;
    private Vector3 cameraModelDif;
    private bool isMoving = false;

    private const string IS_MOVING = "IsMoving";
    
    private void Awake()
    {
        QualitySettings.shadowDistance = 50f;
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#endif

        cameraModelDif = cameraPivot.position - characterModelTransform.position;
    }

    private void Update()
    {
        dirVector = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        
        if(dirVector != Vector3.zero)
        {
            if(!isMoving)
            {
                characterAnimator.SetBool(IS_MOVING, true);
                isMoving = true;
            }

            characterAnimator.speed = dirVector.magnitude * animatorMovementSpeed;
            characterModelTransform.LookAt(characterModelTransform.position + dirVector.normalized);
            characterController.SimpleMove(dirVector * movementSpeed); //To use with no root motion
            // characterController.SimpleMove(Vector3.zero); //To use the gravity and steps climb logic of CharacterController but not the movement
        }
        else if(isMoving)
        {
            characterAnimator.speed = 1f;
            characterAnimator.SetBool(IS_MOVING, false);
            isMoving = false;
        }
        
        cameraPivot.position = characterModelTransform.position + cameraModelDif;
    }
}