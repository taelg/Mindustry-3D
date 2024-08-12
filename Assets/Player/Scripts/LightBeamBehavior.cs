using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LightBeamBehavior : MonoBehaviour {
    [SerializeField] private Transform source;
    [SerializeField] private int segments = 20;
    [SerializeField] private float radius = 1.0f;

    private MeshFilter meshFilter;
    private BoxCollider boxCollider;
    private BlueprintBehavior target;

    private void Start() {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void SetLightBeamTarget(BlueprintBehavior blueprint) {
        this.gameObject.SetActive(true);
        target = blueprint;
        this.boxCollider = blueprint.GetComponent<BoxCollider>();
        StartCoroutine(RemoveTargetAfterBuild(blueprint));
    }

    private IEnumerator RemoveTargetAfterBuild(BlueprintBehavior blueprint) {
        yield return new WaitUntil(() => blueprint.IsBuildCompleted());
        if (target == blueprint)
            target = null;
    }

    private void Update() {
        if (target != null) {
            DrawLightCone();
        } else {
            this.gameObject.SetActive(false);
        }
    }

    private void DrawLightCone() {
        Vector3 startPosition = source.position;
        Vector3 endPosition = target.transform.position;
        endPosition = new Vector3(endPosition.x, 1f, endPosition.z);

        // Get the size of the box collider
        Vector3 boxSize = boxCollider.size;

        // Calculate the radius at the end of the cone
        radius = Mathf.Max(boxSize.x, boxSize.z) / 2;

        // Create the mesh
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 6];

        // Set the start position as the first vertex
        vertices[0] = source.InverseTransformPoint(startPosition);

        // Calculate the vertices around the circumference at the end of the cone
        float angleStep = 360.0f / segments;
        for (int i = 0; i < segments; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            vertices[i + 1] = source.InverseTransformPoint(endPosition + offset);
        }

        // Set the end position as the last vertex
        vertices[segments + 1] = source.InverseTransformPoint(endPosition);

        // Create the triangles for the cone sides
        for (int i = 0; i < segments; i++) {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = (i + 1) % segments + 1;
        }

        // Create the triangles for the base
        int baseIndex = segments * 3;
        for (int i = 0; i < segments; i++) {
            triangles[baseIndex + i * 3] = segments + 1;
            triangles[baseIndex + i * 3 + 1] = (i + 1) % segments + 1;
            triangles[baseIndex + i * 3 + 2] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }


}