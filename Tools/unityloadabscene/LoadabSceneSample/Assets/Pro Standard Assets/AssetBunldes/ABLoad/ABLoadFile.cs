using UnityEngine;
using System.Collections;

namespace AssetBundleEditor
{
    public class ABLoadFile : ABLoad
    {
        public ABLoadFile(int version, bool autorelease = true)
            : base(version, autorelease)
        {
            mABDataType = ABDataType.Normal;
        }

        public override bool Create(string url)
        {
            base.Create(url);

            if (mWww == null)
                mWww = new WWW(mUrl);

            return true;
        }

        public override void Release(bool force = false)
        {
            base.Release(force);

            if (force) mAutoRelease = force;

            if (mAutoRelease && Ref == 0)
            {
                if (mWww != null)
                {
                    if (!Error())
                    {
                        if (isBundle && mWww.assetBundle != null)
                            mWww.assetBundle.Unload(false);
                    }
                    mWww.Dispose();
                    mWww = null;
                }
            }
        }

        public override IEnumerator Waiting()
        {
            yield return mWww;

            ErrorMsg = mWww.error;
        }

        public override bool IsWWW()
        {
            if (mWww == null)
                return true;

            return mWww.isDone;
        }

        public override AssetBundle GetAb()
        {
            if (mWww != null) return mWww.assetBundle;
            else return null;
        }

        public override void Unload()
        {
            base.Unload();

            if (mWww != null)
            {
                if (mWww.bytes != null)
                {
                    wwwmemorysize -= mWww.bytes.Length;
                    totalwwwsize = mWww.bytes.Length;
                }

                if (!Error())
                {
                    if (isBundle && mWww.assetBundle != null)
                        mWww.assetBundle.Unload(true);
                }
                mWww.Dispose();
                mWww = null;


            }

        }

        public override Object[] LoadAll()
        {
            return mWww.assetBundle.LoadAllAssets();
        }

        public override string GetErrorMsg()
        {
            return mWww.error;
        }

    }
}
