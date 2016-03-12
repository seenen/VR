using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

namespace AssetBundleEditor
{
    public abstract class IPackage
    {
        protected BatArg mBatArg;

        protected ABMgr mgr = new ABMgr();

        public IPackage(BatArg arg)
        {
            mBatArg = arg;

            Preprocess();
        }

        void Preprocess()
        {
            if (!mBatArg.IsValid())
                return;

            mBatArg.PreProcess();
        }

        public virtual void Package()
        {

        }

#region MD5

        protected bool IsPackage(string fullname, string DestFolder, string MD5name)
        {
			//
			if (!Directory.Exists(DestFolder))
			{
				Directory.CreateDirectory(DestFolder);
			}

			//  MD5
            string newmd5 = MD5.Com(fullname);

            //  
            string md5file = DestFolder + "/" + MD5name + MD5.extensionName;

            //
            if (!File.Exists(md5file))
            {
                WriteMD5(md5file, newmd5);

                return true;
            }
            else
            {
                string oldmd5 = ReadMD5(md5file);

                if (oldmd5.Equals(newmd5))
                {
                    return false;
                }
                else
                {
                    if (File.Exists(md5file))
                        File.Delete(md5file);

                    WriteMD5(md5file, newmd5);

                    return true;
                }
            }
        }

        protected void WriteMD5(string path, string content)
        {
            Debug.LogWarning("AssetBundleEditor.ABPackage.WriteMD5 " + path + " " + content);

            FileStream fs = new FileStream(path, FileMode.CreateNew);

            StreamWriter sw = new StreamWriter(fs);

            sw.Write(content);

            //清空缓冲区 
            sw.Flush();

            //关闭流 
            sw.Close();

            fs.Close();

        }

        protected string ReadMD5(string path)
        {
            Debug.LogWarning("AssetBundleEditor.ABPackage.ReadMD5 " + path);

            FileStream fs = new FileStream(path, FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            string ret = sr.ReadLine();

            //关闭流 
            sr.Close();

            fs.Close();

            return ret;

        }
#endregion
    }
    

}
