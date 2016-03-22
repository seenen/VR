using UnityEngine;
using System.Collections;

public class ObjImporterSample : MonoBehaviour
{
    [SerializeField]
    public ObjImporter.meshStruct ms;
    public Mesh mesh;

    // Use this for initialization
    void Start () {
        ObjImporter ob = new ObjImporter();
        ms = ob.AnlyFile("F:/GitHub/VR/Tools/stl2obj/Resources/DataFileObj/1.obj");

        mesh = ob.ImportFile("F:/GitHub/VR/Tools/stl2obj/Resources/DataFileObj/1.obj");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
