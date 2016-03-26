using UnityEngine;
using System.Collections;

public class SmoothSample : MonoBehaviour
{
    public GameObject sourceGO;

    public Mesh sourceMesh;
    public Mesh workMesh;

    Smooth s;

    // Use this for initialization
    void Start ()
    {
        MeshFilter mf = (MeshFilter)sourceGO.GetComponent(typeof(MeshFilter));

        sourceMesh = new Mesh();
        sourceMesh = mf.mesh;

        workMesh = CloneMesh(sourceMesh);
        mf.mesh = workMesh;

        long before = System.DateTime.Now.Ticks;

        s = new Smooth(workMesh);

        Vector3[] smoothv = s.Exe_Mesh();

        workMesh.vertices = smoothv;

        Debug.Log("Smooth " + (System.DateTime.Now.Ticks - before) / 10000000.0);

        SmoothGameObject(workMesh, gameObject);

        OpInit();
    }

    void SmoothGameObject(Mesh workMesh, GameObject root)
    {
        GameObject obj = new GameObject("SmoothMesh");
        MeshFilter mf = (MeshFilter)obj.AddComponent<MeshFilter>();
        MeshRenderer mr = (MeshRenderer)obj.AddComponent<MeshRenderer>();

        mf.mesh = workMesh;

        mf.mesh.RecalculateBounds();
        mf.mesh.RecalculateNormals();

        obj.transform.parent = root.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = new Vector3(270, 0, 0);
    }

    // Update is called once per frame
    void Update () {
	
	}

    // Clone a mesh
    private static Mesh CloneMesh(Mesh mesh)
    {
        Mesh clone = new Mesh();
        clone.vertices = mesh.vertices;
        clone.normals = mesh.normals;
        clone.tangents = mesh.tangents;
        clone.triangles = mesh.triangles;
        clone.uv = mesh.uv;
        //clone.uv1 = mesh.uv1;
        clone.uv2 = mesh.uv2;
        clone.uv3 = mesh.uv3;
        clone.uv4 = mesh.uv4;
        clone.bindposes = mesh.bindposes;
        clone.boneWeights = mesh.boneWeights;
        clone.bounds = mesh.bounds;
        clone.colors = mesh.colors;
        clone.name = mesh.name;
        //TODO : Are we missing anything?
        return clone;
    }

    //private static Mesh CloneMesh(Mesh mesh)
    //{
    //    Mesh clone = new Mesh();
    //    clone.vertices = mesh.vertices;
    //    clone.normals = (Vector3[])mesh.normals.Clone();
    //    clone.tangents = (Vector4[])mesh.tangents.Clone();
    //    clone.triangles = (int[])mesh.triangles.Clone();
    //    clone.uv = (Vector2[])mesh.uv;
    //    //clone.uv1 = mesh.uv1;
    //    clone.uv2 = (Vector2[])mesh.uv2.Clone();
    //    clone.uv3 = (Vector2[])mesh.uv3.Clone();
    //    clone.uv4 = (Vector2[])mesh.uv4.Clone();
    //    clone.bindposes = (Matrix4x4[])mesh.bindposes.Clone();
    //    clone.boneWeights = (BoneWeight[])mesh.boneWeights.Clone();
    //    clone.bounds = mesh.bounds;
    //    clone.colors = (Color[])mesh.colors.Clone();
    //    clone.name = mesh.name;
    //    //TODO : Are we missing anything?
    //    return clone;
    //}
    #region 可视化
    
    void OpInit()
    {
        OperateMono mOperateMono = null;

        mOperateMono = GameObject.Find("OperateMono").GetComponent<OperateMono>();

        mOperateMono.mSmooth = s;
    }

    void InitData()
    {

    }

    #endregion
}
