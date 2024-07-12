using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    void Start() {
        StartSingleton();
    }

    private void StartSingleton() {
        if (Instance != null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
            Debug.LogError("You are trying to initialize multiple Singletons of type: " + this.gameObject.name);
        }
    }

    void Update() {
        HandleInputs();

    }

    private void HandleInputs() {
        // TODO: Use this class as an adapter to receive input from different controller types.
    }


}
