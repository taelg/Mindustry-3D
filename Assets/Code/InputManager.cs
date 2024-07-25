using UnityEngine;

public class InputManager : SingletonBehavior<InputManager> {

    private new void Awake() {
        base.Awake();
    }

    private void Update() {
        HandleInputs();

    }

    private void HandleInputs() {
        // TODO: Use this class as an adapter to receive input from different controller types.
    }

}
