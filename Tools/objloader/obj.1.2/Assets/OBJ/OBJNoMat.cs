//#define usedll

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

#endif

#if usedll
    public libObj2Buffer.GeometryBuffer LoadeContent(string text)
#else
    public GeometryBuffer LoadeContent(string text)
#endif
    {
        long before = System.DateTime.Now.Ticks;

#if usedll
        libObj2Buffer.GeometryBuffer buffer;
#else
        GeometryBuffer buffer;
#endif

#if usedll
        buffer = libObj2Buffer.Buffer.ObjContent2Buffer(text);
#else
        buffer = new GeometryBuffer();

        SetGeometryData(buffer, text);
#endif

        //Debug.Log("SetGeometryData " + (System.DateTime.Now.Ticks - before) / 10000000.0);

        before = System.DateTime.Now.Ticks;

        //Debug.Log("Build " + (System.DateTime.Now.Ticks - before) / 10000000.0);

        return buffer;
    }

    public MeshFilter mMeshFilter = null;
    public MeshRenderer mMeshRenderer = null;
    public Material mMaterial = null;
    public GameObject[] ms = null;

    //  创建Mesh并渲染
    public void Build(GeometryBuffer buffer)
    {
        if (mMeshFilter == null)
        {
            ms = new GameObject[buffer.numObjects];
		
		    if ( buffer.numObjects == 1 )
            {
                mMeshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
                mMeshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
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
		
		
        //buffer.PopulateSmoothMeshes(ms, null);

        mMeshFilter.mesh = buffer.PopulateMeshes(ms, null);

        Debug.Log(  "Mesh -> [vertexCount:] " + mMeshFilter.mesh.vertexCount + 
                    " [triangles:] " + mMeshFilter.mesh.triangles.Length +
                    " [normals:] " + mMeshFilter.mesh.normals.Length);

        mMeshRenderer.material = mMaterial;

        mMeshFilter.mesh.RecalculateNormals();
        mMeshFilter.mesh.RecalculateBounds();

    }

    /// <summary>
    /// 渲染Mesh
    /// </summary>
    /// <param name="mesh"></param>
    public void Build(Mesh mesh)
    {
        if (mMeshFilter == null)
        {
            mMeshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));

            mMeshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
            mMeshRenderer.material = mMaterial;
        }

        mMeshFilter.mesh = mesh;

        mMeshFilter.mesh.RecalculateNormals();
        mMeshFilter.mesh.RecalculateBounds();
    }

    public bool BuildNoMesh(GeometryBuffer buffer)
    {
        if (mMeshFilter == null)
        {
            ms = new GameObject[buffer.numObjects];

            if (buffer.numObjects == 1)
            {
                mMeshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));
                mMeshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
                ms[0] = gameObject;
            }
            else if (buffer.numObjects > 1)
            {
                for (int i = 0; i < buffer.numObjects; i++)
                {
                    GameObject go = new GameObject();
                    go.transform.parent = gameObject.transform;
                    mMeshFilter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
                    go.AddComponent(typeof(MeshRenderer));
                    ms[i] = go;
                }
            }
        }

        return buffer.PopulateMeshesNoMesh(ms, null);
    }

    /// <summary>
    /// 创建带Mesh的对象,只针对单个物体
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public Mesh BuildWithMesh(GeometryBuffer buffer, bool Smooth = false)
    {
        GameObject meshGo = new GameObject();

        GameObject[] ms = new GameObject[buffer.numObjects];

        //if (mMeshFilter == null)
        {
            //ms = new GameObject[buffer.numObjects];

            if (buffer.numObjects == 1)
            {
                meshGo.AddComponent(typeof(MeshFilter));
                meshGo.AddComponent(typeof(MeshRenderer));
                ms[0] = meshGo;
            }
            else if (buffer.numObjects > 1)
            {
                for (int i = 0; i < buffer.numObjects; i++)
                {
                    GameObject go = new GameObject();
                    go.transform.parent = meshGo.transform;
                    go.AddComponent(typeof(MeshFilter));
                    go.AddComponent(typeof(MeshRenderer));
                    ms[i] = go;
                }
            }
        }

        return buffer.PopulateMeshes(ms, null, Smooth);
    }

    public static int Cores = 0;	//SystemInfo.processorCount;	// 0 is not mt

    /// <summary>
    /// 变形推送
    /// </summary>
    /// <param name="bufferCache"></param>
    public void DeformationMT(GeometryBuffer bufferCache)
    {
        if (Cores == 0)
            Cores = SystemInfo.processorCount - 1;

        //Vector3[] vertices = SmoothFilter.laplacianFilter(bufferCache.tvertices, bufferCache.triangles);

        mMeshFilter.mesh.vertices = bufferCache.vertices.ToArray();
        mMeshFilter.mesh.triangles = bufferCache.triangles;
        //mMeshFilter.mesh.uv = bufferCache.uvs.ToArray();
        //mMeshFilter.mesh.normals = bufferCache.normals.ToArray();
        mMeshFilter.mesh.RecalculateNormals();
        mMeshFilter.mesh.RecalculateBounds();
    }

#if !usedll
    private void SetGeometryData(GeometryBuffer buffer, string data)
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