using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace AssetBundleEditor
{

    public class ABMgr
    {
#region WatchBundle
        public Dictionary<string, ABLoad> allbundleloads = new Dictionary<string, ABLoad>();

        /// <summary>
        /// 根据bundle的全路径，来加载bundles 
        /// </summary>
        /// <param name="bundlepathname"></param>
        /// <returns></returns>
        public Object[] LoadBundles(string bundlepathname)
        {
            ABLoadFile load = null;

            if (allbundleloads.ContainsKey(bundlepathname))
            {
                load = (ABLoadFile)allbundleloads[bundlepathname];

                Debuger.Log(bundlepathname);

            }
            else
            {
                string targetPath = bundlepathname;

                // AssetBundle 存储路径
                Debuger.Log(targetPath);

                WWW www = new WWW(bundlepathname);

                load = new ABLoadFile(0);
                load.Create(targetPath);

                allbundleloads[bundlepathname] = load;
            }

            Object[] all = load.LoadAll();

            return all;

        }

        public void Close()
        {
            foreach (ABLoad e in allbundleloads.Values)
            {
                ABLoad ab = (ABLoad)e;
                ab.Dispose();
                ab = null;
            }

            allbundleloads.Clear();
        }

        List<string> listDatas = new List<string>();

        public List<string> Reload(List<string> fullnames)
        {
            Close();
            listDatas.Clear();

            foreach (string fi in fullnames)
            {
//                if (!fi.EndsWith(".assetBundles"))
//                    continue;

                listDatas.Add(Path.GetFileName(fi));

                foreach (Object e in LoadBundles("file://" + fi))
                {
                    Debuger.Log("file://" + fi + "   -> " + e.name);

                    listDatas.Add("\t" + e.name + "\t[" + e.GetType() + "] ");
                }
            }

            return listDatas;
        }
#endregion	

#region Create
		public void CreateAssetBunldesALL(string path)
		{
			Caching.CleanCache();

			
            Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            AssetDatabase.Refresh();

#if UNITY_STANDALONE_WIN
            string targetPath = path + "/ALL" + ABConfig.extensionName;

            CreateAssetBunldesALL(SelectedAsset, targetPath, BuildTarget.StandaloneWindows);
#elif UNITY_IPHONE
            string targetPath = path + "/ALL" + ABConfig.extensionName;

            CreateAssetBunldesALL(SelectedAsset, targetPath, BuildTarget.iPhone);
#elif UNITY_ANDROID
            string targetPath = path + "/ALL" + ABConfig.extensionName;

            CreateAssetBunldesALL(SelectedAsset, targetPath, BuildTarget.Android);
#endif
		}
		
        public void CreateAssetBunldesALL()
        {
            Caching.CleanCache();

            Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            AssetDatabase.Refresh();

            // AssetBundle 存储路径
#if UNITY_IPHONE || UNITY_STANDALONE_OSX
            string targetPath = ABConfig.OutputFolderIPhone + "IOS/ALL" + ABConfig.extensionName;

            CreateAssetBunldesALL(SelectedAsset, targetPath, BuildTarget.iPhone);
#elif UNITY_STANDALONE_WIN
            string targetPath = ABConfig.OutputFolderWindows32 + "PC/ALL" + ABConfig.extensionName;

            CreateAssetBunldesALL(SelectedAsset, targetPath, BuildTarget.StandaloneWindows);
#elif UNITY_ANDROID
            string targetPath = ABConfig.OutputFolderAndroid + "AND/ALL" + ABConfig.extensionName;

            CreateAssetBunldesALL(SelectedAsset, targetPath, BuildTarget.Android);
#endif	//	UNITY_STANDALONE_WIN

        }
		
        public void ExecCreateAssetBunldes(string path)
		{
            //取得在 Project 视图中选择的资源(包含子目录中的资源)
            Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (SelectedAsset == null)
            {
                EditorUtility.DisplayDialog("AssetBunlde", "Nothing is Selected", "Close");

                return;
            }

            foreach (Object obj in SelectedAsset)
            {
                if (ABTools.MissingMono(obj)) break;

            	string targetPath = path + "/" + obj.name + ABConfig.extensionName;
				
				Debuger.Log(targetPath);
				
                if (File.Exists(targetPath)) File.Delete(targetPath);

#if UNITY_IPHONE || UNITY_STANDALONE_OSX
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(	obj, 
													null, 
													targetPath, 
													BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
													BuildTarget.iPhone))
                {
                    Debuger.Log(targetPath + "建立完成");
                }
                else
                {
                    Debuger.LogError(obj.name + "建立失败");
                }
#elif UNITY_STANDALONE_WIN
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(	obj, 
													null, 
													targetPath, 
													BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
													BuildTarget.StandaloneWindows))
                {
                    Debuger.Log(targetPath + "建立完成");

                }
                else
                {
                    Debuger.LogError(obj.name + "建立失败");
                }
#elif UNITY_ANDROID
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(	obj, 
													null, 
													targetPath, 
													BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
													BuildTarget.Android))
                {
                    Debuger.Log(targetPath + "建立完成");

                }
                else
                {
                    Debuger.LogError(obj.name + "建立失败");
                }
#endif	//	UNITY_STANDALONE_WIN
				AssetDatabase.Refresh();

            }

            EditorUtility.DisplayDialog("AssetBunlde", "BuildTarget.StandaloneWindows Over", "Close");
		}
		
        public void ExecCreateAssetBunldes()
        {
            //取得在 Project 视图中选择的资源(包含子目录中的资源)
            Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (SelectedAsset == null)
            {
                EditorUtility.DisplayDialog("AssetBunlde", "Nothing is Selected", "Close");

                return;
            }

            foreach (Object obj in SelectedAsset)
            {

                if (ABTools.MissingMono(obj)) continue;

#if UNITY_IPHONE
            string targetPath = ABConfig.OutputFolderIPhone + obj.name + ABConfig.extensionName;
#elif UNITY_STANDALONE_WIN
            string targetPath = ABConfig.OutputFolderWindows32 + obj.name + ABConfig.extensionName;
#elif UNITY_ANDROID
            string targetPath = ABConfig.OutputFolderAndroid + obj.name + ABConfig.extensionName;
#else
            string targetPath = ABConfig.OutputFolderWindows32 + obj.name + ABConfig.extensionName;
#endif	//	UNITY_STANDALONE_WIN

                if (File.Exists(targetPath)) File.Delete(targetPath);

#if UNITY_IPHONE
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(	obj, 
													null, 
													targetPath, 
													BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
													BuildTarget.iPhone))
                {
                    Debuger.Log(targetPath + "建立完成");
                }
                else
                {
                    Debuger.LogError(obj.name + "建立失败");
                }
#elif UNITY_STANDALONE_WIN
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(	obj, 
													null, 
													targetPath, 
													BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
													BuildTarget.StandaloneWindows))
                {
                    Debuger.Log(targetPath + "建立完成");

                }
                else
                {
                    Debuger.LogError(obj.name + "建立失败");
                }
