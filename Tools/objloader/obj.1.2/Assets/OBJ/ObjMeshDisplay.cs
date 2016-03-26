using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ObjMeshDisplay : MonoBehaviour
{
    public bool showNormals = true;
    public bool showTangents = true;
    public float displayLengthScale = 1.0f;

    public Color normalColor = Color.red;
    public Color tangentColor = Color.blue;

    //void OnDrawGizmosSelected()
    void OnDrawGizmos()
    {
        if (triangles == null ||
            normals == null ||
            tangents == null ||
            vertices == null)
            return;

        bool doShowNormals = showNormals && normals.Length == vertices.Length;
        bool doShowTangents = showTangents && tangents.Length == vertices.Length;

        foreach (int idx in triangles)
        {
            Vector3 vertex = transform.TransformPoint(vertices[idx]);

            if (doShowNormals)
            {
                Vector3 normal = transform.TransformDirection(normals[idx]);
                Gizmos.color = normalColor;
                Gizmos.DrawLine(vertex, vertex + normal * displayLengthScale);
            }
            if (doShowTangents)
            {
                Vector3 tangent = transform.TransformDirection(tangents[idx]);
                Gizmos.color = tangentColor;
                Gizmos.DrawLine(vertex, vertex + tangent * displayLengthScale);
            }
        }
    }

    public int[] triangles;
    public Vector3[] normals;
    public Vector4[] tangents;
    public Vector3[] vertices;
    public int triangles_count;
    public int normals_count;
    public int tangents_count;
    public int vertices_count;

    /// <summary>
    /// 顶点填充
    /// </summary>
    /// <param name="content"></param>
    public void UpdateGizmos(GeometryBuffer content)
    {
        triangles = content.triangles;
        normals = content.normals.ToArray();
        vertices = content.vertices.ToArray();

        triangles_count = triangles.Length;
        normals_count = normals.Length;
        tangents_count = 0;
        vertices_count = tangents.Length;

        Debug.Log("[triangles:] " + triangles.Length + "[normals:] " + normals.Length + "[vertices:] " + vertices.Length);
    }

    public bool bUpdateGizmos = false;

    /// <summary>
    /// 刷新显示
    /// </summary>
    void UpdateMesh()
    {
        if (!bUpdateGizmos)
            return;

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null)
        {
            Debug.LogWarning("Cannot find MeshFilter");
            return;
        }
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null)
        {
            Debug.LogWarning("Cannot find mesh");
            return;
        }

        triangles = meshFilter.mesh.triangles;
        normals = meshFilter.mesh.normals;
        vertices = meshFilter.mesh.vertices;
        tangents = meshFilter.mesh.tangents;

        Debug.Log("[triangles:] " + triangles.Length +
                    "[normals:] " + normals.Length +
                    "[vertices:] " + vertices.Length +
                    "[tangents:] " + tangents.Length);

        triangles_count = triangles.Length;
        normals_count = normals.Length;
        tangents_count = tangents.Length;
        vertices_count = vertices.Length;

        bUpdateGizmos = false;
    }

    [ExecuteInEditMode]
    public void Update()
    {
        UpdateMesh();
    }
}
