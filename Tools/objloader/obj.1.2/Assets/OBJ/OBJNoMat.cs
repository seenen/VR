using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using libObj2Buffer;

public class OBJNoMat : MonoBehaviour
{
	public string objPath = "http://www.everyday3d.com/unity3d/obj/monkey.obj";

	//private GeometryBuffer buffer;
	private libObj2Buffer.GeometryBuffer buffer;

    public void LoadeContent(string text)
    {
        long before = System.DateTime.Now.Ticks;

        //SetGeometryData(text);
        buffer = libObj2Buffer.Buffer.ObjContent2Buffer(text);

        Debug.Log("SetGeometryData " + (System.DateTime.Now.Ticks - before) / 10000000.0);

        before = System.DateTime.Now.Ticks;

        Build();

        Debug.Log("Build " + (System.DateTime.Now.Ticks - before) / 10000000.0);
    }

    private void Build()
    {
        GameObject[] ms = new GameObject[buffer.numObjects];
		
		if ( buffer.numObjects == 1 )
        {
			gameObject.AddComponent(typeof(MeshFilter));
			gameObject.AddComponent(typeof(MeshRenderer));
			ms[0] = gameObject;
		}
        else if ( buffer.numObjects > 1 )
        {
			for ( int i = 0; i < buffer.numObjects; i++ )
            {
				GameObject go = new GameObject();
				go.transform.parent = gameObject.transform;
				go.AddComponent(typeof(MeshFilter));
				go.AddComponent(typeof(MeshRenderer));
				ms[i] = go;
			}
		}
		
		buffer.PopulateMeshes(ms, null);
	}

}