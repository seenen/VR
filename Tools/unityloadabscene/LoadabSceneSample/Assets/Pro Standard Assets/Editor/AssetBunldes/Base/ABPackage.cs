using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

namespace AssetBundleEditor
{
    public class ABPackage : IPackage
    {
        public ABPackage(BatArg arg)
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

            Debug.Log("---------- Model ----------" );

            Debug.Log(mBatArg.Model);

            switch (mBatArg.Model)
            {
                case BatModel.Normal:
                    break;
                default:
                    Debug.Log(mBatArg.Model);
                    return;
                    break;
            }

            Debug.Log("---------- NormalTranlate ----------");

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
                foreach (KeyValuePair<string, string> item in mBatArg.srcFolder2destFolderPC)
                {
                    PackageAdd(item.Key, item.Value, BuildTarget.Android);
                }

            }
#endif

        }

#region Index
        /////// <summary>
        /////// 处理索引 
        /////// </summary>
        ////void PrePackage(string SrcFolder, string DestFolder)
        ////{
        ////    //  创建文件夹 
        ////    if (!Directory.Exists(DestFolder))
        ////    {
        ////        Directory.CreateDirectory(DestFolder);
        ////    }

        ////    List<string> waitdo = PreprocessScr2DestFileList(SrcFolder, DestFolder);

        ////    //
        ////    if (File.Exists(DestFolder + "/.cfg"))
        ////    {
        ////        //  读取老的物件表 

        ////    }
        ////    else
        ////    {
        ////    }
        ////}

        /////// <summary>
        /////// 统计现在SrcFolder要处理的文件，转换成Assetbundle的文件List
        /////// </summary>
        /////// <param name="SrcFolder"></param>
        /////// <param name="DestFolder"></param>
        ////List<string> PreprocessScr2DestFileList(string SrcFolder, string DestFolder)
        ////{
        ////    //  记录现有Prefab文件即将转换成文件的所有文件路径
        ////    List<string> prefabs2assetbundle = new List<string>();

        ////    DirectoryInfo di = new DirectoryInfo(SrcFolder);

        ////    foreach (FileInfo fi in di.GetFiles())
        ////    {
        ////        //  判断
        ////        if (!fi.FullName.EndsWith(mBatArg.SearchPrefix))
        ////            continue;

        ////        //  输入资源 
        ////        string assefullname = fi.FullName.Substring(fi.FullName.IndexOf("Assets"));

        ////        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assefullname, typeof(UnityEngine.Object));

        ////        if (obj == null)
        ////            continue;

        ////        //
        ////        //  输出资源 
        ////        string targetPath = DestFolder + "/" + Path.GetFileNameWithoutExtension(fi.FullName) + ABConfig.extensionName;

        ////        prefabs2assetbundle.Add(targetPath);
        ////    }

        ////    return prefabs2assetbundle;

        ////}
#endregion

#region Package
        void PackageAdd(string SrcFolder, string DestFolder, BuildTarget target)
        {
            Debug.Log("AssetBundleEditor.ABPackage.PackageAdd " + SrcFolder + " -> " + DestFolder);

            DirectoryInfo di = new DirectoryInfo(SrcFolder);

            foreach (FileInfo fi in di.GetFiles())
            {
                Debug.Log("File " + fi.FullName);

                //  判断
                if (!fi.FullName.EndsWith(mBatArg.SearchPrefix))
                    continue;

                if (!IsPackage(fi.FullName, DestFolder, Path.GetFileName(fi.FullName)))
                {
                    Debug.Log(" MD5 Not Update ");

                    continue;
                }

                //  输入资源 
                string assefullname = fi.FullName.Substring(fi.FullName.IndexOf("Assets"));

                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assefullname, typeof(UnityEngine.Object));

                if (obj == null)
                {
                    Debug.LogError(assefullname + " [LoadAssetAtPath Failed] ");

                    continue;
                }

                //
                //  输出资源 
                string targetPath = DestFolder + "/" + Path.GetFileNameWithoutExtension(fi.FullName) + ABConfig.extensionName;

                bool suc = mgr.CreateAssetBunldesALLPlatform(obj, targetPath, target);

                if (!suc)
                {
                    Debug.LogError(fi.FullName + " [CreateAssetBundleOne Failed] " + targetPath);

                    continue;

                }
                else
                    Debug.Log("Create " + targetPath);

            }
        }
#endregion


    }

}
