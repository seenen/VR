using UnityEngine;

[ExecuteInEditMode]
public class AveragePixelsCS : MonoBehaviour
{
	public ComputeShader cs;
	
	private int width, height;
	//private ComputeBuffer buffer0;
	private RenderTexture rtout0;

	private void CreateBuffersIfNeeded (RenderTexture src)
	{
		int w = Mathf.CeilToInt (src.width / 8.0f);
		int h = Mathf.CeilToInt (src.height / 8.0f);
		if (rtout0 != null && width == w && height == h)
			return; // all created & same size, do nothing
		
		ReleaseBuffers();
		width = w;
		height = h;
		//buffer0 = ComputeBuffer.GetBuffer (width * height, 4);
		rtout0 = new RenderTexture (width, height, 0, RenderTextureFormat.RGHalf); // RG format just for kicks!
		rtout0.enableRandomWrite = true;
		rtout0.Create();
	}
	
	private void ReleaseBuffers ()
	{
		//if (buffer0 != null) ComputeBuffer.ReleaseBuffer (buffer0);
		//buffer0 = null;
		if (rtout0 != null) DestroyImmediate (rtout0);
		rtout0 = null;
		width = height = 0;
	}
	
	void OnDisable ()
	{
		ReleaseBuffers ();
	}
	
	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		if (!SystemInfo.supportsComputeShaders)
		{
			Graphics.Blit (src, dst);
			Debug.LogWarning ("Compute shaders not supported (not using DX11?)");
			return;
		}

		CreateBuffersIfNeeded (src);
		
		cs.SetTexture (0, "Input", src);
		cs.SetInts ("g_param", src.width, src.height);
		//cs.SetBuffer (0, "Result", buffer0);
		cs.SetTexture (0, "Result", rtout0);
		cs.Dispatch (0, width, height, 1);
		
		Graphics.Blit (rtout0, dst);
	}
}
