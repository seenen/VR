using UnityEngine;

public class UseComputeBuffer : MonoBehaviour {
	
	public bool global = false;
	private ComputeBuffer buffer;
	
	void Start () {
		var mesh = GetComponent<MeshFilter>().mesh;
		
		ReleaseBuffers ();
		
		int n = mesh.vertexCount;
		buffer = new ComputeBuffer (n, 16);
		
		if (global)
		{
			Color[] cols = new Color[n];
			for (int i = 0; i < n; ++i)
				cols[i] = new Color(Mathf.Cos(i*0.1f), Mathf.Sin(i*0.37f), 1.0f, 1.0f);
			buffer.SetData (cols);
			Shader.SetGlobalBuffer ("bufColors", buffer);
		}
		else
		{
			// ComputeBuffer.SetData can take any array of value types, it just copies
			// whatever is there into the buffer
			buffer.SetData (mesh.tangents);
			GetComponent<Renderer>().material.SetBuffer ("bufColors", buffer);
		}
	}
	
	private void ReleaseBuffers() {
		if (buffer != null) buffer.Release();
		buffer = null;
	}
	
	void OnDisable() {
		ReleaseBuffers ();
	}
	
}
