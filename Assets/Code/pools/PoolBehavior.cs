using System.Collections.Generic;
using UnityEngine;

public class PoolBehavior : MonoBehaviour {

    [Header("Internal")]
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int poolSize;
    [SerializeField] private Transform poolParent;

    private List<GameObject> pooledObjects;
    private int limitExceedIn = 0;
    private bool loaded = false;


    private void Awake() {
        LoadPool();
    }
    private void LoadPool() {
        pooledObjects = new List<GameObject>();
        loaded = false;
        InstantiatePoolClones();
        loaded = true;
    }

    private void InstantiatePoolClones() {
        for (int i = 0; i < poolSize; i++) {
            InstantiateNewClone();
        }
    }

    private GameObject InstantiateNewClone() {
        GameObject clone = Instantiate(objectToPool);
        clone.SetActive(false);
        clone.transform.SetParent(poolParent);
        pooledObjects.Add(clone);
        return clone;
    }

    private void OnEnable() {
        DisableAllClones();
    }

    public void DisableAllClones() {
        for (int i = 0; i < pooledObjects.Count; i++)
            pooledObjects[i].SetActive(false);
    }

    public GameObject GetNext() {
        if (!loaded)
            Debug.LogError("Pool not loaded yet, you should not be asking clones yet. Item: " + objectToPool.name);

        GameObject clone = GetFirstAvailableClone();
        bool noCloneAvailable = clone == null;

        if (noCloneAvailable) {
            limitExceedIn++;
            Debug.LogWarning("Pool initial size exceed in: " + limitExceedIn + ", Item: " + objectToPool.name);
            clone = InstantiateNewClone();
        }

        clone.SetActive(true);
        return clone;
    }

    private GameObject GetFirstAvailableClone() {
        for (int i = 0; i < pooledObjects.Count; i++)
            if (!pooledObjects[i].activeSelf)
                return pooledObjects[i];

        return null;
    }

    public bool isLoaded() {
        Debug.Log("is loaded pool " + gameObject.name + "? " + loaded);
        return loaded;
    }

}
