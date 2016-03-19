using UnityEngine;
using System.Collections;
using System.IO;

public class LoadObjArray : MonoBehaviour
{
    public OBJNoMat mOBJNoMat = null;

    ArrayList mlistObjs = new ArrayList();


    void Start()
    {
    }

    void OnGUI()
    {
        // 预加载obj文件
        if (GUI.Button(new Rect(480 - 50, 0, 50, 50), "RreLoadObj"))
        {
            long before = System.DateTime.Now.Ticks;

            for ( int i = 1; i < 2; i++)
            {
                //string path = "F:/GitHub/VR/Tools/stl2obj/Resources/BatStl/" + i.ToString() + "_dannang_after.obj";
                string path = "F:/GitHub/VR/Tools/stl2obj/Resources/DataFileObj/" + i.ToString() + ".obj";

                GeometryBuffer buff = mOBJNoMat.LoadeContent(File.ReadAllText(path));

                bool ret = mOBJNoMat.BuildNoMesh(buff);

                Debug.Log(i + "[flag:] " + ret + " -> [V:] " + buff.vertices.Count + " [T:] " + buff.triangles.Length);

                mlistObjs.Add(buff);

            }

            Debug.Log("RreLoadObj 16 " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        }

        //  预生成Mesh
        if (GUI.Button(new Rect(480 - 50, 100, 50, 50), "Init"))
        {
            Init();
        }

        //  预生成Mesh
        if (GUI.Button(new Rect(480 - 50, 150, 50, 50), "Render"))
        {
            StartCoroutine(Array1());
        }
    }

    void Init()
    {
        mOBJNoMat.Build((GeometryBuffer)mlistObjs[0]);

        mObjMeshDisplay.UpdateGizmos((GeometryBuffer)mlistObjs[0]);
    }

    public ObjMeshDisplay mObjMeshDisplay = null;

    IEnumerator Array1()
    {
        yield return 1;

        int index = 2;

        //do
        //{
            GeometryBuffer content = (GeometryBuffer)mlistObjs[0];

            mOBJNoMat.DeformationMT(content);

            index++;

            if (index == 36) index = 2;

            yield return new WaitForSeconds(0.33f);

        //} while (false);

    }
}
