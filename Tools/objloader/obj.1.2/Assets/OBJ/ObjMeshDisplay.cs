using UnityEngine;
using System.Collections;

public class ObjMeshDisplay : MonoBehaviour
{
    public bool showNormals = true;
    public bool showTangents = true;
    public float displayLengthScale = 1.0f;

    public Color normalColor = Color.red;
    public Color tangentColor = Color.blue;

    void OnDrawGizmosSelected()
    {
        if (triangles == null ||
            normals == null ||
            //tangents == null ||
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

    public void UpdateGizmos(GeometryBuffer content)
    {
        //if (meshFilter == null)
        //{
        //    Debug.LogWarning("Cannot find MeshFilter");
        //    return;
        //}
        //Mesh mesh = meshFilter.sharedMesh;
        //if (mesh == null)
        //{
        //    Debug.LogWarning("Cannot find mesh");
        //    return;
        //}

        triangles = content.triangles;
        normals = content.normals.ToArray();
        vertices = content.vertices.ToArray();

        Debug.Log("[triangles:] " + triangles.Length + "[normals:] " + normals.Length + "[vertices:] " + vertices.Length);
    }
}
