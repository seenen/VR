using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Smooth : IDisposable
{
    Mesh objMesh = null;

    Vector3[] retVec3;

    public _Mesh smoothMesh = null;

    public Smooth(Mesh mesh)
    {
        objMesh = mesh;
    }

    public void Dispose()
    {
        if (smoothMesh != null)
        {
            smoothMesh.Dispose();
            smoothMesh = null;

        }
    }

    public Vector3[] Exe()
    {
        retVec3 = new Vector3[objMesh.vertexCount];

        long before = System.DateTime.Now.Ticks;

        Mesh2_Mesh();

        Debug.Log("Mesh2_Mesh " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        before = System.DateTime.Now.Ticks;

        //PlyManager.Output(smoothMesh, "E:\\engine.ply");
        //ObjManager.Output(smoothMesh, "E:\\engine.obj");

        //XJBG();

        Debug.Log("XJBG " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        before = System.DateTime.Now.Ticks;

        _MeshLaplacian();
        //_ScaleDependentLaplacian(1);
        //Taubin(5, 0.5f, -0.5f);

        //PlyManager.Output(smoothMesh, "E:\\engine_smooth.ply");

        _Mesh2Mesh();

        Debug.Log("_Mesh2Mesh " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        before = System.DateTime.Now.Ticks;

        //ObjManager.Output(objMesh, "E:\\engine_smooth.obj");

        return retVec3;
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

            retVec3[i] = objMesh.vertices[i];

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

    void XJBG()
    {
        _Vector3[] tempList = new _Vector3[smoothMesh.Vertices.Count];
        for (int i = 0; i < smoothMesh.Vertices.Count; i++)
        {
            tempList[i] = smoothMesh.Vertices[i];

            tempList[i].X += UnityEngine.Random.Range(-0.5f, 0.5f);
            tempList[i].Y += UnityEngine.Random.Range(-0.5f, 0.5f);
            tempList[i].Z += UnityEngine.Random.Range(-0.5f, 0.5f);
        }
        for (int i = 0; i < smoothMesh.Vertices.Count; i++)
        {
            smoothMesh.Vertices[i] = tempList[i];
        }
        tempList = null;
    }

    /// <summary>
    /// 拉普拉斯算法
    /// </summary>
    void _Mesh2Mesh()
    {
        for (int i = 0; i < smoothMesh.Vertices.Count; ++i)
        {
            _Vector3 _sv = smoothMesh.Vertices[i];

            //Debug.Log(objMesh.vertices[i].ToString() + " -> " + _sv.X + " " + _sv.Y + " " + _sv.Z);

            retVec3[i].x = _sv.X;
            retVec3[i].y = _sv.Y;
            retVec3[i].z = _sv.Z;
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
            //  生成对象
            GameObject begin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            begin.transform.position = new Vector3(smoothMesh.Vertices[i].X, smoothMesh.Vertices[i].Y, smoothMesh.Vertices[i].Z);
            begin.transform.localScale = Vector3.one * 0.1f;
            ((MeshRenderer)begin.GetComponent<MeshRenderer>()).material = MaterialManager.GetBeginMat();

            PointMono pm = begin.AddComponent<PointMono>();
            pm.index = i;

            smoothMesh.Vertices[i] = tempList[i];

            ////GameObject end = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ////end.transform.position = new Vector3(tempList[i].X, tempList[i].Y, tempList[i].Z);
            ////end.transform.localScale = Vector3.one * 0.05f;
            ////((MeshRenderer)end.GetComponent<MeshRenderer>()).material = MaterialManager.GetEndMat();

            //////Debug.DrawLine(begin.transform.position, end.transform.position);

            ////LineRenderer mLine = begin.AddComponent<LineRenderer>();
            ////mLine.SetWidth(0.1f, 0.01f);
            ////mLine.SetVertexCount(2);
            ////mLine.SetColors(Color.yellow, Color.yellow);
            ////mLine.material = MaterialManager.GetEndMat();
            ////mLine.SetPosition(0, begin.transform.position);
            ////mLine.SetPosition(1, end.transform.position);
            ////((MeshRenderer)mLine.GetComponent<MeshRenderer>()).enabled = true;
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

    #region Taubin
    void Taubin(int iterationTime, float lambda, float mu)
    {
        _Vector3[] tempList = new _Vector3[smoothMesh.Vertices.Count];
        for (int c = 0; c < iterationTime; c++)
        {
            for (int i = 0; i < smoothMesh.Vertices.Count; i++)
            {
                tempList[i] = GetSmoothedVertex_Taubin_Step(i, lambda);
            }
            for (int i = 0; i < smoothMesh.Vertices.Count; i++)
            {
                smoothMesh.Vertices[i] = tempList[i];
            }
            for (int i = 0; i < smoothMesh.Vertices.Count; i++)
            {
                tempList[i] = GetSmoothedVertex_Taubin_Step(i, mu);
            }
            for (int i = 0; i < smoothMesh.Vertices.Count; i++)
            {
                smoothMesh.Vertices[i] = tempList[i];
            }
        }
        tempList = null;
    }

    _Vector3 GetSmoothedVertex_Taubin_Step(int index, float lambda)
    {
        float dx = 0, dy = 0, dz = 0;
        List<long> adjVertices = smoothMesh.AdjInfos[index].VertexAdjacencyList;
        _Vector3 p  = smoothMesh.Vertices[index];
        if (adjVertices.Count == 0)
            return smoothMesh.Vertices[index];
        for (int i = 0; i < adjVertices.Count; i++)
        {
            _Vector3 t = smoothMesh.Vertices[(int)adjVertices[i]];
            dx += (t.X - p.X);
            dy += (t.Y - p.Y);
            dz += (t.Z - p.Z);
        }
        dx /= adjVertices.Count;
        dy /= adjVertices.Count;
        dz /= adjVertices.Count;
        float newx = lambda * dx + p.X;
        float newy = lambda * dy + p.Y;
        float newz = lambda * dz + p.Z;

        return new _Vector3(newx, newy, newz);
    }
    #endregion
}
