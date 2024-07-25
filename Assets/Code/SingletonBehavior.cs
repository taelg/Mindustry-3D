using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour {

    public static T Instance;

    protected void Awake() {
        AwakeSingleton();
    }

    protected void AwakeSingleton() {
        if (Instance == null) {
            Instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
            Debug.LogError("You are trying to initialize multiple Singletons of type: " + this.gameObject.name);
        }
    }

}
