using UnityEngine;
using System.Collections;

public class FogSetting : MonoBehaviour 
{
    public bool fog = false;
    public Color fogColor = Color.black;
    public FogMode fogMode = FogMode.ExponentialSquared;
    public float fogDensity = 270;
    public UnityEngine.Rendering.AmbientMode ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
    public Color ambientLight = Color.white;
    public Color ambientEquatorLight = Color.white;
    public Color ambientGroundLight = Color.white;
    public float ambientIntensity = 1;
}
