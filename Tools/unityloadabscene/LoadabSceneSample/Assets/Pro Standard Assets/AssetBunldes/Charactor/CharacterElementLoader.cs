using UnityEngine;
using System.Collections;

/// <summary>
/// 角色元素AssetBundle加载器   
/// </summary>
public class CharacterElementLoader : AssetBaseLoader
{
    string m_Name;
    Material m_Material;
    StringHolder m_BoneName;

    /// <summary>
    /// 根据角色元素Bundle名来创建一个角色元素加载器  
    /// </summary>
    public CharacterElementLoader(string name, string bundleName)
    {
        m_Name = name;
        m_BundleName = bundleName;
    }

#if !UNITY_EDITOR_ZSN_LOADER
    public override void Loader()
    {
        if (m_WWW.isDone == false || !string.IsNullOrEmpty(m_WWW.error))
        {
            Debug.LogError("CharacterElementLoader.Loader : " + m_WWW.error + " @ " + m_WWW.url); 
            return;
        }
        if (m_GameObject == null)
            m_GameObject = (GameObject)m_WWW.assetBundle.LoadAsset(AssemblerConstant.RENDERER_OBJECT_NAME, typeof(GameObject));

        if (m_Material == null)
            m_Material = (Material)m_WWW.assetBundle.LoadAsset(m_Name, typeof(Material));

        if (m_BoneName == null)
            m_BoneName = (StringHolder)m_WWW.assetBundle.LoadAsset(AssemblerConstant.BONE_NAMES, typeof(StringHolder));
        IsLoaded = true;
    }
#else
    public override void Loader()
    {
        if (m_AssetBundleCreateRequest.isDone == false)
        {
            Debug.LogError("AssetLoaderError m_AssetBundleCreateRequest");
            return;
        }
        if (m_GameObject == null)
            m_GameObject = (GameObject)m_AssetBundleCreateRequest.assetBundle.Load(AssemblerConstant.RENDERER_OBJECT_NAME, typeof(GameObject));

        if (m_Material == null)
            m_Material = (Material)m_AssetBundleCreateRequest.assetBundle.Load(m_Name, typeof(Material));

        if (m_BoneName == null)
            m_BoneName = (StringHolder)m_AssetBundleCreateRequest.assetBundle.Load(AssemblerConstant.BONE_NAMES, typeof(StringHolder));

        IsLoaded = true;
    }
#endif

    public override void Dispose()
    {
        m_GameObject = null;

        m_Material = null;

        m_BoneName = null;

        base.Dispose();

    }

    /// <summary>
    /// 取得这个Assetbundle中的Mesh   
    /// </summary>
    public SkinnedMeshRenderer GetSkinnedMeshRenderer()
    {
        if (IsLoaded)
        {
            //return (SkinnedMeshRenderer)Object.Instantiate(m_GameObject.renderer);

            GameObject go = (GameObject)Object.Instantiate(m_GameObject);
            go.GetComponent<Renderer>().material = (Material)m_Material;
            return (SkinnedMeshRenderer)go.GetComponent<Renderer>();
        }

        return null;
    }

    /// <summary>
    /// 取得这个Assetbundle中包含的骨骼名字列表  
    /// </summary>
    public string[] GetBoneNames()
    {
        if (IsLoaded)
        {
            StringHolder holder = (StringHolder)m_BoneName;
            return holder.content;
        }

        return null;
    }
}

