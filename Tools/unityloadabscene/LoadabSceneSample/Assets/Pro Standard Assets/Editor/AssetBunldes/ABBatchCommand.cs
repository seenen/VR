using UnityEngine;

using System.Collections.Generic;

namespace AssetBundleEditor
{

    class ABBatch
    {
        public static string mInFolder;
        public static string mOutFolder;

        public static BatArg mBatArg = new BatArg();

        #region Normal
        public static void EntryNormalTest()
        {
            string param = @"-CustomArgs:
                            Model=0,    
                            DestHierarchy=0,    
                            PC=1,
                            IOS=1,
                            AND=1, 
                            SrcFolder=ModelsNGUI/Resources/NGUIPrefabs,
                            SearchLoop=1,
                            SearchPrefix=.prefab,
                            DestFolder=E:/Heros/GameEditors/StreamingAssetsBat";

            mBatArg.Model = BatModel.Normal;
            mBatArg.Hierarchy = DestHierarchy.Root;
            mBatArg.PC = true;
            mBatArg.IOS = true;
            mBatArg.AND = true;
            mBatArg.SrcFolder = "Assets/ModelsNGUI/Resources/NGUIPrefabs/";
            mBatArg.SearchLoop = true;
            mBatArg.SearchPrefix = ".prefab";
            mBatArg.DestFolderPath = "E:/Heros/GameEditors/StreamingAssetsBat/";
            mBatArg.DestFolderName = "ngui";

            ABPackage package = new ABPackage(mBatArg);
        }

        public static void ReadNormal(List<string> listDatas)
        {
            int i = 0;

            //  每一行 
            foreach (string s in listDatas)
            {
                EntryPoint("Normal_" + (i++).ToString());

                Debug.Log("----------Begin ----------" + s);

                Dictionary<string, string> dic = ReadLine(s);

                BatArg arg = new BatArg(dic);

                ABPackage package = new ABPackage(arg);

                if (!package.IsValid())
                {
                    Debug.LogError("package.IsValid");
                }
                else
                {
                    package.Package();
                }
                Debug.Log("----------End----------");

            }

        }
        #endregion

        #region Scene
        public static void ReadScene(List<string> listDatas)
        {
            int i = 0;

            //  每一行 
            foreach (string s in listDatas)
            {
                EntryPoint("Scene_" + (i++).ToString());

                Debug.Log("----------Begin ReadScene ----------" + s);

                Dictionary<string, string> dic = ReadLine(s);

                BatArg arg = new BatArg(dic);

                ABPackageScene package = new ABPackageScene(arg);

                package.Package();

                Debug.Log("----------End ReadScene ----------");

            }
        }

		public static void ReadSceneStream(List<string> listDatas)
		{
			int i = 0;
			
			//  ??? 
			foreach (string s in listDatas)
			{
				EntryPoint("SceneStream_" + (i++).ToString());
				
				Debug.Log("----------Begin SceneStream ----------" + s);
				
				Dictionary<string, string> dic = ReadLine(s);
				
				BatArg arg = new BatArg(dic);
				
				ABPackageStreamimgScene package = new ABPackageStreamimgScene(arg);
				
				package.Package();
				
				Debug.Log("----------End SceneStream ----------");
				
			}
		}

        #endregion

        public static void ReadCharactor(List<string> listDatas)
        {
            int i = 0;

            //  每一行 
            foreach (string s in listDatas)
            {
                EntryPoint("Normal_" + (i++).ToString());

                Debug.Log("----------Begin ----------" + s);

                Dictionary<string, string> dic = ReadLine(s);

                BatArg arg = new BatArg(dic);

                ABPackageCharactor package = new ABPackageCharactor(arg);

                if (!package.IsValid())
                {
                    Debug.LogError("package.IsValid");
                }
                else
                {
                    package.Package();
                }
                Debug.Log("----------End----------");
            }
        }

        #region Command
        private const char CUSTOM_ARGS_SEPARATOR = ',';

        static Dictionary<string, string> ReadLine(string line)
        {
            Dictionary<string, string> customArgsDict = new Dictionary<string, string>();

            string[] customArgs = line.Split(CUSTOM_ARGS_SEPARATOR);

            foreach (string customArg in customArgs)
            {
                string[] customArgBuffer = customArg.Split('=');
                if (customArgBuffer.Length == 2)
                {
                    customArgsDict.Add(customArgBuffer[0], customArgBuffer[1]);
                }
                else
                {
                    customArgsDict.Add(customArg, "");
                }
            }

            return customArgsDict;
        }

        static void HandleLog(string logString, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    //  Log
                    ABFileLog.Write(LogFileName, type, logString);
                    break;
                default:
                    ABFileLog.Write(LogFileName, type, logString);
                    ABFileLog.Write(LogFileName, type, stackTrace);
                    break;

            }

        }

        static string LogFileName;

        static void EntryPoint(string logname)
        {
            LogFileName = logname;

            Application.RegisterLogCallback(HandleLog);
        }
        #endregion
    }
}
