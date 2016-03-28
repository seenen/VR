using UnityEngine;

[ExecuteInEditMode]
public class UAVInPixelShader : MonoBehaviour
{
	public Shader shader;
	
	private Material mat;
	private RenderTexture histogram;
		
	void OnDisable ()
	{
		DestroyImmediate (mat); mat = null;
		DestroyImmediate (histogram); histogram = null;
	}
	
	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		if (!SystemInfo.supportsComputeShaders)
		{
			Graphics.Blit (src, dst);
			Debug.LogWarning ("Compute shaders not supported (not using DX11?)");
			return;			
		}
		if (!mat)
		{
			mat = new Material (shader);
			mat.hideFlags = HideFlags.HideAndDontSave;
		}
		if (!histogram)
		{
			histogram = new RenderTexture (256, 4, 0, RenderTextureFormat.ARGB32);
			histogram.hideFlags = HideFlags.HideAndDontSave;
			histogram.enableRandomWrite = true;
			histogram.filterMode = FilterMode.Point;
			histogram.Create();
		}
		
		// clear histogram to black
		Graphics.SetRenderTarget (histogram);
		GL.Clear (false, true, new Color(0,0,0,1));
		
		Graphics.ClearRandomWriteTargets();
		Graphics.SetRandomWriteTarget (1, histogram);
		
		Graphics.Blit (src, dst, mat, 0);
		
		Graphics.ClearRandomWriteTargets();
		
		// draw the histogram itself
		GL.LoadPixelMatrix();
		Graphics.DrawTexture (new Rect(0,-8,512,32), histogram);
	}
}
