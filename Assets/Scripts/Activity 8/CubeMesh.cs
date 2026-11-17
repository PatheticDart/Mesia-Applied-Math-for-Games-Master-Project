using System;
using System.Collections.Generic;
using UnityEngine;

public class CubeMesh : MonoBehaviour
{
    public enum ShapeType { Cube, Pyramid, Sphere }

    #region Static Variables

    private Vector3[] verts;
    private int[][] faces;

    private readonly List<int> visibleEdges = new();

    #endregion

    [SerializeField] private ShapeType shape = ShapeType.Cube;
    [Range(6, 32)][SerializeField] private int sphereSegments = 12;

    [SerializeField] private Material lineMat;
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private float scale = 0.5f;
    [SerializeField] private float yRotation = 0;
    [SerializeField] private float xRotation = 0;

    [Range(1, 500)][SerializeField] private float focalLength = 2f;

    private Mesh mesh;
    private readonly List<Vector3> actualPosition = new();
    private ShapeType currentShape;
    private int currentSegments;

    #region Unity Functions

    private void Awake()
    {
        mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = lineMat;
        BuildShape();
    }

    void LateUpdate()
    {
        if (currentShape != shape || currentSegments != sphereSegments)
        {
            BuildShape();
        }

        actualPosition.Clear();
        foreach (var point in verts)
        {
            actualPosition.Add(Project(point));
        }

        mesh.vertices = actualPosition.ToArray();
        visibleEdges.Clear();

        foreach (var f in faces)
        {
            if (!FacesCamera(actualPosition[f[0]], actualPosition[f[1]], actualPosition[f[2]])) continue;
            for (int i = 0; i < f.Length; i++)
            {
                visibleEdges.Add(f[i]);
                visibleEdges.Add(f[(i + 1) % f.Length]);
            }
        }

        mesh.SetIndices(visibleEdges, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
    }

    #endregion

    #region Shape Generation

    private void BuildShape()
    {
        currentShape = shape;
        currentSegments = sphereSegments;

        if (mesh != null)
        {
            mesh.Clear();
        }

        switch (shape)
        {
            case ShapeType.Cube:
                verts = new Vector3[]
                {
                    new(-1, -1, -1), new(1, -1, -1),
                    new(1, 1, -1), new(-1, 1, -1),
                    new(-1, -1, 1), new(1, -1, 1),
                    new(1, 1, 1), new(-1, 1, 1)
                };

                faces = new int[][]
                {
                    new[] { 0, 1, 2, 3 }, new[] { 5, 4, 7, 6 }, new[] { 4, 0, 3, 7 },
                    new[] { 1, 5, 6, 2 }, new[] { 3, 2, 6, 7 }, new[] { 0, 4, 5, 1 }
                };
                break;

            case ShapeType.Pyramid:
                verts = new Vector3[]
                {
                    new(-1, -1, -1),
                    new(1, -1, -1),
                    new(1, -1, 1),
                    new(-1, -1, 1),
                    new(0, 1, 0)
                };

                faces = new int[][]
                {
                    new[] { 0, 1, 4 },
                    new[] { 1, 2, 4 },
                    new[] { 2, 3, 4 },
                    new[] { 3, 0, 4 },
                    new[] { 0, 3, 2, 1 }
                };
                break;

            case ShapeType.Sphere:
                int rings = sphereSegments;
                int segments = sphereSegments;

                List<Vector3> sphereVerts = new();
                List<int[]> sphereFaces = new();

                sphereVerts.Add(new Vector3(0, 1, 0));

                for (int r = 1; r < rings; r++)
                {
                    float phi = Mathf.PI * 0.5f - (Mathf.PI * r / rings);
                    float y = Mathf.Sin(phi);
                    float ringRadius = Mathf.Cos(phi);

                    for (int s = 0; s < segments; s++)
                    {
                        float theta = 2f * Mathf.PI * s / segments;
                        float x = ringRadius * Mathf.Cos(theta);
                        float z = ringRadius * Mathf.Sin(theta);
                        sphereVerts.Add(new Vector3(x, y, z));
                    }
                }

                int bottomIndex = sphereVerts.Count;
                sphereVerts.Add(new Vector3(0, -1, 0));

                for (int s = 0; s < segments; s++)
                {
                    int next = (s + 1) % segments;
                    sphereFaces.Add(new[] { 0, 1 + s, 1 + next });
                }

                for (int r = 0; r < rings - 2; r++)
                {
                    int rowStart = 1 + r * segments;
                    int nextRowStart = 1 + (r + 1) * segments;

                    for (int s = 0; s < segments; s++)
                    {
                        int next = (s + 1) % segments;
                        int a = rowStart + s;
                        int b = nextRowStart + s;
                        int c = nextRowStart + next;
                        int d = rowStart + next;
                        sphereFaces.Add(new[] { a, b, c, d });
                    }
                }

                int lastRowStart = 1 + (rings - 2) * segments;
                for (int s = 0; s < segments; s++)
                {
                    int next = (s + 1) % segments;
                    sphereFaces.Add(new[] { bottomIndex, lastRowStart + next, lastRowStart + s });
                }

                verts = sphereVerts.ToArray();
                faces = sphereFaces.ToArray();
                break;
        }
    }

    #endregion

    #region Helper

    private Vector2 Project(Vector3 point)
    {
        point *= .5f;

        var xRot = point.x * Mathf.Cos(yRotation * Mathf.Deg2Rad) - point.z * Mathf.Sin(yRotation * Mathf.Deg2Rad);
        var zRot = point.x * Mathf.Sin(yRotation * Mathf.Deg2Rad) + point.z * Mathf.Cos(yRotation * Mathf.Deg2Rad);
        point = new Vector3(xRot, point.y, zRot);

        var yRot = point.y * Mathf.Cos(xRotation * Mathf.Deg2Rad) - point.z * Mathf.Sin(xRotation * Mathf.Deg2Rad);
        var zRotX = point.y * Mathf.Sin(xRotation * Mathf.Deg2Rad) + point.z * Mathf.Cos(xRotation * Mathf.Deg2Rad);
        point = new Vector3(point.x, yRot, zRotX);

        point = new Vector3(point.x + position.x, point.y + position.y, point.z + position.z);

        var p = focalLength / (focalLength + point.z);

        return new Vector2(point.x, point.y) * scale * p;
    }

    private bool FacesCamera(Vector2 a, Vector2 b, Vector2 c)
    {
        return (b - a).x * (c - a).y - (b - a).y * (c - a).x > 0;
    }

    #endregion
}