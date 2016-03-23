using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Smooth
{
    Mesh objMesh = null;

    public Smooth(Mesh mesh)
    {
        objMesh = mesh;
    }

    public Mesh Exe()
    {
        Mesh2_Mesh();

        PlyManager.Output(smoothMesh, "H:\\engine.ply");

        _MeshLaplacian();
        //_ScaleDependentLaplacian(3);

        PlyManager.Output(smoothMesh, "H:\\engine_smooth.ply");

        _Mesh2Mesh();

        return objMesh;
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

        smoothMesh.InitPerVertexVertexAdj();
    }

    void _Mesh2Mesh()
    {
        for (int i = 0; i < smoothMesh.Vertices.Count; ++i)
        {
            _Vector3 _sv = smoothMesh.Vertices[i];
            objMesh.vertices[i].x = _sv.X;
            objMesh.vertices[i].y = _sv.Y;
            objMesh.vertices[i].z = _sv.Z;
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
            tempList[i] = GetSmoothedVertex_Laplacian(i, 0.1f);
        }
        for (int i = 0; i < smoothMesh.Vertices.Count; i++)
        {
            smoothMesh.Vertices[i] = tempList[i];
        }
        tempList = null;
    }

    void _ScaleDependentLaplacian(int iterationTime)
    {
        _Vector3[] tempList = new _Vector3[smoothMesh.Vertices.Count];
        for (int c = 0; c < iterationTime; c++)
        {
            for (int i = 0; i < smoothMesh.Vertices.Count; i++)
            {
                tempList[i] = GetSmoothedVertex_ScaleDependentLaplacian(i);
            }
            for (int i = 0; i < smoothMesh.Vertices.Count; i++)
            {
                smoothMesh.Vertices[i] = tempList[i];
            }
        }
        tempList = null;
    }

    _Vector3 GetSmoothedVertex_ScaleDependentLaplacian(int index, float lambda = 1.0f)
    {
        float dx = 0, dy = 0, dz = 0;
        List<long> adjVertices = smoothMesh.AdjInfos[index].VertexAdjacencyList;
        _Vector3 p = smoothMesh.Vertices[index];
        if (adjVertices.Count == 0)
            return smoothMesh.Vertices[index];

        float sumweight = 0;
        for (int i = 0; i < adjVertices.Count; i++)
        {
            _Vector3 t = smoothMesh.Vertices[(int)adjVertices[i]];
            float distence = GetDistence(p, t);
            dx += (1.0f / distence) * (t.X - p.X);
            dy += (1.0f / distence) * (t.Y - p.Y);
            dz += (1.0f / distence) * (t.Z - p.Z);
            sumweight += (1.0f / distence);
        }
        dx /= sumweight;
        dy /= sumweight;
        dz /= sumweight;
        float newx = lambda * dx + p.X;
        float newy = lambda * dy + p.Y;
        float newz = lambda * dz + p.Z;
        return new _Vector3(newx, newy, newz);
    }

    float GetDistence(_Vector3 p1, _Vector3 p2)
    {
        return (float)Mathf.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y) + (p1.Z - p2.Z) * (p1.Z - p2.Z));
    }

    _Vector3 GetSmoothedVertex_Laplacian(int index, float lambda = 1.0f)
    {
        try
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
        catch(Exception e)
        {
            Debug.LogException(e);
        }

        return null;
    }


    _Mesh smoothMesh = null;
}
