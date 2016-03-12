using UnityEngine;
using System.Collections;
using AssetBundleEditor;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;

public class AssetBundleUti : MFMonoBehaviour
{
    //  普通ab.
    public delegate void AssetBundleUtiHandler(string url, string resname, System.Type type, bool success, string errorcode = "");
    public static event AssetBundleUtiHandler OnFinishLoading;

    ABFactory mFactory = new ABFactory();

    //  场景ab.
    public delegate void AssetBundleSceneUtiHandler(string url, string resname, bool success, string errorcode = "");
    public static event AssetBundleSceneUtiHandler OnSceneFinishLoading;


    static AssetBundleUti mUti;

    protected override void MFAwake()
    {
        base.MFAwake();

        mUti = this;
    }

    protected override void MFStart()
    {
        base.MFStart();

        GameObject.DontDestroyOnLoad(gameObject);

        mUti = this;

    }

    protected override void MFOnDestroy()
    {
        base.MFOnDestroy();

        OnSceneFinishLoading = null;

        OnFinishLoading = null;

        mUti.CleanDataByScene();

        mUti.mFactory.CleanABByScene();

        mUti.mFactory = null;

        mUti = null;
    }

    //
    public static int Count = 0;

    private const int version = 312;

    public static int WaitFrame = 1;


    private static bool _ShowParticleSys = true;

    public static bool ShowParticleSys
    {
        set
        {
            Debuger.Log("AssetBundleUti.ShowParticleSys " + (bool)value);

            mUti.mFactory.EnableParticleSystem((bool)value);

            for (int i = datas.Count - 1; i >= 0; --i)
            {
                ABData data = (ABData)datas[i];
                data.EnableParticleSystem((bool)value);
            }
            _ShowParticleSys = (bool)value;
        }

        get
        {
            return _ShowParticleSys;
        }
    }

    private static SkinQuality _ShowSkinMeshRenderer = SkinQuality.Auto;

    public static SkinQuality ShowSkinMeshRenderer
    {
        set
        {
            Debuger.Log("AssetBundleUti.ShowSkinMeshRenderer " + (int)value);

            mUti.mFactory.EnableSkinMeshRenderer((int)value);

            for (int i = datas.Count - 1; i >= 0; --i)
            {
                ABData data = (ABData)datas[i];
                data.EnableSkinMeshRenderer((SkinQuality)value);
            }
            _ShowSkinMeshRenderer = (SkinQuality)value;
        }

        get
        {
            return _ShowSkinMeshRenderer;
        }
    }

    #region 普通AB.
    /// <summary>
    /// 获取指定wwwurl的对象资源
    /// </summary>
    /// <param name="url">用于www加载的路径</param>
    /// <param name="resname">包内的资源名</param>
    /// <param name="type">包内资源名的加载类型</param>
    /// <param name="abtype">加载的路径是MPQ里的还是本地文件的</param>
    /// <returns></returns>
    public static bool GetAB(string url, string resname, System.Type type, ABDataType abtype = ABDataType.Mpq)
    {
        //  如果是声音，强制变成Normal.
        if (typeof(AudioClip) == type)
            abtype = ABDataType.Normal;

        ABLoad mAB = null;

        //Debuger.Log("GetAB " + url);

        do
        {
            //  获取已有 
            mAB = mUti.mFactory.Get(url, version);

            //Debuger.Log("mAB " + mAB);

            if (mAB == null)
                break;

            //  获取资源对象 
            Object res = mAB.GetObject(resname, type, ShowParticleSys);

            //Debuger.Log("res " + res);

            if (res == null)
                break;

            //  返回 
            if (OnFinishLoading != null)
            {
                //Debuger.Log("AssetBundleUti.OnFinishLoading " + System.IO.Path.GetFileName(url) + " " + resname + " " + type.ToString());

                OnFinishLoading(url, resname, type, true);

            }

            return true;

        } while (true);

        if (mAB == null)
            mAB = mUti.mFactory.Add(url, version, abtype);

        mUti.StartCoroutine(mUti.CallBackSync(mAB, url, resname, type, abtype));

        return true;
    }

    string callbackerror = string.Empty;
    bool bCallback = false;

