using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PointAttachmentInfo : System.IDisposable
{

    public PointAttachmentInfo()
    {
        VertexAdjacencyList = null;
        TriangleAdjacencyList = null;
    }

    public void Dispose()
    {
        ClearVertexAdj();
        ClearTriangleAdj();
    }

    public void ClearVertexAdj()
    {
        
        if (VertexAdjacencyList != null)
        {
            VertexAdjacencyList.Clear();
            VertexAdjacencyList = null;
        }
    }

    public void ClearTriangleAdj()
    {
        if (TriangleAdjacencyList != null)
        {
            TriangleAdjacencyList.Clear();
            VertexAdjacencyList = null;
        }
    }

    public List<long> VertexAdjacencyList;

    public List<long> TriangleAdjacencyList;
}
