using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace AssetBundleEditor
{
    public class ABSelfTestWindow : EditorWindow
    {
        private class StatErrorData
        {
            public string filename;
            public string errortext;
        }

        //public string EndFix = ".assetBundles|.assetbundle";

        private List<StatErrorData> statistics = new List<StatErrorData>();

        [MenuItem("Tool/AssetBundle/SelfTest")]
        public static void ShowWindow()
        {
            ABSelfTestWindow window = (ABSelfTestWindow)EditorWindow.GetWindow(typeof(ABSelfTestWindow));

            window.position = new Rect(0, 0, 960, 640);
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            if (GUILayout.Button("select", GUILayout.Width(100)))
            {
                SrcFolder = EditorUtility.OpenFolderPanel("Select Saved Path", SrcFolder, "Heros");

                srcFolder2destFolderPC.Clear();

                if (!string.IsNullOrEmpty(SrcFolder))
                {
                    Debug.Log(SrcFolder);
                }

            }

            GUILayout.Label("【文件夹路径:】" + SrcFolder, GUILayout.ExpandWidth(false));

            if (!string.IsNullOrEmpty(SrcFolder) &&
                srcFolder2destFolderPC.Count == 0)
            {
                anlyzeFile();
            }

            GUILayout.Label("【文件夹（包括子文件夹）数量:】" + srcFolder2destFolderPC.Count.ToString(), GUILayout.ExpandWidth(false));

            EditorGUILayout.Space();

            SufixShow();


            if (string.IsNullOrEmpty(SrcFolder))
                return;

            anlyzeOp();

            anlyzeShow();
        }

        #region 分析路径阶段.
        Vector2 scrollPos = Vector2.zero;

        private void anlyzeFile()
        {
            if (string.IsNullOrEmpty(SrcFolder))
                return;

            if (srcFolder2destFolderPC.Count == 0)
            {
                statistics.Clear();
                listSufix.Clear();
                Process(SrcFolder, SrcFolder + "/Export", ref srcFolder2destFolderPC);
            }

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.Height(250));

            EditorGUILayout.BeginVertical();
            foreach (KeyValuePair<string, string> item in srcFolder2destFolderPC)
            {
                string srcFilePath = (string)item.Key;

                DirectoryInfo di = new DirectoryInfo(srcFilePath);

                foreach (FileInfo fi in di.GetFiles())
                {
                    if (!listSufix.ContainsKey(Path.GetExtension(fi.FullName)))
                    {
                        SufixData data = new SufixData();
                        data.select = true;
                        data.suffix = Path.GetExtension(fi.FullName);
                        data.count++;

                        listSufix.Add(Path.GetExtension(fi.FullName), data);
                    }
                    else
                    {
                        SufixData data = listSufix[Path.GetExtension(fi.FullName)];
                        data.count++;
                    }

                    //if (!fi.FullName.EndsWith(".assetBundles"))
                    //    continue;

                    GUILayout.Label("    " + fi.FullName);
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.EndScrollView();

        }

        Dictionary<string, SufixData> listSufix = new Dictionary<string, SufixData>();
        Vector2 scrollPosSufix = Vector2.zero;
        int indexSufix = 0;
        private class SufixData
        {
            public bool select = false;
            public string suffix = "";
            public int count = 0;
        };

        void SufixShow()
        {
            if (listSufix.Count == 0)
                return;

            //indexSufix = ABTools.DrawList("", listSufix.ToArray(), indexSufix);
            scrollPosSufix = GUILayout.BeginScrollView(scrollPosSufix, GUILayout.ExpandWidth(true), GUILayout.Height(200));
            foreach (KeyValuePair<string, SufixData> item in listSufix)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
                    item.Value.select = GUILayout.Toggle(item.Value.select, "选中检测");

                    GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUILayout.Label("有:    " + item.Value.count.ToString() + "  个  " + item.Value.suffix);
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        bool IsOKSuffix(string suffix)
        {
            SufixData data = null;

            if (listSufix.TryGetValue(suffix, out data))
            {
                return data.select;
            }

            return false;
        }

        public Dictionary<string, string> srcFolder2destFolderPC = new Dictionary<string, string>();
        string SrcFolder = "";
        void Process(string ScrFolder, string DestFolder, ref Dictionary<string, string> dic)
        {
            ScrFolder = ScrFolder.Replace("\\", "/");
            DestFolder = DestFolder.Replace("\\", "/");

            Debug.Log("AssetBundleEditor.BatArg.Process  " + ScrFolder + " -> " + DestFolder);

            dic.Add(ScrFolder, DestFolder);

            if (true)
            {
                DirectoryInfo di = new DirectoryInfo(ScrFolder);

                foreach (DirectoryInfo ddi in di.GetDirectories())
                {
                    //Debug.Log("SrcDirectory " + ddi.FullName);

                    string ChildFolderName = DestFolder;

                    Debug.Log("AssetBundleEditor.BatArg.Process  " + ddi.FullName + " -> " + ChildFolderName);

                    //dic.Add(ddi.FullName, ChildFolderName);

                    Process(ddi.FullName, ChildFolderName, ref dic);


                    continue;
                }

            }
        }
        #endregion

        #region 分析AB对象阶段.
        bool deeply = false;
        int progress = 0;
        int secs = 0;
        Queue<string> listFiles = new Queue<string>();

        private void anlyzeOp()
        {
            deeply = GUILayout.Toggle(deeply, "是否深度分析(加载包内资源对象)?");

            if (GUILayout.Button("analyze", GUILayout.Width(100)))
            {
                listFiles.Clear();
                statistics.Clear();
                foreach (KeyValuePair<string, string> item in srcFolder2destFolderPC)
                {
                    string srcFilePath = (string)item.Key;

                    DirectoryInfo di = new DirectoryInfo(srcFilePath);

                    foreach (FileInfo fi in di.GetFiles())
                    {
                        if (!IsOKSuffix(Path.GetExtension(fi.FullName)))
                            continue;

                        listFiles.Enqueue(fi.FullName);
                    }
                }

                progress = 0;
                secs = listFiles.Count;
                int count = 0;

                while (listFiles.Count > 0)
                {
                    string filename = listFiles.Dequeue();

                    progress++;

                    if (EditorUtility.DisplayCancelableProgressBar("AssetBundle加载进度窗口 " + progress.ToString() + "/" + secs.ToString(),
                                                                    "当前加载: " + Path.GetFileName(filename), 
                                                                    (float)progress / (float)secs))
                    {
                        EditorUtility.ClearProgressBar();

                        break;
                    }

                    StatErrorData data = analyzeAssetBundle(filename);

                    statistics.Add(data);

                    if (data.errortext != "no error")
                        count++;

                }

                EditorUtility.ClearProgressBar();

                EditorUtility.DisplayDialog("Assetbundle统计结果",
                            "Error Count = " + count.ToString(),
                            "关闭");

            }
        }

        Vector2 scrollPosanlyzeShow = Vector2.zero;

        private void anlyzeShow()
        {
            scrollPosanlyzeShow = GUILayout.BeginScrollView(scrollPosanlyzeShow, GUILayout.ExpandWidth(true));
            foreach (StatErrorData data in statistics)
            {
                if (data.errortext == "no error")
                    continue;


                EditorGUILayout.BeginHorizontal();
                {
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
                    GUILayout.Label("    " + data.filename);

                    GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                    GUILayout.Label("    " + data.errortext);
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

        }

        StatErrorData LogData = null;

        private StatErrorData analyzeAssetBundle(string assetPath)
        {
            if (!IsOKSuffix(Path.GetExtension(assetPath)))
                return null;

            StatErrorData data = new StatErrorData();
            data.filename = assetPath;
			WWW www = null;

            try
            {
                string escName = WWW.EscapeURL("file:///" + assetPath);
                Debug.Log(escName);

                LogData = data;

                Application.RegisterLogCallback(HandleLog);

                www = new WWW("file:///" + assetPath);
                //WWW www = WWW.LoadFromCacheOrDownload("file:///" + assetPath, 0);

                if (!string.IsNullOrEmpty(www.error))
                {
                    data.errortext = www.error;

                }
                else
                {
                    data.errortext = "no error";

                    if (deeply &&
                        www.assetBundle != null)
                    {
                        Object[] loadedObjects = www.assetBundle.LoadAllAssets();
                        if (loadedObjects == null)
                            data.errortext = "all = null";

                        if (!string.IsNullOrEmpty(www.error))
                        {
                            data.errortext = www.error;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                data.errortext = e.Message;
            }

			if (www.assetBundle != null)
				www.assetBundle.Unload(true);
			
			www.Dispose();

            Application.RegisterLogCallback(null);
            LogData = null;
            

            return data;
        }

        void HandleLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Warning)
            {
                LogData.errortext += "\n";
                LogData.errortext += message;
            }
        }

        #endregion

        void OnInspectorUpdate() { Repaint(); }
    }
}
