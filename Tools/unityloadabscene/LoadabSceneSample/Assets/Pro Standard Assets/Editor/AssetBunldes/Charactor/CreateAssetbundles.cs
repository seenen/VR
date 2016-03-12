//#define UNITY_STANDALONE_WIN

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

// create assetbundle according different platform

public class CreateAssetbundles
{
    public enum PLATFORM
    {
        Win,
        iOS,
        Android,
    }

    // Assets bundle folder & temp prefabs
    public const string ASSETBUNDLE_SUFFIX = ".assetbundle";

#if UNITY_STANDALONE_WIN
    public static string ASSETBUNDLE_PATH = "Assets/StreamingAssets/PC/player/";
    public static string ELEMENTS_DATA_ASSETBUNDLE_PATH = "Assets/StreamingAssets/PC/player/CharacterElementDatabase.assetbundle";
#elif UNITY_IPHONE
    public static string ASSETBUNDLE_PATH 				= "Assets/StreamingAssets/IOS/player/";
    public static string ELEMENTS_DATA_ASSETBUNDLE_PATH 	= "Assets/StreamingAssets/IOS/player/CharacterElementDatabase.assetbundle";
#elif UNITY_ANDROID
    public static string ASSETBUNDLE_PATH 				= "Assets/StreamingAssets/ANDROID/player/";
    public static string ELEMENTS_DATA_ASSETBUNDLE_PATH 	= "Assets/StreamingAssets/ANDROID/player/CharacterElementDatabase.assetbundle";
#else
    public static string ASSETBUNDLE_PATH 				= "Assets/StreamingAssets/PC/player/";
    public static string ELEMENTS_DATA_ASSETBUNDLE_PATH 	= "Assets/StreamingAssets/PC/player/CharacterElementDatabase.assetbundle";
#endif
    //#if UNITY_STANDALONE_WIN	
    //    public const string ASSETBUNDLE_PATH 				= "Assets/StreamingAssets/assetbundle/win/";
    //    public const string ELEMENTS_DATA_ASSETBUNDLE_PATH 	= "Assets/StreamingAssets/assetbundle/win/CharacterElementDatabase.assetbundle";
    //#elif UNITY_IPHONE
    //    public const string ASSETBUNDLE_PATH 				= "Assets/StreamingAssets/assetbundle/ios/";
    //    public const string ELEMENTS_DATA_ASSETBUNDLE_PATH 	= "Assets/StreamingAssets/assetbundle/ios/CharacterElementDatabase.assetbundle";
    //#elif UNITY_ANDROID
    //    public const string ASSETBUNDLE_PATH 				= "Assets/StreamingAssets/assetbundle/android";
    //    public const string ELEMENTS_DATA_ASSETBUNDLE_PATH 	= "Assets/StreamingAssets/assetbundle/android/CharacterElementDatabase.assetbundle";
    //#else
    //    public const string ASSETBUNDLE_PATH 				= "Assets/StreamingAssets/assetbundle/win/";
    //    public const string ELEMENTS_DATA_ASSETBUNDLE_PATH 	= "Assets/StreamingAssets/assetbundle/win/CharacterElementDatabase.assetbundle";
    //#endif

    public const string TEMP_PREFAB_PATH = "Assets/prefabs/";
    public const string STRING_HOLDER_PATH = "Assets/prefabs/bonenames.asset";
    public const string ELEMENTS_DATA_HOLDER_PATH = "Assets/prefabs/CharacterElementDatabase.asset";



    // Resource path
    public const string RES_PATH_MODEL = "Assets/Resources/Model/";
    //	public const string RES_PATH_SOUND 	= "Assets/Resource/Sound/";
    //	public const string RES_PATH_TEXT 	= "Assets/Resource/Text/";
    //	public const string RES_PATH_IMAGE 	= "Assets/Resource/Texture/";

    public const string MATERIAL_PATH = "Materials";
    public const string TEXTURE_PATH = "Textures";
    public const string ANIMATION_PATH = "Anim";

