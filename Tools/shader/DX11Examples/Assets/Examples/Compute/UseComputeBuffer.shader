Shader "DX11/Use Compute Buffer" {
SubShader {
Pass {

CGPROGRAM
#pragma target 5.0

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

struct vs_input {
	float4 vertex : POSITION;
};

StructuredBuffer<float4> bufColors;


struct ps_input {
	float4 pos : SV_POSITION;
	float4 color : COLOR;
};

ps_input vert (vs_input v, uint id : SV_VertexID)
{
	ps_input o;
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.color = bufColors[id] * 0.5 + 0.5;
	return o;
}

float4 frag (ps_input i) : COLOR
{
	return i.color;
}

ENDCG

}
}

Fallback Off
}
