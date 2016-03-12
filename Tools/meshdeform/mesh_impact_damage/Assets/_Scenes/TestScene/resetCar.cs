using UnityEngine;
using System.Collections;

public class resetCar : MonoBehaviour {
	
	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown("r"))
		{
			target.position = new Vector3(0.0f, 0.2f, 0.0f);
			target.rotation = Quaternion.identity;
		}
	
	}
}
