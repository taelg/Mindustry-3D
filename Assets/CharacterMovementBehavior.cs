using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovementBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float moveSpeed;

    [Space]
    [Header("Internal")]
    [SerializeField] private Transform lookingDirection;
    [SerializeField] private CharacterController characterController;

    [Space]
    [Header("To be Removed")] //TODO: Create a class to handle animator states.
    [SerializeField] private Animator animator;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private bool isMoving;

    private void Update() {
        UpdateInputs();
        Move();
        UpdatePlayerRotation();
        UpdateAnimator();
    }

    private void UpdateInputs() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() {
    }

    private void Move() {
        moveDirection = (lookingDirection.forward * verticalInput) + (lookingDirection.right * horizontalInput);
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        isMoving = moveDirection.magnitude > 0;
    }

    private void UpdatePlayerRotation() {
        if (isMoving)
            this.transform.rotation = lookingDirection.transform.rotation;
    }

    private void UpdateAnimator() {
        animator.SetBool("isMoving", isMoving);
    }

}
