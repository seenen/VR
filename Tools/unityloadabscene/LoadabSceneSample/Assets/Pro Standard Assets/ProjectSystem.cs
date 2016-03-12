using UnityEngine;
using System.Collections;
using System.Security.AccessControl;

public class ProjectSystem : MFMonoBehaviour
{
    public enum NetSetting
    {
        Null,
        In,
        Out,
    }

    static public string ApplicationdataPath = string.Empty;

    protected override void MFStart()
    {
        base.MFStart();

        ApplicationdataPath = Application.dataPath;

		Debuger.Log("Application.dataPath->" + Application.dataPath);
		Debuger.Log("Application.persistentDataPath->" + Application.persistentDataPath);
		Debuger.Log("Application.streamingAssetsPath->" + Application.streamingAssetsPath);
    }
    
    protected override void MFAwake()
	{
        base.MFAwake();

        Application.targetFrameRate = 30;
        
        GameObject.DontDestroyOnLoad(gameObject);
		
		BaseInit();

        ABFileLog.mStorageFolder = StorageFolder;

	}
	
    #region Storage
    public static string ProjectName = "/com.moretektest.heros";
    //private static string ProjectName = "/moretektest";

    /// <summary>
    /// »ñÈ¡¿É¶ÁÐŽµÄŽæŽ¢Â·Ÿ¶
    /// </summary>
    public static string StorageFolder
	{
		get
		{
            //Debuger.Log("StorageFolder = " + BaseStorageFolder + ProjectName);

            return BaseStorageFolder + ProjectName;
		}
	}

    /// <summary>
    /// »ñÈ¡StreamingAssetsµÄÄ¿ÂŒ£¬²»Ö§³Ö¶þœøÖÆŒÓÔØ
    /// </summary>
    public static string StreamingAssets
	{
		get
		{
			return PrefixPlatform + BaseStreamingAssets + BasePlatformFolder;
		}
	}

    /// <summary>
    /// »ñÈ¡StreamingAssetsµÄÄ¿ÂŒ£¬²»Ö§³Ö¶þœøÖÆŒÓÔØ
    /// </summary>
    public static string VersionAssets
    {
        get
        {
            return PrefixPlatform + BaseVersionAssets + "/update_version.txt";
        }
    }

    //本地的StreamingAssets路径，用于编辑器
    public static string LocalStreamingAssets
    {
        get
        {
            return PrefixPlatform + Application.dataPath + "/StreamingAssets" + BasePlatformFolder;
        }
    }

    /// <summary>
    /// »ñÈ¡StreamingAssetsµÄÄ¿ÂŒ
    /// </summary>
    public static string StreamingAssetsFolder
	{
		get
		{
			return BaseStreamingAssets + BasePlatformFolder;
		}
	}

    public static string SoundPrefix
	{
		get
		{
			return SoundPrefixPlatform;
		}
	}

    #endregion
	
    #region Base
	