#elif UNITY_ANDROID
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(obj,
                                                    null,
                                                    targetPath,
                                                    BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
                                                    BuildTarget.Android))
                {
                    Debuger.Log(targetPath + "建立完成");

                }
                else
                {
                    Debuger.LogError(obj.name + "建立失败");
                }
#endif	//	UNITY_STANDALONE_WIN
                AssetDatabase.Refresh();

            }

            EditorUtility.DisplayDialog("AssetBunlde", "BuildTarget.StandaloneWindows Over", "Close");

        }

        public bool CreateAssetBunldesALLPlatform(Object obj, string targetPath, BuildTarget target)
        {
            Object[] all = new Object[1] { obj };

            return CreateAssetBunldesALLPlatform(all, targetPath, target);

        }

#endregion

#region Scene

        public string CreateAssetBundlesScene(SceneData thisSceneData, string path, BuildTarget target)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Caching.CleanCache();


            AssetDatabase.Refresh();

            AssetDatabase.GetAssetPath((Object)thisSceneData.scene);
            AssetDatabase.GetAssetPath((Object)thisSceneData.scene_nav);
            AssetDatabase.GetAssetPath((Object)thisSceneData.scene_Camera_custom);
            AssetDatabase.GetAssetPath((Object)thisSceneData.scene_Lightmap);