    #region Character
    [MenuItem("AssetBundle/Create Character Bundle/Win")]
    static void CreateAssetBundle_Character_Win()
    {
        CreateAssetBundle_Character(PLATFORM.Win);
    }

    [MenuItem("AssetBundle/Create Character Bundle/iOS")]
    static void CreateAssetBundle_Character_iOS()
    {
        CreateAssetBundle_Character(PLATFORM.iOS);
    }

    [MenuItem("AssetBundle/Create Character Bundle/Android")]
    static void CreateAssetBundle_Character_Android()
    {
        CreateAssetBundle_Character(PLATFORM.Android);
    }
    #endregion

    #region Text
    [MenuItem("AssetBundle/Create Text Bundle/Win")]
    static void CreateAssetBundle_Text_Win()
    {
        CreateAssetBundle_Text(PLATFORM.Win);
    }

    [MenuItem("AssetBundle/Create Text Bundle/iOS")]
    static void CreateAssetBundle_Text_iOS()
    {
        CreateAssetBundle_Text(PLATFORM.iOS);
    }

    [MenuItem("AssetBundle/Create Text Bundle/Android")]
    static void CreateAssetBundle_Text_Android()
    {
        CreateAssetBundle_Text(PLATFORM.Android);
    }
    #endregion

    #region Image
    [MenuItem("AssetBundle/Create Image Bundle/Win")]
    static void CreateAssetBundle_Image_Win()
    {
        CreateAssetBundle_Image(PLATFORM.Win);
    }

    [MenuItem("AssetBundle/Create Image Bundle/iOS")]
    static void CreateAssetBundle_Image_iOS()
    {
        CreateAssetBundle_Image(PLATFORM.iOS);
    }

    [MenuItem("AssetBundle/Create Image Bundle/Android")]
    static void CreateAssetBundle_Image_Android()
    {
        CreateAssetBundle_Image(PLATFORM.Android);
    }
    #endregion


    #region Sound
    [MenuItem("AssetBundle/Create Sound Bundle/Win")]
    static void CreateAssetBundle_Sound_Win()
    {
        CreateAssetBundle_Sound(PLATFORM.Win);
    }

    [MenuItem("AssetBundle/Create Sound Bundle/iOS")]
    static void CreateAssetBundle_Sound_iOS()
    {
        CreateAssetBundle_Sound(PLATFORM.iOS);
    }

    [MenuItem("AssetBundle/Create Sound Bundle/Android")]
    static void CreateAssetBundle_Sound_Android()
    {
        CreateAssetBundle_Sound(PLATFORM.Android);
    }
    #endregion


    static void CreateAssetBundle_Sound(PLATFORM platform)
    {
        CreateAssetBundlePath();

        List<AudioSource> audios = CollectAll<AudioSource>("Assets/Resource/Sound");

        foreach (AudioSource t in audios)
        {
            Debug.Log(t.name);
        }

        Log("CreateAssetbundles CreateAssetBundle_Sound");
    }

    static void CreateAssetBundle_Image(PLATFORM platform)
    {
        CreateAssetBundlePath();

        List<Texture> imgs = CollectAll<Texture>("Assets/Resource/Texture");

        foreach (Texture t in imgs)
        {
            Debug.Log(t.name);
        }

        Log("CreateAssetbundles CreateAssetBundle_Image");
    }

    static void CreateAssetBundle_Text(PLATFORM platform)
    {
        CreateAssetBundlePath();

        List<TextAsset> texts = CollectAll<TextAsset>("Assets/Resource/Text");

        List<Object> toinclude = new List<Object>();

        foreach (TextAsset t in texts)
        {
            Debug.Log(t.name);
            toinclude.Add(t);
        }

        Log("CreateAssetbundles CreateAssetBundle_Text");
    }
	
	public static void CreateAssetBundle_Character(string DestFolder, PLATFORM platform)
	{
		string mBatchPath = ASSETBUNDLE_PATH;
		
		ASSETBUNDLE_PATH = DestFolder + "/";
		
		CreateAssetBundle_Character(platform);
		
		ASSETBUNDLE_PATH = mBatchPath;
	}

