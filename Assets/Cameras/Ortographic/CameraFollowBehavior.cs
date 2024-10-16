using UnityEngine;

public class CameraFollowBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float followSpeed;
    [Tooltip("How much faster should the camera be when player stop moving.")]
    [SerializeField] private float playerStillSpeedMultiplier;

    [Space]
    [Header("Internal")]
    [SerializeField] private PlayerMovementBehavior player;

    private Vector3 targetPosition;
    private float currentSpeed;

    private void LateUpdate() {
        UpdateTargetPosition();
        UpdateCurrentSpeed();
        MoveCamera();
    }

    private void UpdateTargetPosition() {
        float targetX = player.transform.position.x;
        float selfY = this.transform.position.y;
        float targetZ = player.transform.position.z;
        targetPosition = new Vector3(targetX, selfY, targetZ);
    }

    private void UpdateCurrentSpeed() {
        currentSpeed = player.IsMoving() ? followSpeed : followSpeed * playerStillSpeedMultiplier;
    }

    private void MoveCamera() {
        Vector3 newPosition = Vector3.Lerp(this.transform.position, targetPosition, currentSpeed * Time.deltaTime);
        float notChangedHeight = this.transform.position.y;
        this.transform.position = new Vector3(newPosition.x, notChangedHeight, newPosition.z);
    }

}
