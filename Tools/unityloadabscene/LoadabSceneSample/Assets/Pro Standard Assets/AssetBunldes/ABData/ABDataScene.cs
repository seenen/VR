using UnityEngine;
using System.Collections;
using AssetBundleEditor;
using System.Collections.Generic;

public class ABDataScene : ABData
{
    ABLoadScene mLoadScene;

    public ABDataScene(string url, string resname, System.Type type, ABLoadScene sceneload) 
        : base(url, resname, type, sceneload)
    {
        mProgress = 0;

        mCount = mLoadScene.asset.material.Length + 3;
    }

    public override void Create()
    {
        mLoadScene = (ABLoadScene)mLoad;

        mProgress = 0;

        //mDontDestroyOnLoad = true;

        _Create();
    }

    internal protected float StampTime(float begintime, string content)
    {
        Debuger.Log("[time:]" + (Time.realtimeSinceStartup - begintime).ToString("f5") + "\t\t" + content);

        return Time.realtimeSinceStartup;
    }

    public void _Create()
    {
        Debuger.Log("ABDataScene.CreateSync");

        float counttime = Time.realtimeSinceStartup;

        {
            _scene = (GameObject)GameObject.Instantiate(mLoadScene.scene);
            _scene.name = mLoadScene.scene.name;

            counttime = StampTime(counttime, "ABDataScene._Create  mLoadScene.scene");

            mProgress++;

            {
                AssetBundleRequest request = null;
                Dictionary<AssetBundleRequest, Material> dicRequestMat = new Dictionary<AssetBundleRequest, Material>();
                Material material = null;
                Dictionary<string, Material> dicStringMat = new Dictionary<string, Material>();
                Transform trans = null;

                Debuger.Log("ABDataScene.CreateSync " + mLoadScene.asset.material.Length.ToString());

                Debuger.Log("mLoadScene.asset.material = " + mLoadScene.asset.material.Length);


                Material tmpMat = null;

                for (int i = 0; i < mLoadScene.asset.material.Length; ++i)
                {
                    counttime = Time.realtimeSinceStartup;

                    StringPrefabMaterial e = (StringPrefabMaterial)mLoadScene.asset.material[i];

                    mProgress++;

                    trans = _scene.transform.Find(e.meshrendererGameObject.Substring(_scene.name.Length + 1));
#if 异步

                    request = (AssetBundleRequest)mLoadScene.mAssetBundleRequestObject[e.assetbundlePath];

                    if (request == null)
                    {
                        Debug.LogError(_scene.name + " request == null" + e.meshrendererGameObject);

                        continue;
                    }

                    if (dicMat.TryGetValue(request, out tmpMat))
                    {

                    }
                    else
                    {
                        if (request.asset == null)
                        {
                            Debug.LogError(_scene.name + " request.asset == null" + e.meshrendererGameObject);

                            continue;
                        }

                        //
                        if (!e.meshrendererGameObject.StartsWith(_scene.name))
                        {
                            Debug.LogError(_scene.name + " No StartWith " + e.meshrendererGameObject);

                            continue;
                        }

                        tmpMat = (Material)GameObject.Instantiate(request.asset);

                        dicMat.Add(request, tmpMat);

                    }
#else
                    if (dicStringMat.TryGetValue(e.assetbundlePath, out tmpMat))
                    {

                    }
                    else
                    {
                        material = (Material)mLoadScene.mMaterialObject[e.assetbundlePath];

                        if (material != null)
                        {
                            tmpMat = (Material)GameObject.Instantiate((Material)mLoadScene.mMaterialObject[e.assetbundlePath]);

                            dicStringMat.Add(e.assetbundlePath, tmpMat);

                        }
                    }
#endif

                    if (trans != null)
                    {
                        MeshRenderer mr = trans.gameObject.GetComponent<MeshRenderer>();

                        mr.material = tmpMat;

                        counttime = StampTime(counttime, "\tABDataScene._Create GetComponent<MeshRenderer>" + e.meshrendererGameObject);


                    }
                    //buger.Log("ABDataScene.CreateSync " + e.meshrendererGameObject + " " + (Time.realtimeSinceStartup - counttime).ToString());
                }

                dicRequestMat.Clear();
                dicRequestMat = null;
            }

            counttime = StampTime(counttime, "ABDataScene._Create mLoadScene.asset.material.Length");

            _ABLoadGoMono = _scene.AddComponent<ABLoadGoMono>();
            _ABLoadGoMono.EnablePS = AssetBundleUti.ShowParticleSys;

        }

        {
            if (mLoadScene.nav != null)
            {
                _nav = (GameObject)GameObject.Instantiate(mLoadScene.nav);
                _nav.name = mLoadScene.scene.name + ABLoadScene.Nav;
                _nav.AddComponent<ABLoadGoMono>();
            }

            counttime = StampTime(counttime, "ABDataScene._Create Instantiate(mLoadScene.nav)");

            mProgress++;

        }

        {
            _lightmap = mLoadScene.lightmap;

            mProgress++;

        }

        {
            _asset = mLoadScene.asset;

            mProgress++;

        }

        mLoadScene.Release(true);

        counttime = StampTime(counttime, "ABDataScene._Create mLoadScene.Release(true)");

        Debuger.Log("ABDataScene.CreateSync " + (Time.realtimeSinceStartup - counttime).ToString("f5"));
    }

    GameObject _scene;

    public GameObject scene
    {
        get
        {
            return _scene;
        }
    }

    GameObject _nav;

    public GameObject nav
    {
        get
        {
            return _nav;
        }
    }

    Texture2D _lightmap;

    public Texture2D lightmap
    {
        get
        {
            return _lightmap;
        }
    }

    //  材质球保存对象.
    StringPrefab _asset = null;

    public StringPrefab asset
    {
        get
        {
            return _asset;
        }
    }

    public override void Destory()
    {
        if (mLoadScene != null)
        {
            mLoadScene = null;
        }

        if (_ABLoadGoMono != null)
        {
            _ABLoadGoMono.Clear();
            GameObject.Destroy(_ABLoadGoMono);
            _ABLoadGoMono = null;
        }

        if (_scene != null)
        {
            _scene.SetActive(false);
            GameObject.Destroy(_scene);
            _scene = null;
        }
        _scene = null;

        if (_nav != null)
        {
            _nav.SetActive(false);
            GameObject.Destroy(_nav);
            _nav = null;
        }
        _nav = null;
        _asset = null;
        _lightmap = null;

        base.Destory();
    }

}