	public static string BaseStreamingAssets = string.Empty;
    public static string BasePlatformFolder = string.Empty;
	public static string PrefixPlatform = string.Empty;
    static string BaseStorageFolder = string.Empty;
	static string SoundPrefixPlatform = string.Empty;
    public static string BaseMPQ = string.Empty;
	public static bool bJarflag = false;
    public static string BaseVersionAssets = string.Empty;
	void BaseInit()
	{
		{
        //  persistentDataPath
            BaseStorageFolder =
#if UNITY_STANDALONE_WIN
        Application.persistentDataPath;
#endif

#if UNITY_IPHONE
        Application.persistentDataPath;
#endif

#if UNITY_ANDROID
#if UNITY_EDITOR
        Application.persistentDataPath;
#else
        string.IsNullOrEmpty(Application.persistentDataPath) ? PlatformMgr.Cardname() : Application.persistentDataPath;
#endif
#endif
            Debuger.Log("BaseStorageFolder->" + BaseStorageFolder);
		}

		{
			bJarflag = BaseStorageFolder.Contains("!/") ? true : false;
		}

		{
            /// <summary>
    /// ²»Í¬ÔËÐÐÆœÌšStreamingAssetsµÄÂ·Ÿ¶ 
    /// </summary>
            BaseVersionAssets = 
#if UNITY_STANDALONE_WIN
	#if UNITY_EDITOR
    	//Application.dataPath + "/../../GameEditors";
        //  MPQ模式.
        StorageFolder + "/MPQ/updates_png";
#else
        //Application.dataPath;
        //  MPQ模式.
        StorageFolder + "/MPQ/updates_png";	
#endif
#endif

#if UNITY_IPHONE
#if UNITY_EDITOR
			//Application.dataPath + "/../../GameEditors";
			//  MPQ模式.
			StorageFolder + "/MPQ/updates_pvr";
#else
		//Application.dataPath + "/Raw"; 
			//  MPQ模式.
			StorageFolder + "/MPQ/updates_pvr";	
			#endif
#endif

#if UNITY_ANDROID
#if UNITY_EDITOR
//    Application.dataPath + "/../../GameEditors";
			//  MPQ模式.
			StorageFolder + "/MPQ/updates_etc";

#else               //  ÔËÐÐÊ±
#if UNITY_STANDALONE_WIN // PCÆ½Ì¨ÏÂµÄ°²×¿
	    //Application.dataPath ;
			//  MPQ模式.
			StorageFolder + "/MPQ/updates_etc";

#else
        //Application.dataPath + "!/assets";  
			//  MPQ模式.
			StorageFolder + "/MPQ/updates_etc";

#endif
#endif
#endif

            BaseStreamingAssets = BaseVersionAssets + "/StreamingAssets";
			Debuger.Log("BaseStreamingAssets->" + BaseStreamingAssets);

		}

		{
            /// <summary>
    /// ²»Í¬ÔËÐÐÆœÌšStreamingAssetsµÄÂ·Ÿ¶ 
    /// </summary>
            BaseMPQ = 
#if UNITY_STANDALONE_WIN
	#if UNITY_EDITOR
    	Application.dataPath + "/../../GameEditors";
#else
        Application.dataPath + "/StreamingAssets";
#endif
#endif

#if UNITY_IPHONE
#if UNITY_EDITOR
	    Application.dataPath + "/../../GameEditors/StreamingAssets";
#else
		Application.dataPath + "/Raw"; 
#endif
#endif

#if UNITY_ANDROID
#if UNITY_EDITOR
    Application.dataPath + "/../../GameEditors/StreamingAssets";
#else               //  ÔËÐÐÊ±
#if UNITY_STANDALONE_WIN // PCÆ½Ì¨ÏÂµÄ°²×¿
	    Application.dataPath + "/StreamingAssets";
#else
        Application.dataPath + "!/assets";  
#endif
#endif
#endif
			Debuger.Log("BaseMPQ->" + BaseMPQ);
		}

		{
    /// <summary>
    /// ²»Í¬ÔËÐÐÆœÌšÏÂStreamingAssetsµÄÎÄŒþŒÐ 
    /// </summary>
    BasePlatformFolder =
#if UNITY_STANDALONE_WIN
        "/PC";
#elif UNITY_IPHONE
        "/IOS";
#elif UNITY_ANDROID
        "/Android";
#endif
			Debuger.Log("BasePlatformFolder->" + BasePlatformFolder);

		}

		{
    PrefixPlatform = 
#if UNITY_STANDALONE_WIN
        "file:///";
#elif UNITY_IPHONE
		#if UNITY_EDITOR
		"file:///";
		#else
		"file://";
		#endif
#elif UNITY_ANDROID
#if UNITY_EDITOR
        "file:///";
        #else
		bJarflag ? "jar:file://" : "file://";
		#endif
#else
		"Not valid Platform";
#endif
			Debuger.Log("PrefixPlatform->" + PrefixPlatform);

		}

		{
    SoundPrefixPlatform = 
#if UNITY_STANDALONE_WIN
        ".ogg";
#elif UNITY_IPHONE
        ".mp3";
#elif UNITY_ANDROID
        ".mp3";
#else
		"Not valid Platform";
#endif
			Debuger.Log("SoundPrefixPlatform->" + SoundPrefixPlatform);

		}
	}

#endregion Base

#region MPQ
    /// <summary>
    /// MPQ的下载地址是内网还是外网.
    /// </summary>
    public static string UPDATE_NETWORK
    {
        get;
        set;
    }

    /// <summary>
    /// 分段资源的默认名.
    /// </summary>
    public static string UPDATE_DEFAULT_RESNAME
    {
        get;
        set;
    }

