using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Smooth
{
    Mesh objMesh = null;

    public Smooth(Mesh mesh)
    {
        objMesh = mesh;
    }

    public void Exe()
    {
        Mesh2_Mesh();
        _MeshLaplacian();
        _Mesh2Mesh();
    }

    void Mesh2_Mesh()
    {
        smoothMesh = new _Mesh();

        Debug.Log("Mesh V: " + objMesh.vertexCount + " T: " + objMesh.triangles.Length);

        /// 顶点转换
        for (int i = 0; i < objMesh.vertexCount; ++i )
        {
            _Vector3 _v = new _Vector3();
            _v.X = objMesh.vertices[i].x;
            _v.Y = objMesh.vertices[i].y;
            _v.Z = objMesh.vertices[i].z;

            smoothMesh.AddVertex(_v);
        }

        /// 顶点索引
        for (int i = 0; i < objMesh.triangles.Length;  )
        {
            _Triangle _t = new _Triangle();
            _t.P0Index = objMesh.triangles[i];
            _t.P1Index = objMesh.triangles[i+1];
            _t.P2Index = objMesh.triangles[i+2];

            smoothMesh.AddFace(_t);

            i += 3;
        }
    }

    void _Mesh2Mesh()
    {
        for (int i = 0; i < smoothMesh.Vertices.Count; ++i)
        {
            objMesh.vertices[i].x = smoothMesh.Vertices[i].X;
            objMesh.vertices[i].y = smoothMesh.Vertices[i].Y;
            objMesh.vertices[i].z = smoothMesh.Vertices[i].Z;
        }
    }

    /// <summary>
    /// Laplacians this instance.
    /// </summary>
    void _MeshLaplacian()
    {
        _Vector3[] tempList = new _Vector3[smoothMesh.Vertices.Count];
        for (int i = 0; i < smoothMesh.Vertices.Count; i++)
        {
            tempList[i] = GetSmoothedVertex_Laplacian(i);
        }
        for (int i = 0; i < smoothMesh.Vertices.Count; i++)
        {
            smoothMesh.Vertices[i] = tempList[i];
        }
        tempList = null;
    }

    _Vector3 GetSmoothedVertex_Laplacian(int index, float lambda = 1.0f)
    {
        float nx = 0, ny = 0, nz = 0;

        List<long> adjVertices = smoothMesh.AdjInfos[index].VertexAdjacencyList;
        if (adjVertices.Count == 0)
            return smoothMesh.Vertices[index];

        _Vector3  P = smoothMesh.Vertices[index];
        for (int i = 0; i < adjVertices.Count; i++)
        {
            nx += smoothMesh.Vertices[(int)adjVertices[i]].X;
            ny += smoothMesh.Vertices[(int)adjVertices[i]].Y;
            nz += smoothMesh.Vertices[(int)adjVertices[i]].Z;
        }
        nx /= adjVertices.Count;
        ny /= adjVertices.Count;
        nz /= adjVertices.Count;
        float newx = P.X + lambda * (nx - P.X);
        float newy = P.Y + lambda * (ny - P.Y);
        float newz = P.Z + lambda * (nz - P.Z);

        return new _Vector3(newx, newy, newz);
    }


    _Mesh smoothMesh = null;
}
