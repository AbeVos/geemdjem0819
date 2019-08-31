using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Ring
{
    private List<Vector3> vertices;

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public Ring(Transform transform, float radius, int nVertices)
    {
        vertices = new List<Vector3>();
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;

        for (int i = 0; i < nVertices; i++) {
            var t = 2 * Mathf.PI * i / nVertices;

            var x = radius * Mathf.Cos(t);
            var z = radius * Mathf.Sin(t);

            var vertex = new Vector3(x, 0, z);

            vertices.Add(vertex);
        }
    }

    /// Retrieve transformed vertices from this Ring.
    public List<Vector3> Vertices
    {
        get {
            var matrix = Matrix4x4.TRS(position, rotation, scale);

            var transformedVertices = new List<Vector3>();

            foreach (Vector3 vector in vertices) {
                transformedVertices.Add(matrix.MultiplyPoint3x4(vector));
            }

            return transformedVertices;
        }
        private set { vertices = value; }
    }
}

[RequireComponent(typeof(MeshFilter))]
public class PlantBranch : MonoBehaviour
{
    public int nVertices = 8;

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
    private List<Ring> branchRings;

    protected void Start() {
        growBase = new GameObject().transform;
        growBase.parent = transform;
        lastPosition = growBase.position;
        growDirection = new Quaternion();

        branchSegments = new List<Transform>();
        branchRings = new List<Ring>();
        branchRings.Add(new Ring(growBase, 1f, nVertices));

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

    protected void Update()
    {
        var direction = growTarget.position - growBase.position;

        if (direction.magnitude > 0f)
        {
            growDirection = YLookRotation(direction.normalized, Vector3.up);
        }

        if (isGrowing)
        {
            TickBranch();
        }
    }

    /// Update the branch growing.
    public void TickBranch()
    {
        // Rotate growBase towards its target and add some noise to the rotation.
        growBase.rotation = Quaternion.RotateTowards(
            growBase.rotation, growDirection, Mathf.PI * turnSpeed * Time.deltaTime
        ) * Random.rotation.Pow(Time.deltaTime * directionVariance);

        var position = growBase.position;
        position += Time.deltaTime * growSpeed * growBase.up;

        growBase.position = position; 

        // Create a new segment once the tip has moved far enouch away
        // from the rest of the branch.
        if (Vector3.Distance(lastPosition, position) > segmentLength)
        {
            lastPosition = position;

            branchRings.Add(new Ring(growBase, 1f, nVertices));
            GenerateMesh();
        }
    }

    /// Update the MeshFilter's mesh using the branchRings' vertices.
    public void GenerateMesh()
    {
        var vertexList = new List<Vector3>();

        foreach (Ring ring in branchRings)
        {
            vertexList.AddRange(ring.Vertices);
        }

        var vertexArray = vertexList.ToArray();

        meshFilter.mesh.vertices = vertexArray;

        var numRings = branchRings.Count;
        var numQuads = (numRings - 1) * nVertices;

        var tris = new int[3 * numQuads];

        for (var i = 0; i < numQuads; i++)
        {
            tris[3*i] = i;
            tris[3*i+1] = i + nVertices;
            tris[3*i+2] = ((i + 1) % nVertices) + (i - (i % nVertices));
        }

        meshFilter.mesh.triangles = tris;

        meshFilter.mesh.RecalculateNormals();
    }

    private Quaternion YLookRotation(Vector3 right, Vector3 up)
    {
        Quaternion upToForward = Quaternion.Euler(90f, 0f, 0f);
        Quaternion forwardToTarget = Quaternion.LookRotation(right, up);

        return forwardToTarget * upToForward;
    }
}