using UnityEngine;

public class TileManager : MonoBehaviour {

    public static TileManager instance;

    private void Start() {
        StartSingleton();

    }

    protected void StartSingleton() {
        if (instance != null) {
            instance = this;
        } else {
            Destroy(this.gameObject);
            Debug.LogError("You are trying to initialize multiple Singletons of type: " + this.gameObject.name);
        }
    }

}
