using UnityEngine;
using System.Collections;
using System.IO;

namespace AssetBundleEditor
{
    public class GameObjectNav : Object
    {
    }

    public class ABLoadScene : ABLoad
    {
        public static string GameObject = "_nomaterial";
        public static string Material = "_material";
        public static string Lightmap = "LightmapFar-0";
        public static string Nav = "_nav";

        //  Lightmap.
        public Texture2D lightmap = null;

        //  无材质球场景.
        public GameObject scene = null;

        //  材质球保存对象.
        public StringPrefab asset = null;

        //  碰撞盒.
        public GameObject nav = null;

        //  加载适配器.
        private ABLoad loadAdapter = null;

        //  对象资源名.
        private string mResName;

        public ABLoadScene(int version, ABDataType abtype) 
            : base(version)
        {
            mABDataType = abtype;

            switch (abtype)
            {
                case ABDataType.Mpq:
                    //loadAdapter = new ABLoadMpq(version, false);
                    break;
                case ABDataType.Normal:
                    loadAdapter = new ABLoadFile(version,false);
                    break;
            }

            loadAdapter.mDontDestroyOnLoad = true;

            mAutoRelease = false;

            //mDontDestroyOnLoad = true;
        }

        public override bool Create(string url)
        {
            bool bCreate = loadAdapter.Create(url);

            mUrl = url;

            mResName = FormatRes(url);

            isBundle = IsBundle(url);

            return bCreate;
        }

