using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

namespace AssetBundleEditor
{
    /// <summary>
    /// Bundle工厂 
    /// </summary>
    public class ABFactory
    {
        private Dictionary<string, ABLoad> mABLoads;
        private Dictionary<string, ABLoadScene> mABLoadScenes;

        public ABFactory()
        {
            mABLoads = new Dictionary<string, ABLoad>();
            mABLoadScenes = new Dictionary<string, ABLoadScene>();
        }

        public ABLoad Get(string url, int version)
        {
            //Debug.Log(mBundlet.Count + " " + url);

            string keyName = url + version.ToString();
            ABLoad ret;
            mABLoads.TryGetValue(keyName, out ret);
            return ret;
        }

        public ABLoad Add(string url, int version, ABDataType abtype)
        {
            string keyName = url + version.ToString();

            ABLoad bl = null;

            switch (abtype)
            {
                case ABDataType.Normal:
                    bl = new ABLoadFile(version, true);
                    break;
                case ABDataType.Mpq:
                    //bl = new ABLoadMpq(version, true);
                    break;
            }
            mABLoads[keyName] = bl;

            return bl;
        }

        public void CleanABByScene()
        {            
            ////foreach (KeyValuePair<string, ABLoad> e in mABLoads)
            ////{
            ////    ABLoad bl = (ABLoad)mABLoads[e.Key];
            ////    if (!bl.IsDontDestroyOnLoad())
            ////    {
            ////        bl.Dispose();
            ////        bl = null;

            ////        mABLoads.Remove(e.Key);
            ////    }
            ////}
            ////mABLoads.Clear();
            ////mABLoads = null;


            {
                List<string> listABLoadOrders = new List<string>(mABLoads.Keys);
                List<ABLoad> listABLoad = new List<ABLoad>(mABLoads.Values);
                for (int i = 0; i < listABLoad.Count; i++)
                {
                    ABLoad bl = (ABLoad)listABLoad[i];
                    if (!bl.IsDontDestroyOnLoad())
                    {
                        bl.Dispose();
                        bl = null;
                    }

                    //  根据序号去删除.
                    mABLoads.Remove(listABLoadOrders[i]);
                }
                listABLoadOrders.Clear();
                listABLoad.Clear();
                listABLoadOrders = null;
                listABLoad = null;
            }

            {
                List<string> listABLoadOrders = new List<string>(mABLoadScenes.Keys);
                List<ABLoadScene> listABLoad = new List<ABLoadScene>(mABLoadScenes.Values);
                for (int i = 0; i < listABLoad.Count; i++)
                {
                    ABLoadScene bl = (ABLoadScene)listABLoad[i];
                    if (!bl.IsDontDestroyOnLoad())
                    {
                        bl.Dispose();
                        bl = null;
                    }

                    //  根据序号去删除.
                    mABLoadScenes.Remove(listABLoadOrders[i]);
                }
                listABLoadOrders.Clear();
                listABLoad.Clear();
                listABLoadOrders = null;
                listABLoad = null;
            }

            ////foreach (KeyValuePair<string, ABLoadScene> e in mABLoadScenes)
            ////{
            ////    ABLoadScene bl = (ABLoadScene)mABLoadScenes[e.Key];
            ////    if (!bl.IsDontDestroyOnLoad())
            ////    {
            ////        bl.Dispose();
            ////        bl = null;

            ////        mABLoadScenes.Remove(e.Key);
            ////    }

            ////}
            ////mABLoadScenes.Clear();
            ////mABLoadScenes = null;
        }

        #region ABLoadScene
        public ABLoadScene GetABLoadScene(string url, int version)
        {
            //Debug.Log(mBundlet.Count + " " + url);

            string keyName = url + version.ToString();

            ABLoadScene ret;
            mABLoadScenes.TryGetValue(keyName, out ret);

            return ret;
        }

        public ABLoadScene AddABLoadScene(string url, int version, ABDataType abtype)
        {
            string keyName = url + version.ToString();

            ABLoadScene bl = new ABLoadScene(version, abtype);
            mABLoadScenes[keyName] = bl;

            return bl;
        }
        #endregion

        #region 过滤控制.
        public void EnableParticleSystem(bool enable)
        {
            //foreach (KeyValuePair<string, ABLoad> e in mABLoads)
            //{
            //    ABLoad bl = (ABLoad)mABLoads[e.Key];
            //    bl.EnableParticleSystems(enable);
            //}
        }

        public void EnableSkinMeshRenderer(int enable)
        {
            //foreach (KeyValuePair<string, ABLoad> e in mABLoads)
            //{
            //    ABLoad bl = (ABLoad)mABLoads[e.Key];
            //    bl.EnableParticleSystems(enable);
            //}
        }

        #endregion
    }
}
