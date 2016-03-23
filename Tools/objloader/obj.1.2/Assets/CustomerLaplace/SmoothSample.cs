using UnityEngine;
using System.Collections;

public class SmoothSample : MonoBehaviour
{

    public GameObject sourceGO;

	// Use this for initialization
	void Start ()
    {
        MeshFilter mf = (MeshFilter)sourceGO.GetComponent(typeof(MeshFilter));

        Mesh workMesh = CloneMesh(mf.mesh);

        Smooth s = new Smooth(workMesh);

        Mesh smoothMesh = s.Exe();

        mf.mesh = smoothMesh;

        mf.mesh.RecalculateBounds();
        mf.mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update () {
	
	}

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

}
