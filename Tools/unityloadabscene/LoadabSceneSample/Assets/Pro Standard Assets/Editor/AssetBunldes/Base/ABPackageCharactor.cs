using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace AssetBundleEditor 
{

    public class ABPackageCharactor : IPackage
    {

        public ABPackageCharactor(BatArg arg)
            : base(arg)
        {
        }

        public bool IsValid()
        {
            return mBatArg.IsValid();
        }

        public override void Package()
        {
            if (!mBatArg.IsValid())
                return;

            Debug.Log("---------- ABPackageCharactor ----------");

            Debug.Log(mBatArg.Model);

            switch (mBatArg.Model)
            {
                case BatModel.Charactor:
                    break;
                default:
                    Debug.LogError(mBatArg.Model);
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
            //  输入资源 
            string assetfolder = SrcFolder.Substring(SrcFolder.IndexOf("Assets"));

            Debug.Log(assetfolder);

            Object o = AssetDatabase.LoadAssetAtPath(assetfolder, typeof(Object));

            Selection.activeObject = o;
			
			switch(target)
			{
			case BuildTarget.iOS:
				CreateAssetbundles.CreateAssetBundle_Character(DestFolder, CreateAssetbundles.PLATFORM.iOS);
				break;
			case BuildTarget.StandaloneWindows:
				CreateAssetbundles.CreateAssetBundle_Character(DestFolder, CreateAssetbundles.PLATFORM.Win);
				break;
			case BuildTarget.Android:
				CreateAssetbundles.CreateAssetBundle_Character(DestFolder, CreateAssetbundles.PLATFORM.Android);
				break;
			}
			
            return;

        }



#endregion

    }

}
