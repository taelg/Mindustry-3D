using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomBehavior : MonoBehaviour
{

    [SerializeField] private int minCameraDistance = 3;
    [SerializeField] private int maxCameraDistance = 30;
    [SerializeField] private int wheelStep = 4;
    [SerializeField] private float zoomSpeed = 5f;

    private Camera thisCamera;
    private float targetZoom;

    private void Start()
    {
        thisCamera = this.GetComponent<Camera>();
        targetZoom = GetStartingDistance();
    }

    private float GetStartingDistance()
    {
        return maxCameraDistance / minCameraDistance + minCameraDistance;
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        HandleZoomInput();
        thisCamera.orthographicSize = Mathf.Lerp(thisCamera.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
    }

    private void HandleZoomInput()
    {
        float scroll = Input.mouseScrollDelta.y;
        bool isZooming = scroll != 0;
        if (isZooming)
        {
            bool isZoomingOut = scroll < 0;
            float zoomValue = isZoomingOut ? wheelStep : -wheelStep;
            targetZoom = Clamped(thisCamera.orthographicSize + zoomValue);
        }
    }

    private float Clamped(float distance)
    {
        return Mathf.Clamp(distance, minCameraDistance, maxCameraDistance);
    }

}
