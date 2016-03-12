using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace AssetBundleEditor
{
    public class ABTools : MonoBehaviour
    {
        public static List<string> GetContent(Stream stream)
        {
            List<string> lines = new List<string>();

            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line) && !line.StartsWith("//"))
                {
                    lines.Add(line);
                }
            }
            sr.Close();

            return lines;

        }

        static public int DrawList(string field, string[] list, int sel, params GUILayoutOption[] options)
        {
            if (list.Length <= 0)
            {
                Debug.LogWarning("DrawList Param");

                return -1;
            }

            if (sel < 0 || sel >= list.Length)
                sel = 0;

            string selection = list[sel];

            if (list != null && list.Length > 0)
            {
                int index = 0;
                if (string.IsNullOrEmpty(selection)) selection = list[0];

                // We need to find the sprite in order to have it selected
                if (!string.IsNullOrEmpty(selection))
                {
                    for (int i = 0; i < list.Length; ++i)
                    {
                        if (selection.Equals(list[i], System.StringComparison.OrdinalIgnoreCase))
                        {
                            index = i;
                            break;
                        }
                    }
                }

                // Draw the sprite selection popup
                index = string.IsNullOrEmpty(field) ?
                    EditorGUILayout.Popup(index, list, options) :
                    EditorGUILayout.Popup(field, index, list, options);

                return index;
            }

            return -1;
        }

        public static BuildTarget BuildTarget
        {
            get
            {

#if UNITY_STANDALONE_WIN
                return BuildTarget.StandaloneWindows;
#elif UNITY_IPHONE
                return BuildTarget.iPhone;
#elif UNITY_ANDROID
                return BuildTarget.Android;
#endif

            }

        }

        static public bool MissingMono(Object o)
        {
            if (o == null)
                return false;

            Debug.Log(o.name + " " + o.GetType() + " " + typeof(GameObject));
            
            if (o.GetType() != typeof(GameObject) )
                return false;


            string path = AssetDatabase.GetAssetOrScenePath(o);

            Debug.Log(path);

            GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

            Debug.Log(obj.name);

            foreach (Component c in obj.GetComponentsInChildren(typeof(Component)))
            {
                if (c == null)
                {
                    Debug.LogError(obj.name + " Missing Mono ");

                    //return true;
                }
                else
                {
                    Debug.Log(c.name);
                }
            }

            //GameObject.DestroyImmediate(obj);

            return false;
        }

        /// <summary>
        /// 获取某个子节点的全路径.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        static public string GetParentPath(GameObject go)
        {
            if (go == null)
                return string.Empty;

            string path = go.name;

            GameObject tmp = go;

            while (tmp.transform.parent != null)
            {
                path = tmp.transform.parent.name + "/" + path;

                tmp = tmp.transform.parent.gameObject;
            }

            return path;
        }

    }

}
