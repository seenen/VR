using UnityEngine;

// Postprocessing script.
//
// First pass just renders a quad with a shader that darkens the screen, but that
// shader also picks out "bright pixels" and writes out their positions into
// an Append compute buffer.
//
// In the second pass, we place a textured sprite at each of those pixels using
// DrawProceduralIndirect. Vertex shader reads the sprite position for each vertex,
// and geometry shader expands that to a quad.
//
// The first pass generated an unknown number of points, that is why "Indirect" draw
// is used where the amount of points to render is read from another buffer which is
// filled using CopyCount. This way the whole process stays on the GPU without the
// need to read anything back to the CPU.

[ExecuteInEditMode]
public class DrawIndirect : MonoBehaviour
{
	public Texture2D sprite;
	[RangeAttribute(0.02f,0.5f)]
	public float size = 0.1f;
	public Color color = new Color(1.0f,0.6f,0.3f,0.03f);
	public Shader shader;
	private Material mat;
	private ComputeBuffer cbDrawArgs;
	private ComputeBuffer cbPoints;

	private void CreateResources ()
	{
		if (cbDrawArgs == null)
		{
			cbDrawArgs = new ComputeBuffer (1, 16, ComputeBufferType.DrawIndirect);
			var args = new int[4];
			args[0] = 0;
			args[1] = 1;
			args[2] = 0;
			args[3] = 0;
			cbDrawArgs.SetData (args);
		}
		if (cbPoints == null)
		{
			cbPoints = new ComputeBuffer (10000, 8, ComputeBufferType.Append);
		}
		if (mat == null)
		{
			mat = new Material(shader);
			mat.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void ReleaseResources ()
	{
		if (cbDrawArgs != null) cbDrawArgs.Release (); cbDrawArgs = null;
		if (cbPoints != null) cbPoints.Release(); cbPoints = null;
		Object.DestroyImmediate (mat);
	}
	
	void OnDisable ()
	{
		ReleaseResources ();
	}
	
	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		if (!shader)
			return;
		if (!SystemInfo.supportsComputeShaders)
		{
			Graphics.Blit (src, dst);
			Debug.LogWarning ("Compute shaders not supported (not using DX11?)");
			return;			
		}
		CreateResources ();

		mat.SetTexture("_Sprite", sprite);
		mat.SetFloat("_Size", size);
		mat.SetColor("_Color", color);

		Graphics.SetRandomWriteTarget (1, cbPoints);
		Graphics.Blit (src, dst, mat, 0);

		Graphics.ClearRandomWriteTargets ();
		Graphics.SetRenderTarget (dst);
		
		ComputeBuffer.CopyCount (cbPoints, cbDrawArgs, 0);
		mat.SetBuffer ("pointBuffer", cbPoints);
		mat.SetPass (1);
		Graphics.DrawProceduralIndirect (MeshTopology.Points, cbDrawArgs, 0);
	}
}
