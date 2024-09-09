using UnityEngine;

public class CameraZoomBehavior : MonoBehaviour {

    [SerializeField] private int minCameraDistance = 10;
    [SerializeField] private int initialCameraDistance = 15;
    [SerializeField] private int maxCameraDistance = 35;
    [SerializeField] private int scrollSpeed = 3;
    [SerializeField] private float zoomSpeed = 8f;

    private float currentCameraDistance;

    private float targetCameraDistance;
    private float normalizedScroll;
    private bool isScrolling = false;

    private void Start() {
        SetCameraDistance(initialCameraDistance);
    }

    private void Update() {
        UpdateScrollInput();
        UpdateTargetCameraDistance();
        LerpMove();
    }

    private void UpdateScrollInput() {
        normalizedScroll = Mathf.Clamp(Input.mouseScrollDelta.y, -2f, 2f);
        isScrolling = normalizedScroll != 0f;
    }

    private void UpdateTargetCameraDistance() {
        if (isScrolling) {
            float targetDistance = this.transform.position.y - (normalizedScroll * scrollSpeed);
            targetCameraDistance = targetDistance;
        }
    }

    private void LerpMove() {
        float newDistance = Mathf.Lerp(currentCameraDistance, targetCameraDistance, Time.deltaTime * zoomSpeed);
        SetCameraDistance(newDistance);
    }

    private float Clamped(float distance) {
        return Mathf.Clamp(distance, minCameraDistance, maxCameraDistance);
    }

    private void SetCameraDistance(float newDistance) {
        float selfX = this.transform.position.x;
        float selfZ = this.transform.position.z;
        this.transform.position = new Vector3(selfX, Clamped(newDistance), selfZ);
        currentCameraDistance = newDistance;
    }

}
