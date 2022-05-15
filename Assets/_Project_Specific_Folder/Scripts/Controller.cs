using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Controller : MonoBehaviour
{
    public enum State
    {
        left,
        right,
        middle
    }


    public State direction;


    [Header("Control & Movement")]

   
 
    private Rigidbody playerRigidbody;
    private Vector3 deviation;
    public float maxDragDistance = 40f;
    private Vector3 moveDirection = Vector3.forward;
    private Vector3 currentDirection = Vector3.forward;
    public float sensitivity = 300f;
    public float SwipeSensitivity = 1f;
    public float turnTreshold = 15f;
    private Vector3 mouseStartPosition;
    private Vector3 mouseCurrentPosition;
    protected Quaternion targetRotation;
    [SerializeField] private float movementSmoothing;
    [SerializeField] private float rotationSmoothing;
    public Touch initTouch = new Touch();
    public Vector3 originPos;
    public float positionX, positionY;
    public float speed = 0.5f, computerSpeed, dir = -1f;
    public float rotationSpeed;
    public float mapWidth = 2f;
    public bool touching = false;
    private Rigidbody rb;
    public bool end;
    Camera cam;
    float speeds;
    public float EndSceneSpeed = .5f;
    public int count = 0;
    public int TileCount;
    public int multiply;
    BoxCollider m_BoxCollider;
    bool playedd;
    Camera m_Cam;

  

    void Start()
    {
 
        m_Cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        positionX = 0f;
        positionY = transform.localPosition.y;
        originPos = transform.localPosition;
      

       

        cam = Camera.main;
        direction = State.middle;
        playerRigidbody = GetComponent<Rigidbody>();
        m_BoxCollider = GetComponent<BoxCollider>();
        //   target = GameObject.FindGameObjectWithTag("End").transform.GetChild(1);
    }
    Vector3 lastPosition = Vector3.zero;
    private GameObject lastChildObject;
    private Transform lastChild;









    private void Update()
    {



        #region Controls
        speeds = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
        if (!GameManager.Instance.StartGame)
            return;
     
            HandlePlayerMovement();
        
        #endregion Controls



    }
    public void HandlePlayerMovement()
    {

        if (Input.GetAxis("Mouse X") > .1f)
        {
            direction = State.right;
          //  transform.DOLocalRotate(new Vector3(5, -90, 13), .1f);
        }
        if (Input.GetAxis("Mouse X") < -.1f)
        {

            direction = State.left;
         //   transform.DOLocalRotate(new Vector3(-5, -90, 13), .1f);
        }
        if (Input.GetAxis("Mouse X") == 0)
        {

            direction = State.middle;
        //    transform.DOLocalRotate(new Vector3(0, -90, 13), .3f);
        }


        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)        //if finger touches the screen
            {
                if (touching == false)
                {
                    touching = true;
                    initTouch = touch;
                }
            }
            else if (touch.phase == TouchPhase.Moved)       //if finger moves while touching the screen
            {
                float deltaX = initTouch.position.x - touch.position.x;
                positionX -= (deltaX / (float)Screen.width) / Time.deltaTime * speed * dir;
                positionX = Mathf.Clamp(positionX, -6, 6);      //to set the boundaries of the player's position
                transform.localPosition = new Vector3(-positionX, positionY, 3.1188f);
                initTouch = touch;
            }
            else if (touch.phase == TouchPhase.Ended)       //if finger releases the screen
            {
                initTouch = new Touch();
                touching = false;
            }
        }

        //if you play on computer---------------------------------
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * computerSpeed;     //you can move by pressing 'a' - 'd' or the arrow keys
        Vector3 newPosition = rb.transform.localPosition + Vector3.right * x;
        newPosition.x = Mathf.Clamp(newPosition.x, -2,2);
        transform.localPosition = newPosition;
        //--------------------------------------------------------

        if (Input.GetAxis("Horizontal") > .1f)
        {
            direction = State.right;
            transform.DOLocalRotate(new Vector3(6, -90, 20), .1f);
        }
        if (Input.GetAxis("Horizontal") < -.1f)
        {

            direction = State.left;
            transform.DOLocalRotate(new Vector3(-6, -90,20), .1f);
        }
        if (Input.GetAxis("Horizontal") == 0)
        {

            direction = State.middle;
            transform.DOLocalRotate(new Vector3(0, -90, 20), .2f);
        }

    }
}