    static void CreateAssetBundle_Character(PLATFORM platform)
    {
        Log("CreateAssetbundles CreateAssetBundle_Characters");

        int processedBundleCount = 0;

        CreateAssetBundlePath();
        //		AssetDatabase.DeleteAsset(ASSETBUNDLE_PATH);
        //		AssetDatabase.DeleteAsset(TEMP_PREFAB_PATH);

        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            // filters
            if (!(obj is GameObject))
                continue;
            if (obj.name.Contains("@"))
                continue;
            if (!AssetDatabase.GetAssetPath(obj).Contains("Model"))
                continue;

            if (AssetDatabase.GetAssetPath(obj).Contains("Female"))
                continue;

            /* 
             * Assets/Resource/Model/Characters/Female
             * Assets/Resource/Model/Characters/Female/Female.fbx
             * Assets/Resource/Model/Characters/Female/Anim/
             * Assets/Resource/Model/Characters/Female/PerTextureMaterials/
             * Assets/Resource/Model/Characters/Female/textures/
             *
             */

            string fullpath = AssetDatabase.GetAssetPath(obj);
            string rootpath = fullpath.Substring(0, fullpath.LastIndexOf('/') + 1);

            string materialpath = rootpath + MATERIAL_PATH;
            string animpath = rootpath + ANIMATION_PATH;
            string texturepath = rootpath + TEXTURE_PATH;

            GameObject characterFBX = (GameObject)obj;
            string name = characterFBX.name.ToLower();

            DeleteExistAssetbundle(name + "_characterbase");

            if (name.Contains("_"))
                Debug.LogError("fbx name is illegal: " + name);

            string log = "-----------------\n";

            log += ("Create AssetBundle for [ " + name + " ] \tPath: " + fullpath + "\n");

            // fbx
            //			if(false)
            {
                log += "Prepare FBX: \n";

                log += ("\tName: " + name + "\n");

                GameObject characterClone = (GameObject)Object.Instantiate(characterFBX);

                // remove animation
                foreach (Animation anim in characterClone.GetComponentsInChildren<Animation>())
                    GameObject.DestroyImmediate(anim);

                // delete skinned mesh
                bool skinned = false;
                foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    skinned = true;
                    Object.DestroyImmediate(smr.gameObject);
                }

                //				foreach(MeshRenderer smr in characterClone.GetComponentsInChildren<MeshRenderer>())
                //					Object.DestroyImmediate(smr.gameObject);

                //				if(skinned)
                //					characterClone.AddComponent<SkinnedMeshRenderer>();

                // save asset bundle
                string path = ASSETBUNDLE_PATH + name + "_characterbase" + ASSETBUNDLE_SUFFIX;
                Object prefab = GetPrefab(characterClone, "characterbase");

                bool result = false;
                if (platform == PLATFORM.Win)
                {
                    result = BuildPipeline.BuildAssetBundle(prefab, null, path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows);
                }
                else if (platform == PLATFORM.iOS)
                {
                    result = BuildPipeline.BuildAssetBundle(prefab, null, path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.iOS);
                }
                else if (platform == PLATFORM.Android)
                {
                    result = BuildPipeline.BuildAssetBundle(prefab, null, path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
                }

                if (!result)
                    Debug.LogWarning("Failed! Create AssetBundle: " + path);
                //				else
                //					Debug.Log("Success! Create AssetBundle: " + path);

                ++processedBundleCount;

                // delete prefab
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(prefab));
            }

            // mesh

            bool findSkinnedMeshRenderer = false;
            log += "Prepare Mesh: \n";

