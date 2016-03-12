using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace AssetBundleEditor
{

    public class ABBatchWindow : EditorWindow 
    {
        [MenuItem("Tool/AssetBundle/Batch", false)]
        public static void Main()
        {
            thisWindow = (ABBatchWindow)EditorWindow.GetWindow(typeof(ABBatchWindow));
            thisWindow.title = "ABBatchWindow";
            thisWindow.Show();
        }

        static ABBatchWindow thisWindow;



        void OnGUI()
        {
            BundleSingleBatch();

            BundleSceneBatch();

            BundleSceneCharactor();

			BundleSceneStream();
        }

        List<string> listSceneBatch = new List<string>();
        Vector2 scrollPos = Vector2.zero;

        void BundleSceneBatch()
        {
            TextAsset Scene = AssetDatabase.LoadAssetAtPath("Assets/Models/ABFScene.txt", typeof(TextAsset)) as TextAsset;

            if (Scene == null)
                return;

            listSceneBatch.Clear();
            listSceneBatch = ABTools.GetContent(new MemoryStream(Scene.bytes));

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));

            foreach (string e in listSceneBatch)
            {
                GUILayout.Label(e, GUILayout.ExpandWidth(true));
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pack Scene Bundles", GUILayout.ExpandWidth(true)))
            {
                ABBatch.ReadScene(listSceneBatch);

            }
            GUILayout.EndHorizontal();
        }

		List<string> listSceneStreamBatch = new List<string>();
		Vector2 scrollPosSceneSteam = Vector2.zero;
		
		void BundleSceneStream()
		{
			TextAsset Scene = AssetDatabase.LoadAssetAtPath("Assets/Models/ABFSceneStream.txt", typeof(TextAsset)) as TextAsset;
			
			listSceneStreamBatch.Clear();
			listSceneStreamBatch = ABTools.GetContent(new MemoryStream(Scene.bytes));
			
			scrollPosSceneSteam = GUILayout.BeginScrollView(scrollPosSceneSteam, GUILayout.ExpandWidth(true));
			
			foreach (string e in listSceneStreamBatch)
			{
				GUILayout.Label(e, GUILayout.ExpandWidth(true));
			}
			
			GUILayout.EndScrollView();
			
			GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pack StreamScene Bundle", GUILayout.ExpandWidth(true)))
			{
				ABBatch.ReadSceneStream(listSceneStreamBatch);
				
			}
			GUILayout.EndHorizontal();
		}

        List<string> listSingleBatch = new List<string>();
        Vector2 scrollPosSingle = Vector2.zero;

        void BundleSingleBatch()
        {
            TextAsset Scene = AssetDatabase.LoadAssetAtPath("Assets/Models/ABSingle.txt", typeof(TextAsset)) as TextAsset;

            if (Scene == null)
                return;

            listSingleBatch.Clear();
            listSingleBatch = ABTools.GetContent(new MemoryStream(Scene.bytes));

            scrollPosSingle = GUILayout.BeginScrollView(scrollPosSingle, GUILayout.ExpandWidth(true));

            foreach (string e in listSingleBatch)
            {
                GUILayout.Label(e, GUILayout.ExpandWidth(true));
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pack Single Bundle", GUILayout.ExpandWidth(true)))
            {
                ABBatch.ReadNormal(listSingleBatch);

            }
            GUILayout.EndHorizontal();
        }

        List<string> listCharactorBatch = new List<string>();
        Vector2 scrollPosCharactor = Vector2.zero;

        void BundleSceneCharactor()
        {
            TextAsset Scene = AssetDatabase.LoadAssetAtPath("Assets/Models/ABCharactor.txt", typeof(TextAsset)) as TextAsset;

            if (Scene == null)
                return;

            listCharactorBatch.Clear();
            listCharactorBatch = ABTools.GetContent(new MemoryStream(Scene.bytes));

            scrollPosCharactor = GUILayout.BeginScrollView(scrollPosCharactor, GUILayout.ExpandWidth(true));

            foreach (string e in listCharactorBatch)
            {
                GUILayout.Label(e, GUILayout.ExpandWidth(true));
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pack Charactor Bundle", GUILayout.ExpandWidth(true)))
            {
                ABBatch.ReadCharactor(listCharactorBatch);

            }
            GUILayout.EndHorizontal();
        }


    }
}