    public IEnumerator CallBackSync(ABLoad mAB, string url, string resname, System.Type type, ABDataType abtype)
    {
        //Debuger.Log("AssetBundleUti.CallBack " + System.IO.Path.GetFileName(url) + " " + resname + " " + type.ToString());
        
        Count++;

        callbackerror = string.Empty;
        bCallback = true;

        do
        {
            if (!mAB.Create(url))
            {   
                callbackerror = "ABLoad Create Error " + url;
                bCallback = false;

                break;
            }

            yield return StartCoroutine(mAB.Waiting());
		
            //Debuger.Log("Waiting End ");

            while (true)
            {
                if (mAB.IsWWW())
                    break;

                yield return null;
            }
		
            //Debuger.Log("Error " + mAB.Error());

		    if (mAB.Error())
		    {
                //Debuger.LogError(mAB.GetErrorMsg());
                callbackerror = mAB.GetErrorMsg();
                bCallback = false;

                break;
		    }
		    else
		    {
	            if (typeof(AudioClip) == type)
	            {
	                AudioClip ac = mAB.Load(resname);
	
                    //Debuger.Log(resname + " " + ac.length + " " + ac.samples + " " + ac.isReadyToPlay + " " + ac.channels);
                    //Debuger.Log(ac.isReadyToPlay);
	
	                if (ac != null)
	                {
	                    while (true)
	                    {
	                        if (ac.isReadyToPlay)
	                            break;

                            yield return null;
                        }

                        ac.name = resname;
	                    mAB.Pair(resname, ac, type);
	
	                    //  返回
                        if (OnFinishLoading != null)
                        {
                            //Debuger.Log("AssetBundleUti.OnFinishLoading " + System.IO.Path.GetFileName(url) + " " + resname + " " + type.ToString());

                            OnFinishLoading(url, resname, type, true);
                        }
	
	                }
	                else
	                {
	                    Debuger.LogError("Load " + resname + " in " + url);

                        callbackerror = mAB.GetErrorMsg();
                        bCallback = false;

                        break;
	                }
	            }
	            else
	            {
                    AssetBundle ab = mAB.GetAb();
                    if (ab == null)
                    {
                        callbackerror = "AssetBundle == null";
                        bCallback = false;

                        break;
                    }

                    AssetBundleRequest request = ab.LoadAssetAsync(resname, type);
                    if (request == null)
                    {
                        callbackerror = "LoadAsync AssetBundleRequest == null";
                        bCallback = false;

                        break;
                    }

	                while (true)
	                {
	                    if (request.isDone)
	                        break;

                        yield return null;
                    }

                    mAB.Pair(resname, request.asset, type);
	
	                //  返回
                    if (OnFinishLoading == null)
                    {
                        //Debuger.Log("AssetBundleUti.OnFinishLoading " + System.IO.Path.GetFileName(url) + " " + resname + " " + type.ToString());
                        callbackerror = "OnFinishLoading == null";
                        bCallback = false;

                        break;
                    }

                    OnFinishLoading(url, resname, type, true);
	            }
		    }

        }
        while (false);

        if (!bCallback)
        {
            Debuger.LogError("Error " + mAB.Error() + " callbackerror: " + callbackerror);

            MessageBox(callbackerror + " \r\n " + OnFinishLoading.ToString());

            OnFinishLoading(url, resname, type, false, callbackerror);

            if (mAB != null)
            {
                mAB.Dispose();
                mAB = null;
            }
        }
        else
        {
            releaseQueue.Enqueue(mAB);
        }

        Count--;

        if (Count <= 0)
        {
            Count = 0;
            releaseAllBundles();
        }
    }

    private Queue<ABLoad> releaseQueue = new Queue<ABLoad>();

    private void releaseAllBundles()
    {
        while (releaseQueue.Count > 0)
        {
            ABLoad loader = releaseQueue.Dequeue();

            if (loader != null)
            {
                loader.Release();
            }
        }
    }
    #endregion

