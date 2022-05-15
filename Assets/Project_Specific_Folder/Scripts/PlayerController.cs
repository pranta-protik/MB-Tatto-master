using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

  
    public class PlayerController : MonoBehaviour
    {


        [Header("Control & Movement")]
        public float speed = 0.7f;
        [Range(0f, 1f)]
        public float movementSmoothing = .5f;
        [Range(0f, 1f)]
        public float rotationSmoothing = .25f;
        private Vector3 mouseCurrentPos;
        private Vector3 mouseStartPos;
        private Vector3 moveDirection;
        private Vector3 targetDirection;
        private float currentDragDistance;
        [SerializeField]
        private float maxDragDistance = 150f;
        [SerializeField]
        private float turnTreshold = 20f;
        [SerializeField]
        private float sensitivity = 1000.0f;
        private Vector3 currentDirection;
        private bool canMove;

    void Start()
        {

        }

        void Update()
        {
        if(Input.GetMouseButton(0))
            HandlePlayerMovement();
        }

        public void HandlePlayerMovement()
        {
            mouseCurrentPos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                mouseStartPos = mouseCurrentPos;

 

                if (!canMove)
                {
                    canMove = true;
                
                }

               
            }
          
            if (Input.GetMouseButton(0) )
            {
                float distance = (mouseCurrentPos - mouseStartPos).magnitude;
                if (distance > turnTreshold)
                {
                    if (distance > sensitivity)
                    {
                        mouseStartPos = mouseCurrentPos - (moveDirection * sensitivity);
                    }

                    currentDirection = -(mouseStartPos - mouseCurrentPos).normalized;
                    moveDirection.x = Mathf.Lerp(moveDirection.x, currentDirection.x, movementSmoothing);
                    moveDirection.z = Mathf.Lerp(moveDirection.z, currentDirection.y, movementSmoothing);
                    moveDirection.y = 0f;
                }

                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

                    if (canMove && transform.rotation != targetRotation)
                    {
                        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0f, 360f, 0f) * Time.smoothDeltaTime);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * deltaRotation, rotationSmoothing);
                    }
                }
            }

            if (canMove)
            {
                transform.position += transform.forward * speed * 20f * Time.deltaTime;
            }
        }





    }
