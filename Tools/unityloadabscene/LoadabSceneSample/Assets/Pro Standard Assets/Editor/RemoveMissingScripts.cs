using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class RemoveMissingScripts : EditorWindow
{
    public static string componentType = "missing";

    [MenuItem("CUSTOM/Remove Missing Script Entries")]
	public static void RemoveMissin()
    {
        //{
        //    GameObject[] SelectedAsset = (GameObject[])Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //    foreach (GameObject obj in SelectedAsset)
        //    {
        //        foreach (Component c in obj.GetComponents(typeof(Component)))
        //        {
        //            if (c == null)
        //            {
        //                Debug.LogError("NULL " + obj.name);
        //                Object.DestroyImmediate(c);
        //            }
        //        }
        //    }
        //}

        foreach (Transform t in Selection.transforms)
        {
            List<GameObject> selection = new List<GameObject>();

            Debug.Log(t.GetComponentsInChildren(typeof(Component)).Length);

            foreach (Component c in t.GetComponentsInChildren(typeof(Component)))
            {
                if (c == null)
                {
                    selection.Add(t.gameObject);

                    Debug.LogError("NULL " + t.name);
                    Object.DestroyImmediate(c);
                }
                //else
                //    Debug.Log(c.GetType());
            }

            Debug.Log(selection.Count);
            Selection.objects = (GameObject[])selection.ToArray();

        }
    }


    //当界面发生变化时重绘
    void OnInspectorUpdate()
    {
        Repaint();
    }
}
