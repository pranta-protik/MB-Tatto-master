using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class HandController : MonoBehaviour
{
    public int handId;

    [Header("Control & Movement")] 
    public float positionXClampValue = 4f;
    public float mobileSpeed = 0.8f; 
    public float computerSpeed = 15f;
    
    private float _positionX, _positionY;
    private bool _isTouching;
    
    void Start()
    { 
        _positionX = 0f;
        _positionY = 3.1549f;
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
                _positionX += (deltaX / (float) Screen.width) / Time.deltaTime * mobileSpeed;
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
        newPosition.x = Mathf.Clamp(newPosition.x, -positionXClampValue,positionXClampValue);
        transform.localPosition = newPosition;
        
        if (Input.GetAxis("Horizontal") > .1f)
        {
            transform.DOLocalRotate(new Vector3(6, -90, 25), .3f);
        }
        if (Input.GetAxis("Horizontal") < -.1f)
        {
            transform.DOLocalRotate(new Vector3(-6, -90,25), .3f);
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            transform.DOLocalRotate(new Vector3(0, -90, 25), .3f);
        }
    }
}
