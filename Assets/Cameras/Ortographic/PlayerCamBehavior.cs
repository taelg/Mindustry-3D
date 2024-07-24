using System;
using UnityEngine;

public class PlayerCamBehavior : MonoBehaviour {

    [Header("Configurable")]
    [SerializeField] private float horizontalLookSensitivity;
    [SerializeField] private float verticalLookSensitivity;

    [Space]
    [Header("Internal")]
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Transform lookAtDirectionRig;

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

    // Update is called once per frame
    void Update() {
        Look();

    }

    private void Look() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * horizontalLookSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * verticalLookSensitivity;

        horizontalRotation += mouseX;
        verticalRotation -= mouseY;

        verticalRotation = Math.Clamp(verticalRotation, -90, 90);

        this.transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        characterTransform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
        lookAtDirectionRig.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);



    }


}
