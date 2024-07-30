using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableBehavior : MonoBehaviour, IPoolableItem {
    [SerializeField] protected PlaceableType type;
    [SerializeField] protected BoxCollider boxCollider;

    public bool TryPlace() {
        bool isEnoughtSpace = GridSystemManager.Instance.IsGridEmpty(this);
        if (!isEnoughtSpace)
            return false;

        GridSystemManager.Instance.TakeSpace(this);
        return true;
    }

    public virtual void Reset() { }

    public Vector3 GetSize() {
        if (!boxCollider)
            boxCollider = this.transform.GetComponent<BoxCollider>();

        return boxCollider.size;
    }


}
