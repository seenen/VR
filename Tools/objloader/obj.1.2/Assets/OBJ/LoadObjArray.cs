using UnityEngine;
using System.Collections;
using System.IO;

public class LoadObjArray : MonoBehaviour
{
    public OBJNoMat mOBJNoMat = null;

    ArrayList mlistObjs = new ArrayList();

    int index = 0;

    void Start()
    {
        index = 0;
    }

    void OnGUI()
    {
        // 预加载obj文件
        if (GUI.Button(new Rect(480 - 50, 0, 50, 50), "RreLoadObj"))
        {
            long before = System.DateTime.Now.Ticks;

            for ( int i = 0; i < 16; i++)
            {
                string path = "F:/GitHub/VR/Tools/stl2obj/Resources/BatStl/" + i.ToString() + "_dannang_after.obj";

                mlistObjs.Add(File.ReadAllText(path));
            }

            Debug.Log("RreLoadObj 16 " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        }

        //  预生成Mesh
        if (GUI.Button(new Rect(480 - 50, 100, 50, 50), "Render"))
        {
            StartCoroutine(Array1());
        }
    }

    IEnumerator Array1()
    {
        yield return 1;

        do
        {
            string content = (string)mlistObjs[index];

            mOBJNoMat.LoadeContent(content);

            index++;

            if (index == 16) index = 1;

            yield return new WaitForSeconds(0.33f);

            mOBJNoMat.Clear();

        } while (true);

    }
}
