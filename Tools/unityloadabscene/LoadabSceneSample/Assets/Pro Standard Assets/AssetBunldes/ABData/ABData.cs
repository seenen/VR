using UnityEngine;
using System.Collections;
using AssetBundleEditor;
using System.Collections.Generic;

public class ABData
{
    #region Count
    public static int mRef = 0;
    private static List<ABData> mReferenceList = new List<ABData>();
    public static string DumpAllABDataList()
    {
        string sout = string.Empty;

        for (int i = mReferenceList.Count - 1; i >= 0; --i )
        {
            sout += ((ABData)mReferenceList[i]).Log();
            sout += "\r\n";
        }
        
        return sout;
    }
    #endregion

    protected ABLoad mLoad;
    protected string mUrl;
    protected string mResname;
    protected System.Type mType;
    protected ABLoadGoMono _ABLoadGoMono;
    
    protected int mCount;
    protected int mProgress;
    protected bool mDontDestroyOnLoad = false;

    public ABData(string url, string resname, System.Type type, ABLoad load)
    {
        mLoad = load;
        mUrl = url;
        mResname = resname;
        mType = type;

        Create();

        mRef++;
        mReferenceList.Add(this);

        mCount = 0;
        mProgress = 1;
    }

    public virtual float Percent()
    {
        if (mCount == 0)
            return 0;

        Debuger.Log("ABData.mProgress mProgress " + mProgress + "mCount " + mCount);

        return (float)mProgress / (float)mCount;
    }

    public virtual IEnumerator CreateSync()
    {
        yield return 1;
    }

    public virtual void Create()
    {
        if (mType == null)
        {
            Debuger.LogError(mType);

            return;
        }
        Object o = mLoad.GetObject(mResname, mType);

        if (mType == typeof(GameObject))
        {
            _gameobject = (GameObject)GameObject.Instantiate(o);
            _gameobject.name = mUrl + " " + mResname;
            _ABLoadGoMono = _gameobject.AddComponent<ABLoadGoMono>();
            _ABLoadGoMono.EnablePS = AssetBundleUti.ShowParticleSys;
        }
        else if (mType == typeof(AudioClip))
            _audioclip = (AudioClip)(o);
        else if (mType == typeof(Material))
            _mat = (Material)(o);
        else if (mType == typeof(Texture2D))
            _texture2d = (Texture2D)(o);
        else
            Debuger.LogError(mType);

        o = null;

        mCount++;
    }

    public void EnableParticleSystem(bool enable)
    {
        if (_ABLoadGoMono != null)
            _ABLoadGoMono.EnablePS = enable;
    }

    public void EnableSkinMeshRenderer(SkinQuality enable)
    {
        if (_ABLoadGoMono != null)
            _ABLoadGoMono.EnableSR = enable;
    }

    public virtual void Destory()
    {
        if (mLoad != null)
        {
            mLoad = null;
        }
        if (_ABLoadGoMono != null)
        {
            _ABLoadGoMono.Clear();
            GameObject.DestroyImmediate(_ABLoadGoMono);
            _ABLoadGoMono = null;
        }

        if (_gameobject != null)
        {
            _gameobject.SetActive(false);
            GameObject.DestroyImmediate(_gameobject);
            _gameobject = null;
        }
        _gameobject = null;
        _audioclip  = null;
        _mat        = null;
        _texture2d  = null;
    }

    public bool IsDontDestroyOnLoad()
    {
        return mDontDestroyOnLoad;
    }

    private bool mIsDispose = false;

    public void Dispose()
    {
        if (mIsDispose)
            return;

        Destory();

        mIsDispose = true;

        mRef--;
        mReferenceList.Remove(this);

    }

    protected GameObject _gameobject;

    public GameObject gameobject
    {
        get
        {
            return _gameobject;
        }
    }

    protected AudioClip _audioclip;

    public AudioClip audioclip
    {
        get
        {
            return _audioclip;
        }
    }


    protected Material _mat;

    public Material material
    {
        get
        {
            return _mat;
        }
    }

    protected Texture2D _texture2d;

    public Texture2D texture2d
    {
        get
        {
            return _texture2d;
        }
    }

    //protected ABLoadScene _abloadscene;

    //public ABLoadScene ABScene
    //{
    //    get
    //    {
    //        return _abloadscene;
    //    }
    //}

    public virtual string Log()
    {
        return "[url:]" + mUrl + "[resname:]" + mResname + "[type:]" + mType + "[ABLoad:]" + mLoad.ToString();
    }
}
