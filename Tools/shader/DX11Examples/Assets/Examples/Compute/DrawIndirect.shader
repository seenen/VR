Shader "DX11/DrawIndirect" {
Properties {
	_MainTex ("", 2D) = "white" {}
	_Sprite ("", 2D) = "white" {}
}
SubShader {

Pass
{
	ZWrite Off ZTest Always Cull Off Fog { Mode Off }

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma target 5.0
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert (appdata v)
	{
		v2f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;
		return o;
	}

	sampler2D _MainTex;
	AppendStructuredBuffer<float2> pointBufferOutput : register(u1);


	fixed4 frag (v2f i) : COLOR0
	{
		fixed4 c = tex2D (_MainTex, i.uv);
		fixed4 c1 = tex2D (_MainTex, i.uv + 2.0/_ScreenParams.xy);
		float lumc = Luminance (c);
		float lumc1 = Luminance (c1);
		[branch]
		if (lumc > 0.98 && lumc > lumc1)
		{
			pointBufferOutput.Append (i.uv);
		}
		return c * 0.6;
	}
	ENDCG
}


Pass {

	ZWrite Off ZTest Always Cull Off Fog { Mode Off }
	Blend SrcAlpha One

	CGPROGRAM
	#pragma target 5.0

	#pragma vertex vert
	#pragma geometry geom
	#pragma fragment frag

	#include "UnityCG.cginc"

	StructuredBuffer<float2> pointBuffer;

	struct vs_out {
		float4 pos : SV_POSITION;
	};

	vs_out vert (uint id : SV_VertexID)
	{
		vs_out o;
		o.pos = float4(pointBuffer[id] * 2.0 - 1.0, 0, 1);
		return o;
	}

	struct gs_out {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	float _Size;

	[maxvertexcount(4)]
	void geom (point vs_out input[1], inout TriangleStream<gs_out> outStream)
	{
		float dx = _Size;
		float dy = _Size * _ScreenParams.x / _ScreenParams.y;
		gs_out output;
		output.pos = input[0].pos + float4(-dx, dy,0,0); output.uv=float2(0,0); outStream.Append (output);
		output.pos = input[0].pos + float4( dx, dy,0,0); output.uv=float2(1,0); outStream.Append (output);
		output.pos = input[0].pos + float4(-dx,-dy,0,0); output.uv=float2(0,1); outStream.Append (output);
		output.pos = input[0].pos + float4( dx,-dy,0,0); output.uv=float2(1,1); outStream.Append (output);
		outStream.RestartStrip();
	}

	sampler2D _Sprite;
	fixed4 _Color;

	fixed4 frag (gs_out i) : COLOR0
	{
		fixed4 col = tex2D (_Sprite, i.uv);
		return _Color * col;
	}

	ENDCG

}

}

Fallback Off
}
