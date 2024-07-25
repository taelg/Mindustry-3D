using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoomBehavior : MonoBehaviour {

    [SerializeField] private int minCameraDistance = 5;
    [SerializeField] private int initialZoomDistance = 11;
    [SerializeField] private int maxCameraDistance = 30;
    [SerializeField] private int scrollSpeed = 5;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private new Camera camera;

    private float targetZoom;
    private float scroll;
    private bool isScrolling = false;

    private void Start() {
        targetZoom = initialZoomDistance;
    }

    private void Update() {
        UpdateScrollInput();
        UpdateZoomTarget();
        SmoothlyZoom();
    }

    private void UpdateScrollInput() {
        scroll = Input.mouseScrollDelta.y;
        isScrolling = scroll != 0;
    }

    private void UpdateZoomTarget() {
        if (isScrolling) {
            float zoomValue = -scroll * scrollSpeed;
            targetZoom = Clamped(camera.orthographicSize + zoomValue);
        }
    }

    private void SmoothlyZoom() {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
    }

    private float Clamped(float distance) {
        return Mathf.Clamp(distance, minCameraDistance, maxCameraDistance);
    }

}
