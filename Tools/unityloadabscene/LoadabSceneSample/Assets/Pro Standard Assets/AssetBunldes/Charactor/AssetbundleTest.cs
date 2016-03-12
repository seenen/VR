using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetbundleTest : MonoBehaviour 
{
    private static AssetbundleTest m_Instance = null;

    public static AssetbundleTest GetInstance()
    {
        return m_Instance;
    }
	
	public Assembler assember;
	public uint _id;
	
	IEnumerator Start()
	{
		assember = Assembler.GetInstance();
		
		assember.OnFinishLoading += HandleOnFinishLoading;
		
		yield return new WaitForSeconds(3);
		
		string avatar = "zs";
		
		//AssemblerInfo info1 = new AssemblerInfo(0, avatar, avatar + "_head_0000", avatar + "_body_0000", "", "weapon0001", "", "wing", "zs_wing_0001");
        //AssemblerInfo info1 = new AssemblerInfo(0, avatar, avatar + "_head_0000", avatar + "_body_0000", "", "", "", "", "");
		//AssemblerInfo info2 = new AssemblerInfo(1, avatar, avatar + "_head_0001", avatar + "_body_0001", "", "weapon0001", "", "wing", "zs_wing_0001");
		
        //_id = assember.Create(info1, this);
//		assember.Create(info2);
		
        for(int i = 0; i < 20; ++i)
        {
            AssemblerInfo info1 = new AssemblerInfo(0, avatar, avatar + "_head_0000", avatar + "_body_0000", "", "", "", "", "");

            assember.Create(info1, this);

            yield return Random.Range(0, 30);
        }
	}

    void Awake()
    {
        m_Instance = this;
    }

	void HandleOnFinishLoading (uint id)
	{
        Debug.Log(id);

		if(id == _id)
		{
			GameObject o = Assembler.GetInstance().GetObject(id);
			o.transform.position = new Vector3(3, 0, 0);
			
			o.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			o.GetComponent<Animation>().Play("n_run");
			
			Transform[] hips = o.GetComponentsInChildren<Transform>();
		
			foreach(Transform hip in hips)
			{
				if(hip.name.Equals("wing"))
				{
					GameObject wing = hip.gameObject;
					wing.GetComponent<Animation>().wrapMode = WrapMode.Loop;
					wing.GetComponent<Animation>().Play("n_run");
					
					break;
				}
			}
		}
		else if(id == 1)
		{
			GameObject o = Assembler.GetInstance().GetObject(1);
			o.transform.position = new Vector3(-3, 0, 0);
			
			o.GetComponent<Animation>().wrapMode = WrapMode.Loop;
			o.GetComponent<Animation>().Play("n_run");
			
			Transform[] hips = o.GetComponentsInChildren<Transform>();
			
			foreach(Transform hip in hips)
			{
				if(hip.name.Equals("wing"))
				{
					GameObject wing = hip.gameObject;
					wing.GetComponent<Animation>().wrapMode = WrapMode.Loop;
					wing.GetComponent<Animation>().Play("n_run");
					
					break;
				}
			}
		}
	}
	
	void Update () 
	{
		//Assembler.GetInstance().Update();
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10, 10, 200, 500));

		AddCategory("head");
		AddCategory("body");
		AddCategory("foot");
		
		AddCategory("wing");
		AddCategory("weapon");
		
		GUILayout.EndArea();
	}
	
	void AddCategory(string category)
	{
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("<", GUILayout.Width(50)))
		{
//			AssemblerInfo info2 = new AssemblerInfo(0, "ck", "ck_head_0001", "ck_body_0001", "ck_foot_0001", "", "", "", "");
//			Assembler.GetInstance().Create(info2);
		}
		
		GUILayout.Box(category, GUILayout.Width(50));
		
		if(GUILayout.Button(">", GUILayout.Width(50)))
		{
//			AssemblerInfo info1 = new AssemblerInfo(0, "ck", "ck_head_0000", "ck_body_0000", "ck_foot_0000", "weapon0001", "weapon0001", "wing", "wing_0000");
//			Assembler.GetInstance().Create(info1);
		}
		
		GUILayout.EndHorizontal();
	}
}
