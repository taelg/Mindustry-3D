using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float moveSpeed = 2000f;
    [SerializeField] private float acceleration = 5000f;
    [SerializeField] private float rotationSpeed = 10f;

    [Space]
    [Header("Internal")]
    [SerializeField] private Rigidbody rigidyBody;
    private Vector3 moveDirection;
    private Vector3 lastInputedMoveDirection = Vector3.forward;


    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        UpdateMoveDirection();
        ApplyMoveForces();
        ClampMoveSpeed();
        FaceMoveDirection();

    }

    private void UpdateMoveDirection() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        lastInputedMoveDirection = moveDirection.magnitude > 0.01f ? moveDirection : lastInputedMoveDirection;
    }

    private void ApplyMoveForces() {
        rigidyBody.AddForce(moveDirection * acceleration * Time.deltaTime);
    }

    private void ClampMoveSpeed() {
        bool tooFast = rigidyBody.velocity.magnitude > moveSpeed;
        Vector3 maxSpeed = rigidyBody.velocity.normalized * moveSpeed;
        rigidyBody.velocity = tooFast ? maxSpeed : rigidyBody.velocity;
    }

    private void FaceMoveDirection() {
        Quaternion target = Quaternion.LookRotation(lastInputedMoveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
    }

    public bool IsMoving() {
        return moveDirection.magnitude > 0;
    }



}
