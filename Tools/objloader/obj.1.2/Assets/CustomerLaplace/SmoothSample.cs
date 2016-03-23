using UnityEngine;
using System.Collections;

public class SmoothSample : MonoBehaviour
{

    public GameObject sourceGO;

	// Use this for initialization
	void Start ()
    {
        MeshFilter mf = (MeshFilter)sourceGO.GetComponent(typeof(MeshFilter));

        Smooth s = new Smooth(mf.mesh);

        s.Exe();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
