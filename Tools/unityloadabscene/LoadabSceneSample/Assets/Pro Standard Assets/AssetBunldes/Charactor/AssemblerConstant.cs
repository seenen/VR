/**************************************************
 * 上海美峰数码科技有限公司(http://www.morefuntek.com)  
 * 功能描述: 从AssetBundle生成人物模型  
 * 作者:		wangwenfei 
 * 时间:  	2014年1月8日  
 * 版本:		1.0 
 **************************************************
 */
//#define Sync

using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Object=UnityEngine.Object;










public class AssemblerConstant
{
	public const string ASSETBUNDLE_SUFFIX		= ".assetbundle";
	public const string ANIMATION_SUFFIX		= "_animations";
	public const string CHARACTER_BASE_NAME 	= "characterbase";
	public const string RENDERER_OBJECT_NAME 	= "rendererobject";
	public const string BONE_NAMES				= "bonenames";
	
	public const string HEAD_PART_NAME			= "head";
	public const string BODY_PART_NAME			= "body";
	public const string FOOT_PART_NAME			= "foot";
	
	public const string R_WEAPON_POINT			= "R Hand1";
	public const string L_WEAPON_POINT			= "L Hand1";
	public const string BACK_WEAPON_POINT		= "BackPoint";
	
	public const string R_WEAPON_NAME			= "Weapon_R";
	public const string L_WEAPON_NAME			= "Weapon_L";
	public const string BACK_WEAPON_NAME		= "Weapon_B";
	
	public const string WING_POINT				= "chibangpoint";
	public const string WING_NAME				= "wing";
	public const string WING_MESH_NAME			= "wing_mesh";
}