            //			if(false)
            {
                List<Material> materials = CollectAll<Material>(materialpath);

                foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    List<Object> toinclude = new List<Object>();

                    // skinned mesh
                    GameObject rendererClone = (GameObject)EditorUtility.InstantiatePrefab(smr.gameObject);
                    GameObject rendererParent = rendererClone.transform.parent.gameObject;
                    rendererClone.transform.parent = null;
                    Object.DestroyImmediate(rendererParent);
                    Object renderPrefab = GetPrefab(rendererClone, "rendererobject");
                    toinclude.Add(renderPrefab);

                    // material
                    log += ("\tMesh: " + smr.name + "\n");

                    log += "\t\tMaterial: \n";

                    foreach (Material m in materials)
                    {
                        if (m.name.Contains(smr.name.ToLower()))
                        {
                            toinclude.Add(m);

                            log += ("\t\t" + m.name + "\n");
                        }
                    }

                    log += "\tBones: \n";

                    // bone assemble information
                    List<string> boneNames = new List<string>();
                    foreach (Transform t in smr.bones)
                        boneNames.Add(t.name);

                    StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
                    holder.content = boneNames.ToArray();
                    AssetDatabase.CreateAsset(holder, STRING_HOLDER_PATH);
                    toinclude.Add(AssetDatabase.LoadAssetAtPath(STRING_HOLDER_PATH, typeof(StringHolder)));

                    // save the assetbundle
                    string bundleName = smr.name.ToLower();
                    string path = ASSETBUNDLE_PATH + bundleName + ASSETBUNDLE_SUFFIX;

                    bool result = false;
                    if (platform == PLATFORM.Win)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows);
                    }
                    else if (platform == PLATFORM.iOS)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.iOS);
                    }
                    else if (platform == PLATFORM.Android)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
                    }

                    if (!result)
                        Debug.LogWarning("Failed! Create AssetBundle: " + path);
                    //					else
                    //						Debug.Log("Success! Create AssetBundle: " + path);

                    ++processedBundleCount;

                    // delete temp assets
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(renderPrefab));
                    AssetDatabase.DeleteAsset(STRING_HOLDER_PATH);

                    findSkinnedMeshRenderer = true;
                }
            }

            if (!findSkinnedMeshRenderer)
            {
                List<Material> materials = CollectAll<Material>(materialpath);

                foreach (MeshRenderer smr in characterFBX.GetComponentsInChildren<MeshRenderer>(true))
                {
                    List<Object> toinclude = new List<Object>();

                    // skinned mesh
                    GameObject rendererClone = (GameObject)EditorUtility.InstantiatePrefab(smr.gameObject);
                    GameObject rendererParent = rendererClone.transform.parent.gameObject;
                    rendererClone.transform.parent = null;
                    Object.DestroyImmediate(rendererParent);
                    Object renderPrefab = GetPrefab(rendererClone, "rendererobject");
                    toinclude.Add(renderPrefab);

                    // material
                    log += ("\tMesh: " + smr.name + "\n");

                    log += "\t\tMaterial: \n";

                    foreach (Material m in materials)
                    {
                        if (m.name.Contains(smr.name.ToLower()))
                        {
                            toinclude.Add(m);

                            log += ("\t\t" + m.name + "\n");
                        }
                    }

                    // save the assetbundle
                    string bundleName = smr.name.ToLower();
                    string path = ASSETBUNDLE_PATH + bundleName + ASSETBUNDLE_SUFFIX;

                    bool result = false;
                    if (platform == PLATFORM.Win)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows);
                    }
                    else if (platform == PLATFORM.iOS)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.iOS);
                    }
                    else if (platform == PLATFORM.Android)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
                    }

                    if (!result)
                        Debug.LogWarning("Failed! Create AssetBundle: " + path);
                    //					else
                    //						Debug.Log("Success! Create AssetBundle: " + path);

                    ++processedBundleCount;

                    // delete temp assets
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(renderPrefab));
                    AssetDatabase.DeleteAsset(STRING_HOLDER_PATH);
                }
            }

            // animation
            {
                log += "Prepare Animation: \n";

				if ((Directory.Exists(animpath) && fullpath.IndexOf("_") == -1) ||
				    (Directory.Exists(animpath) && fullpath.IndexOf("wing") != -1))
                {
                    List<AnimationClip> acs = CollectAll<AnimationClip>(animpath);
                    List<Object> toinclude = new List<Object>();
                    List<string> animNames = new List<string>();

                    foreach (AnimationClip ac in acs)
                    {
                        toinclude.Add(ac);
                        animNames.Add(ac.name);

                        log += ("\t" + ac.name + "\n");
                    }

                    // animation information
                    StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
                    holder.content = animNames.ToArray();
                    AssetDatabase.CreateAsset(holder, STRING_HOLDER_PATH);
                    toinclude.Add(AssetDatabase.LoadAssetAtPath(STRING_HOLDER_PATH, typeof(StringHolder)));

                    // save the assetbundle
                    string bundleName = name + "_animations";
                    string path = ASSETBUNDLE_PATH + bundleName + ASSETBUNDLE_SUFFIX;

                    bool result = false;
                    if (platform == PLATFORM.Win)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows);
                    }
                    else if (platform == PLATFORM.iOS)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.iOS);
                    }
                    else if (platform == PLATFORM.Android)
                    {
                        result = BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
                    }

                    if (!result)
                        Debug.LogWarning("Failed! Create AssetBundle: " + path);
                    //					else
                    //						Debug.Log("Success! Create AssetBundle: " + path);

                    ++processedBundleCount;

                    AssetDatabase.DeleteAsset(STRING_HOLDER_PATH);
                }
            }

            ++processedBundleCount;

            log += "-----------------\n";

            Log(log);
        }

        if (processedBundleCount == 0)
        {
            //EditorUtility.DisplayDialog("AssetBundle Generator", "[ " + processedBundleCount + " ] been created. Select the Resource/Model/ folder to build assetbundle.", "OK");
			Debug.Log("AssetBundle Generator" + "[ " + processedBundleCount + " ] been created. Select the Resource/Model/ folder to build assetbundle.");
        }
        else
        {
            //			UpdateCharacterElementDatabase.Update();
            //EditorUtility.DisplayDialog("AssetBundle Generator", "[ " + processedBundleCount + " ] been created.", "OK");
			Debug.Log("AssetBundle Generator" +  "[ " + processedBundleCount + " ] been created.");
        }
    }

    static List<T> CollectAll<T>(string path) where T : Object
    {
        List<T> l = new List<T>();

        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (file.Contains(".meta")) continue;
                T asset = (T)AssetDatabase.LoadAssetAtPath(file, typeof(T));
                if (asset == null) throw new Exception("Asset is not " + typeof(T) + ": " + file);
                l.Add(asset);
            }
        }

        return l;
    }

    static Object GetPrefab(GameObject go, string name)
    {
		string windowspath = Application.dataPath.Substring(0, Application.dataPath.Length - 6) + TEMP_PREFAB_PATH;
		
		if (!Directory.Exists(windowspath))
		{
			Directory.CreateDirectory(windowspath);
			
			AssetDatabase.Refresh();
		}

		Object tempPrefab = EditorUtility.CreateEmptyPrefab(TEMP_PREFAB_PATH + name + ".prefab");
        tempPrefab = EditorUtility.ReplacePrefab(go, tempPrefab);
        GameObject.DestroyImmediate(go);

        return tempPrefab;
    }

    static void CreateAssetBundlePath()
    {
        if (!Directory.Exists(ASSETBUNDLE_PATH))
            Directory.CreateDirectory(ASSETBUNDLE_PATH);
    }

    static void DeleteExistAssetbundle(string name)
    {
        string[] files = Directory.GetFiles(ASSETBUNDLE_PATH);

        foreach (string file in files)
        {
            if (file.EndsWith(ASSETBUNDLE_SUFFIX) && file.Contains("/assetbundle/" + name))
                File.Delete(file);
        }
    }

    static void Log(string s)
    {
        //		Debug.Log(s);	
    }

    static void LogWarning(string s)
    {
        //		Debug.LogWarning(s);
    }

    static void LogError(string s)
    {
        //		Debug.LogError(s);
    }
}
