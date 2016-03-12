using UnityEngine;
using System.Collections;
using AssetBundleEditor;

public class ABBuildScene : MonoBehaviour 
{
    string url = null;

    IEnumerator Start()
    {
        Shader.WarmupAllShaders();

        //yield return StartCoroutine(ReloadVersionInfo());

        yield return new WaitForEndOfFrame();

        string resname = "Grammil_01";

        string filePath = "/Region/" + resname + ".assetBundles";

        //  加载.
        AssetBundleUti.OnSceneFinishLoading += AssetBundleUtiCallBack;

        //string url = ProjectSystem.StreamingAssets + filePath;
#if UNITY_IOS
		url = ProjectSystem.PrefixPlatform + Application.streamingAssetsPath + "/IOS/scene/map_haiwan_001.assetBundles";
#else
		url = "file:///F:/GitHub/VR/Tools/unityloadabscene/Resources/scene/Grammil_01.assetBundles";
#endif

		Debug.Log(url);

        if (!AssetBundleUti.GetABScene(url, resname, null, AssetBundleEditor.ABDataType.Normal))
        {

        }
    }

    ABDataScene data = null;

    private void AssetBundleUtiCallBack(string url, string resname, bool success, string errorcode = "")
    {
        Debuger.Log("AssetBundleUtiCallBack:" + url + " " + resname + " " + success);

        if (success)
        {
            data = AssetBundleUti.GetObjectScene(url);

            Shader.WarmupAllShaders();

        }

    }

    void OnGUI()
    {
        if (data == null)
            return;

        if (GUI.Button(new Rect(0, 400, 300, 300), "Release "))
            StartCoroutine(Release());

    }

    public IEnumerator Release()
    {
        yield return null;

        AssetBundleUti.CleanByScene();

    }

}
