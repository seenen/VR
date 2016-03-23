//#define use_buff
#define use_mesh

using UnityEngine;
using System.Collections;
using System.IO;

public class LoadObjArray : MonoBehaviour
{
    public OBJNoMat mOBJNoMat = null;

#if use_buff
    ArrayList mlistObjs = new ArrayList();
#endif

#if use_mesh
    ArrayList mListMeshs = new ArrayList();
#endif

    int MAX_COUNT = 1;

    void Start()
    {
    }

    void OnGUI()
    {
        // 预加载obj文件
        if (GUI.Button(new Rect(0, 0, 50, 50), "RreLoadObj"))
        {
            long before = System.DateTime.Now.Ticks;

#if use_buff
            for ( int i = 1; i < MAX_COUNT + 1; i++)
            {
                string path = "F:/GitHub/VR/Tools/stl2obj/Resources/DataFileObj/" + i.ToString() + ".obj";

                GeometryBuffer buff = mOBJNoMat.LoadeContent(File.ReadAllText(path));

                bool ret = mOBJNoMat.BuildNoMesh(buff);

                Debug.Log(i + "[flag:] " + ret + " -> [V:] " + buff.vertices.Count + " [T:] " + buff.triangles.Length);

                mlistObjs.Add(buff);

            }

            Debug.Log("RreLoadObj 16 " + (System.DateTime.Now.Ticks - before) / 10000000.0);
#endif

#if use_mesh
            for ( int i = 1; i < MAX_COUNT + 1; i++)
            {
                string path = "F:/GitHub/VR/Tools/stl2obj/Resources/DataFileObj/" + i.ToString() + ".obj";

                GeometryBuffer buff = mOBJNoMat.LoadeContent(File.ReadAllText(path));

                Mesh sourceMesh = mOBJNoMat.BuildWithMesh(buff);

                //  工作网格
                Mesh workingMesh = CloneMesh(sourceMesh);

                workingMesh.vertices = SmoothFilter.laplacianFilter( workingMesh.vertices, workingMesh.triangles );

                Debug.Log(i + "[flag:] " + sourceMesh + " -> [V:] " + buff.vertices.Count + " [T:] " + buff.triangles.Length);

                mListMeshs.Add(workingMesh);

            }

            Debug.Log("BuildWithMesh 16 " + (System.DateTime.Now.Ticks - before) / 10000000.0);
#endif

        }

        //  预生成Mesh
        if (GUI.Button(new Rect(0, 100, 50, 50), "Init"))
        {
            Init();
        }

        //  预生成Mesh
        if (GUI.Button(new Rect(0, 150, 50, 50), "Render"))
        {
            StartCoroutine(Array1());
        }
    }

    void Init()
    {
#if use_buff
        GeometryBuffer gb = (GeometryBuffer)mlistObjs[0];

        mOBJNoMat.Build(gb);

        mObjMeshDisplay.UpdateGizmos(gb);
#endif

#if use_mesh
        Mesh m = (Mesh)mListMeshs[0];

        mOBJNoMat.Build(m);
#endif
    }

    public ObjMeshDisplay mObjMeshDisplay = null;

    public int index = 1;
    IEnumerator Array1()
    {
        yield return 1;

#if use_buff
        do
        {
            GeometryBuffer content = (GeometryBuffer)mlistObjs[index];

            mOBJNoMat.DeformationMT(content);

            index++;

            if (index == MAX_COUNT) index = 0;

            yield return new WaitForSeconds(2);
            //yield return new WaitForSeconds(0.010f);

        } while (true);
#endif

#if use_mesh
        do
        {
            Mesh m = (Mesh)mListMeshs[index];

            mOBJNoMat.Build(m);

            index++;

            if (index == MAX_COUNT) index = 0;

            yield return new WaitForSeconds(0.01f);

        } while (true);

#endif

    }


    // Clone a mesh
    private Mesh CloneMesh(Mesh mesh)
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
