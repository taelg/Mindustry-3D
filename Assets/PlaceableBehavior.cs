using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableBehavior : MonoBehaviour {
    [SerializeField] protected BoxCollider boxCollider;

    public void TryPlace() {
        bool isEnoughtSpace = GridSystemManager.Instance.IsGridEmpty(this);
        if (isEnoughtSpace) {
            GridSystemManager.Instance.TakeSpace(this);
        } else {
            this.gameObject.SetActive(false);
        }
    }

    public Vector3 GetSize() {
        return boxCollider.size;
    }


}
