using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace AssetBundleEditor 
{

    public class ABPackageStreamimgScene : IPackage
    {

        public ABPackageStreamimgScene(BatArg arg)
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
                case BatModel.SceneStream:
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

            foreach (FileInfo fi in di.GetFiles())
            {
                //  ≈–∂œ
                if (!fi.FullName.EndsWith(mBatArg.SearchPrefix))
                    continue;

                Debug.Log("File " + fi.FullName);

                if (!Directory.Exists(DestFolder))
                    Directory.CreateDirectory(DestFolder);

                //   ‰»Î◊ ‘¥ 
                string assefullname = fi.FullName.Substring(fi.FullName.IndexOf("Assets"));

                string[] levels = new string[1];
                levels[0] = assefullname;

                Debug.Log(assefullname + " " + DestFolder + " " + target);

                mgr.CreateStreamedSceneAssetBundle(levels, DestFolder + "/" + Path.GetFileNameWithoutExtension(fi.FullName), target);

            }

        }



#endregion

    }

}
