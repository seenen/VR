using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 空壳单例
/// 方便在非继承MoniBehaviour的类中使用MoniBehaviour类中才可以调用的方法
/// 需要将该脚本绑定到场景中的一个物体上(比如 LoginScene中的Main Camera)
/// </summary>
public class UnitySingleton : MonoBehaviour
{
    private static UnitySingleton _instance;
    private static bool applicationQuit = false;
    private float lastUpdate;

    private static object _lock = new object();

    public static UnitySingleton Instance
    {
        get
        {
            if (applicationQuit)
            {
                Debuger.Log("The application is quit. ");
                return null;
            }

            lock (_lock)
            {

                if (_instance == null)
                {
                    GameObject singleton = GameObject.Find("(singleton)");
                    if (singleton == null)
                    {
                        singleton = new GameObject("(singleton)");
                        UnityEngine.Object.DontDestroyOnLoad(singleton);

                        _instance = singleton.AddComponent<UnitySingleton>();
                    }
                    else
                    {
                        _instance = singleton.GetComponent<UnitySingleton>();
                    }
                }

                return _instance;
            }
        }
    }

    public static T addSingleton<T>() where T : UnityEngine.Component
    {
        GameObject go = Instance.gameObject;
        T t = go.GetComponent<T>();
        if (t != null)
        {
            return t;
        }
        return go.AddComponent<T>();
    }

    void Start()
    {
        lastUpdate = Time.realtimeSinceStartup;
    }

    public void OnDestroy()
    {
        applicationQuit = true;
    }

    public void FixedUpdate()
    {
        lastUpdate = Time.realtimeSinceStartup;
    }

    public bool needDeffer()
    {
        float now = Time.realtimeSinceStartup;
        return now - lastUpdate > 1.0 / 30.0;
    }

}
