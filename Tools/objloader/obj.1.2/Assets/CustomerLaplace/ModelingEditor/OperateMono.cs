using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OperateMono : MonoBehaviour {

    public PointMono Selected = null;

    public int SelectedVertexCount = 0;

    public bool bUpdate = false;

    public GameObject[] relationPoints = new GameObject[6];

    public Smooth mSmooth = null;

    // Use this for initialization
    void Start () {
	
	}

	// Update is called once per frame
	void Update ()
    {
        if (!bUpdate)
            return;

        bUpdate = false;

        //  删除之前的模型
        if (relationPoints != null)
        {
            foreach (GameObject p in relationPoints)
            {
                GameObject.Destroy(p);
            }
        }

        List<long> vertexList = mSmooth.smoothMesh.AdjInfos[Selected.index].VertexAdjacencyList;

        SelectedVertexCount = vertexList.Count;

        //  新建相关的点
        for (int i = 0; i < SelectedVertexCount; ++i)
        {
            long e = vertexList[i];    
            _Vector3 _v = mSmooth.smoothMesh.Vertices[(int)e];

            GameObject begin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            begin.transform.position = new Vector3(_v.X, _v.Y, _v.Z);
            begin.transform.localScale = Vector3.one * 0.05f;
            //((MeshRenderer)begin.GetComponent<MeshRenderer>()).material = MaterialManager.GetBeginMat();

            relationPoints[i] = begin;
        }

    }
}
