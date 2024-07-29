using System.Linq;
using UnityEngine;

public class PlaceableGhostBehavior : PlaceableBehavior {


    [SerializeField] private Material materialBlue;
    [SerializeField] private Material materialRed;
    [SerializeField] private MeshRenderer[] meshRenderers;

    public void Preview() {
        UpdateMaterial();
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

    public PlaceableBehavior Build() {
        PlaceableBehavior placeable = PoolManager.Instance.GetPoolByType(type).GetNext().GetComponent<PlaceableBehavior>();
        placeable.transform.position = this.transform.position;
        placeable.transform.forward = this.transform.forward;
        this.gameObject.SetActive(false);
        GridSystemManager.Instance.TakeSpace(placeable);
        return placeable;
    }

}
