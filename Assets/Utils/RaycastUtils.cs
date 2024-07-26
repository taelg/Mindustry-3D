
using UnityEngine;

public class RaycastUtils {

    public static Vector3 GetMouseWorldPosition(Camera activeCamera) {
        return activeCamera.orthographic
            ? GetMouseWorldPositionOrtographic(activeCamera)
            : GetMouseWorldPositionPerspective(activeCamera);
    }

    private static Vector3 GetMouseWorldPositionPerspective(Camera activeCamera) {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = activeCamera.ScreenPointToRay(mouseScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    private static Vector3 GetMouseWorldPositionOrtographic(Camera activeCamera) {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = activeCamera.ScreenPointToRay(mouseScreenPosition);
        Vector3 mouseWorldPosition = activeCamera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, activeCamera.nearClipPlane));
        return mouseWorldPosition;
    }

}