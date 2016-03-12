using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// AssetBundle加载器  
/// </summary>
public class Assembler : MonoBehaviour
{
    static Dictionary<string, CharacterLoader> s_CharacterLoaders = new Dictionary<string, CharacterLoader>();
    static Dictionary<string, ModelLoader> s_ModelLoaders = new Dictionary<string, ModelLoader>();
    static Dictionary<string, CharacterElementLoader> s_CharacterElementLoaders = new Dictionary<string, CharacterElementLoader>();
    static Dictionary<string, AnimationLoader> s_AnimationAssemblers = new Dictionary<string, AnimationLoader>();

    private static Assembler s_Instance;

    public static Assembler GetInstance()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("__Assembler");
            if (go != null)s_Instance = go.GetComponent<Assembler>();
        }

        return s_Instance;
    }
	
	void Start()
	{
        DontDestroyOnLoad(gameObject);
	}

    void OnDestroy()
    {
        CleanByScene();
    }

    void Update()
    {

    }

    public void CleanByScene()
    {
        //Debug.Log("Assembler.Clean ");

        mCorout_isRunning = false;

        this.StopCoroutine("LoadCoroutine");

        //StopAllCoroutines();

        foreach (var gameObj in m_Objects.Values)
        {
            GameObject go = (GameObject)gameObj;
            if (go != null)
            {
                go.SetActive(true);
                UnityEngine.Object.DestroyImmediate(go);
                go = null;
            }
        }
        m_Objects.Clear();

        foreach (CharacterLoader item in s_CharacterLoaders.Values) 
        {
            CharacterLoader loader = (CharacterLoader)item;
            loader.Dispose();
            loader = null;
        }
        s_CharacterLoaders.Clear();

        foreach (ModelLoader item in s_ModelLoaders.Values)
        {
            ModelLoader loader = (ModelLoader)item;
            loader.Dispose();
            loader = null;
        }
        s_ModelLoaders.Clear();

        foreach (CharacterElementLoader item in s_CharacterElementLoaders.Values)
        {
            CharacterElementLoader loader = (CharacterElementLoader)item;
            loader.Dispose();
            loader = null;
        }
        s_CharacterElementLoaders.Clear();

        foreach (AnimationLoader item in s_AnimationAssemblers.Values)
        {
            AnimationLoader loader = (AnimationLoader)item;
            loader.Dispose();
            loader = null;
        }
        s_AnimationAssemblers.Clear();

        foreach (var item in m_Assemblers.Values)
        {
            AssemblerInfo loader = (AssemblerInfo)item;
            loader = null;
        }
        m_Assemblers.Clear();

        mUniqueID = 1;

        OnFinishLoading = null;

    }

    private Dictionary<uint, AssemblerInfo> m_Assemblers = new Dictionary<uint, AssemblerInfo>();
    private Dictionary<uint, GameObject> m_Objects = new Dictionary<uint, GameObject>();
    private bool mCorout_isRunning = false;
    public delegate void EventHandler(uint id);
    public event EventHandler OnFinishLoading;

    /// <summary>
    /// 根据ID取得一个模型   
    /// </summary>
    public GameObject GetObject(uint id)
    {
        if (m_Objects.ContainsKey(id))
        {
            return m_Objects[id];
        }

        return null;
    }

    public void RemoveObject(uint id)
    {
        if (m_Objects.ContainsKey(id))
        {
            GameObject go = (GameObject)m_Objects[id];
            go.SetActive(false);
            DestroyImmediate(go);
            go = null;

            m_Objects.Remove(id);
        }
    }

    #region 根据AssemblerInfo创建模型

    uint mUniqueID = 1;

    public uint GetID()
    {
        mUniqueID++;

        return mUniqueID;
    }

    /// <summary>
    /// 根据AssemblerInfo创建一个   
    /// </summary>
    public uint Create(AssemblerInfo info, object obj, uint id = 0)
    {
        if(id == 0)
        {
            id = GetID();
        }

        Update(id, info);

        //Debug.Log(".Create  " + id + " " + obj.ToString());

        return id;
    }

    public void Update(uint id, AssemblerInfo info)
    {
        if (info == null) return;

        //Debug.Log(".Update  " + id + " " + JsonWriter.Serialize(info));

        info.m_ID = id;

        if (!string.IsNullOrEmpty(info.m_Character)
            && !s_CharacterLoaders.ContainsKey(info.m_Character))
        {
            CharacterLoader cl = new CharacterLoader(info.m_Character + "_" + AssemblerConstant.CHARACTER_BASE_NAME + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_CharacterLoaders.Add(info.m_Character, cl);
        }

        if (!string.IsNullOrEmpty(info.m_HeadElement)
            && !s_CharacterElementLoaders.ContainsKey(info.m_HeadElement))
        {
            CharacterElementLoader cel = new CharacterElementLoader(info.m_HeadElement, info.m_HeadElement + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_CharacterElementLoaders.Add(info.m_HeadElement, cel);
        }

        if (!string.IsNullOrEmpty(info.m_BodyElement)
            && !s_CharacterElementLoaders.ContainsKey(info.m_BodyElement))
        {
            CharacterElementLoader cel = new CharacterElementLoader(info.m_BodyElement, info.m_BodyElement + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_CharacterElementLoaders.Add(info.m_BodyElement, cel);
        }

        if (!string.IsNullOrEmpty(info.m_FootElement)
            && !s_CharacterElementLoaders.ContainsKey(info.m_FootElement))
        {
            CharacterElementLoader cel = new CharacterElementLoader(info.m_FootElement, info.m_FootElement + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_CharacterElementLoaders.Add(info.m_FootElement, cel);
        }

        if (!string.IsNullOrEmpty(info.m_Character)
            && !s_AnimationAssemblers.ContainsKey(info.m_Character + AssemblerConstant.ANIMATION_SUFFIX))
        {
            AnimationLoader aa = new AnimationLoader(info.m_Character + AssemblerConstant.ANIMATION_SUFFIX + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_AnimationAssemblers.Add(info.m_Character + AssemblerConstant.ANIMATION_SUFFIX, aa);
        }

        if (!string.IsNullOrEmpty(info.m_WeaponR)
            && !s_ModelLoaders.ContainsKey(info.m_WeaponR))
        {
            ModelLoader ml = new ModelLoader(info.m_WeaponR + "_" + AssemblerConstant.CHARACTER_BASE_NAME + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_ModelLoaders.Add(info.m_WeaponR, ml);
        }

        if (!string.IsNullOrEmpty(info.m_WeaponL)
            && !s_ModelLoaders.ContainsKey(info.m_WeaponL))
        {
            ModelLoader ml = new ModelLoader(info.m_WeaponL + "_" + AssemblerConstant.CHARACTER_BASE_NAME + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_ModelLoaders.Add(info.m_WeaponL, ml);
        }

        //if (!string.IsNullOrEmpty(info.m_Wing)
        //    && !s_CharacterLoaders.ContainsKey(info.m_Wing))
        //{

        //}

        if (!string.IsNullOrEmpty(info.m_WingElement)
            && !s_CharacterElementLoaders.ContainsKey(info.m_WingElement))
        {

            CharacterLoader ml = new CharacterLoader(info.m_WingElement + "_" + AssemblerConstant.CHARACTER_BASE_NAME + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_CharacterLoaders.Add(info.m_WingElement, ml);

            AnimationLoader aa = new AnimationLoader(info.m_WingElement + AssemblerConstant.ANIMATION_SUFFIX + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_AnimationAssemblers.Add(info.m_WingElement + AssemblerConstant.ANIMATION_SUFFIX, aa);

            CharacterElementLoader cel = new CharacterElementLoader(info.m_WingElement, info.m_WingElement + AssemblerConstant.ASSETBUNDLE_SUFFIX);
            s_CharacterElementLoaders.Add(info.m_WingElement, cel);
        }

        //if (!string.IsNullOrEmpty(info.m_Wing)
        //    && !s_AnimationAssemblers.ContainsKey(info.m_Wing + AssemblerConstant.ANIMATION_SUFFIX))
        //{

        //}

        info.m_IsDone = false;
        if (m_Assemblers.ContainsKey(id))
        {
            m_Assemblers[id] = new AssemblerInfo(ref info);
            //m_Assemblers.Remove(id);
        }
        else
        {
            m_Assemblers.Add(id, info);
        }

        if (!mCorout_isRunning)
        {
            mCorout_isRunning = true;
            StartCoroutine(LoadCoroutine());
        }

        //Debug.Log("Create assemble: " + DecToHex(id) + " status: " + info.m_IsDone);

        return;
    }
    #endregion

    private GameObject FindGameObject(GameObject root, string name)
    {
        Transform[] hips = root.GetComponentsInChildren<Transform>();

        foreach (Transform hip in hips)
        {
            if (hip.name.Equals(name))
            {
                return hip.gameObject;
            }
        }

        return null;
    }

    /// <summary>
    /// 更新部件   
    /// </summary>
    private void UpdateElement(GameObject root, string partName, string eleName)
    {
        if (string.IsNullOrEmpty(eleName))
        {
            Debug.LogError("AssetbundleAssembler UpdateElement partName: " + partName + " eleName: " + eleName);
            return;
        }

        Transform[] hips = root.GetComponentsInChildren<Transform>();

        GameObject part = FindGameObject(root, partName);
        if (part == null)
        {
            part = new GameObject();
            part.name = partName;
            part.transform.parent = root.transform;
            part.AddComponent<SkinnedMeshRenderer>();
        }

        SkinnedMeshRenderer smr = s_CharacterElementLoaders[eleName].GetSkinnedMeshRenderer();

        List<Transform> bones = new List<Transform>();
        CharacterElementLoader loader = (CharacterElementLoader)s_CharacterElementLoaders[eleName];
        //test
        var loaders = loader.GetBoneNames();
        foreach (string bone in loaders)
        {
            foreach (Transform hip in hips)
            {
                if (hip.name != bone)
                    continue;

                bones.Add(hip);
                break;
            }
        }

        SkinnedMeshRenderer partSmr = part.GetComponent<SkinnedMeshRenderer>();
        if (partSmr != null)
        {
            partSmr.sharedMesh = smr.sharedMesh;
            partSmr.bones = bones.ToArray();
            partSmr.materials = smr.materials;
            partSmr.rootBone = (bones == null || bones.Count == 0) ? null : bones[0];
        }
        UnityEngine.Object.Destroy(smr.gameObject);
    }

    /// <summary>
    /// 更新武器    
    /// </summary>
    private void UpdateWeapon(GameObject root, string archor, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            if (archor.Equals(AssemblerConstant.R_WEAPON_POINT))
            {
                GameObject oldWeapon = FindGameObject(root, AssemblerConstant.R_WEAPON_NAME);
                if (oldWeapon != null)
                {
                    oldWeapon.SetActive(false);
                    GameObject.Destroy(oldWeapon);
                    oldWeapon = null;
                }
            }
            else if (archor.Equals(AssemblerConstant.L_WEAPON_POINT))
            {
                GameObject oldWeapon = FindGameObject(root, AssemblerConstant.L_WEAPON_NAME);
                if (oldWeapon != null)
                {
                    oldWeapon.SetActive(false);
                    GameObject.Destroy(oldWeapon);
                    oldWeapon = null;
                }
            }
            else
            {
                GameObject oldWeapon = FindGameObject(root, AssemblerConstant.BACK_WEAPON_NAME);
                if (oldWeapon != null)
                {
                    oldWeapon.SetActive(false);
                    GameObject.Destroy(oldWeapon);
                    oldWeapon = null;
                }
            }

            return;
        }
        else
        {
            bool isWeaponPointFound = false;

            Transform[] hips = root.GetComponentsInChildren<Transform>();

            GameObject weapon = s_ModelLoaders[name].GetGameObject();

            if (weapon == null)
            {
                return;
            }

            if (archor.Equals(AssemblerConstant.R_WEAPON_POINT))
            {
                GameObject oldWeapon = FindGameObject(root, AssemblerConstant.R_WEAPON_NAME);
                if (oldWeapon != null)
                {
                    oldWeapon.SetActive(false);
                    GameObject.Destroy(oldWeapon);
                    oldWeapon = null;
                }
            }
            else if (archor.Equals(AssemblerConstant.L_WEAPON_POINT))
            {
                GameObject oldWeapon = FindGameObject(root, AssemblerConstant.L_WEAPON_NAME);
                if (oldWeapon != null)
                {
                    oldWeapon.SetActive(false);
                    GameObject.Destroy(oldWeapon);
                    oldWeapon = null;
                }
            }
            else
            {
                GameObject oldWeapon = FindGameObject(root, AssemblerConstant.BACK_WEAPON_NAME);
                if (oldWeapon != null)
                {
                    oldWeapon.SetActive(false);
                    GameObject.Destroy(oldWeapon);
                    oldWeapon = null;
                }
            }

            foreach (Transform t in hips)
            {
                if (t.name.Contains(archor))
                {
                    weapon.transform.parent = t;
                    weapon.transform.localPosition = Vector3.zero;
                    //					weapon.transform.localRotation = Quaternion.identity;

                    if (archor.Equals(AssemblerConstant.R_WEAPON_POINT))
                    {
                        weapon.transform.localEulerAngles = Vector3.zero;
                        weapon.name = AssemblerConstant.R_WEAPON_NAME;
                    }
                    else if (archor.Equals(AssemblerConstant.L_WEAPON_POINT))
                    {
                        weapon.transform.localEulerAngles = new Vector3(0, 0, 0);
                        weapon.name = AssemblerConstant.L_WEAPON_NAME;
                    }
                    else
                    {
                        weapon.transform.localEulerAngles = new Vector3(0, 0, 0);
                        weapon.name = AssemblerConstant.BACK_WEAPON_NAME;
                    }

                    isWeaponPointFound = true;

                    break;
                }
            }

            foreach (Transform t in weapon.GetComponentsInChildren<Transform>())
                if (t.gameObject != null)
                    t.gameObject.layer = root.layer;

            if (!isWeaponPointFound)
            {
                Debug.LogWarning("AssetbundleAssembler UpdateWeapon Could NOT found weapon point " + archor + " for " + name);
            }
        }
    }

    AnimationState mWingState = null;
    /// <summary>
    /// 更新翅膀  
    /// </summary>
    private void UpdateWing(GameObject root, string wingName, string wingEleName)
    {
        if (string.IsNullOrEmpty(wingName))
        {
            GameObject wing = FindGameObject(root, AssemblerConstant.WING_NAME);
            if (wing != null)
            {
                wing.SetActive(false);
                GameObject.Destroy(wing);
                wing = null;
            }
        }
        else
        {
            Transform[] hips = root.GetComponentsInChildren<Transform>();

            GameObject wing = FindGameObject(root, AssemblerConstant.WING_NAME);
            if (wing == null)
            {
                wing = s_CharacterLoaders[wingName].GetGameObject();
                if (wing != null)
                {
                    wing.transform.localEulerAngles = root.transform.eulerAngles;
                }
                else
                {
                    Debug.LogError("wing " + wingName + " " + wingEleName + "not found!");
                    return;
                }
            }

            wing.name = AssemblerConstant.WING_NAME;

            UpdateElement(wing, AssemblerConstant.WING_MESH_NAME, wingEleName);

            foreach (Transform t in hips)
            {
                if (t.name.Equals(AssemblerConstant.WING_POINT))
                {
                    wing.transform.parent = t;

                    wing.transform.localPosition = Vector3.zero;
                    wing.transform.localRotation =Quaternion.Euler(new Vector3(270, 90, 0));

                    break;
                }
            }

            foreach (Transform t in wing.GetComponentsInChildren<Transform>())
                if (t.gameObject != null)
                    t.gameObject.layer = root.layer;

            CreateAnimation(wing, wingName);

            wing.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            mWingState = wing.GetComponent<Animation>()["n_run"];
            if (mWingState != null)
                wing.GetComponent<Animation>().Play(mWingState.name);
        }
    }

    /// <summary>
    /// 创建动画 
    /// </summary>
    private void CreateAnimation(GameObject root, string character)
    {
        if (root.GetComponent<Animation>() == null)
        {
            root.AddComponent<Animation>();
        }

        if (root.GetComponent<Animation>().GetClipCount() == 0)
        {
            Dictionary<string, AnimationClip> animations = s_AnimationAssemblers[character + AssemblerConstant.ANIMATION_SUFFIX].GetAnimations();
            foreach (KeyValuePair<string, AnimationClip> anim in animations)
            {
                root.GetComponent<Animation>().AddClip(anim.Value, anim.Key);
            }
        }

        root.GetComponent<Animation>().cullingType = AnimationCullingType.BasedOnRenderers;
    }


    List<AssemblerInfo> mLastAssemblerList = new List<AssemblerInfo>();

    private IEnumerator LoadCoroutine()
    {
        //Debug.Log("Assembler.LoadCoroutine ");

        while (mCorout_isRunning)
        {
            List<AssemblerInfo> AssemblerList = new List<AssemblerInfo>(m_Assemblers.Values);
            foreach (var info in AssemblerList)
            {
                if (!info.m_IsDone)
                {
                    info.m_IsDone = true;
                    if (!s_CharacterLoaders[info.m_Character].IsLoaded)
                    {
                        yield return s_CharacterLoaders[info.m_Character].isDone;
                        while (true) { if (s_CharacterLoaders[info.m_Character].isDone.isDone) break; yield return 1; }
                        s_CharacterLoaders[info.m_Character].Loader();
                        s_CharacterLoaders[info.m_Character].Release();
                    }

                    if (!string.IsNullOrEmpty(info.m_HeadElement) && !s_CharacterElementLoaders[info.m_HeadElement].IsLoaded)
                    {
                        yield return s_CharacterElementLoaders[info.m_HeadElement].isDone;
                        while (true) { if (s_CharacterElementLoaders[info.m_HeadElement].isDone.isDone) break; yield return 1; }
                        s_CharacterElementLoaders[info.m_HeadElement].Loader();
                        s_CharacterElementLoaders[info.m_HeadElement].Release();
                    }

                    if (!string.IsNullOrEmpty(info.m_BodyElement) && !s_CharacterElementLoaders[info.m_BodyElement].IsLoaded)
                    {
                        yield return s_CharacterElementLoaders[info.m_BodyElement].isDone;
                        while (true) { if (s_CharacterElementLoaders[info.m_BodyElement].isDone.isDone) break; yield return 1; }
                        s_CharacterElementLoaders[info.m_BodyElement].Loader();
                        s_CharacterElementLoaders[info.m_BodyElement].Release();
                    }

                    if (!string.IsNullOrEmpty(info.m_FootElement) && !s_CharacterElementLoaders[info.m_FootElement].IsLoaded)
                    {
                        yield return s_CharacterElementLoaders[info.m_FootElement].isDone;
                        while (true) { if (s_CharacterElementLoaders[info.m_FootElement].isDone.isDone) break; yield return 1; }
                        s_CharacterElementLoaders[info.m_FootElement].Loader();
                        s_CharacterElementLoaders[info.m_FootElement].Release();
                    }

                    if (!s_AnimationAssemblers[info.m_Character + AssemblerConstant.ANIMATION_SUFFIX].IsLoaded)
                    {
                        yield return s_AnimationAssemblers[info.m_Character + AssemblerConstant.ANIMATION_SUFFIX].isDone;
                        while (true) { if (s_AnimationAssemblers[info.m_Character + AssemblerConstant.ANIMATION_SUFFIX].isDone.isDone) break; yield return 1; }
                        s_AnimationAssemblers[info.m_Character + AssemblerConstant.ANIMATION_SUFFIX].Loader();
                        s_AnimationAssemblers[info.m_Character + AssemblerConstant.ANIMATION_SUFFIX].Release();
                    }

                    if (!string.IsNullOrEmpty(info.m_WeaponR) && !s_ModelLoaders[info.m_WeaponR].IsLoaded)
                    {
                        yield return s_ModelLoaders[info.m_WeaponR].isDone;
                        while (true) { if (s_ModelLoaders[info.m_WeaponR].isDone.isDone) break; yield return 1; }
                        s_ModelLoaders[info.m_WeaponR].Loader();
                        s_ModelLoaders[info.m_WeaponR].Release();
                    }

                    if (!string.IsNullOrEmpty(info.m_WeaponL) && !s_ModelLoaders[info.m_WeaponL].IsLoaded)
                    {
                        yield return s_ModelLoaders[info.m_WeaponL].isDone;
                        while (true) { if (s_ModelLoaders[info.m_WeaponL].isDone.isDone) break; yield return 1; }
                        s_ModelLoaders[info.m_WeaponL].Loader();
                        s_ModelLoaders[info.m_WeaponL].Release();
                    }

                    //Debug.Log("info.m_WingElement " + info.m_WingElement);

                    if (!string.IsNullOrEmpty(info.m_WingElement) && !s_CharacterLoaders[info.m_WingElement].IsLoaded)
                    {
                        yield return s_CharacterLoaders[info.m_WingElement].isDone;
                        while (true) { if (s_CharacterLoaders[info.m_WingElement].isDone.isDone) break; yield return 1; }
                        s_CharacterLoaders[info.m_WingElement].Loader();
                        s_CharacterLoaders[info.m_WingElement].Release();
                    }

                    if (!string.IsNullOrEmpty(info.m_WingElement) && !s_CharacterElementLoaders[info.m_WingElement].IsLoaded)
                    {
                        yield return s_CharacterElementLoaders[info.m_WingElement].isDone;
                        while (true) { if (s_CharacterElementLoaders[info.m_WingElement].isDone.isDone) break; yield return 1; }
                        s_CharacterElementLoaders[info.m_WingElement].Loader();
                        s_CharacterElementLoaders[info.m_WingElement].Release();
                    }

                    if (!string.IsNullOrEmpty(info.m_WingElement) && !s_AnimationAssemblers[info.m_WingElement + AssemblerConstant.ANIMATION_SUFFIX].IsLoaded)
                    {
                        yield return s_AnimationAssemblers[info.m_WingElement + AssemblerConstant.ANIMATION_SUFFIX].isDone;
                        while (true) { if (s_AnimationAssemblers[info.m_WingElement + AssemblerConstant.ANIMATION_SUFFIX].isDone.isDone) break; yield return 1; }
                        s_AnimationAssemblers[info.m_WingElement + AssemblerConstant.ANIMATION_SUFFIX].Loader();
                        s_AnimationAssemblers[info.m_WingElement + AssemblerConstant.ANIMATION_SUFFIX].Release();
                    }

                    try
                    {
                        GameObject root;
                        if (m_Objects.ContainsKey(info.m_ID))
                            root = m_Objects[info.m_ID];
                        else
                            root = s_CharacterLoaders[info.m_Character].GetGameObject();

                        if (root == null)
                            continue;

                        root.name = info.m_Character + "_" + info.m_ID;
                        Transform[] hips = root.GetComponentsInChildren<Transform>();
                        UpdateElement(root, AssemblerConstant.HEAD_PART_NAME, info.m_HeadElement);
                        UpdateElement(root, AssemblerConstant.BODY_PART_NAME, info.m_BodyElement);

                        UpdateWeapon(root, AssemblerConstant.R_WEAPON_POINT, info.m_WeaponR);
                        UpdateWeapon(root, AssemblerConstant.L_WEAPON_POINT, info.m_WeaponL);

                        UpdateWing(root, info.m_WingElement, info.m_WingElement);
                        CreateAnimation(root, info.m_Character);
                        if (!m_Objects.ContainsKey(info.m_ID))
                        {
                            m_Objects.Add(info.m_ID, root);
                        }

                        EnableShadow(root, false);

                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }

                    if (OnFinishLoading != null)
                        OnFinishLoading(info.m_ID);
                }
            }

            //  全部完成的话 则退出.
            if (mAssemblersIsDone())
                mCorout_isRunning = false;

        }
    }

    void EnableShadow(GameObject root, bool flag)
    {
        foreach (SkinnedMeshRenderer e in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            e.castShadows = flag;
            e.receiveShadows = flag;
            e.useLightProbes = flag;
        }

        foreach (MeshRenderer e in root.GetComponentsInChildren<MeshRenderer>(true))
        {
            e.castShadows = flag;
            e.receiveShadows = flag;
            e.useLightProbes = flag;
        }

        //foreach (Animation a in root.GetComponentsInChildren<Animation>(true))
        //{
        //    a.cullingType = AnimationCullingType.BasedOnRenderers;
        //}
    }

    private bool mAssemblersIsDone()
    {
        foreach (var item in m_Assemblers.Values)
        {
            if (item.m_IsDone == false)
                return false;
        }
        return true;
    }

    public static string AssetbundleBaseURL
    {
        get
        {
            return ProjectSystem.StreamingAssets + "/player/";

        }
		set
		{
		}
    }

    public static string DecToHex(uint id)
    {
        return "0x" + id.ToString("X");
    }
}
