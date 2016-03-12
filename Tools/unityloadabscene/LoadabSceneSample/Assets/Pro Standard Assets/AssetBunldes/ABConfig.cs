using UnityEngine;

namespace AssetBundleEditor
{

    public sealed class ABConfig
    {
        public static string OutputFolderWindows32 = Application.dataPath + "/Models/Bundle/PC/";
        public static string OutputFolderIPhone = Application.dataPath + "/Models/Bundle/MAC/";
        public static string OutputFolderAndroid = Application.dataPath + "/Models/Bundle/ANDROID/";

        public static string extensionName = ".assetBundles";

        public static string OutputFolderSceneWindows = Application.dataPath + "/Models/Bundle/PC/";
        public static string OutputFolderSceneAndroid = Application.dataPath + "/Models/Bundle/ANDROID/";
        public static string OutputFolderSceneIOS = Application.dataPath + "/Models/Bundle/MAC/";

        public static string PrefixPlatform = 
#if UNITY_STANDALONE_WIN
        "file:///";
#elif UNITY_IPHONE
		#if UNITY_EDITOR
		"file:///";
		#else
		"file://";
		#endif
#elif UNITY_ANDROID
#if UNITY_EDITOR
        "file:///";
        #else
		true ? "jar:file://" : "file://";
		#endif
#else
		"Not valid Platform";
#endif

}

    public class SceneData
    {
        public GameObject scene;
        public GameObject scene_nomaterial;
        public GameObject scene_nav;
        public GameObject scene_Camera_custom;
        public Texture2D scene_Lightmap;
        public StringPrefab scene_material_asset;

        public bool IsValid(out string errorcode)
        {
            errorcode = "no error";

            if (!scene)
            {
                errorcode = "no scene";
                return false;
            }

            if (!scene_nav || 
                !scene_nav.name.EndsWith("_nav") ||
                !scene_nav.name.StartsWith(scene.name))
            {
                errorcode = "no scene_nav";
                return false;
            }

            if (scene_Camera_custom != null )
            {
                if (scene_Camera_custom.name.EndsWith("_camera") ||
                    !scene_Camera_custom.name.StartsWith(scene.name))
                {
                    errorcode = "error scene_camera";
                    return false;
                }
            }

            if (!scene_Lightmap || 
                !scene_Lightmap.name.StartsWith("LightmapFar-")
                )
            {
                errorcode = "no scene_Lightmap";
                return false;
            }

            return true;
        }
    }
}