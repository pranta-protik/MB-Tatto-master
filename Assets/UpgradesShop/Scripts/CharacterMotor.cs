using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Transform characterModelTransform;
    [SerializeField] private float movementSpeed = 1f;

    private Vector3 dirVector;

    private void Awake()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#endif
    }

    private void Update()
    {
        dirVector = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        
        if(dirVector != Vector3.zero)
        {
            characterModelTransform.LookAt(characterModelTransform.position + dirVector.normalized);
            characterController.SimpleMove(dirVector * movementSpeed);
        }
    }
}