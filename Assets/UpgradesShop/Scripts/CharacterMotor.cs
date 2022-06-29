using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterModelTransform;
    [SerializeField] private float movementSpeed = 1f;
    
    private Vector3 dirVector;

    private void FixedUpdate()
    {
        dirVector = Vector3.zero;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dirVector += Vector3.forward;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dirVector += Vector3.left;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dirVector += Vector3.back;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dirVector += Vector3.right;
        }

        if(dirVector != Vector3.zero)
        {
            characterModelTransform.LookAt(characterModelTransform.position + dirVector);
            characterController.Move(dirVector * movementSpeed);
        }
    }
}