//			string targetPathOld = path + "/" + thisSceneData.scene.name + "_old" + ABConfig.extensionName;
//                
//            {
//                Object[] SelectedAsset = new Object[4];
//                SelectedAsset[0] = (Object)thisSceneData.scene;
//                SelectedAsset[1] = (Object)thisSceneData.scene_nav;
//                SelectedAsset[2] = (Object)thisSceneData.scene_Lightmap;
//                SelectedAsset[3] = (Object)thisSceneData.scene_Camera_custom;
//
//				CreateAssetBunldesALL(SelectedAsset, targetPathOld, target);
//            }

			string targetPath = path + "/" + thisSceneData.scene.name + ABConfig.extensionName;
			{
                
                Object[] SelectedAsset = new Object[5];
                SelectedAsset[0] = (Object)thisSceneData.scene_nomaterial;
                SelectedAsset[1] = (Object)thisSceneData.scene_material_asset;
                SelectedAsset[2] = (Object)thisSceneData.scene_nav;
                SelectedAsset[3] = (Object)thisSceneData.scene_Lightmap;
                SelectedAsset[4] = (Object)thisSceneData.scene_Camera_custom;

				CreateAssetBunldesALL(SelectedAsset, targetPath, target);
            }

            return targetPath;
        }
        #endregion

		#region StreamScene
		public string CreateStreamedSceneAssetBundle(string[] levels, string path, BuildTarget target)
		{
			//  levels, "Streamed-Level1.unity3d", BuildTarget.WebPlayer 
			string ret = BuildPipeline.BuildStreamedSceneAssetBundle(levels, path + ".unity3d", target);
			
			return ret;
		}
		#endregion

        #region Inner

        bool CreateAssetBunldesALLPlatform(Object[] SelectedAsset, string targetPath, BuildTarget target)
        {
            if (SelectedAsset == null || SelectedAsset.Length == 0)
            {
                //EditorUtility.DisplayDialog("AssetBunlde", "Nothing is Selected", "Close");
                Debuger.LogError("Nothing is Selected or targetPath is invalid");

                return false;
            }

            if (File.Exists(targetPath))
            {
                //if (EditorUtility.DisplayDialog("AssetBunlde", "Target is Exist, Is Delete?", "Delete", "No Delete"))
                {
                    File.Delete(targetPath);

                    AssetDatabase.Refresh();

                }
            }

            if (BuildPipeline.BuildAssetBundle(null,
                                    SelectedAsset,
                                    targetPath,
                                    BuildAssetBundleOptions.CollectDependencies,
                                    target))
            {
                Debuger.Log("AssetBundleEditor.ABMgr.CreateAssetBunldesALLPlatform Success " + target + " " + targetPath);

                return true;

            }
            else
            {
                Debuger.Log("AssetBundleEditor.ABMgr.CreateAssetBunldesALLPlatform Failed " + target + " " + targetPath);

                return false;

                //EditorUtility.DisplayDialog("AssetBunlde", "BuildTarget.iPhone Failed", "Close");
            }

        }

        bool CreateAssetBunldesALL(Object[] SelectedAsset, string targetPath, BuildTarget BT)
        {
            foreach(Object o in SelectedAsset)
            {
                if (ABTools.MissingMono(o)) 
                    return false;

            }

            if (SelectedAsset == null || SelectedAsset.Length == 0)
            {
                //EditorUtility.DisplayDialog("AssetBunlde", "Nothing is Selected", "Close");
                Debuger.LogError("Nothing is Selected or targetPath is invalid");

                return false;
            }

            if (File.Exists(targetPath))
            {
                //if (EditorUtility.DisplayDialog("AssetBunlde", "Target is Exist, Is Delete?", "Delete", "No Delete"))
                {
                    File.Delete(targetPath);

                    AssetDatabase.Refresh();

                }
            }

            if (BuildPipeline.BuildAssetBundle(	null, 
												SelectedAsset, 
												targetPath, 
												BuildAssetBundleOptions.CollectDependencies,
												BT))
            {
                AssetDatabase.Refresh();

                return true;

            }
            else
            {
                AssetDatabase.Refresh();

                return false;
            }
        }
        #endregion

        #region 过滤拆分.
        string GetDepencyPath(string name, GameObject obj)
        {
            Debug.Log("ABMgr GetDepencyPath");

            foreach (string e in sDepencyList)
            {
                string abname = Path.GetFileNameWithoutExtension(e);

                string endfix = Path.GetExtension(e);

                if (endfix != ".mat")
                    continue;

                if (name == abname)
                {
                    Debug.Log("ABMgr GetDepencyPath " + name + " is " + e);

                    return e;
                }
            }

            Debug.LogError("ABMgr GetDepencyPath " + name + " " + obj.name);

            return string.Empty;
        }

        //  关联关系.
        string[] sDepencyList = null;

        //  脱离Material后的prefab.
        public GameObject nomaterialscene = null;

        //  资源.
        public StringPrefab materialasset = null;

        //  目录
        public string mDestFolder = null;
        private string mMaterialName = null;

        public void DecomposePre(Object obj, string DestFolder, string MaterialName)
        {
            Debug.Log("ABMgr DecomposePre");

            mDestFolder = DestFolder;

            GetDepencys(obj);

            DecomposeMaterialAsset(obj);
        }

        void GetDepencys(Object obj)
        {
            ArrayList list = new ArrayList();

            //  依赖关系.
            string path = AssetDatabase.GetAssetPath(obj);

            Debug.Log(path);

            list.Add(path);

            string[] paths = (string[])list.ToArray(typeof(string));
            sDepencyList = AssetDatabase.GetDependencies(paths);

            foreach (string e in sDepencyList)
            {
                Debug.Log(e);
            }

        }

        public void DecomposeEnd()
        {
            Debug.Log("ABMgr DecomposeEnd");

            sDepencyList = null;
        }

        /// <summary>
        /// 针对传入的硬盘对象，创建一个纹理配置文件asset
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        void DecomposeMaterialAsset(Object obj)
        {
            //  输出目录
            string folder = AssetDatabase.GetAssetPath(obj);
            folder = Path.GetDirectoryName(folder);

            ArrayList listMatNames = new ArrayList();
			Dictionary<string, int> dicListMatNames = new Dictionary<string, int>();

            GameObject characterClone = (GameObject)Object.Instantiate(obj);

            characterClone.name = obj.name;

            //  搜寻所有的MeshRender.
            foreach (MeshRenderer ren in characterClone.GetComponentsInChildren<MeshRenderer>())
            {
                {
                    //  sharedMaterials
                    if (ren.sharedMaterials != null)
                    {
                        int count = ren.sharedMaterials.Length;

                        for (int i = count - 1; i >= 0; i--)
                        {
                            Material m = (Material)ren.sharedMaterials[i];

                            if (m == null)
								Debug.LogError("Missing " + ren.gameObject.name + " -> Material in " + characterClone.name);
                            else
                            {
                                //Debug.Log(characterClone.name + "/" + ren.gameObject.name + "/" + m.name);

                                StringPrefabMaterial prefabmat = new StringPrefabMaterial();
                                //  挂载的对象.
                                prefabmat.meshrendererGameObject = ABTools.GetParentPath(ren.gameObject) + "_" + ren.gameObject.GetInstanceID();
								//
								ren.gameObject.name += "_" + ren.gameObject.GetInstanceID();
                               //  资源名称.
                                prefabmat.assetname = m.name;
                                //  资源对象.
                                prefabmat.assetbundlePath = GetDepencyPath(m.name, ren.gameObject);
                                //  纹理的AB.
                                prefabmat.assetbundlePath = CreateMaterialAB(prefabmat.assetbundlePath);

                                listMatNames.Add(prefabmat);

								//	Count Times
								int curcount = -1;

								if (dicListMatNames.TryGetValue(prefabmat.assetbundlePath, out curcount))
									curcount++;
								else
									curcount = 1;

								dicListMatNames[prefabmat.assetbundlePath] = curcount;
                            }
                        }

                        ren.sharedMaterials = new Material[0];
                    }
                }
            }


            //  创建一个不带Material的Prefab.
            string nomaterialscenePath = folder + "/" + obj.name + "_nomaterial.prefab";
            Object o = PrefabUtility.CreateEmptyPrefab(nomaterialscenePath);
            PrefabUtility.ReplacePrefab(characterClone, o);

            //  创建配置文件
            StringPrefab holder = ScriptableObject.CreateInstance<StringPrefab>();
            holder.material = (StringPrefabMaterial[])listMatNames.ToArray(typeof(StringPrefabMaterial));
            string materialassetPath = folder + "/" + obj.name + "_material.asset";
            AssetDatabase.CreateAsset(holder, materialassetPath);

			//	
			string txtPath = mDestFolder + "/" + obj.name + ".txt";
			string content = string.Empty;
			foreach(KeyValuePair<string, int> e in dicListMatNames)
			{
				content += e.Value;
				content += "\t";
				content += e.Key;
				content += "\r\n";
			}
			File.WriteAllText(txtPath, content);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GameObject.DestroyImmediate(characterClone);

            nomaterialscene = (GameObject)AssetDatabase.LoadAssetAtPath(nomaterialscenePath, typeof(GameObject));
            materialasset = (StringPrefab)AssetDatabase.LoadAssetAtPath(materialassetPath, typeof(StringPrefab));
        }

        string CreateMaterialAB(string matpath)
        {
            if (!File.Exists(matpath))
            {
                Debug.LogError(matpath);

                return matpath;
            }

			//
			int index = matpath.ToLower().IndexOf("/materials");

			if (index == -1)
			{
				Debug.LogError("No Materials In " + matpath.ToLower());

				return matpath;
			}

			string tmp = matpath.Substring(0, index + 1);

			DirectoryInfo info = new DirectoryInfo(tmp);

			mMaterialName = info.Name;

			//return null;

            //  贴图所在的文件夹
            DirectoryInfo di = new DirectoryInfo(mDestFolder);

            //  场景名
            string matsfolder = mDestFolder + "/" + mMaterialName;

            if (!Directory.Exists(matsfolder))
                Directory.CreateDirectory(matsfolder);

            Material mat = (Material)AssetDatabase.LoadAssetAtPath(matpath, typeof(Material));

            string name = Path.GetFileNameWithoutExtension(matpath) + "_" + MD5.Com(matpath);

            //string targetPath = Path.GetDirectoryName(matpath) + "/" + name + ABConfig.extensionName;
            string targetPath = matsfolder + "/" + name + ABConfig.extensionName;

            if (File.Exists(targetPath))
                return mMaterialName + "/" + name + ABConfig.extensionName;

#if UNITY_STANDALONE_WIN
            CreateAssetBunldesALLPlatform(mat, targetPath, BuildTarget.StandaloneWindows);
#elif UNITY_IPHONE
			CreateAssetBunldesALLPlatform(mat, targetPath, BuildTarget.iPhone);
#elif UNITY_ANDROID
            CreateAssetBunldesALLPlatform(mat, targetPath, BuildTarget.Android);
#endif
            return mMaterialName + "/" + name + ABConfig.extensionName;
        }
        #endregion
    }
}