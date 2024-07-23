using UnityEngine;

public class CameraFollowBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float followSpeed;
    [Tooltip("How much faster should the camera be when player stop moving.")]
    [SerializeField] private float playerStillSpeedMultiplier;

    [Header("Internal")]
    [SerializeField] private PlayerMovementBehavior playerMovement;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate() {
        Transform target = playerMovement.transform;
        float speed = playerMovement.IsMoving() ? followSpeed : followSpeed * playerStillSpeedMultiplier;
        this.transform.position = Vector3.Lerp(this.transform.position, target.position, speed * Time.deltaTime);
    }


}
