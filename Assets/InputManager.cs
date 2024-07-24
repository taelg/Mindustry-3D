using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    private void Start() {
        StartSingleton();
    }

    protected void StartSingleton() {
        if (Instance != null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this.gameObject);
            Debug.LogError("You are trying to initialize multiple Singletons of type: " + this.gameObject.name);
        }
    }

    private void Update() {
        HandleInputs();

    }

    private void HandleInputs() {
        // TODO: Use this class as an adapter to receive input from different controller types.
    }

}
