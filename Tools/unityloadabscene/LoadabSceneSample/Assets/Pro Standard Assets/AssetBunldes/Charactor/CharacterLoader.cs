using UnityEngine;
using System.Collections;

/// <summary>
/// ��ɫAssetBundle������  
/// </summary>
public class CharacterLoader : AssetBaseLoader
{
    public CharacterLoader(string bundleName) : base(bundleName) { }

    public override void Dispose()
    {
        base.Dispose();
    }


}
