using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AssetBundleEditor
{
    public class ABWindow : EditorWindow
    {
        [MenuItem("Tool/AssetBundle/Single", false)]
        public static void ABWindowMain()
        {
            thisWindow = (ABWindow)EditorWindow.GetWindow(typeof(ABWindow));
            thisWindow.title = "BundleWindow";
            thisWindow.Show();
        }
        static ABWindow thisWindow;

        static ABMgr m_BundleMgr
		{
			get
			{
				if (_abmgr == null)
					_abmgr = new ABMgr();
				
				return _abmgr;
			}
		}
		
		static ABMgr _abmgr;

        void Load()
        {
            if (listDatas == null)
                listDatas = new List<string>();
       }

        void Unload()
        {
            Caching.CleanCache();

            if (listDatas != null) { listDatas.Clear(); listDatas = null; }
            if (_abmgr != null) { _abmgr.Close(); _abmgr = null; }
        }

        string platformSel;

        List<string> listDatas = new List<string>();

        public GUISkin skin;

        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {
            if (skin == null)
                skin = new GUISkin();

            GUILayout.Box("Zhang Shining Publish at 2013-12-27 17:35", skin.FindStyle("Box"), GUILayout.ExpandWidth(false));

            BundleObject();

            BundleShow();

        }

        void BundleObject()
        {
            GUILayout.Box("Pack Bundle To(/AssetBunldes/Output/*.*)", GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Pack A Bundle(ALL.assetsbundle)", GUILayout.ExpandWidth(true)))
                {
					string path = EditorUtility.OpenFolderPanel("Select Saved Path", "Assets", "ALL");
					
					if (!string.IsNullOrEmpty(path))
                    	m_BundleMgr.CreateAssetBunldesALL(path);
                }

                if (GUILayout.Button("Pack More Bundles", GUILayout.ExpandWidth(true)))
                {
                    string path = EditorUtility.OpenFolderPanel("Select Saved Path", "Assets", "ALL");
					
					if (!string.IsNullOrEmpty(path))
                   		m_BundleMgr.ExecCreateAssetBunldes(path);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Box("Unpack Bundle From(/AssetBunldes/Output/*.*)", GUILayout.ExpandWidth(true));// .Width(300));

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Unpack Selection AssetsBundles", GUILayout.ExpandWidth(true)))
                {
                    List<string> fullnames = new List<string>();

                    foreach (Object e in Selection.objects)
                    {
                        //  sub "Assets"
                        string prefixpath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);

                        string path = prefixpath + AssetDatabase.GetAssetOrScenePath(e);

                        fullnames.Add(path);
                    }
                    listDatas = m_BundleMgr.Reload(fullnames);
                }
            }
            GUILayout.EndHorizontal();
        }

        void BundleShow()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));

            foreach (string e in listDatas)
            {
                GUILayout.Label(e, GUILayout.ExpandWidth(true));
            }

            GUILayout.EndScrollView();
        }


        void OnInspectorUpdate() { Repaint(); }

        void OnFocus() { Load(); }

        void OnDisable() { Unload(); }
    }
}