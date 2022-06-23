using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandController : MonoBehaviour
{
    public int handId;

    [Header("Control & Movement")] [SerializeField]
    private float positionXClampValue = 4f;

    [SerializeField] private float mobileSpeed = 0.8f;
    [SerializeField] private float computerSpeed = 15f;
    public ERotationAxis rotationAxis;
    [SerializeField] private float rotationDuration = 0.3f;

    private float _positionX, _positionY;
    private bool _isTouching;
    private bool _canRotate;

    void Start()
    {
        _positionX = 0f;
        _positionY = 3.1549f;
        _canRotate = true;
    }

    private void Update()
    {
        if (GameManager.Instance.hasGameStarted)
        {
            HandlePlayerMovement();
        }
    }

    private void HandlePlayerMovement()
    {
        //Mobile control
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (_isTouching == false)
                {
                    _isTouching = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x;
                _positionX -= (deltaX / Screen.width) / Time.deltaTime * mobileSpeed;
                _positionX = Mathf.Clamp(_positionX, -positionXClampValue, positionXClampValue);
                transform.localPosition = new Vector3(-_positionX, _positionY, 3.1188f);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _isTouching = false;
            }
        }

        //Keyboard control
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * computerSpeed;
        Vector3 newPosition = transform.localPosition + Vector3.right * x;
        newPosition.x = Mathf.Clamp(newPosition.x, -positionXClampValue, positionXClampValue);
        transform.localPosition = newPosition;

        if (Input.GetAxis("Horizontal") > .1f)
        {
            transform.DOLocalRotate(new Vector3(6, -90, 25), .3f);
        }

        if (Input.GetAxis("Horizontal") < -.1f)
        {
            transform.DOLocalRotate(new Vector3(-6, -90, 25), .3f);
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            transform.DOLocalRotate(new Vector3(0, -90, 25), .3f);
        }
    }

    public void RotateHandAlongXAxis()
    {
        if (_canRotate)
        {
            _canRotate = false;
            transform.GetChild(0).DOBlendableRotateBy(new Vector3(0f, 360f, 0f), rotationDuration, RotateMode.FastBeyond360).OnComplete(() => { _canRotate = true; });
        }
    }

    public void RotateHandAlongYAxis()
    {
        if (_canRotate)
        {
            _canRotate = false;
            transform.GetChild(0).DOLocalRotate(new Vector3(-360f, 0f, 0f), rotationDuration, RotateMode.LocalAxisAdd).SetRelative(true)
                .OnComplete(() => { _canRotate = true; });
        }
    }
}
