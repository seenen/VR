#define usedll

using UnityEngine;
using System;
using System.Globalization;

public class OBJNoMat : MonoBehaviour
{
	public string objPath = "http://www.everyday3d.com/unity3d/obj/monkey.obj";

#if usedll
    private libObj2Buffer.GeometryBuffer buffer;
#else
    private const string O = "o";
    private const string G = "g";
    private const string V = "v";
    private const string VT = "vt";
    private const string VN = "vn";
    private const string F = "f";
    private const string MTL = "mtllib";
    private const string UML = "usemtl";
    private string mtllib;

    private GeometryBuffer buffer;
#endif

    public void LoadeContent(string text)
    {
        long before = System.DateTime.Now.Ticks;

#if usedll
        buffer = libObj2Buffer.Buffer.ObjContent2Buffer(text);
#else
        buffer = new GeometryBuffer();

        SetGeometryData(text);
#endif

        Debug.Log("SetGeometryData " + (System.DateTime.Now.Ticks - before) / 10000000.0);

        before = System.DateTime.Now.Ticks;

        Build();

        Debug.Log("Build " + (System.DateTime.Now.Ticks - before) / 10000000.0);
    }

    public void Clear()
    {

    }

    public MeshFilter mMeshFilter = null;
    public GameObject[] ms = null;

    private void Build()
    {
        if (mMeshFilter == null)
        {
            ms = new GameObject[buffer.numObjects];
		
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
                    mMeshFilter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
				    go.AddComponent(typeof(MeshRenderer));
				    ms[i] = go;
			    }
		    }
        }
		
		buffer.PopulateMeshes(ms, null);
	}

#if !usedll
    private void SetGeometryData(string data)
    {
        string[] lines = data.Split("\n".ToCharArray());

        for (int i = 0; i < lines.Length; i++)
        {
            string l = lines[i];

            if (l.IndexOf("#") != -1) l = l.Substring(0, l.IndexOf("#"));
            string[] p = l.Split(" ".ToCharArray());

            switch (p[0])
            {
                case O:
                    buffer.PushObject(p[1].Trim());
                    break;
                case G:
                    buffer.PushGroup(p[1].Trim());
                    break;
                case V:
                    buffer.PushVertex(new Vector3(cf(p[1]), cf(p[2]), cf(p[3])));
                    break;
                case VT:
                    buffer.PushUV(new Vector2(cf(p[1]), cf(p[2])));
                    break;
                case VN:
                    buffer.PushNormal(new Vector3(cf(p[1]), cf(p[2]), cf(p[3])));
                    break;
                case F:
                    for (int j = 1; j < p.Length; j++)
                    {
                        string[] c = p[j].Trim().Split("/".ToCharArray());
                        FaceIndices fi = new FaceIndices();
                        fi.vi = ci(c[0]) - 1;
                        if (c.Length > 1 && c[1] != "") fi.vu = ci(c[1]) - 1;
                        if (c.Length > 2 && c[2] != "") fi.vn = ci(c[2]) - 1;
                        buffer.PushFace(fi);
                    }
                    break;
                case MTL:
                    mtllib = p[1].Trim();
                    break;
                case UML:
                    buffer.PushMaterialName(p[1].Trim());
                    break;
            }
        }

        // buffer.Trace();
    }

    private float cf(string v)
    {
        return Convert.ToSingle(v.Trim(), new CultureInfo("en-US"));
    }

    private int ci(string v)
    {
        return Convert.ToInt32(v.Trim(), new CultureInfo("en-US"));
    }

#endif

}