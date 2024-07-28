using System.Linq;
using UnityEngine;

public class PlaceableGhostBehavior : MonoBehaviour {

    [SerializeField] private Material materialBlue;
    [SerializeField] private Material materialRed;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private MeshRenderer[] meshRenderers;

    public void Preview() {
        UpdateMaterial();
    }

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

    private void UpdateMaterial() {
        bool isEnoughtSpace = GridSystemManager.Instance.IsGridEmpty(this);
        Material material = isEnoughtSpace ? materialBlue : materialRed;
        SetAllMaterialsTo(material);
    }

    private void SetAllMaterialsTo(Material material) {
        foreach (MeshRenderer mesh in meshRenderers) {
            int meshMaterialCount = mesh.materials.Length;
            Material[] newMaterialArray = Enumerable.Repeat(material, meshMaterialCount).ToArray();
            mesh.materials = newMaterialArray;
        }
    }


}
