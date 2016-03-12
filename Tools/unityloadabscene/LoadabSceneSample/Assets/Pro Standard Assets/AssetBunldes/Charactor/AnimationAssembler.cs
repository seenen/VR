using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 动画AssetBundle加载器   
/// </summary>
public class AnimationLoader : AssetBaseLoader
{
    StringHolder m_AnimName;
    Dictionary<string, AnimationClip> m_Anims = new Dictionary<string, AnimationClip>();

    public AnimationLoader(string bundleName) : base(bundleName) { }

#if !UNITY_EDITOR_ZSN_LOADER
    public override void Loader()
    {
        if (m_WWW.isDone == false || !string.IsNullOrEmpty(m_WWW.error))
        {
            Debug.LogError("AnimationLoader.Loader  : " + m_WWW.error + " @ " + m_WWW.url);
            return;
        }
        if (m_AnimName == null)
            m_AnimName = (StringHolder)m_WWW.assetBundle.LoadAsset(AssemblerConstant.BONE_NAMES, typeof(StringHolder));

        if (m_AnimName == null)
        {
            Debug.LogError("AnimationLoader.Loader m_AnimName " + m_WWW.url + " By " + AssemblerConstant.BONE_NAMES);
            return;
        }

        StringHolder holder = (StringHolder)m_AnimName;
        foreach (string name in holder.content)
        {
            if (!m_Anims.ContainsKey(name))
            {
                AnimationClip ar = (AnimationClip)m_WWW.assetBundle.LoadAsset(name, typeof(AnimationClip));
                m_Anims.Add(name, ar);
            }
        }
        IsLoaded = true;
    }
#else
    public override void Loader()
    {
        if (m_AssetBundleCreateRequest.isDone == false)
        {
            Debug.LogError("AnimationLoader : m_AssetBundleCreateRequest");
            return;
        }
        if (m_AnimName == null)
            m_AnimName = (StringHolder)m_AssetBundleCreateRequest.assetBundle.Load(AssemblerConstant.BONE_NAMES, typeof(StringHolder));

        if (m_AnimName == null)
        {
            Debug.LogError("AnimationLoader m_AssetBundleCreateRequest");
            return;
        }

        StringHolder holder = (StringHolder)m_AnimName;
        foreach (string name in holder.content)
        {
            if (!m_Anims.ContainsKey(name))
            {
                AnimationClip ar = (AnimationClip)m_AssetBundleCreateRequest.assetBundle.Load(name, typeof(AnimationClip));
                m_Anims.Add(name, ar);
            }
        }
        IsLoaded = true;
    }
#endif

    public override void Dispose()
    {
        foreach(AnimationClip e in m_Anims.Values)
        {
            AnimationClip clip = (AnimationClip)e;
            clip = null;
        }

        m_Anims.Clear();

        m_AnimName = null;

        base.Dispose();

    }

    /// <summary> 
    /// 取得这个Assetbundle中包含的所有的动画  
    /// </summary>
    public Dictionary<string, AnimationClip> GetAnimations()
    {
        return m_Anims;
    }
}