    #region 加载新的场景格式
    public static bool GetABScene(string url, string resname, System.Type type, ABDataType abtype = ABDataType.Mpq)
    {
#if UNITY_STANDALONE_WIN
        if (url.ToLower().EndsWith(".mp3"))
        {
            Debuger.LogError("ABLoad.Load GetAudioClip Not Supported Mp3! " + url);

            return false;
        }
#endif

        ABLoadScene mAB = null;

        Debuger.Log("GetABScene " + url);

        do
        {
            //  获取已有 
            mAB = mUti.mFactory.GetABLoadScene(url, version);

            Debuger.Log("mAB " + mAB);

            if (mAB == null)
                break;

            //  返回 
            if (OnSceneFinishLoading != null)
            {
                //Debuger.Log("AssetBundleUti.OnFinishLoading " + System.IO.Path.GetFileName(url) + " " + resname + " " + type.ToString());

                OnSceneFinishLoading(url, resname, true);

            }

            return true;

        } while (true);

        if (mAB == null)
            mAB = mUti.mFactory.AddABLoadScene(url, version, abtype);

        mUti.StartCoroutine(mUti.SceneCallBack(mAB, url, resname, abtype));

        return true;
    }

    public IEnumerator SceneCallBack(ABLoadScene mAB, string url, string resname, ABDataType abtype = ABDataType.Mpq)
    {
        Count++;

        string errorcode = string.Empty;

        do
        {
            if (!mAB.Create(url))
            {
                errorcode = "ABLoadScene Create " + url;

                break;
            }

            yield return StartCoroutine(mAB.Waiting());

            Debuger.Log("Waiting End ");

            while (true)
            {
                if (mAB.IsWWW())
                    break;

                yield return null;
            }

            if (mAB.Error())
            {
                errorcode = mAB.GetErrorMsg();

                break;
            }
            else
            {
                //  返回
                if (OnSceneFinishLoading != null)
                {
                    //Debuger.Log("AssetBundleUti.OnFinishLoading " + System.IO.Path.GetFileName(url) + " " + resname + " " + type.ToString());

                    OnSceneFinishLoading(url, resname, true);
                }

            }

            yield return 1;
        }
        while (false);

        if (OnSceneFinishLoading != null)
        {
            //Debuger.Log("AssetBundleUti.OnFinishLoading " + System.IO.Path.GetFileName(url) + " " + resname + " " + type.ToString());

            curContent = errorcode;

            OnSceneFinishLoading(url, resname, false, errorcode);
        }

        Count--;

        releaseQueue.Enqueue(mAB);

        if (Count <= 0)
        {
            Count = 0;
            releaseAllBundles();
        }

    }

    #endregion

    #region 清楚数据.
    public static void CleanByScene()
    {

        mUti.CleanDataByScene();

        mUti.mFactory.CleanABByScene();

        mUti.mFactory = null;

        mUti.mFactory = new ABFactory();

        //Debuger.Log("ABData Count : " + ABData.mRef);
    }

    static List<ABData> datas = new List<ABData>();

    public static ABData GetObject(string url, string resname, System.Type type)
    {
        ABLoad load = mUti.mFactory.Get(url, version);

        if (load == null)
            return null;
		
		if (load.Error())
			return null;

        Object o = load.GetObject(resname, type, ShowParticleSys);
        if (o == null)
            return null;

        if (null == type)
        {
            return null;
        }

        ABData data = new ABData(url, resname, type, load);

        datas.Add(data);

        return data;
    }

    public static ABDataScene GetObjectScene(string url)
    {
        ABLoadScene load = mUti.mFactory.GetABLoadScene(url, version);

        if (load == null)
            return null;

        if (load.Error())
            return null;

        ABDataScene data = new ABDataScene(url, "", typeof(ABLoadScene), load);

        datas.Add(data);

        return data;
    }

    public static void RemoveAbData(ABData _data)
    {
        for (int i = datas.Count - 1; i >= 0; --i)
        {
            ABData data = (ABData)datas[i];

            if (_data == data)
            {
                datas.Remove(data);

                data.Dispose();

                data = null;

                _data = null;

                break;
            }

        }
    }

    void CleanDataByScene()
    {
        for(int i = datas.Count - 1; i >= 0; --i)
        {
            ABData data = (ABData)datas[i];

            if (!data.IsDontDestroyOnLoad())
            {
                datas.Remove(data);

                data.Dispose();

                data = null;
            }

        }

    }
    #endregion

    string curContent = string.Empty;

    public void MessageBox(string content)
    {
        curContent = content;

        Time.timeScale = 0;
    }

}
