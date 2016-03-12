using UnityEngine;
using System.Collections;

public class ABSelfTest : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        string url = "file:///I:/npc/ef_04003.assetBundles";

        WWW www = new WWW(url);

               yield return www;

        while(true)
        {
            if (www.isDone)
                break;

            yield return 1;
        }

        AssetBundle ab = null;

        Application.RegisterLogCallback(HandleLog);

        try
        {
            ab = www.assetBundle;

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

        Application.RegisterLogCallback(null);

        Debug.Log("Is error: " + www.error);

        ab = null;

        if (www.assetBundle != null)
            www.assetBundle.Unload(true);
        www.Dispose();

    }

    void HandleLog(string message, string stackTrace, LogType type)
    {

    }

    // Update is called once per frame
    void Update () {
	
	}
}