    /// <summary>
    /// 主要 CDN 下载目录前缀
    /// </summary>
    public static string UPDATE_REMOTE_ROOT_URL
    {
        get;
        set;
        //get
        //{
        //    switch (m_bIsInNet)
        //    {
        //        case NetSetting.In:
        //            //return "http://192.168.1.107:80/res/";	//	zhangshining
        //            //return "http://192.168.1.236/http/res/";	//	mengbowen
        //            //return "http://192.168.1.13:8081/res_server/"; 		// wumin1
        //            return "http://192.168.1.13:8081/out_res_server/"; 		// wumin2
        //        case NetSetting.Out:
        //            return "http://dl2.shanggu3.moreu.cn/http/res/"; // cdn
        //        case NetSetting.Null:
        //            return "http://dl2.shanggu3.moreu.cn/http/res/";
        //    }
        //    return string.Empty;
        //}
    }

    /// <summary>
    /// 备用 CDN 下载目录前缀
    /// </summary>
    public static string UPDATE_REMOTE_ROOT_URL_2
    {
        set;
        get;
        //get
        //{
        //    switch (m_bIsInNet)
        //    {
        //        case NetSetting.In:
        //            //return "http://192.168.1.107:80/res/";	//	zhangshining
        //            //gu:更改过地址    
        //            return "http://192.168.1.226/test/http/res/"; 		// wumin 
        //        case NetSetting.Out:
        //            return "http://dl.shanggu3.moreu.cn/http/res/"; // cnd2
        //        case NetSetting.Null:
        //            return "http://dl.shanggu3.moreu.cn/http/res/";
        //    }
        //    return string.Empty;
        //}
    }

    #region __图片重定向__

        private static string PREFIX   = "mpq://";
        private static string LOADPATH = "/ui_edit/res";

        public static string UPDATE_FILE_NAME
        {
            get
            {
#if (M3Z_TEST)
                return "/updates_etc/update_version.txt";
#elif (UNITY_ANDROID)
                return "/updates_etc/update_version.txt";
#elif (UNITY_IPHONE)
                return "/updates_pvr/update_version.txt";
#elif (UNITY_STANDALONE_WIN)
                return "/updates_png/update_version.txt";
#else
                return "/updates_png/update_version.txt";
#endif
            }
        }

//        public static string UPDATE_FILE_NAME
//        {
//            get
//            {
//#if (M3Z_TEST)
//                return "updates_etc/update_version.txt";
//#elif (UNITY_ANDROID)
//                return "updates_etc/update_version.txt";
//#elif (UNITY_IPHONE)
//                return "updates_pvr/update_version.txt";
//#elif (UNITY_STANDALONE_WIN)
//                return "updates_png/update_version.txt";
//#else
//                return "updates_png/update_version.txt";
//#endif
//            }
//        }

        public static string RedirectImage(string resource)
        {
#if (M3Z_TEST)
            return resource.Substring(0, resource.LastIndexOf(".")) + ".etc.g3z";
#elif (UNITY_ANDROID)
            return resource.Substring(0, resource.LastIndexOf(".")) + ".etc.g3z";
#elif (UNITY_IPHONE)
            return resource.Substring(0, resource.LastIndexOf(".")) + ".pvr.g3z";
#elif (UNITY_STANDALONE_WIN)
            return resource;
#else
            return resource;
#endif
        }

    /// <summary>
    /// 获得加载前缀.
    /// </summary>
    /// <returns></returns>
        public static string GetRootPath()
        {
            string path = PREFIX + LOADPATH;
#if ((UNITY_EDITOR && (!M3Z_TEST)) || UNITY_STANDALONE_WIN)
            path = LOADPATH;
#endif
            return path;
        }

        #endregion


#endregion MPQ

#region Login
        public static string PROXY_URL
        {
            set;
            get;
        }
        public static string MF_YHT_URL
        {
            set;
            get;
            //get
            //{
            //    switch (ProjectSystem.m_bIsInNet)
            //    {
            //        case ProjectSystem.NetSetting.In:
            //            return "http://192.168.1.44:18080/passport/common";
            //        //return "http://180.166.68.26:8080/MF_Passport/common"; //少林测试
            //        case ProjectSystem.NetSetting.Out:
            //            //return "http://yht.morefuntek.com/common";  //外网正式
            //            return "http://yhtcs.morefuntek.com:18023/passport/common";
            //    }

            //    return string.Empty;
            //    //return "http://yht44.morefuntek.com:18080/passport/common";//外网测试
            //    //return "http://yhtcs.morefuntek.com:18023/passport/common";
            //}
        }

        public static string VOICE_URL;
	
	public static string MF_GAME_URL
    {
        get;
        set;
    }
#endregion
}
