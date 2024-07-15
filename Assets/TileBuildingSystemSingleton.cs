using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour, ISingleton {

    public static TileManager instance;

    private void Start() {
        StartSingleton();

    }

    public void StartSingleton() {
        if (instance != null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            Debug.LogError("You are trying to initialize multiple Singletons of type: " + this.gameObject.name);
        }
    }

}
