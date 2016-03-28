using UnityEngine;

public class DrawInstancedFromBuffer : MonoBehaviour {
	
	public Material material;
	public int vertexCount = 30;
	public int instanceCount = 100;
	
	private ComputeBuffer bufferPoints;
	private ComputeBuffer bufferPos;
	private Vector3[] origPos;
	private Vector3[] pos;
	
	void Start () {
		var verts = new Vector3[vertexCount];
		for (var i = 0; i < vertexCount; ++i)
		{
			float phi = i * Mathf.PI * 2.0f / (vertexCount-1);
			verts[i] = new Vector3(Mathf.Cos(phi), Mathf.Sin(phi), 0.0f);
		}
		
		ReleaseBuffers ();
		
		bufferPoints = new ComputeBuffer (vertexCount, 12);
		bufferPoints.SetData (verts);
		material.SetBuffer ("buf_Points", bufferPoints);

		origPos = new Vector3[instanceCount];
		for (var i = 0; i < instanceCount; ++i)
			origPos[i] = Random.insideUnitSphere * 5.0f;
		pos = new Vector3[instanceCount];
		bufferPos = new ComputeBuffer (instanceCount, 12);
		material.SetBuffer ("buf_Positions", bufferPos);
	}
	
	private void ReleaseBuffers() {
		if (bufferPoints != null) bufferPoints.Release();
		bufferPoints = null;
		if (bufferPos != null) bufferPos.Release();
		bufferPos = null;
	}
	
	void OnDisable() {
		ReleaseBuffers ();
	}
	
	// each frame, update the positions buffer (one vector per instance)
	void Update () {
		var t = Time.timeSinceLevelLoad;
		for (var i = 0; i < instanceCount; ++i)
		{
			var x = Mathf.Sin ((t + i) * 1.17f);
			var y = Mathf.Sin ((t - i) * 1.0f);
			var z = Mathf.Cos ((t + i) * 1.87f);
			pos[i] = origPos[i] + new Vector3(x,y,z);
		}
		bufferPos.SetData (pos);
	}
	
	// called if script attached to the camera, after all regular rendering is done
	void OnPostRender () {
		material.SetPass (0);
		Graphics.DrawProcedural (MeshTopology.LineStrip, vertexCount, instanceCount);
	}
}
