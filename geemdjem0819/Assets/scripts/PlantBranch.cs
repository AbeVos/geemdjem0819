using System.Collections;
using System.Collections.Generic;
using interfaces;
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
public class PlantBranch : MonoBehaviour, ITickable
{
    public int nVertices = 8;

    public float branchRadius = 1f;
    public float startRadius = 1f;
    public float tipFalloff = 5f;

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
        branchRings.Add(new Ring(growBase, startRadius, nVertices));

        meshFilter = GetComponent<MeshFilter>();
        
        if (growTarget == null) growTarget = Camera.main.transform;
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
            Tick();
        }
    }

    /// Update the branch growing.
    public void Tick()
    {
        if (!isActiveAndEnabled) return;

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

            branchRings.Add(new Ring(growBase, startRadius, nVertices));
            GenerateMesh();
        }
    }

    /// Update the MeshFilter's mesh using the branchRings' vertices.
    public void GenerateMesh()
    {
        var vertexList = new List<Vector3>();

        for (var i = 0; i < branchRings.Count; i++)
        {
            var ring = branchRings[i];
            var iInv = branchRings.Count - i;

            ring.scale = branchRadius * Vector3.one * (1 - Mathf.Exp(-1 / tipFalloff * iInv));

            vertexList.AddRange(ring.Vertices);
        }

        meshFilter.mesh.vertices = vertexList.ToArray();

        var numRings = branchRings.Count;
        var numQuads = (numRings - 1) * nVertices;

        var tris = new int[6 * numQuads];

        for (var i = 0; i < numQuads; i++)
        {
            tris[3*i] = i;
            tris[3*i+1] = i + nVertices;
            tris[3*i+2] = RingModulo(i, nVertices);

            tris[3*i + 3 * numQuads] = RingModulo(i, nVertices);
            tris[3*i + 3 * numQuads+1] = i + nVertices;
            tris[3*i + 3 * numQuads+2] = RingModulo(i, nVertices) + nVertices;
        }

        meshFilter.mesh.triangles = tris;
        meshFilter.mesh.RecalculateNormals();
    }

    private Quaternion YLookRotation(Vector3 right, Vector3 up)
    {
        var upToForward = Quaternion.Euler(90f, 0f, 0f);
        var forwardToTarget = Quaternion.LookRotation(right, up);

        return forwardToTarget * upToForward;
    }

    /// Get the vertex index corresponding to the vertex on the next
    /// position on the Ring.
    private int RingModulo(int i, int nVertices)
    {
        return ((i + 1) % nVertices) + (i - (i % nVertices));
    }
}