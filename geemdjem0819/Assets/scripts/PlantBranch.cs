using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PlantBranch : MonoBehaviour
{
    public float segmentLength = 1f;
    public float growSpeed = 1f;
    public float turnSpeed = 3f;

    public float directionVariance = 1f;

    public Transform growTarget;

    // Global direction of growth.
    private Transform growBase;
    // Last position (local) where a branch segment has been spawned.
    private Vector3 lastPosition;
    private Quaternion growDirection;

    private bool isGrowing = true;
    public bool IsGrowing { get; set; }

    private MeshFilter meshFilter;

    private List<Transform> branchSegments;
    
    protected void Start() {
        growBase = new GameObject().transform;
        growBase.parent = transform;
        lastPosition = growBase.position;
        growDirection = new Quaternion();

        branchSegments = new List<Transform>();

        meshFilter = GetComponent<MeshFilter>();
        
        /*
        var mesh = new Mesh();
        meshFilter.mesh = mesh;

        var vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width, 0, 0),
            new Vector3(0, height, 0),
            new Vector3(width, height, 0)
        };
        mesh.vertices = vertices;

        var tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        var normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        var uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;
        */
    }

    protected void Update() {
        var direction = growTarget.position - growBase.position;

        if (direction.magnitude > 0f) {
            growDirection = YLookRotation(direction.normalized, Vector3.up);
        }

        if (isGrowing) {
            TickBranch();
        }
    }

    public void TickBranch() {
        growBase.rotation = Quaternion.RotateTowards(
            growBase.rotation, growDirection, Mathf.PI * turnSpeed * Time.deltaTime
        ) * Random.rotation.Pow(Time.deltaTime * directionVariance);
        var position = growBase.position;
        position += Time.deltaTime * growSpeed * growBase.up;

        growBase.position = position; 

        // Create a new segment once the tip has moved far enouch away
        // from the rest of the branch.
        if (Vector3.Distance(lastPosition, position) > segmentLength) {
            lastPosition = position;

            var segment = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
            segment.position = growBase.position;
            segment.rotation = growBase.rotation;

            if (branchSegments.Count == 0) {
                segment.parent = transform;
            }
            else {
                var last = branchSegments[branchSegments.Count - 1];
                // segment.parent = last;
                segment.parent = transform;
                growBase.parent = last;
            }
            branchSegments.Add(segment);
        }
    }

    /// Create a circle of vertices.
    private List<Vector3> CreateCircle(Transform transform, float radius, int n_vertices) {
        var vertices = new List<Vector3>();

        for (int i=0; i<n_vertices; i++) {
            var t = i / n_vertices;

            var vertex = new Vector3(0, 0, 0);

            vertices.Add(vertex);
        }

        return vertices;
    }

    private Quaternion YLookRotation(Vector3 right, Vector3 up) {
        Quaternion upToForward = Quaternion.Euler(90f, 0f, 0f);
        Quaternion forwardToTarget = Quaternion.LookRotation(right, up);

        return forwardToTarget * upToForward;
    }
}