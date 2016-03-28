Shader "DX11/UAV In Pixel Shader" {
Properties {
	_MainTex ("Texture", 2D) = "white" {}
}
SubShader {
Pass {
	Cull Off ZWrite Off ZTest Always Fog { Mode Off }

CGPROGRAM
#pragma target 5.0

#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

sampler2D _MainTex;

RWTexture2D<float4> _OutputTex;


float4 frag (v2f_img i) : COLOR
{
	float4 col = tex2D (_MainTex, i.uv);
	
	int2 loc;
	
	loc.x = int(col.r*255.0); loc.y = 0;
	_OutputTex[loc] = float4(1,0,0,1);
	
	loc.x = int(col.g*255.0); loc.y = 1;
	_OutputTex[loc] = float4(0,1,0,1);
	
	loc.x = int(col.b*255.0); loc.y = 2;
	_OutputTex[loc] = float4(0,0,1,1);
	
	return lerp (col, Luminance(col), 0.5);
}

ENDCG

}
}

Fallback Off
}
