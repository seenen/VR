using UnityEngine;
using System.Collections;

/// <summary>
/// ½ÇÉ«AssetBundle¼ÓÔØÆ÷  
/// </summary>
public class CharacterLoader : AssetBaseLoader
{
    public CharacterLoader(string bundleName) : base(bundleName) { }

    public override void Dispose()
    {
        base.Dispose();
    }


}
