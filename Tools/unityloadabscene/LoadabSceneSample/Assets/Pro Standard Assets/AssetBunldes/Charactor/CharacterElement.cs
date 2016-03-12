//#define Sync

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Object=UnityEngine.Object;

[Serializable]
public class CharacterElement
{
	public string name;
	public string bundleName;
	
	static Dictionary<string, WWW> wwws = new Dictionary<string, WWW>();

#if Sync
	AssetBundleRequest gameObjectRequest;
	AssetBundleRequest materialRequest;
	AssetBundleRequest boneNameRequest;
#else
    GameObject m_GameObject;
    Material m_Material;
    StringHolder m_BoneName;
#endif
	
	public CharacterElement(string name, string bundleName)
	{
		this.name = name;
		this.bundleName = bundleName;
	}
	
	public WWW WWW
	{
		get
		{
			if(!wwws.ContainsKey(bundleName))
			{
				wwws.Add(bundleName, new WWW(Assembler.AssetbundleBaseURL + bundleName));
			}
			
			return wwws[bundleName];
		}
	}
	
	public bool IsLoaded
	{
		get
		{
			if(!WWW.isDone)
				return false;

#if Sync
            if (gameObjectRequest == null)
			{
				gameObjectRequest = WWW.assetBundle.LoadAsync("rendererobject", typeof(GameObject));
			}
			
			if(materialRequest == null)
			{
				materialRequest = WWW.assetBundle.LoadAsync(name, typeof(Material));
			}
			
			if(boneNameRequest == null)
			{
				boneNameRequest = WWW.assetBundle.LoadAsync("bonenames", typeof(StringHolder));
			}
			
			if(!gameObjectRequest.isDone)
				return false;
			
			if(!materialRequest.isDone)
				return false;
			
			if(!boneNameRequest.isDone)
				return false;
#else
            if (m_GameObject == null)
                m_GameObject = (GameObject)WWW.assetBundle.LoadAsset("rendererobject", typeof(GameObject));

            if (m_Material == null)
                m_Material = (Material)WWW.assetBundle.LoadAsset(name, typeof(Material));

            if (m_BoneName == null)
                m_BoneName = (StringHolder)WWW.assetBundle.LoadAsset("bonenames", typeof(StringHolder));
#endif
			
			return true;
		}
	}
	
	public SkinnedMeshRenderer GetSkinnedMeshRenderer()
	{
#if Sync
		GameObject go = (GameObject)Object.Instantiate(gameObjectRequest.asset);
		go.renderer.material = (Material)materialRequest.asset;
#else
        GameObject go = (GameObject)Object.Instantiate(m_GameObject);
        go.GetComponent<Renderer>().material = (Material)m_Material;
#endif
        return (SkinnedMeshRenderer)go.GetComponent<Renderer>();
	}
	
	public bool IsSkinnedMeshRenderer()
	{
		bool result = true;
#if Sync
		GameObject go = (GameObject)Object.Instantiate(gameObjectRequest.asset);
#else
        GameObject go = (GameObject)Object.Instantiate(m_GameObject);
#endif
        result = (go.GetComponent<Renderer>() is SkinnedMeshRenderer);
		GameObject.Destroy(go);
		
		return result;
	}
	
	public string[] GetBoneNames()
	{
#if Sync
        StringHolder holder = (StringHolder)boneNameRequest.asset;
#else
        StringHolder holder = (StringHolder)m_BoneName;
#endif
        return holder.content;
    }
}
