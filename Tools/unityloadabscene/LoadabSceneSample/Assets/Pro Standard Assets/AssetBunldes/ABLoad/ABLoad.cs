//#define UNITY_EDITOR_ZSN

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundleEditor
{
	public enum ABDataType
	{
		Normal,
		Mpq,
	}

    public class ABLoad
    {
        #region Count
        public static int mRef = 0;
        public static int mRefRelease = 0;
        public static int totalmpqsize = 0;
        public static int totalwwwsize = 0;
        private static List<ABLoad> mReferenceList = new List<ABLoad>();
        public static string DumpAllABLoadList()
        {
            string sout = "**********AssetBundleEditor.ABLoad.DumpAllABLoadList********** ";
            sout += "\r\n";

            for (int i = mReferenceList.Count - 1; i >= 0; --i)
            {
                ABLoad load = (ABLoad)mReferenceList[i];
                sout += i.ToString() + "\t";
                sout += load.mType + "\t";
                sout += load.Log() + "\t";
                sout += "\r\n";
            }

            return sout;
        }
        private bool mIsDispose = false;
        public static string DumpAllNotReleaseAbLoadList()
        {
            string sout = "**********AssetBundleEditor.ABLoad.DumpAllNotReleaseAbLoadList********** ";
            sout += "\r\n";

            for (int i = mReferenceList.Count - 1; i >= 0; --i)
            {
                ABLoad load = (ABLoad)mReferenceList[i];
                if (!load.m_IsRelease)
                {
                    sout += i.ToString() + "\t";
                    sout += load.mType + "\t";
                    sout += load.Log();
                    sout += "\r\n";
                }
            }

            return sout;

        }
        #endregion

        private Hashtable mObject;
        private Hashtable mObjectTexture2D;
        private Hashtable mObjectClips;
        private Hashtable mObjectMaterial;

        //  WWW对象. 
        protected WWW mWww = null;
        //  链接.
        protected string mUrl;
        //  资源类型. 
        protected bool isBundle = false;
        //  版本号.
        protected int mVersion;
        //  计数器.
        protected int Ref = 0;
        //
        protected System.Type mType = null;

        //
        protected ABDataType mABDataType = ABDataType.Normal;
        //
        protected AssetBundleCreateRequest createrequest;
        //
        protected int mpqmemorysize = 0;
        //
        protected int wwwmemorysize = 0;
        //
        protected bool m_IsRelease = false;
        //  是否在使用后自动释放.
        protected bool mAutoRelease = true;
        //  切场景是否释放.
        public bool mDontDestroyOnLoad = false;

        public ABLoad(int version, bool autorelease = true)
        {
            mVersion = version;

            mRef++;
            mReferenceList.Add(this);
            
            mIsDispose = false;
            m_IsRelease = false;

            mObject = new Hashtable();
            mObjectTexture2D = new Hashtable();
            mObjectClips = new Hashtable();
            mObjectMaterial = new Hashtable();

            mAutoRelease = autorelease;

            Ref++;
        }

        public virtual void Unload()
        {
            Clear();
        }

        public bool IsDontDestroyOnLoad()
        {
            return mDontDestroyOnLoad;
        }

        public virtual void Dispose()
        {
            Unload();

            if (mIsDispose)
                return;

            mIsDispose = true;

            mRefRelease--;

            mRef--;
            mReferenceList.Remove(this);

        }

#if UNITY_EDITOR_ZSN
        string mFilePath = "";
#endif
        public virtual bool Create(string url)
        {
            mUrl = url.Replace("\\", "/");

            IsBundle(url);

            return true;
        }

        protected bool IsBundle(string rul)
        {

            if (mUrl.IndexOf("assetBundles") > -1)
                isBundle = true;

            return isBundle;
        }

        /// <summary>
        /// 减少当前Load引用次数.
        /// </summary>
        public virtual void Release(bool force = false)
        {
            if (force) mAutoRelease = force;

            if (mAutoRelease)
                Ref--;

            if (isBundle && 
                Ref == 0)
            {
                mRefRelease++;

                m_IsRelease = true;
            }
            //Debug.Log("ABLoad.Release Ref " + Ref.ToString() + " " + mUrl);

        }

        /// <summary>
        /// 加载等待.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Waiting()
        {
            yield return mWww;
        }

        /// <summary>
        /// 是否加载完毕.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsWWW()
        {
            return false;
        }

        protected string ErrorMsg = string.Empty;

        /// <summary>
        /// 是否有错误.
        /// </summary>
        /// <returns></returns>
		public virtual bool Error()
		{
            if (string.IsNullOrEmpty(ErrorMsg))
				return false;
			
			return true;
		}

        /// <summary>
        /// 返回错误吗.
        /// </summary>
        /// <returns></returns>
		public virtual string GetErrorMsg()
		{
            return string.Empty;
		}

        /// <summary>
        /// 获取对象.
        /// </summary>
        /// <returns></returns>
        public virtual AssetBundle GetAb()
        {
            return null;
        }

        /// <summary>
        /// 加载音效.
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public AudioClip Load(string res)
        {
            AudioClip mClip = null;

            if (mUrl.ToLower().EndsWith(".ogg"))
            {
                mClip = mWww.GetAudioClip(false, true, AudioType.OGGVORBIS);
            }
            else if (mUrl.ToLower().EndsWith(".wav"))
            {
                mClip = mWww.GetAudioClip(false, true, AudioType.WAV);
            }
            else if (mUrl.ToLower().EndsWith(".mp3"))
            {
#if UNITY_STANDALONE_WIN
                Debuger.LogError("ABLoad.Load GetAudioClip Not Supported Mp3! " + mUrl);

                return null;
#else
                mClip = mWww.GetAudioClip(false, false, AudioType.MPEG);
#endif
            }
            else
            {
                Debuger.LogError("ABLoad.Load GetAudioClip Not Supported " + mUrl);

                return null;
            }

            ErrorMsg = mWww.error;

            if (mClip == null)
            {
                Debuger.LogError("ABLoad.Load GetAudioClip Failed! " + mUrl);

                return null;
            }

            if (mClip != null &&
                mClip.length == 0 &&
                mClip.samples == 0)
            {
                mClip.name = res + "_error";
                Object.Destroy(mClip);
                mClip = null;
            }

            return mClip;
        }

        public virtual bool Pair(string resname, Object res, System.Type type)
        {
            if (res == null)
                return false;

            mType = type;

            res.name = resname;

            if (type == typeof(GameObject))
            {
                if (!mObject.ContainsKey(resname)) mObject[resname] = (GameObject)res;
            }
            else if (type == typeof(AudioClip))
            {
                if (!mObjectClips.ContainsKey(resname)) mObjectClips[resname] = (AudioClip)res;
            }
            else if (type == typeof(Texture2D))
            {
                if (!mObjectTexture2D.ContainsKey(resname)) mObjectTexture2D[resname] = (Texture2D)res;
            }
            else if (type == typeof(Material))
            {
                if (!mObjectMaterial.ContainsKey(resname)) mObjectMaterial[resname] = (Material)res;
            }

            return false;
        }

        public void EnableParticleSystems(bool enable)
        {
            for (int i = 0; i < mObject.Count; ++i)
            {
                GameObject res = (GameObject)mObject[i];

                if (res == null)
                    continue;

                ABLoadGoMono gomono = res.GetComponent<ABLoadGoMono>();
                gomono.EnablePS = enable;
                gomono = null;
            }

        }

        public virtual Object GetObject(string resname, System.Type type, bool particle = true)
        {
            if (type == typeof(GameObject))
            {
                if (mObject.ContainsKey(resname)) return (Object)mObject[resname];
            }
            else if (type == typeof(AudioClip))
            {
                if (mObjectClips.ContainsKey(resname)) return (Object)mObjectClips[resname];
            }
            else if (type == typeof(Texture2D))
            {
                if (mObjectTexture2D.ContainsKey(resname)) return (Object)mObjectTexture2D[resname];
            }
            else if (type == typeof(Material))
            {
                if (mObjectMaterial.ContainsKey(resname)) return (Object)mObjectMaterial[resname];
            }

            return null;
        }

        protected void Clear()
        {
            if (mObjectClips != null)
            {
                for (int i = 0; i < mObjectClips.Count; ++i)
                {
                    AudioClip res = (AudioClip)mObjectClips[i];
                    res = null;
                }
                mObjectClips.Clear();
                mObjectClips = null;
            }

            if (mObjectTexture2D != null)
            {
                for (int i = 0; i < mObjectTexture2D.Count; ++i)
                {
                    Texture2D res = (Texture2D)mObjectTexture2D[i];
                    res = null;
                }
                mObjectTexture2D.Clear();
                mObjectTexture2D = null;
            }

            if (mObjectMaterial != null)
            {
                for (int i = 0; i < mObjectMaterial.Count; ++i)
                {
                    Material res = (Material)mObjectMaterial[i];
                    res = null;
                }
                mObjectMaterial.Clear();
                mObjectMaterial = null;
            }

            if (mObject != null)
            {
                for (int i = 0; i < mObject.Count; ++i)
                {
                    GameObject res = (GameObject)mObject[i];

                    if (res != null)
                    {
                        ABLoadGoMono gomono = res.GetComponent<ABLoadGoMono>();
                        gomono.Clear();
                        GameObject.Destroy(gomono);
                        gomono = null;
                    }
                    GameObject.Destroy(res);
                    res = null;
                }
                mObject.Clear();
                mObject = null;
            }

        }

        /// <summary>
        /// 加载所有对象 
        /// </summary>
        /// <returns></returns>
        public virtual Object[] LoadAll()
        {
            return null;
        }

        public string Log()
        {
            return this.ToString() + 
                        " [mUrl:]" + mUrl + 
                        "[type:]" + mABDataType +
                        "[MpqMemorySize:]" + mpqmemorysize +
                        "[WwwMemorySize:]" + wwwmemorysize
                        ;
        }
    }
}