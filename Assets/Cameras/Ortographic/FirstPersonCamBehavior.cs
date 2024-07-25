using UnityEngine;

public class FirstPersonCamBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float horizontalLookSensitivity;
    [SerializeField] private float verticalLookSensitivity;

    [Space]
    [Header("Internal")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerHeadRig;

    private float horizontalRotation;
    private float verticalRotation;

    private void Start() {
        LockMousePointer(true);
        HideMousePointer(true);
    }

    private void LockMousePointer(bool lockPointer) {
        Cursor.lockState = lockPointer ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void HideMousePointer(bool hide) {
        Cursor.visible = !hide;
    }

    private void Update() {
        UpdateInputs();
        RotateCamera();
        RotatePlayer();
    }

    private void UpdateInputs() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * horizontalLookSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * verticalLookSensitivity;
        horizontalRotation += mouseX;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
    }

    private void RotateCamera() {
        this.transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    private void RotatePlayer() {
        player.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        playerHeadRig.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }


}
