﻿
/*
 * .obj模型加载器
 * 作者：秦元培
 * 时间：2015年11月11日
 */
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class ObjModelLoader : MonoBehaviour 
{
    public string objPath = "F:/GitHub/VR/Tools/stl2obj/Resources/dannang/dannang_after.obj";

    void Start()
    {
        long before = System.DateTime.Now.Ticks;

        //LoadFormAsset("cube2", "");
        LoadFormFile(objPath, "");

        Debug.Log("LoadFormFile " + (System.DateTime.Now.Ticks - before) / 10000000.0);
    }


    /// <summary>
    /// 从本地文件中加载一个.obj模型
    /// </summary>
    /// <param name="modelFilePath">模型文件路径</param>
    /// <param name="texturePath">贴图文件路径</param>
    public void LoadFormFile(string modelFilePath, string texturePath)
    {
		if(!File.Exists(modelFilePath))
			Debug.Log("请确认obj模型文件是否存在!");
		if(!modelFilePath.EndsWith(".obj"))
		   Debug.Log("请确认这是一个obj模型文件");

        long before = System.DateTime.Now.Ticks;

        //读取内容
        StreamReader reader = new StreamReader(modelFilePath,Encoding.Default);
		string content = reader.ReadToEnd();
		reader.Close();

        Debug.Log("StreamReader " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        before = System.DateTime.Now.Ticks;

        //解析内容
        ObjMesh objInstace = new ObjMesh();
		objInstace = objInstace.LoadFromObj(content);

        Debug.Log("LoadFromObj " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        before = System.DateTime.Now.Ticks;

        //计算网格
        Mesh mesh = new Mesh();
		mesh.vertices = objInstace.VertexArray;
		mesh.triangles = objInstace.TriangleArray;
		if(objInstace.UVArray.Length > 0)
			mesh.uv = objInstace.UVArray;
		if(objInstace.NormalArray.Length>0)
			mesh.normals = objInstace.NormalArray;
		mesh.RecalculateBounds();

        Debug.Log("Mesh " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        before = System.DateTime.Now.Ticks;

        //生成物体
        GameObject go = gameObject;
		MeshFilter meshFilter = go.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;
		

		MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

        Debug.Log("GameObject " + (System.DateTime.Now.Ticks - before) / 10000000.0);
        before = System.DateTime.Now.Ticks;

        //获取mtl文件路径
        string mtlFilePath = modelFilePath.Replace(".obj",".mtl");
        //从mtl文件中加载材质
        //Material[] materials = ObjMaterial.Instance.LoadFormMtl(mtlFilePath, texturePath);
    }

    /// <summary>
    /// 从网络加载一个.obj模型
    /// </summary>
    /// <param name="modelFilePath"></param>
    /// <param name="textureFilePath"></param>
    public void LoadFormWWW(string modelFilePath, string textureFilePath)
    {
        
    }
	
}
