using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace AssetBundleEditor 
{

    public class ABPackageScene : IPackage
    {

        public ABPackageScene(BatArg arg)
            : base(arg)
        {
        }

        public override void Package()
        {
            if (!mBatArg.IsValid())
                return;

            Debug.Log("---------- Model ----------");

            Debug.Log(mBatArg.Model);

            switch (mBatArg.Model)
            {
                case BatModel.Scene:
                    break;
                default:
                    Debug.Log(mBatArg.Model);
                    return;
                    break;
            }

#if UNITY_STANDALONE_WIN
            if (mBatArg.PC)
            {
                foreach (KeyValuePair<string, string> item in mBatArg.srcFolder2destFolderPC)
                {
                    PackageAdd(item.Key, item.Value, BuildTarget.StandaloneWindows);
                }

            }
#elif UNITY_IPHONE
            if (mBatArg.IOS)
            {
                foreach (KeyValuePair<string, string> item in mBatArg.srcFolder2destFolderIOS)
                {
                    PackageAdd(item.Key, item.Value, BuildTarget.iPhone);
                }

            }
#elif UNITY_ANDROID
            if (mBatArg.AND)
            {
                foreach (KeyValuePair<string, string> item in mBatArg.srcFolder2destFolderAND)
                {
                    PackageAdd(item.Key, item.Value, BuildTarget.Android);
                }

            }
#endif

        }

#region Package
        void PackageAdd(string SrcFolder, string DestFolder, BuildTarget target)
        {
            DirectoryInfo di = new DirectoryInfo(SrcFolder);

            //  场景名
            string scenename = di.Name;

            //  场景prefab
            string sceneprefabpath = SrcFolder.Replace(scenename, "Prefabs") + "/" + scenename + ".prefab";
            if (!File.Exists(sceneprefabpath))
            {
                Debug.LogError("File is not Exist" + sceneprefabpath);
                return;
            }
            string scenenavprefabpath = SrcFolder.Replace(scenename, "Prefabs") + "/" + scenename + "_nav.prefab";
            if (!File.Exists(scenenavprefabpath))
            {
                Debug.LogError("File is not Exist" + scenenavprefabpath);
                return;
            }
            string scenelightmap = SrcFolder + "/LightmapFar-0.exr";
            if (!File.Exists(scenelightmap))
            {
                Debug.LogError("File is not Exist" + scenelightmap);
                return;
            }

            bool a = IsPackage(sceneprefabpath, DestFolder, Path.GetFileName(sceneprefabpath));
            bool b = IsPackage(scenenavprefabpath, DestFolder, Path.GetFileName(scenenavprefabpath));
            bool c = IsPackage(scenelightmap, DestFolder, scenename + "_" + Path.GetFileName(scenelightmap));


            if (!a && !b && !c)
            {
                Debug.Log(" MD5 Not Need Update ");
                return;
            }

            sceneprefabpath = sceneprefabpath.Substring(sceneprefabpath.IndexOf("Assets"));
            scenenavprefabpath = scenenavprefabpath.Substring(scenenavprefabpath.IndexOf("Assets"));
            scenelightmap = scenelightmap.Substring(scenelightmap.IndexOf("Assets"));

            SceneData sd = new SceneData();
            sd.scene = (GameObject)AssetDatabase.LoadAssetAtPath(sceneprefabpath, typeof(UnityEngine.Object));
            Decompose(sd.scene, ref sd, DestFolder, di.Parent.Name);
            sd.scene_nav = (GameObject)AssetDatabase.LoadAssetAtPath(scenenavprefabpath, typeof(UnityEngine.Object));
            sd.scene_Lightmap = (Texture2D)AssetDatabase.LoadAssetAtPath(scenelightmap, typeof(UnityEngine.Object));

            mgr.CreateAssetBundlesScene(sd, DestFolder, target);

            return;

        }

        //void PackageAdd(string SrcFolder, string DestFolder, BuildTarget target)
        //{
        //    DirectoryInfo di = new DirectoryInfo(SrcFolder);

        //    foreach (DirectoryInfo child in di.GetDirectories())
        //    {
        //        string childFolder = child.FullName;

        //        //  场景名
        //        string scenename = Path.GetFileName(childFolder);

        //        //  场景prefab
        //        string sceneprefabpath = childFolder + "/" + scenename + ".prefab";
        //        if (!File.Exists(sceneprefabpath))
        //        {
        //            Debug.LogError("File is not Exist" + sceneprefabpath);
        //            continue;

        //        }
        //        string scenenavprefabpath = childFolder + "/" + scenename + "_nav.prefab";
        //        if (!File.Exists(scenenavprefabpath))
        //        {
        //            Debug.LogError("File is not Exist" + scenenavprefabpath);
        //            continue;

        //        }
        //        string scenelightmap = childFolder + "/LightmapFar-0.exr";
        //        if (!File.Exists(scenelightmap))
        //        {
        //            Debug.LogError("File is not Exist" + scenelightmap);
        //            continue;

        //        }

        //        bool a = IsPackage(sceneprefabpath, DestFolder, Path.GetFileName(sceneprefabpath));
        //        bool b = IsPackage(scenenavprefabpath, DestFolder, Path.GetFileName(scenenavprefabpath));
        //        bool c = IsPackage(scenelightmap, DestFolder, scenename + "_" + Path.GetFileName(scenelightmap));
				
				
        //        if ( !a && !b && !c	)
        //        {
        //            Debug.Log(" MD5 Not Need Update ");

        //            continue;
        //        }

        //        sceneprefabpath = sceneprefabpath.Substring(sceneprefabpath.IndexOf("Assets"));
        //        scenenavprefabpath = scenenavprefabpath.Substring(scenenavprefabpath.IndexOf("Assets"));
        //        scenelightmap = scenelightmap.Substring(scenelightmap.IndexOf("Assets"));

        //        SceneData sd = new SceneData();
        //        sd.scene = (GameObject)AssetDatabase.LoadAssetAtPath(sceneprefabpath, typeof(UnityEngine.Object));
        //        Decompose(sd.scene, ref sd, DestFolder);
        //        sd.scene_nav = (GameObject)AssetDatabase.LoadAssetAtPath(scenenavprefabpath, typeof(UnityEngine.Object));
        //        sd.scene_Lightmap = (Texture2D)AssetDatabase.LoadAssetAtPath(scenelightmap, typeof(UnityEngine.Object));

        //        mgr.CreateAssetBundlesScene(sd, DestFolder, target);

        //    }

        //}



#endregion

#region chai
        void Decompose(Object go, ref SceneData sd, string DestFolder, string MaterialFolder)
        {
            ABMgr mgr = new ABMgr();

            mgr.DecomposePre(go, DestFolder, MaterialFolder);

            sd.scene_nomaterial = mgr.nomaterialscene;
            sd.scene_material_asset = mgr.materialasset;

            mgr.DecomposeEnd();
        }
#endregion
    }

}
