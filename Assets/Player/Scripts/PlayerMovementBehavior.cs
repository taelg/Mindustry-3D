using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float moveSpeed = 2000f;
    [SerializeField] private float acceleration = 5000f;
    [SerializeField] private float rotationSpeed = 10f;

    [Space]
    [Header("Internal")]
    [SerializeField] private Rigidbody rigidBody;

    private Vector3 lastValidMoveDirection = Vector3.forward;
    private Vector3 moveDirection;


    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        UpdateMoveDirection();
        ApplyMoveForces();
        ClampMoveSpeed();
        FaceMovingDirection();

    }

    private void UpdateMoveDirection() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        lastValidMoveDirection = moveDirection.magnitude > 0.01f ? moveDirection : lastValidMoveDirection;
    }

    private void ApplyMoveForces() {
        rigidBody.AddForce(moveDirection * acceleration * Time.deltaTime);
    }

    private void ClampMoveSpeed() {
        Vector3 clampedVelocity = Vector3.ClampMagnitude(rigidBody.velocity, moveSpeed);
        rigidBody.velocity = clampedVelocity;
    }

    private void FaceMovingDirection() {
        Quaternion target = Quaternion.LookRotation(lastValidMoveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
    }

    public bool IsMoving() {
        return moveDirection.magnitude > 0;
    }

}
