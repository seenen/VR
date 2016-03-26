using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System;
using System.Linq;

public class _Mesh : System.IDisposable
{
    public void Dispose()
    {
        if (IsPerVertexTriangleInfoEnabled)
            ClearPerVertexTriangleAdj();
        if (IsPerVertexVertexInfoEnabled)
            ClearPerVertexVertexAdj();
    }

    public List<_Vector3> Vertices = new List<_Vector3>();
    public List<_Triangle> Faces = new List<_Triangle>();
    public List<PointAttachmentInfo> AdjInfos = new List<PointAttachmentInfo>();

    bool IsPerVertexVertexInfoEnabled;
    bool IsPerVertexTriangleInfoEnabled;

    public _Mesh()
    {
        IsPerVertexTriangleInfoEnabled=false;
        IsPerVertexVertexInfoEnabled=false;
    }

    public long AddVertex(_Vector3 toAdd)
    {
        long index = (long)Vertices.Count;
        Vertices.Add(toAdd);

        return index;
    }

    public long AddFace(_Triangle tri)
    {
        //Debug.Log("_Mesh.AddFace " + tri.P0Index + " " + tri.P1Index + " " + tri.P2Index);

        long index = (long)Faces.Count;
        Faces.Add(tri);

        return index;
    }

    public void InitPerVertexVertexAdj()
    {
        if (IsPerVertexVertexInfoEnabled)
            ClearPerVertexVertexAdj();

        IsPerVertexVertexInfoEnabled = true;

        for (int i = 0; i < Vertices.Count; ++i)
        {
            PointAttachmentInfo info = new PointAttachmentInfo();
            AdjInfos.Add(info);
        }

        //if (AdjInfos.Count != Vertices.Count)
        //    ListExtra.Resize<PointAttachmentInfo>(AdjInfos, Vertices.Count);

        int vcount = Vertices.Count;
        int fcount = Faces.Count;
        for (int i = 0; i < vcount; i++)
        {
            //  创建一个顶点预设列表.
            List<long> vertexAdjacencyList = new List<long>();
            if (vertexAdjacencyList == null) { return; }
            vertexAdjacencyList.Capacity = 6;
            AdjInfos[i].VertexAdjacencyList = vertexAdjacencyList;
        }

        for (int i = 0; i < fcount; i++)
        {
            //Debug.Log("Faces: " + i);

            //if (i == 719)
            //{
            //    Debug.Log("PPP");
            //}
            _Triangle t = Faces[i];
            int index0 = (int)t.P0Index;
            int index1 = (int)t.P1Index;
            int index2 = (int)t.P2Index;
            List<long> p0list = AdjInfos[index0].VertexAdjacencyList;
            List<long> p1list = AdjInfos[index1].VertexAdjacencyList;
            List<long> p2list = AdjInfos[index2].VertexAdjacencyList;

            if (p0list.IndexOf(t.P1Index) == -1)
                p0list.Add(t.P1Index);
            if (p0list.IndexOf(t.P2Index) == -1)
                p0list.Add(t.P2Index);

            if (p1list.IndexOf(t.P0Index) == -1)
                p1list.Add(t.P0Index);
            if (p1list.IndexOf(t.P2Index) == -1)
                p1list.Add(t.P2Index);

            if (p2list.IndexOf(t.P0Index) == -1)
                p2list.Add(t.P0Index);
            if (p2list.IndexOf(t.P1Index) == -1)
                p2list.Add(t.P1Index);

            AdjInfos[index0].VertexAdjacencyList = p0list;
            AdjInfos[index1].VertexAdjacencyList = p1list;
            AdjInfos[index2].VertexAdjacencyList = p2list;

            //Debug.Log(i + " 0 的链表数量 " + AdjInfos[0].VertexAdjacencyList.Count);
        }
    }

    public void InitPerVertexTriangleAdj()
    {
        if (IsPerVertexTriangleInfoEnabled)
            ClearPerVertexTriangleAdj();

        IsPerVertexTriangleInfoEnabled = true;

        for (int i = 0; i < Vertices.Count; ++i)
        {
            PointAttachmentInfo info = new PointAttachmentInfo();
            AdjInfos.Add(info);
        }

        //if (AdjInfos.Count != Vertices.Count)
        //    ListExtra.Resize<PointAttachmentInfo>(AdjInfos, Vertices.Count);

        for (int i = 0; i < Vertices.Count; i++)
        {
            List<long> triangleAdjacencyList = new List<long>();
            if (triangleAdjacencyList == null) { return; }
            triangleAdjacencyList.Capacity = 6;
            AdjInfos[i].TriangleAdjacencyList = triangleAdjacencyList;
        }
        for (int i = 0; i < Faces.Count; i++)
        {
            _Triangle t = Faces[i];
            List<long> t0list = AdjInfos[(int)t.P0Index].TriangleAdjacencyList;
            List<long> t1list = AdjInfos[(int)t.P1Index].TriangleAdjacencyList;
            List<long> t2list = AdjInfos[(int)t.P2Index].TriangleAdjacencyList;
            t0list.Add(i);
            t1list.Add(i);
            t2list.Add(i);
        }
    }

    public void ClearPerVertexVertexAdj()
    {
        try
        {
            if (!IsPerVertexVertexInfoEnabled)
                return;

            for (int i = 0; i < Vertices.Count; i++)
            {
                if (AdjInfos[i].VertexAdjacencyList != null)
                {
                    AdjInfos[i].VertexAdjacencyList.Clear();
                    AdjInfos[i].Dispose();
                    AdjInfos[i].VertexAdjacencyList = null;
                }
            }
            IsPerVertexVertexInfoEnabled = false;

        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void ClearPerVertexTriangleAdj()
    {
        if (!IsPerVertexTriangleInfoEnabled)
            return;

        for (int i = 0; i < Vertices.Count; i++)
        {
            if (AdjInfos[i].TriangleAdjacencyList != null)
            {
                AdjInfos[i].TriangleAdjacencyList.Clear();
                AdjInfos[i].TriangleAdjacencyList = null;
            }
        }
        IsPerVertexTriangleInfoEnabled = false;
    }

    public bool GetIsPerVertexVertexInfoEnabled()
    {
        return IsPerVertexVertexInfoEnabled;
    }

    public bool GetIsPerVertexTriangleInfoEnabled()
    {
        return IsPerVertexTriangleInfoEnabled;
    }
}

public static class ListExtra
{
    public static void Resize<T>(this List<T> list, int sz, T c)
    {
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = sz;
            list.AddRange(Enumerable.Repeat(c, sz - cur));
        }
    }
    public static void Resize<T>(this List<T> list, int sz) where T : new()
    {
        Resize(list, sz, new T());
    }
}