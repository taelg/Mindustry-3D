using UnityEngine;

public class CharacterMovementBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float moveSpeed;

    [Space]
    [Header("Internal")]
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
        UpdateAnimator();
    }

    private void UpdateInputs() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() {
    }

    private void Move() {
        moveDirection = (this.transform.forward * verticalInput) + (this.transform.right * horizontalInput);
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        isMoving = moveDirection.magnitude > 0;
    }

    private void UpdateAnimator() {
        animator.SetBool("isMoving", isMoving);
    }

}
