using UnityEngine;
using UnityEngine.Events;

public class MouseInputManager : SingletonBehavior<MouseInputManager> {

    public bool leftButtonDown { get; private set; }
    public bool rightButtonDown { get; private set; }
    public bool leftButtonStay { get; private set; }
    public bool rightButtonStay { get; private set; }
    public float scroll { get; private set; }
    private Drag lastDrag;
    private Drag drag;
    public event UnityAction<Drag> OnDragStart;
    public event UnityAction OnDragEnd;

    public Vector3 GetMouseWorldPosition() {
        Vector3 pos = RaycastUtils.GetMouseWorldPosition(Camera.main);
        Vector3 normalizedHeight = new Vector3(pos.x, 0, pos.z);
        return normalizedHeight;
    }

    public Drag GetLatestDrag() {
        return drag != null ? drag : lastDrag;
    }

    private void Update() {
        HandleMouseInputs();
    }

    private void HandleMouseInputs() {
        UpdateMouseButtonDown();
        UpdateMouseButtonStay();
        UdpateMouseScroll();
        UdpateMouseDrag();
    }

    private void UpdateMouseButtonDown() {
        leftButtonDown = Input.GetMouseButtonDown(0);
        rightButtonDown = Input.GetMouseButtonDown(1);
    }

    private void UpdateMouseButtonStay() {
        leftButtonStay = Input.GetMouseButton(0);
        rightButtonStay = Input.GetMouseButton(1);
    }

    private void UdpateMouseScroll() {
        scroll = Input.GetAxis("Mouse ScrollWheel");
    }

    private void UdpateMouseDrag() {
        bool isDrag = drag != null;
        bool stillDragging = leftButtonStay || rightButtonStay;
        bool startDrag = !isDrag && stillDragging;
        bool endDrag = isDrag && !stillDragging;
        bool notInMenu = !RaycastUtils.IsMouseOverUI();

        if (startDrag && notInMenu) {
            drag = GetNewDrag();
            OnDragStart?.Invoke(drag);
        } else if (endDrag) {
            drag.End();
            OnDragEnd?.Invoke();
            lastDrag = drag;
            drag = null;
        }
    }

    private Drag GetNewDrag() {
        byte mouseButton = leftButtonStay ? Drag.MOUSE_BUTTON_LEFT : Drag.MOUSE_BUTTON_RIGHT;
        return new Drag(mouseButton);
    }


}