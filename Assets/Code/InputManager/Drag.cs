
using UnityEngine;

public class Drag {
    public static byte MOUSE_BUTTON_LEFT = 0;
    public static byte MOUSE_BUTTON_RIGHT = 1;
    public static byte MOUSE_BUTTON_MIDDLE = 2;

    public readonly Vector3 initialPos;
    public readonly byte mouseButton;

    private Vector3 finalPos;
    private bool isEnded;


    public Drag(byte mouseButton) {
        this.initialPos = GetCurrentPosition();
        this.mouseButton = mouseButton;
        isEnded = false;
    }

    public bool IsEnded() {
        return isEnded;
    }

    public void End() {
        this.finalPos = GetCurrentPosition();
        isEnded = true;
    }

    public Vector3 GetPrimaryAxisDirection() {
        Vector3 currentPos = GetLatestPosition();
        Vector3 direction = currentPos - initialPos;

        Debug.DrawLine(currentPos, initialPos, Color.cyan, 10);

        float distanceX = Mathf.Abs(direction.x);
        float distanceZ = Mathf.Abs(direction.z);

        return distanceX > distanceZ
            ? new Vector3(Mathf.Sign(direction.x), 0, 0)
            : new Vector3(0, 0, Mathf.Sign(direction.z));
    }

    public Vector3 GetLatestPosition() {
        return isEnded ? finalPos : GetCurrentPosition();
    }

    private Vector3 GetCurrentPosition() {
        Vector3 pos = RaycastUtils.GetMouseWorldPosition(Camera.main);
        //Vector3 normalizedHeight = new Vector3(pos.x, 0, pos.z);
        return pos;
    }

}