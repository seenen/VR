using UnityEngine;
using System.Collections;

public class AssetBaseLoader
{
    protected string m_BundleName;
#if !UNITY_EDITOR_ZSN_LOADER
    protected WWW m_WWW;
#else
    protected AssetBundleCreateRequest m_AssetBundleCreateRequest;
#endif
    protected GameObject m_GameObject;
    public bool IsLoaded;

    public AssetBaseLoader() { }

    public AssetBaseLoader(string bundleName)
    {
        m_BundleName = bundleName;
    }

#if !UNITY_EDITOR_ZSN_LOADER
    /// <summary>
    /// 协程加载
    /// </summary>
    public virtual void Loader()
    {
        if (m_WWW.isDone == false || !string.IsNullOrEmpty(m_WWW.error))
        {
            Debug.LogError("AssetBaseLoader.Loader : " + m_WWW.error + " @ " + m_WWW.url);
            return;
        }
        if (m_WWW.assetBundle == null)
        {
            Debug.LogWarning("AssetBaseLoader Loader" + m_BundleName + " assetBundle is null");
            return;
        }

        m_GameObject = (GameObject)m_WWW.assetBundle.LoadAsset(AssemblerConstant.CHARACTER_BASE_NAME, typeof(GameObject));
        IsLoaded = true;
    }
#else
    /// <summary>
    /// 协程加载
    /// </summary>
    public virtual void Loader()
    {
        if (m_AssetBundleCreateRequest.isDone == false)
        {
            Debug.LogError("AssetLoaderError m_AssetBundleCreateRequest");
            return;
        }
        if (m_AssetBundleCreateRequest.assetBundle == null)
        {
            Debug.LogWarning("AssetBaseLoader Loader" + m_BundleName + " assetBundle is null");
            return;
        }

        m_GameObject = (GameObject)m_AssetBundleCreateRequest.assetBundle.Load(AssemblerConstant.CHARACTER_BASE_NAME, typeof(GameObject));
        IsLoaded = true;
    }
#endif

    public virtual void Release()
    {
        m_WWW.assetBundle.Unload(false);
    }

#if !UNITY_EDITOR_ZSN_LOADER
    public virtual WWW isDone
    {
        get
        {
            string url = Assembler.AssetbundleBaseURL + m_BundleName;

            //  下面的代码是把内存存成文件再加载，用于检测内存泄漏的.
#if UNITY_EDITOR_ZSN_LOADER
            try
            {
                string mFilePath = string.Empty;

                string mpqurl = url.Substring(url.IndexOf("/StreamingAssets/"));

                mFilePath = url.Substring(url.IndexOf(ProjectSystem.PrefixPlatform) + ProjectSystem.PrefixPlatform.Length);

                if (!System.IO.File.Exists(mFilePath))
                {
                    byte[] data = CommonUI_Unity3D.Impl.UnityDriver.UnityInstance.LoadData(CommonUI_Unity3D.Impl.UnityDriver.PREFIX_MPQ + mpqurl);

                    string dir = System.IO.Path.GetDirectoryName(mFilePath);

                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);

                    System.IO.File.WriteAllBytes(mFilePath, data);
                }

                url = ProjectSystem.PrefixPlatform + mFilePath;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
#endif
            if (m_WWW == null)
                m_WWW = new WWW(url);

            return m_WWW;
        }
    }
#else
    public virtual AssetBundleCreateRequest isDone
    {
        get
        {

            if (m_AssetBundleCreateRequest == null)
            {
                string mpqurl = Assembler.AssetbundleBaseURL + m_BundleName;

                mpqurl = mpqurl.Substring(mpqurl.IndexOf("/StreamingAssets/"));

                m_AssetBundleCreateRequest = CommonUI_Unity3D.Impl.UnityDriver.UnityInstance.LoadAssetBundle(CommonUI_Unity3D.Impl.UnityDriver.PREFIX_MPQ + mpqurl);
            }

            return m_AssetBundleCreateRequest;
        }
    }
#endif

    public GameObject GetGameObject()
    {
        if (IsLoaded)
        {
            return (GameObject)Object.Instantiate(m_GameObject);
        }

        return null;
    }

#if !UNITY_EDITOR_ZSN_LOADER
    public virtual void Dispose()
    {
        //Debug.Log(    "AssetBaseLoader.Dispose " + 
        //                m_GameObject != null ? m_GameObject.name : "no gameobject " +
        //                m_WWW != null ?  m_WWW.url : "no www");

        m_GameObject = null;

        if (m_WWW == null) return;
        if (m_WWW != null && m_WWW.assetBundle != null)
        {
            m_WWW.assetBundle.Unload(true);
            m_WWW.Dispose();
            m_WWW = null;
        }
    }
#else
    public virtual void Dispose()
    {
        m_GameObject = null;

        if (m_AssetBundleCreateRequest == null) return;
        if (m_AssetBundleCreateRequest != null && m_AssetBundleCreateRequest.assetBundle != null)
        {
            m_AssetBundleCreateRequest.assetBundle.Unload(true);
        }
        m_AssetBundleCreateRequest = null;
    }
#endif

}
