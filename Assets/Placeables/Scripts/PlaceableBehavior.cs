using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableBehavior : MonoBehaviour, IPoolableItem {
    [SerializeField] protected PlaceableType type;
    [SerializeField] protected BoxCollider boxCollider;

    public bool TryPlace() {
        bool isEnoughtSpace = GridSystemManager.Instance.IsGridEmpty(this);
        if (isEnoughtSpace) {
            GridSystemManager.Instance.TakeSpace(this);
            return true;
        } else {
            this.gameObject.SetActive(false);
            return false;
        }
    }

    public Vector3 GetSize() {
        return boxCollider.size;
    }

    public void Reset() { }

}
