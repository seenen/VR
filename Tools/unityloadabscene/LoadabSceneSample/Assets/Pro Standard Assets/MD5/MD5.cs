using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public class MD5
{
    public static string extensionName = ".md5";

    public enum MD5ErrorCode
    {
        E_NoError,
        E_Param,
        E_ComputeHash,
    }

    static private MD5ErrorCode _ecode;

    public static string Com(string filepath)
    {
        _ecode = MD5ErrorCode.E_NoError;

        if (!File.Exists(filepath))
        {
            _ecode = MD5ErrorCode.E_Param;

            return string.Empty;
        }

        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        FileStream fst = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);

        string ret = Com(fst);

        fst.Flush();
        fst.Close();
        fst = null;

        return ret; 

    }

    /// <summary>
    /// ¶Ô×Ö·û´®½øÐÐHash.
    /// </summary>
    /// <param name="myString"></param>
    /// <returns></returns>
    public static string ComContent(string myString)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
        byte[] targetData = md5.ComputeHash(fromData);
        string byte2String = null;

        for (int i = 0; i < targetData.Length; i++)
        {
            byte2String += targetData[i].ToString("x");
        }

        return byte2String;
    }

    private static string Com(Stream stream)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        md5.ComputeHash(stream);

        byte[] hash = md5.Hash;

        if (hash == null)
        {
            _ecode = MD5ErrorCode.E_ComputeHash;

            return string.Empty;
        }

        StringBuilder sb = new StringBuilder();

        foreach (byte byt in hash)
        {

            sb.Append(String.Format("{0:X1}", byt));

        }

        string ret = sb.ToString();

        sb = null;

        return ret; 

    }
}
