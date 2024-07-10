using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed;
    [SerializeField] private float horizontalLookSensitivity;
    [SerializeField] private float verticalLookSensitivity;

    [SerializeField] private Transform lookingAtTransform;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        LockMousePointer(true);
        HideMousePointer(true);
    }

    private void LockMousePointer(Boolean lockPointer) {
        Cursor.lockState = lockPointer ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void HideMousePointer(Boolean hide) {
        Cursor.visible = !hide;
    }


    private void Update()
    {
        Look();
        Move();
    }

    private void Move() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontalInput, 0, verticalInput);
        characterController.Move(move * Time.deltaTime * speed);
    }

    private void Look() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * horizontalLookSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * verticalLookSensitivity;
        xRotation += mouseX;
        yRotation -= mouseY;

    }

}
