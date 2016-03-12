using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

namespace AssetBundleEditor
{

    public class CreateFromPrefab
    {
        public enum PLATFORM
        {
            Win,
            iOS,
            Android,
        }

        public static string ASSETBUNDLE_PATH = "Assets/StreamingAssets/PC/scene/";

        #region 拆包.
        [MenuItem("AssetBundle/CreateFromPrefab/Win_拆")]
        static void CreateFromPrefab_Win()
        {
            GetDepency();

            CreateFromPrefab_Scene(PLATFORM.Win);

        }
        #endregion

        static void CreateAssetBundlePath()
        {
            if (!Directory.Exists(ASSETBUNDLE_PATH))
                Directory.CreateDirectory(ASSETBUNDLE_PATH);
        }

        public static void CreateFromPrefab_Scene(string DestFolder, PLATFORM p)
        {

        }

        static void CreateFromPrefab_Scene(PLATFORM p)
        {
            Debug.Log("CreateAssetbundles CreateFromPrefab_Scene");

            CreateAssetBundlePath();

            ArrayList listMatNames = new ArrayList();

            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
            {
                // filters
                if (!(obj is GameObject))
                    continue;
                //  
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
                                    Debug.LogError("Missing " + obj.name + " -> Material");
                                else
                                {
                                    //Debug.Log(characterClone.name + "/" + ren.gameObject.name + "/" + m.name);

                                    StringPrefabMaterial prefabmat = new StringPrefabMaterial();
                                    //  资源名称.
                                    prefabmat.assetname = m.name;
                                    //  挂载的对象.
                                    prefabmat.meshrendererGameObject = ABTools.GetParentPath(ren.gameObject);
                                    //  资源对象.
                                    prefabmat.assetbundlePath = GetDepencyPath(m.name);

                                    listMatNames.Add(prefabmat);

                                }

                            }

                            ren.sharedMaterials = new Material[0];
                        }

                    }
                }

                //  创建一个不带Material的Prefab.
                Object o = PrefabUtility.CreateEmptyPrefab(ASSETBUNDLE_PATH + "/" + obj.name + "_nomaterial.prefab");
                PrefabUtility.ReplacePrefab(characterClone, o);

                //  创建配置文件
                StringPrefab holder = ScriptableObject.CreateInstance<StringPrefab>();
                holder.material = (StringPrefabMaterial[])listMatNames.ToArray(typeof(StringPrefabMaterial));
                AssetDatabase.CreateAsset(holder, ASSETBUNDLE_PATH + "/" + obj.name + "_material.asset");

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject.DestroyImmediate(characterClone);
            }
        }

        ///// <summary>
        ///// 获取某个子节点的全路径.
        ///// </summary>
        ///// <param name="go"></param>
        ///// <returns></returns>
        //static string GetParentPath(GameObject go)
        //{
        //    if (go == null)
        //        return string.Empty;

        //    string path = go.name;

        //    GameObject tmp = go;

        //    while (tmp.transform.parent != null)
        //    {
        //        path = tmp.transform.parent.name + "/" + path;

        //        tmp = tmp.transform.parent.gameObject;
        //    }

        //    return path;
        //}

        /// <summary>
        /// 获取所有依赖项.
        /// </summary>
        static void GetDepency()
        {
            Debug.Log("CreateAssetbundles GetDepency");

            ArrayList list = new ArrayList();

            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
            {
                // filters
                if (!(obj is GameObject))
                    continue;

                //  依赖关系.
                string path = AssetDatabase.GetAssetPath(obj);

                Debug.Log(path);

                list.Add(path);


            }

            string[] paths = (string[])list.ToArray(typeof(string));
            sDepencyList = AssetDatabase.GetDependencies(paths);

            foreach (string e in sDepencyList)
            {
                Debug.Log(e);
            }
        }

        static string[] sDepencyList = null;

        static string GetDepencyPath(string name)
        {
            foreach (string e in sDepencyList)
            {
                string abname = Path.GetFileNameWithoutExtension(e);

                string endfix = Path.GetExtension(e);

                if (endfix != ".mat")
                    continue;

                if (name == abname)
                {
                    Debug.Log("GetDepencyPath " + name + " is " + e);

                    return e;
                }
            }

            Debug.LogError("GetDepencyPath " + name);

            return string.Empty;
        }

        #region 组合.
        [MenuItem("AssetBundle/CreateFromPrefab/Win_组")]
        static void CreateFromPrefab_Win_Revert()
        {
            GameObject pref = null;

            StringPrefab SP = null;


            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
            {
                string path = AssetDatabase.GetAssetPath(obj);

                //  加载纹理的配置文件.
                StringPrefab sp = (StringPrefab)AssetDatabase.LoadAssetAtPath(path, typeof(StringPrefab));

                if (sp != null)
                {
                    SP = sp;

                    continue;
                }

                //  对象
                if ((obj is GameObject))
                {
                    pref = (GameObject)obj;

                    continue;
                }

                //foreach (StringPrefabMaterial e in sp.material)
                //{
                //    Debug.Log(e.assetbundlePath + " " + e.meshrendererGameObject);
                //}
            }

            GameObject characterClone = (GameObject)Object.Instantiate(pref);

            characterClone.name = "map_haiwan_001";

            foreach (StringPrefabMaterial e in SP.material)
            {
                Material m = (Material)AssetDatabase.LoadAssetAtPath(e.assetbundlePath, typeof(Material));

                GameObject go = GameObject.Find(e.meshrendererGameObject);

                MeshRenderer mr = go.GetComponent<MeshRenderer>();

                Material[] mat = new Material[1];

                mat[0] = m;

                mr.sharedMaterials = mat;

            }

        }
        #endregion
    }
}
