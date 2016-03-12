using UnityEngine;
using System.Collections;
//using JsonFx.Json;
//using CmdEditor;
using System.IO;
using System.Collections.Generic;

namespace AssetBundleEditor
{
    public enum BatModel
    {
        Null = -1,
        Normal = 0,
        Scene,
		Charactor,
		SceneStream,
        SceneMaterial,
    }

    public enum DestHierarchy
    {
        Null = -1,
        Root = 0,   //  只输出到根目录 
        Relative = 1,   //  输出到对应的子目录 

    }

    public class BatArg
    {
        public BatArg() { }

        //  打包模式
        public BatModel Model = BatModel.Null;
        public DestHierarchy Hierarchy = DestHierarchy.Null;

        //  平台
        public bool PC = false;
        public bool IOS = false;
        public bool AND = false;

        //  源 
        public string SrcFolder = string.Empty;
        public string SrcFolderName = string.Empty;
        public bool SearchLoop = false;
        public string SearchPrefix = string.Empty;

        //  目标 
        public string DestFolderPath = string.Empty;
        public string DestFolderName = string.Empty;

        public BatArg(Dictionary<string, string> keys)
        {
            Model = (BatModel)(int.Parse(keys["Model"]));
            Hierarchy = (DestHierarchy)(int.Parse(keys["DestHierarchy"]));
            PC = keys.ContainsKey("PC");
            IOS = keys.ContainsKey("IOS");
            AND = keys.ContainsKey("AND");
            if (keys.TryGetValue("SrcFolder", out SrcFolder))	
			{
				SrcFolder = Application.dataPath + SrcFolder;
				SrcFolder.Replace("\\", "/");
			}
            if (keys.TryGetValue("SrcFolderName", out SrcFolderName)) SrcFolderName.Replace("\\", "/");
            SearchLoop = keys.ContainsKey("SearchLoop");
            if (keys.TryGetValue("SearchPrefix", out SearchPrefix))	SearchPrefix.Replace("\\", "/");
            if (keys.TryGetValue("DestFolder", out DestFolderPath))	
			{
				DestFolderPath = Application.dataPath + "/../.." + DestFolderPath;
				DestFolderPath.Replace("\\", "/");
			}
            if (keys.TryGetValue("DestFolderName", out DestFolderName))	DestFolderName.Replace("\\", "/");
			
//			Debug.Log(JsonWriter.Serialize(this));

        }

        //public BatArg(string commandline)
        //{
        //    Model = (BatModel)(int.Parse(CommandLineReader.GetCustomArgument("Model")));
        //    Hierarchy = (DestHierarchy)(int.Parse(CommandLineReader.GetCustomArgument("DestHierarchy")));
        //    PC = CommandLineReader.GetCustomArgument("PC") == "1" ? true : false;
        //    IOS = CommandLineReader.GetCustomArgument("IOS") == "1" ? true : false;
        //    AND = CommandLineReader.GetCustomArgument("AND") == "1" ? true : false;
        //    SrcFolder = CommandLineReader.GetCustomArgument("SrcFolder").Replace("\\", "/");
        //    SearchLoop = CommandLineReader.GetCustomArgument("SearchLoop") == "1" ? true : false;
        //    SearchPrefix = CommandLineReader.GetCustomArgument("SearchPrefix").Replace("\\", "/");
        //    DestFolderPath = CommandLineReader.GetCustomArgument("DestFolder").Replace("\\", "/");

        //    //Debug.Log(JsonConvert.DeserializeObject<BatArg>(this));
        //}

        string sError;

        public bool IsValid()
        {
            sError = string.Empty;

            do
            {
                if (Model == BatModel.Null)
                {
                    sError = Model.ToString();
                    break;
                }

                Debug.Log(SrcFolder + " Null");

                if (!Directory.Exists(SrcFolder))
                {
                    sError = "SrcFolder: " + SrcFolder;
                    break;
                }

                if (SrcFolder.StartsWith("Assets"))
                {
                    sError = SrcFolder + " No StartsWith Assets";
                    break;
                }
				
                if (!Directory.Exists(DestFolderPath))
                {
                    //sError = "DestFolder: " + DestFolderPath;
                    Directory.CreateDirectory(DestFolderPath);
					//break;
                }

                if (Hierarchy == DestHierarchy.Null)
                {
                    sError = Hierarchy.ToString();
                    break;
                }

                if (string.IsNullOrEmpty(DestFolderName))
                {
                    sError = "DestFolderName";
                    break;
                }
//
//                if (string.IsNullOrEmpty(SearchPrefix))
//                {
//                    sError = "SearchPrefix";
//                    break;
//                }

                if (!PC && !IOS && !AND )
                {
                    sError = "No Select Platform";
                    break;
                }

            }
            while (false);

            if (string.IsNullOrEmpty(sError))
                return true;

            Debug.LogError(sError);

            return false;
        }

        /// <summary>
        /// 预处理所有的路径和导出路径的指向 
        /// </summary>
        public void PreProcess()
        {
            Debug.Log("---------- PreProcess ----------");

#if UNITY_STANDALONE_WIN
            srcFolder2destFolderPC.Clear();
            if (PC)
            {
                Debug.Log("---------- PC Platform ----------");

                Process(SrcFolder, DestFolderPath + "/PC/" + DestFolderName, ref srcFolder2destFolderPC);
            }
#elif UNITY_IPHONE
            srcFolder2destFolderIOS.Clear();
            if (IOS) 
            {
                Debug.Log("---------- IOS Platform ----------");

                Process(SrcFolder, DestFolderPath + "/IOS/" + DestFolderName, ref srcFolder2destFolderIOS);
            }
#elif UNITY_ANDROID
            srcFolder2destFolderAND.Clear();
            if (AND) 
            {
                Debug.Log("---------- AND Platform ----------");

                Process(SrcFolder, DestFolderPath + "/Android/" + DestFolderName, ref srcFolder2destFolderAND);
            }
#endif
            return;
        }

        public Dictionary<string, string> srcFolder2destFolderPC = new Dictionary<string, string>();
        public Dictionary<string, string> srcFolder2destFolderIOS = new Dictionary<string, string>();
        public Dictionary<string, string> srcFolder2destFolderAND = new Dictionary<string, string>();

        void Process(string ScrFolder, string DestFolder, ref Dictionary<string, string> dic )
        {
            ScrFolder = ScrFolder.Replace("\\", "/");
            DestFolder = DestFolder.Replace("\\", "/");

            Debug.Log("AssetBundleEditor.BatArg.Process  " + ScrFolder + " -> " + DestFolder);

            dic.Add(ScrFolder, DestFolder);

            if (SearchLoop)
            {
                DirectoryInfo di = new DirectoryInfo(ScrFolder);

                foreach (DirectoryInfo ddi in di.GetDirectories())
                {
                    //Debug.Log("SrcDirectory " + ddi.FullName);

                    string ChildFolderName = DestFolder;

                    if (Hierarchy == DestHierarchy.Relative)
                    {
                        ChildFolderName = DestFolder + Path.GetFileName(ddi.FullName) + "/";

                        Debug.Log("CreateDirectory " + ChildFolderName);

                        //if (!Directory.Exists(ChildFolderName))
                        //    Directory.CreateDirectory(ChildFolderName);

                    }

                    Debug.Log("AssetBundleEditor.BatArg.Process  " + ddi.FullName + " -> " + ChildFolderName);

                    //dic.Add(ddi.FullName, ChildFolderName);

                    Process(ddi.FullName, ChildFolderName, ref dic);


                    continue;
                }

            }
        }
    }
}
