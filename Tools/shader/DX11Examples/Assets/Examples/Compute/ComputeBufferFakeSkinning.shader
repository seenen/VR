Shader "DX11/Fake Skinning with Compute Buffer" {
Properties {
	_MainTex ("Texture", 2D) = "white" {}
}
SubShader {
Pass {
	Tags {"LightMode" = "Vertex"}

CGPROGRAM
#pragma target 5.0

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

struct vs_input {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
	float4 color : COLOR; // color.r is used as "bone index"!
};

StructuredBuffer<float4x4> buf_BoneMatrices;


struct ps_input {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float4 color : COLOR;
};

ps_input vert (vs_input v, uint id : SV_VertexID)
{
	ps_input o;
	
	int boneIndex = (int)(v.color.r * 255.0f);
	
	float4x4 worldMat = buf_BoneMatrices[boneIndex];
	//float4x4 worldMat = _Object2World;
	
	float4 worldPos = mul (worldMat, v.vertex);
	o.pos = mul (UNITY_MATRIX_VP, worldPos);
	
	float3 worldN = mul ((float3x3)worldMat, v.normal);	
	float3 viewN = mul ((float3x3)UNITY_MATRIX_V, worldN);
	
    o.color.rgb = unity_LightColor[0].rgb * max(0, dot(viewN, unity_LightPosition[0].xyz)) + UNITY_LIGHTMODEL_AMBIENT.rgb;
    o.color.a = 1.0f;
    
	o.uv = v.texcoord;
	
	return o;
}

sampler2D _MainTex;

float4 frag (ps_input i) : COLOR
{
	return tex2D (_MainTex, i.uv) * i.color * 2.0f;
}

ENDCG

}
}

Fallback Off
}
