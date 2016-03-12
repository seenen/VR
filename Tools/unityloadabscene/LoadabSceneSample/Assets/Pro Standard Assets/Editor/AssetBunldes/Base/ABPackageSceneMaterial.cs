using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace AssetBundleEditor
{
    public class ABPackageSceneMaterial : IPackage
    {

        public ABPackageSceneMaterial(BatArg arg)
            : base(arg)
        {
        }

        public override void Package()
        {
            if (!mBatArg.IsValid())
                return;

            Debug.Log("---------- ABPackageSceneMaterial ----------");

            Debug.Log(mBatArg.Model);

            switch (mBatArg.Model)
            {
                case BatModel.SceneMaterial:
                    break;
                default:
                    Debug.LogError(mBatArg.Model);
                    return;
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
#elif UNITY_ANDROID
#endif
        }

        #region Package

        void PackageAdd(string SrcFolder, string DestFolder, BuildTarget target)
        {
            //  输入资源 
            string assetfolder = SrcFolder.Substring(SrcFolder.IndexOf("Assets"));

            Debug.Log(assetfolder);

            Object o = AssetDatabase.LoadAssetAtPath(assetfolder, typeof(Object));

            Selection.activeObject = o;

            switch (target)
            {
                case BuildTarget.iOS:
                    break;
                case BuildTarget.StandaloneWindows:
                    CreateFromPrefab.CreateFromPrefab_Scene(DestFolder, CreateFromPrefab.PLATFORM.Win);
                    break;
                case BuildTarget.Android:
                    break;
            }

            return;

        }



        #endregion

    }
}
