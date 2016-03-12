using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class ABFileLog
{
    static private string FileFolder = string.Empty;

    static ABFileLog()
    {
        mStorageFolder = Application.persistentDataPath;
    }

    static private Dictionary<string, string> LogFileDic = new Dictionary<string, string>();

    //  Current Time
    static private string CurrentTime = string.Empty;

    static private string filePathName = string.Empty;

    static public string mStorageFolder = string.Empty;

    static void DelOld(string filePrefix)
    {
        try
        {
            string[] files = Directory.GetFiles(mStorageFolder);

            if (files == null)
                return;

            foreach(string e in files)
            {
                string str = e.Replace("\\", "/").ToLower();

                //  �Ƿ����ļ�.
                if (str.StartsWith(filePrefix.ToLower()))
                {
                    DateTime dt = File.GetCreationTime(e);

                    //  ��������ľ�ɾ��.
                    if (DateTime.Now.Date.Day - dt.AddDays(3).Day > 0)
                    //if (DateTime.Compare(DateTime.Now.Date, System.DateTime.da) != 0)
                    {
                        File.Delete(str);
                    }
                }
            }
        }
        catch(Exception e)
        {

        }
    }

    //  д�ļ� 
    static string WriteFile(string flt)
    {
        string newfile = string.Empty;

        try
        {
            if (LogFileDic.TryGetValue(flt, out newfile))
                return newfile;

            if (!Directory.Exists(mStorageFolder))
                Directory.CreateDirectory(mStorageFolder);

            filePathName = null;

            {
                CurrentTime = "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HHmm");

                newfile = mStorageFolder + "/" + flt.ToString() + CurrentTime + ".txt";

                Debug.Log("ABFileLog WriteFile filePathName: " + newfile);

                FileInfo TheFile = new FileInfo(newfile);

                if (TheFile.Exists)
                    TheFile.Delete();

                StreamWriter fileWriter = File.CreateText(newfile);
                fileWriter.WriteLine("Create " + newfile);
                fileWriter.WriteLine("");
                fileWriter.Close();

                LogFileDic.Add(flt, newfile);

            }

            DelOld(mStorageFolder + "/" + flt.ToString());

            //  ɾ������ǰ��Ϣ.
            Debug.Log("ABFileLog " + DateTime.Now.Date);

            return newfile;
        }
        catch (Exception e)
        {
            Debuger.LogWarning(e);

            Debuger.LogError(e.Message + " newfile " + newfile);
        }

        return string.Empty;
    }

    public static void Write(string filename, LogType lf, string log)
    {
        try
        {
            string filePathName = WriteFile(filename);

            FileStream fs = new FileStream(filePathName, FileMode.Append);

            if (fs.Length >= 15 * 1024 * 1024)
            {
                fs.Close();

                fs = new FileStream(filePathName, FileMode.Create);
            }

            StreamWriter sw = new StreamWriter(fs);

            //��ʼд�� 
            sw.WriteLine("");
            //
            string str = "[";
            str += System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            str += "]";
            str += "\t";
            str += lf.ToString();
            str += "\t\t";
            str += log;

            sw.Write(str);
            //��ջ����� 
            sw.Flush();
            //�ر��� 
            sw.Close();
            fs.Close();

        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static string GetFileName(string flt)
    {
        if (LogFileDic.TryGetValue(flt, out filePathName))
            return filePathName;

        return string.Empty;
    }
 }
