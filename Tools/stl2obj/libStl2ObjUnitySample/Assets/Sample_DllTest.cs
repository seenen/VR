using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Sample_DllTest : MonoBehaviour
{

    [DllImport("dllCppTest", EntryPoint = "dlltest")]
    private static extern long dlltest();

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 200, 200), "Test DLL"))
        {
            long before = System.DateTime.Now.Ticks;
            Debug.Log("dlltest=" + dlltest() / 10000000.0 );
            Debug.Log("take " + (System.DateTime.Now.Ticks - before) / 10000000.0 );
        }

        if (GUI.Button(new Rect(300, 300, 200, 200), "Test Mono"))
        {
            long before = System.DateTime.Now.Ticks;
            Debug.Log("monotest=" + monotest() / 10000000.0 );
            Debug.Log("take " + (System.DateTime.Now.Ticks - before) / 10000000.0 );
        }

    }


    long monotest()

    {
        long a = 1;
        int b = 0;
        while (b < 1000000000)
        {
            a = a + b;
            b++;
        }
        return a;
    }
}
