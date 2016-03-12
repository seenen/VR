using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Sample_STL2OBJ : MonoBehaviour
{

    [DllImport("dllCppTest", EntryPoint = "formatDataStl2Obj")]
    private unsafe static extern bool formatDataStl2Obj(char* filename);

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 200, 200), "Test Stl2Obj"))
        {
            long before = System.DateTime.Now.Ticks;

            //因为使用指针，因为要声明非安全域

            unsafe

            {

                //在传递字符串时，将字符所在的内存固化，

                //并取出字符数组的指针

                fixed (char* p = &(path.ToCharArray()[0]))

                {

                    //调用方法

                    bool ok = formatDataStl2Obj(p);

                    Debug.Log(ok);

                }

            }

            Debug.Log("take " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        }


    }

    public string path;
}
