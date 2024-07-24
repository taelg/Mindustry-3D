
using UnityEngine;

public class RaycastUtils {

    public static Vector3 GetMouseWorldPosition(Camera activeCamera) {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = activeCamera.ScreenPointToRay(mouseScreenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

}