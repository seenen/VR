using UnityEngine;
using System.Collections;


/// <summary>
/// 角色模型数据结构  
/// </summary>
public class AssemblerInfo
{
    public uint m_ID;
    public bool m_IsDone;

    public string m_Character;

    public string m_HeadElement;
    public string m_BodyElement;
    public string m_FootElement;

    public string m_WeaponR;
    public string m_WeaponL;
    public string m_WeaponB;

    public string m_Wing;
    public string m_WingElement;

    public AssemblerInfo()
    {
    }

    //public AssemblerInfo(uint id, AssemblerInfo info)
    //{
    //    m_ID = id;
    //    m_IsDone = false;

    //    m_Character = info.m_Character;

    //    m_HeadElement = info.m_HeadElement;
    //    m_BodyElement = info.m_BodyElement;
    //    //		m_FootElement 	= _FootElement;	

    //    m_WeaponR = info.m_WeaponR;
    //    m_WeaponL = info.m_WeaponL;
    //    //		m_WeaponB 		= _WeaponB;	

    //    m_Wing = info.m_Wing;
    //    m_WingElement = info.m_WingElement;
    //}

    /// <summary>
    /// 构造一个AssemblerInfo.  
    /// </summary>
    /// <param name='id'>
    /// 物体id  
    /// </param>
    /// <param name='_Character'>
    /// 模型名  
    /// </param>
    /// <param name='_HeadElement'>
    /// 头部名字  
    /// </param>
    /// <param name='_BodyElement'>
    /// 身体名字  
    /// </param> 
    /// <param name='_WeaponB'>
    /// 背上武器挂载点  
    /// </param>
    /// <param name='_WeaponR'>
    /// 右手武器名  
    /// </param>
    /// <param name='_WeaponL'>
    /// 左手武器名  
    /// </param>
    /// <param name='_Wing'>
    /// 翅膀字段，固定为wing  
    /// </param>
    /// <param name='_WingElement'>
    /// 翅膀名  
    /// </param>
    public AssemblerInfo(   uint id, 
                            string _Character, 
                            string _HeadElement, 
                            string _BodyElement, 
                            string _WeaponB, 
                            string _WeaponR, 
                            string _WeaponL, 
                            string _Wing, 
                            string _WingElement)
    {
        m_ID = id;
        m_IsDone = false;

        m_Character = _Character;

        m_HeadElement = _HeadElement;
        m_BodyElement = _BodyElement;
        //m_FootElement = _FootElement;	

        m_WeaponR = _WeaponR;
        m_WeaponL = _WeaponL;
        m_WeaponB = _WeaponB;	

        m_Wing = _Wing;
        m_WingElement = _WingElement;
    }

    public AssemblerInfo Clone()
    {
        AssemblerInfo assemblerInfo = new AssemblerInfo();
        assemblerInfo.m_ID = this.m_ID;
        assemblerInfo.m_Character = this.m_Character;
        assemblerInfo.m_HeadElement = this.m_HeadElement;
        assemblerInfo.m_BodyElement = this.m_BodyElement;
        assemblerInfo.m_WeaponR = this.m_WeaponR;
        assemblerInfo.m_WeaponL = this.m_WeaponL;
        assemblerInfo.m_WeaponB = this.m_WeaponB;
        assemblerInfo.m_Wing = this.m_Wing;
        assemblerInfo.m_WingElement = this.m_WingElement;
        assemblerInfo.m_IsDone = false;
        return assemblerInfo;
    }

    public AssemblerInfo(ref AssemblerInfo info)
    {
        this.m_ID = info.m_ID;// id;
        this.m_IsDone = false;

        this.m_Character = info.m_Character;

        this.m_HeadElement = info.m_HeadElement;
        this.m_BodyElement = info.m_BodyElement;
        //m_FootElement = _FootElement;	

        this.m_WeaponR = info.m_WeaponR;
        this.m_WeaponL = info.m_WeaponL;
        this.m_WeaponB = info.m_WeaponB;

        this.m_Wing = info.m_Wing;
        this.m_WingElement = info.m_WingElement;
    }

    public bool IsEffective()
    {
        do
        {
            if (string.IsNullOrEmpty(m_Character))
                break;

            if (string.IsNullOrEmpty(m_HeadElement))
                break;

            if (string.IsNullOrEmpty(m_BodyElement))
                break;

            return true;

        } while (false);

        return false;
    }
}

