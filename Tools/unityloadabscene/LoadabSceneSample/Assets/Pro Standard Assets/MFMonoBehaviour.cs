using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MFMonoBehaviour : MonoBehaviour 
{
    #region Count
    public static int mRef = 0;

    private static List<MFMonoBehaviour> mReferenceList = new List<MFMonoBehaviour>();
    public static string DumpMFMonoBehaviourList()
    {
        string sout = string.Empty;

        for (int i = mReferenceList.Count - 1; i >= 0; --i)
        {
            sout += ((MFMonoBehaviour)mReferenceList[i]).name;
            sout += "\r\n";
        }

        return sout;
    }

    bool bRef = false;

    void Ref()
    {
        if (bRef)
            return;

        bRef = true;

        mRef++;
        mReferenceList.Add(this);

        //Debuger.Log("MFMonoBehaviour Ref: " + this.ToString() + " mRef: " + mRef);

    }

    bool bDispose = false;

    void Dispose()
    {
        if (bDispose)
            return;

        bDispose = true;

        mRef--;
        mReferenceList.Remove(this);

        //Debuger.Log("MFMonoBehaviour Dispose: " + this.ToString() + " mRef: " + mRef);

    }

    #endregion

    void Awake()
    {
        // made virtual in case of furture usage
        Ref();

        try
        {
            MFAwake();
        }
        catch (Exception e)
        {
            Debuger.LogWarning(e);

            Debuger.LogError("MFMonoBehaviour.Awake " + gameObject.name + " " + e.Message);
        }
    }

    void Start()
    {
        // made virtual in case of furture usage
        Ref();

        try
        {
            MFStart();
        }
        catch (Exception e)
        {
            Debuger.LogWarning(e);

            Debuger.LogError("MFMonoBehaviour.Start " + gameObject.name + " " + e.Message);
       }

    }

    private void OnEnable()
    {
        try
        {
            MFOnEnable();
        }
        catch (Exception e)
        {
            Debuger.LogWarning(e);

            Debuger.LogError("MFMonoBehaviour.OnEnable " + gameObject.name + " " + e.Message);
        }
    }

    private void OnDisable()
    {
        try
        {
            MFOnDisable();
        }
        catch (Exception e)
        {
            Debuger.LogWarning(e);

            Debuger.LogError("MFMonoBehaviour.OnDisable " + gameObject.name + " " + e.Message);
        }
    }

    private void OnDestroy()
    {
        try
        {
            Dispose();

            MFOnDestroy();
        }
        catch (Exception e)
        {
            Debuger.LogWarning(e);

            Debuger.LogError("MFMonoBehaviour.OnDestroy " + gameObject.name + " " + e.Message);
        }

    }

    protected virtual void MFAwake()
    {
    }

    protected virtual void MFStart()
    {
    }


    protected virtual void MFOnEnable()
    {
    }

    protected virtual void MFOnDisable()
    {
    }

    protected virtual void MFOnDestroy()
    {
    }


}
