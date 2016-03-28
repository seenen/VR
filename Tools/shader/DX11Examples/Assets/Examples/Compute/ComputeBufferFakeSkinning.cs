using UnityEngine;

public class ComputeBufferFakeSkinning : MonoBehaviour {
	
	public int instanceCount = 10;
	
	private ComputeBuffer buffer;
	private Matrix4x4[] matrices;
	
	void Start () {
		var mesh = new Mesh();
		
		var tris = new int[instanceCount * 12];
		var verts = new Vector3[instanceCount * 4];
		var uvs = new Vector2[instanceCount * 4];
		var colors = new Color[instanceCount * 4];
		
		for (var i = 0; i < instanceCount; ++i)
		{
			tris[i*12+ 0] = i*4+0;
			tris[i*12+ 1] = i*4+3;
			tris[i*12+ 2] = i*4+1;
			tris[i*12+ 3] = i*4+1;
			tris[i*12+ 4] = i*4+3;
			tris[i*12+ 5] = i*4+2;
			tris[i*12+ 6] = i*4+2;
			tris[i*12+ 7] = i*4+3;
			tris[i*12+ 8] = i*4+0;
			tris[i*12+ 9] = i*4+0;
			tris[i*12+10] = i*4+1;
			tris[i*12+11] = i*4+2;
			verts[i*4+0] = new Vector3(-.5f,0,-.5f);
			verts[i*4+1] = new Vector3( .5f,0,-.5f);
			verts[i*4+2] = new Vector3(   0,0, 1);
			verts[i*4+3] = new Vector3(   0,1, 0);
			uvs[i*4+0] = new Vector2(0,0);
			uvs[i*4+1] = new Vector2(1,0);
			uvs[i*4+2] = new Vector2(.5f,0);
			uvs[i*4+3] = new Vector2(.5f,1);
			var col = new Color(i/255.0f,0,0,0);
			colors[i*4+0] = col;
			colors[i*4+1] = col;
			colors[i*4+2] = col;
			colors[i*4+3] = col;
		}
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.colors = colors;
		mesh.triangles = tris;
		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
		
		ReleaseBuffers ();
		
		buffer = new ComputeBuffer (instanceCount, 64);
		GetComponent<Renderer>().material.SetBuffer ("buf_BoneMatrices", buffer);
		
		matrices = new Matrix4x4[instanceCount];
	}
	
	private void ReleaseBuffers() {
		if (buffer != null) buffer.Release();
		buffer = null;
	}
	
	void OnDisable() {
		ReleaseBuffers ();
	}
	
	void Update () {
		var t = Time.timeSinceLevelLoad;
		for (var i = 0; i < instanceCount; ++i)
		{
			t += 13.0f;
			matrices[i].SetTRS (new Vector3(i,transform.position.y,0), Quaternion.Euler(t * (i*4+13.0f), t * (i*2+7.0f), t * 2.0f), new Vector3(1,1,1));
		}
		buffer.SetData (matrices);
	}	
}