        private string FormatRes(string bundlePath)
        {
            string res = "Error ResName";

            if (!string.IsNullOrEmpty(bundlePath))
            {
                try
                {
                    res = Path.GetFileNameWithoutExtension(bundlePath);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return res;
        }

        public override IEnumerator Waiting()
        {
            float begintime = Time.realtimeSinceStartup;

            yield return loadAdapter.Waiting();

            Debuger.Log("Waiting End ");

            while (true)
            {
                if (loadAdapter.IsWWW())
                    break;

                yield return null;
            }

        //    _Waiting();
        //}

        //private void _Waiting()
        //{ 
            ErrorMsg = loadAdapter.GetErrorMsg();

            if (string.IsNullOrEmpty(ErrorMsg))
            {

                {
                    Pair(mResName, loadAdapter.GetAb().LoadAsset(mResName + ABLoadScene.GameObject, typeof(GameObject)), typeof(GameObject));
                }

                {
                    Pair(ABLoadScene.Lightmap, loadAdapter.GetAb().LoadAsset(ABLoadScene.Lightmap, typeof(Texture2D)), typeof(Texture2D));
                }

                {
                    Pair(mResName + ABLoadScene.Material, loadAdapter.GetAb().LoadAsset(mResName + ABLoadScene.Material, typeof(StringPrefab)), typeof(StringPrefab));
                }

                {
                    Pair(ABLoadScene.Nav, loadAdapter.GetAb().LoadAsset(mResName + ABLoadScene.Nav, typeof(GameObject)), typeof(GameObjectNav));
                }

                {
                    if (asset != null)
                    {
                        foreach (StringPrefabMaterial e in asset.material)
                        {
                            //Debuger.Log("assetbundlePath " + e.assetbundlePath);

                            float counttime = Time.realtimeSinceStartup;
#if 异步
                            AssetBundleRequest R = (AssetBundleRequest)mAssetBundleRequestObject[e.assetbundlePath];
#else
                            Material R = (Material)mMaterialObject[e.assetbundlePath];
#endif
                            if (R == null)
                            {
                                string fileurl = System.IO.Path.GetDirectoryName(mUrl) + "/" + e.assetbundlePath;

                                ABLoad loadAdapterTmp = null;

                                switch (mABDataType)
                                {
                                    case ABDataType.Mpq:
                                        //loadAdapterTmp = new ABLoadMpq(mVersion);
                                        break;
                                    case ABDataType.Normal:
                                        loadAdapterTmp = new ABLoadFile(mVersion);
                                        break;
                                }

                                loadAdapterTmp.Create(fileurl);

                                {
                                    yield return loadAdapterTmp.Waiting();

                                    while (true)
                                    {
                                        if (loadAdapterTmp.IsWWW())
                                            break;

                                        yield return null;
                                    }

                                }

                                if (string.IsNullOrEmpty(loadAdapterTmp.GetErrorMsg()))
                                {
                                    bool success = true;

                                    do
                                    {

                                        if (loadAdapterTmp.GetAb() == null)
                                        {
                                            Debuger.LogError("loadAdapterTmp.GetAb() == null" + " fileurl: " + fileurl + " mUrl: " + mUrl);

                                            success = false;

                                            ErrorMsg = "loadAdapterTmp.GetAb() == null";

                                            break;
                                        }

                                        //string resname = System.IO.Path.GetFileName(e.meshrendererGameObject);

                                        //Debuger.Log("3 " + resname);

#if 异步
                                        AssetBundleRequest requestMat = loadAdapterTmp.GetAb().LoadAsync(e.assetname, typeof(Material));

                                        mAssetBundleRequestObject[e.assetbundlePath] = requestMat;

                                        while (true)
                                        {
                                            if (requestMat.isDone)
                                                break;

                                            yield return null;
                                        }

                                        //Debuger.Log("4 " + request);

                                        if (requestMat.asset == null)
                                        {
                                            Debuger.LogError("requestMat.asset == null" + " fileurl: " + fileurl + " mUrl: " + mUrl);

                                            ErrorMsg = "requestMat.asset == null";

                                            success = false;

                                            break;
                                        }

#else
                                        Material m = (Material)loadAdapterTmp.GetAb().LoadAsset(e.assetname, typeof(Material));

                                        mMaterialObject[e.assetbundlePath] = m;
#endif
                                        mAbLoadObject.Add(loadAdapterTmp);
                                    }
                                    while (false);

                                    if (!success)
                                    {
                                        loadAdapterTmp.Dispose();

                                        loadAdapterTmp = null;
                                    }
                                }
                                else
                                {
                                    Debuger.LogError(loadAdapterTmp.GetErrorMsg() + " fileurl: " + fileurl + " mUrl: " + mUrl);

                                    ErrorMsg = loadAdapterTmp.GetErrorMsg();
                                }

                            }

                            R = null;

                            Debuger.Log("\t\tAssetBundleEditor.ABLoadScene " + (Time.realtimeSinceStartup - counttime).ToString("f5") + " File: " + e.assetbundlePath);

                        }
                    }
                    else
                    {
                        ErrorMsg = "asset == null";

                        Debuger.LogError("asset == null");
                    }

                }

            }

            Debuger.Log("AssetBundleEditor.ABLoadScene " + (Time.realtimeSinceStartup - begintime).ToString("f5") + " Count: " + asset.material.Length);
        }

        public override bool IsWWW()
        {
            return loadAdapter.IsWWW();
        }

        public override bool Error()
        {
            return loadAdapter.Error();
        }

        public override string GetErrorMsg()
        {
            return loadAdapter.GetErrorMsg();
        }

        public Hashtable mAssetBundleRequestObject = new Hashtable();
        public Hashtable mMaterialObject = new Hashtable();
        public ArrayList mWWWObject = new ArrayList();
        public ArrayList mAbLoadObject = new ArrayList();

        AssetBundleRequest requestGo = null;
        AssetBundleRequest requestNav = null;
        AssetBundleRequest requestAsset = null;
        AssetBundleRequest requestTexture = null;

        /// <summary>
        /// 临时申请的材质的WWW即时释放.
        /// </summary>
        public override void Release(bool force = false)
        {
            base.Release(force);

            //  基础数据不能释放.
            loadAdapter.Release(false);

            if (force)  mAutoRelease = force;

            if (mAutoRelease && Ref == 0)
            {
                foreach (Material e in mMaterialObject.Values)
                {
                    Material m = (Material)e;

                    m = null;
                }
                mMaterialObject.Clear();

                foreach (AssetBundleRequest e in mAssetBundleRequestObject.Values)
                {
                    AssetBundleRequest requestTmp = (AssetBundleRequest)e;

                    requestTmp = null;
                }
                mAssetBundleRequestObject.Clear();

                foreach (ABLoad e in mAbLoadObject)
                {
                    ABLoad abloadTmp = (ABLoad)e;

                    switch (mABDataType)
                    {
                        case ABDataType.Mpq:
                            //((ABLoadMpq)abloadTmp).Release(force);
                            break;
                        case ABDataType.Normal:
                            ((ABLoadFile)abloadTmp).Release(force);
                            break;
                    }
                    abloadTmp = null;
                }
                mAbLoadObject.Clear();
            }
        }

        public override void Unload()
        {
            base.Unload();

            nav = null;
            lightmap = null;
            asset = null;
            scene = null;

            requestGo = null;
            requestNav = null;
            requestAsset = null;
            requestTexture = null;

            {
                foreach (Material e in mMaterialObject.Values)
                {
                    Material m = (Material)e;

                    m = null;
                }
                mMaterialObject.Clear();
            }

            {
                foreach (AssetBundleRequest e in mAssetBundleRequestObject.Values)
                {
                    AssetBundleRequest requestTmp = (AssetBundleRequest)e;

                    requestTmp = null;
                }
                mAssetBundleRequestObject.Clear();

            }

            {
                foreach (ABLoad e in mAbLoadObject)
                {
                    ABLoad abloadTmp = (ABLoad)e;

                    switch (mABDataType)
                    {
                        case ABDataType.Mpq:
                            //((ABLoadMpq)abloadTmp).Unload();
                            break;
                        case ABDataType.Normal:
                            ((ABLoadFile)abloadTmp).Unload();
                            break;
                    }
                }
            }

            if (loadAdapter != null)
                loadAdapter.Unload();

        }

        public override void Dispose()
        {
            Unload();

            {
                foreach (ABLoad e in mAbLoadObject)
                {
                    ABLoad abloadTmp = (ABLoad)e;

                    switch (mABDataType)
                    {
                        case ABDataType.Mpq:
                            //((ABLoadMpq)abloadTmp).Dispose();
                            break;
                        case ABDataType.Normal:
                            ((ABLoadFile)abloadTmp).Dispose();
                            break;
                    }
                    abloadTmp = null;
                }
                mAbLoadObject.Clear();
            }

            {
                if (loadAdapter != null)
                    loadAdapter.Dispose();

                loadAdapter = null;
            }

            base.Dispose();
        }

        public override bool Pair(string resname, Object res, System.Type type)
        {
            if (res == null)
            {
                Debuger.LogError("ABLoadScene.Pair " + resname + " " + type.ToString());

                return false;
            }

            res.name = resname;

            if (type == typeof(GameObjectNav))
            {
                nav = (GameObject)res;
                nav.name = res.name;
            }
            else if (type == typeof(GameObject))
            {
                scene = (GameObject)res;
                scene.name = res.name;
            }
            else if (type == typeof(StringPrefab))
            {
                asset = (StringPrefab)res;
            }
            else if (type == typeof(Texture2D))
            {
                lightmap = (Texture2D)res;
            }

            return true;
        }

        public override Object GetObject(string resname, System.Type type, bool particle = true)
        {
            if (type == typeof(GameObjectNav))
            {
                return nav;
            }
            else if (type == typeof(GameObject))
            {
                return scene;
            }
            else if (type == typeof(Texture2D))
            {
                return lightmap;
            }

            return null;
        }

    }
}
