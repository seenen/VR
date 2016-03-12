using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ABLoadGoMono : MonoBehaviour 
{

	// Use this for initialization
    public MeshRenderer[] mMeshRenderers;
    public SkinnedMeshRenderer[] mSkinnedMeshRenderers;
    public TrailRenderer[] mTrailRenderers;
    public ParticleSystem[] mParticleSystems;
    public Animation[] mAnimations;
    public MonoBehaviour[] mMonoBehaviour;
    public string[] mShader;
    public ABData _AbData;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        mAnimations = gameObject.GetComponentsInChildren<Animation>();

        mMeshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mMeshRenderers.Length; ++i)
        {
            mMeshRenderers[i].castShadows = false;
            mMeshRenderers[i].receiveShadows = false;
            mMeshRenderers[i].useLightProbes = false;
        }

        mSkinnedMeshRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < mSkinnedMeshRenderers.Length; ++i)
        {
            mSkinnedMeshRenderers[i].castShadows = false;
            mSkinnedMeshRenderers[i].receiveShadows = false;
            mSkinnedMeshRenderers[i].useLightProbes = false;
        }

        mTrailRenderers = gameObject.GetComponentsInChildren<TrailRenderer>();

        mParticleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();

        ArrayList list = new ArrayList();

        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            foreach (MonoBehaviour c in t.GetComponents(typeof(MonoBehaviour)))
            {
                if (c != null && c != this)
                {
                    list.Add(c);
                }
            }
        }

        mMonoBehaviour = (MonoBehaviour[])list.ToArray(typeof(MonoBehaviour));
        list.Clear();
        list = null;

        ////ArrayList list = new ArrayList();

        ////if (mMeshRenderers != null)
        ////{
        ////    //
        ////    for ( int i = 0; i < mMeshRenderers.Length; ++i )
        ////    {
        ////        for (int j = 0; j < mMeshRenderers[i].sharedMaterials.Length; ++j)
        ////        {
        ////            mMeshRenderers[i].castShadows = false;
        ////            mMeshRenderers[i].receiveShadows = false;
        ////            //if (mMeshRenderers[i].sharedMaterials[j].mainTexture != null)
        ////            //{
        ////            //    mMeshRenderers[i].sharedMaterials[j].mainTexture.filterMode = FilterMode.Point;
        ////            //    mMeshRenderers[i].sharedMaterials[j].mainTexture.wrapMode = TextureWrapMode.Clamp;
        ////            //}
        ////            list.Add(mMeshRenderers[i].sharedMaterials[j].shader.name);
        ////        }

        ////    }

        ////}

        ////if (mSkinnedMeshRenderers != null)
        ////{
        ////    //
        ////    for (int i = 0; i < mSkinnedMeshRenderers.Length; ++i)
        ////    {
        ////        for (int j = 0; j < mSkinnedMeshRenderers[i].sharedMaterials.Length; ++j)
        ////        {
        ////            mSkinnedMeshRenderers[i].castShadows = false;
        ////            mSkinnedMeshRenderers[i].receiveShadows = false;
        ////            mSkinnedMeshRenderers[i].sharedMaterials[j].mainTexture.filterMode = FilterMode.Point;
        ////            mSkinnedMeshRenderers[i].sharedMaterials[j].mainTexture.wrapMode = TextureWrapMode.Clamp;
        ////            list.Add(mSkinnedMeshRenderers[i].sharedMaterials[j].shader.name);
        ////        }
        ////    }
        ////}

        ////if (mTrailRenderers != null)
        ////{
        ////    //
        ////    for (int i = 0; i < mTrailRenderers.Length; ++i)
        ////    {
        ////        for (int j = 0; j < mTrailRenderers[i].sharedMaterials.Length; ++j)
        ////        {
        ////            mTrailRenderers[i].castShadows = false;
        ////            mTrailRenderers[i].receiveShadows = false;
        ////            mTrailRenderers[i].sharedMaterials[j].mainTexture.filterMode = FilterMode.Point;
        ////            mTrailRenderers[i].sharedMaterials[j].mainTexture.wrapMode = TextureWrapMode.Clamp;
        ////            list.Add(mTrailRenderers[i].sharedMaterials[j].shader.name);
        ////        }
        ////    }
        ////}

        ////if (list.Count != 0)
        ////{
        ////    mShader = new string[list.Count];

        ////    int index = 0;
        ////    foreach (string e in list)
        ////    {
        ////        mShader[index] = e;
        ////        index++;
        ////    }

        ////    mShader = GetString(mShader);
        ////}

        EnableParticleSystems(_TargetPs);
    }

    string[] GetString(string[] values)
    {
        if (values == null)
            return null;

        List<string> list = new List<string>();

        for (int i = 0; i < values.Length; i++)//遍历数组成员.
        {
            if (list.IndexOf(values[i]) == -1)//对每个成员做一次新数组查询如果没有相等的则加到新数组.
                list.Add(values[i]);
        }

        mShader = new string[list.Count];

        int index = 0;
        foreach (string e in list)
        {
            mShader[index] = e;
            index++;
        }

        return mShader;


    } 

    public void Clear()
    {
        if (mMeshRenderers != null)
        {
            for (int i = 0; i < mMeshRenderers.Length; ++i)
            {
                mMeshRenderers[i] = null;
            }
        }
        if (mSkinnedMeshRenderers != null)
        {
            for (int i = 0; i < mSkinnedMeshRenderers.Length; ++i)
            {
                mSkinnedMeshRenderers[i] = null;
            }
        }
        if (mTrailRenderers != null)
        {
            for (int i = 0; i < mTrailRenderers.Length; ++i)
            {
                mTrailRenderers[i] = null;
            }
        }
        if (mParticleSystems != null)
        {
            for (int i = 0; i < mParticleSystems.Length; ++i)
            {
                mParticleSystems[i] = null;
            }
        }
        if (mAnimations != null)
        {
            for (int i = 0; i < mAnimations.Length; ++i)
            {
                mAnimations[i] = null;
            }
        }
        if (mMonoBehaviour != null)
        {
            for (int i = 0; i < mMonoBehaviour.Length; ++i)
            {
                Destroy(mMonoBehaviour[i]);
                mMonoBehaviour[i] = null;
            }
        }
        if (mShader != null)
        {
            for (int i = 0; i < mShader.Length; ++i)
            {
                mShader[i] = null;
            }
        }

        mMeshRenderers = null;
        mSkinnedMeshRenderers = null;
        mTrailRenderers = null;
        mParticleSystems = null;
        mAnimations = null;
        mMonoBehaviour = null;
        mShader = null;
    }

    void OnDestroy()
    {
        Clear();

        if (_AbData != null)
        {
            AssetBundleUti.RemoveAbData(_AbData);

            _AbData = null;
        }
    }

    public void EnableAnimation(bool enable)
    {
        if (mAnimations == null)
            return;

        for (int i = 0; i < mAnimations.Length; i++)
        {
            mAnimations[i].enabled = enable;
            mAnimations[i].playAutomatically = enable;
        }
    }

    public void EnableMeshRenderers(bool enable)
    {
        if (mMeshRenderers == null)
            return;

        for (int i = 0; i < mMeshRenderers.Length; i++)
        {
            mMeshRenderers[i].enabled = enable;
        }
    }

    public void EnableSkinnedMeshRenderers(SkinQuality enable)
    {
        if (mSkinnedMeshRenderers == null)
            return;

        
        for (int i = 0; i < mSkinnedMeshRenderers.Length; i++)
        {
            mSkinnedMeshRenderers[i].enabled = true;
            mSkinnedMeshRenderers[i].quality = (SkinQuality)enable;
        }
    }

    public void EnableTrailRenderers(bool enable)
    {
        if (mTrailRenderers == null)
            return;

        for (int i = 0; i < mTrailRenderers.Length; i++)
        {
            mTrailRenderers[i].enabled = enable;
        }
    }

    private bool _TargetPs = true;

    public bool EnablePS
    {
        set
        {
            _TargetPs = (bool)value;
        }
    }

    private bool _Ps = false;

    private SkinQuality _TargetSR = 0;

    public SkinQuality EnableSR
    {
        set
        {
            _TargetSR = (SkinQuality)value;
        }
    }

    private SkinQuality _Sr = 0;

    void Update()
    {
        {
            if (_Ps != _TargetPs)
            {
                EnableParticleSystems(_TargetPs);

                _Ps = _TargetPs;
            }
        }

        {
            if (_Sr != _TargetSR)
            {
                EnableSkinnedMeshRenderers(_TargetSR);

                _Sr = _TargetSR;
            }
        }
    }

    void EnableParticleSystems(bool enable)
    {
        EnablePS = enable;

        if (mParticleSystems == null)
            return;

        //Debuger.Log("ABLoadGoMono.EnableParticleSystems " + gameObject.name + " " + enable);

        for (int i = 0; i < mParticleSystems.Length; i++)
        {
            if (!enable)
            {
                mParticleSystems[i].Stop();
                //mParticleSystems[i].gameObject.SetActive(false);
            }
            else
            {
                //mParticleSystems[i].gameObject.SetActive(true);
                mParticleSystems[i].Play();
            }
        }
    }

    void EnableFog(bool enable)
    {
        RenderSettings.fog = enable;
    }
    
    void EnableShader(string key)
    {
        for (int i = 0; i < mMeshRenderers.Length; ++i)
        {
            for (int j = 0; j < mMeshRenderers[i].materials.Length; ++j)
            {
                if (mMeshRenderers[i].materials[j].shader.name.StartsWith(key))
                    Destroy(mMeshRenderers[i].materials[j]);
            }
        }

        for (int i = 0; i < mSkinnedMeshRenderers.Length; ++i)
        {
            for (int j = 0; j < mSkinnedMeshRenderers[i].materials.Length; ++j)
            {
                if (mSkinnedMeshRenderers[i].materials[j].shader.name.StartsWith(key))
                    Destroy(mSkinnedMeshRenderers[i].materials[j]);
            }
        }

        for (int i = 0; i < mTrailRenderers.Length; ++i)
        {
            for (int j = 0; j < mTrailRenderers[i].materials.Length; ++j)
            {
                if (mTrailRenderers[i].materials[j].shader.name.StartsWith(key))
                    Destroy(mTrailRenderers[i].materials[j]);
            }
        }
    }
}

