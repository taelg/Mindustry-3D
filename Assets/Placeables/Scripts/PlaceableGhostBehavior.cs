using System.Linq;
using UnityEngine;

public class PlaceableGhostBehavior : PlaceableBehavior, IPoolableItem {


    [SerializeField] private Material materialBlue;
    [SerializeField] private Material materialRed;
    [SerializeField] private MeshRenderer[] meshRenderers;
    private bool readyToBuild = false;

    public void UpdatePreview() {
        UpdateMaterial();
    }

    public void Reset() {
        readyToBuild = false;
    }

    public void SetReadyToBuild(bool readyToBuild) {
        this.readyToBuild = readyToBuild;
    }

    public bool IsReadyToBuild() {
        return readyToBuild;
    }

    public PlaceableBehavior Build() {
        if (readyToBuild) {
            PlaceableBehavior placeable = PoolManager.Instance.GetPoolByType(type).GetNext().GetComponent<PlaceableBehavior>();
            placeable.transform.position = this.transform.position;
            placeable.transform.forward = this.transform.forward;
            placeable.transform.SetParent(this.transform.parent);
            this.gameObject.SetActive(false);
            GridSystemManager.Instance.TakeSpace(placeable);
            return placeable;
        } else {
            Debug.LogError("You can't calling Build on a Placeable not yet ready to build.");
            return null;
        }
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
