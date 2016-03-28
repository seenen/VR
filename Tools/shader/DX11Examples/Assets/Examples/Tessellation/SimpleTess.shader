Shader "Simple Tess" {
Properties {
	_MainTex ("Main Texture", 2D) = "white" {}
}
SubShader {
Pass {
	Tags {"LightMode" = "Vertex"}

CGPROGRAM
#pragma target 5.0

//#pragma vertex VS_RenderScene
//#pragma fragment PS_RenderSceneTextured

#pragma vertex VS_RenderSceneWithTessellation
#pragma fragment PS_RenderSceneTextured
#pragma hull HS_PNTriangles
#pragma domain DS_PNTriangles

#include "UnityCG.cginc"


//=================================================================================================================================
// Buffers, Textures and Samplers
//=================================================================================================================================

Texture2D _MainTex;
SamplerState	sampler_MainTex;

//=================================================================================================================================
// Shader structures
//=================================================================================================================================


struct VS_RenderSceneInput
{
    float3 vertex : POSITION;
    float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
};

struct HS_Input
{
    float4 f4Position   : POS;
    float3 f3Normal     : NORMAL;
    float2 f2TexCoord   : TEXCOORD;
};

struct HS_ConstantOutput
{
    float fTessFactor[3]    : SV_TessFactor;
    float fInsideTessFactor : SV_InsideTessFactor;
};

struct HS_ControlPointOutput
{
    float3    f3Position    : POS;
    float3    f3Normal      : NORMAL;
    float2    f2TexCoord    : TEXCOORD;
};

struct DS_Output
{
    float4 f4Position   : SV_Position;
    float2 f2TexCoord   : TEXCOORD0; 
    float4 f4Diffuse    : COLOR0;
};

struct PS_RenderSceneInput
{
    float4 f4Position   : SV_Position;
    float2 f2TexCoord   : TEXCOORD0;
    float4 f4Diffuse    : COLOR0;
};

struct PS_RenderOutput
{
    float4 f4Color      : SV_Target0;
};

PS_RenderSceneInput VS_RenderScene( VS_RenderSceneInput I )
{
    PS_RenderSceneInput O;
    
    O.f4Position = mul (UNITY_MATRIX_MVP, float4(I.vertex, 1.0f));
	float3 viewN = mul ((float3x3)UNITY_MATRIX_IT_MV, I.normal);
    
    // Calc diffuse color    
    O.f4Diffuse.rgb = unity_LightColor[0].rgb * max(0, dot(viewN, unity_LightPosition[0].xyz)) + UNITY_LIGHTMODEL_AMBIENT.rgb;
    O.f4Diffuse.a = 1.0f;
    
    // Pass through texture coords
    O.f2TexCoord = I.texcoord; 
    
    return O;    
}


HS_Input VS_RenderSceneWithTessellation( VS_RenderSceneInput I )
{
    HS_Input O;
	
	// To view space
    O.f4Position = mul(UNITY_MATRIX_MV, float4(I.vertex,1.0f));
	O.f3Normal = mul ((float3x3)UNITY_MATRIX_IT_MV, I.normal);
        
    O.f2TexCoord = I.texcoord;
    
    return O;
}


HS_ConstantOutput HS_PNTrianglesConstant (InputPatch<HS_Input, 3> I)
{
    HS_ConstantOutput O = (HS_ConstantOutput)0;
	O.fTessFactor[0] = O.fTessFactor[1] = O.fTessFactor[2] = 2.0f;
	O.fInsideTessFactor = 2.0f;
    return O;
}

[domain("tri")]
[partitioning("fractional_odd")]
[outputtopology("triangle_cw")]
[patchconstantfunc("HS_PNTrianglesConstant")]
[outputcontrolpoints(3)]
HS_ControlPointOutput HS_PNTriangles( InputPatch<HS_Input, 3> I, uint uCPID : SV_OutputControlPointID )
{
    HS_ControlPointOutput O = (HS_ControlPointOutput)0;
    O.f3Position = I[uCPID].f4Position.xyz;
    O.f3Normal = I[uCPID].f3Normal;
    O.f2TexCoord = I[uCPID].f2TexCoord;
    return O;
}


[domain("tri")]
DS_Output DS_PNTriangles( HS_ConstantOutput HSConstantData, const OutputPatch<HS_ControlPointOutput, 3> I, float3 bary : SV_DomainLocation )
{
    DS_Output O = (DS_Output)0;

    float3 f3Position = bary.x * I[0].f3Position + bary.y * I[1].f3Position + bary.z * I[2].f3Position;
    float3 f3Normal = bary.x * I[0].f3Normal + bary.y * I[1].f3Normal + bary.z * I[2].f3Normal;

    // Normalize the interpolated normal    
    f3Normal = normalize( f3Normal );

    // Linearly interpolate the texture coords
    O.f2TexCoord = I[0].f2TexCoord * bary.x + I[1].f2TexCoord * bary.y + I[2].f2TexCoord * bary.z;

    // Calc diffuse color    
    O.f4Diffuse.rgb = unity_LightColor[0].rgb * max( 0, dot( f3Normal, unity_LightPosition[0].xyz ) ) + UNITY_LIGHTMODEL_AMBIENT.rgb;
    O.f4Diffuse.a = 1.0f; 

    // Transform position with projection matrix
    O.f4Position = mul (UNITY_MATRIX_P, float4(f3Position.xyz,1.0));
        
    return O;
}


PS_RenderOutput PS_RenderSceneTextured( PS_RenderSceneInput I )
{
    PS_RenderOutput O;
    
    O.f4Color = _MainTex.Sample( sampler_MainTex, I.f2TexCoord ) * I.f4Diffuse;
    
    return O;
}

ENDCG

}
}

Fallback "VertexLit"
}
