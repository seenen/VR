// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Research/Normal Map" {
	Properties {
		_Color ("Color Tint", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("Diffuse Texture", 2D) = "white" {}
		_BumpMap ("Normal Texture", 2D) = "bump" {}
		_BumpDepth ("Bump Depth", Range(-2.0, 2.0)) = 1
		_SpecColor ("Specular Color", Color) = (1.0,1.0,1.0,1.0)
		_Shininess ("Shininess", Float) = 10
		_RimColor ("Rim Color", Color) = (1.0,1.0,1.0,1.0)
		_RimPower ("Rim Power", Range(0.1, 10.0)) = 3.0
	}
	
	SubShader {
		Pass {
			Tags { "LightMode" = "ForwardBase" }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			// Properties Variables
			uniform float4 _Color;
			uniform float4 _SpecColor;
			uniform float4 _RimColor;
			
			uniform float _Shininess;
			uniform float _RimPower;
			uniform float _BumpDepth;
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST; // Scale and Offset
			
			uniform sampler2D _BumpMap;
			uniform float4 _BumpMap_ST; // Scale and Offset
			
			// Unity defined variables
			uniform float4 _LightColor0;			
			
			// Structs
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal: NORMAL;
				float4 tangent: TANGENT;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput{	
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 normalWorld : TEXCOORD2;
				float3 tangentWorld : TEXCOORD3;
				float3 binormalWorld : TEXCOORD4;
			};
			
			// Vertex Function			
			vertexOutput vert(vertexInput v) {
				vertexOutput o;
				
				o.normalWorld = v.normal; // Normal direction
				o.tangentWorld = v.tangent; // Tangent direction
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w); // Binormal direction
				
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // Vertex MVP position
				o.worldPos = mul(unity_ObjectToWorld, v.vertex); // Vertex world position
				o.tex = v.texcoord; // Texture coordenate
				
				return o;
			}
			
			// Fragment Function
			float4 frag (vertexOutput i): COLOR {
				
				// Directions
				float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldPos.xyz);
				float3 lightDirection;
				float attenuation;
				
				// Check if directional or spot light
				if (_WorldSpaceLightPos0.w == 0.0)
				{
					attenuation = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				} 
				else
				{
					float3 f2lVector = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
					float f2lDistance = length(f2lVector);
					attenuation = 1.0 / f2lDistance;
					lightDirection = normalize(f2lVector);
				}
				
				// Texture Mapping
				// Tex2D gets data out of the texture at float2 coordinate (since its an 2D): i.tex.xy // Texture coordenate
				// Our _MainTex_ST holds Scaling and Offset (or Tiling and Offset)
				// So we scale it and offset it.
				// float4 tex is a RGBA data from texture, so we can use it like an color (xyz, rgb,etc)
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				float4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);
				
				// unpackNormal function
				// Unity has an UnpackNormal function built-in CgInclude, but here, we'll do it manually, to understand how it works
				float3 localCoords = float3(2 * texN.ag - float2(1.0, 1.0), 0.0); // Now, texN.ag goes from -1 to 1... instead of 0 to 1.
				
				// This, is the bump "depthness".
				// This value, is REALLY near to 1, but not 1... You could use only 1 if you wanted.
				// Or, for some fanciness, we could toogle it, as we want, using a property.
				// Try out values to see how it affects.
				// localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords);
				localCoords.z = _BumpDepth;
				
				// Normal transpose matrix
				float3x3 local2WorldTranspose = float3x3(
					i.tangentWorld,
					i.binormalWorld,
					i.normalWorld
				);
				
				// calculate normal direction
				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
				
				// Reflections
				
				float3 diffuseReflection = _LightColor0.rgb * attenuation * saturate(dot(normalDirection, lightDirection));
				float3 specularReflection = pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess) * _SpecColor.rgb * diffuseReflection;
				
				// Rim Lightning
				
				float rim = 1 - dot(viewDirection, normalDirection);
				float3 rimLightning = _RimColor.rgb * pow(rim, _RimPower) * diffuseReflection;
				
				// Final Light
				float3 lightFinal = diffuseReflection + specularReflection + rimLightning;
				
				// Now we blend everything together.
				// Lightning, texture and tint color
				return float4(lightFinal * tex.rgb * _Color.rgb, 1.0);
			}
			
			ENDCG
		}
		Pass {
			Tags { "LightMode" = "ForwardAdd" }
			Blend One One
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			// Properties Variables
			uniform float4 _Color;
			uniform float4 _SpecColor;
			uniform float4 _RimColor;
			
			uniform float _Shininess;
			uniform float _RimPower;
			uniform float _BumpDepth;
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST; // Scale and Offset
			
			uniform sampler2D _BumpMap;
			uniform float4 _BumpMap_ST; // Scale and Offset
			
			// Unity defined variables
			uniform float4 _LightColor0;			
			
			// Structs
			struct vertexInput {
				float4 vertex : POSITION;
				float3 normal: NORMAL;
				float4 tangent: TANGENT;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput{	
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float3 normalWorld : TEXCOORD2;
				float3 tangentWorld : TEXCOORD3;
				float3 binormalWorld : TEXCOORD4;
			};
			
			// Vertex Function			
			vertexOutput vert(vertexInput v) {
				vertexOutput o;
				
				o.normalWorld = v.normal; // Normal direction
				o.tangentWorld = v.tangent; // Tangent direction
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w); // Binormal direction
				
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex); // Vertex MVP position
				o.worldPos = mul(unity_ObjectToWorld, v.vertex); // Vertex world position
				o.tex = v.texcoord; // Texture coordenate
				
				return o;
			}
			
			// Fragment Function
			float4 frag (vertexOutput i): COLOR {
				
				// Directions
				float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldPos.xyz);
				float3 lightDirection;
				float attenuation;
				
				// Check if directional or spot light
				if (_WorldSpaceLightPos0.w == 0.0)
				{
					attenuation = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				} 
				else
				{
					float3 f2lVector = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
					float f2lDistance = length(f2lVector);
					attenuation = 1.0 / f2lDistance;
					lightDirection = normalize(f2lVector);
				}
				
				// Texture Mapping
				// Tex2D gets data out of the texture at float2 coordinate (since its an 2D): i.tex.xy // Texture coordenate
				// Our _MainTex_ST holds Scaling and Offset (or Tiling and Offset)
				// So we scale it and offset it.
				// float4 tex is a RGBA data from texture, so we can use it like an color (xyz, rgb,etc)
				//float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				float4 texN = tex2D(_BumpMap, i.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);
				
				// unpackNormal function
				// Unity has an UnpackNormal function built-in CgInclude, but here, we'll do it manually, to understand how it works
				float3 localCoords = float3(2 * texN.ag - float2(1.0, 1.0), 0.0); // Now, texN.ag goes from -1 to 1... instead of 0 to 1.
				
				// This, is the bump "depthness".
				// This value, is REALLY near to 1, but not 1... You could use only 1 if you wanted.
				// Or, for some fanciness, we could toogle it, as we want, using a property.
				// Try out values to see how it affects.
				// localCoords.z = 1.0 - 0.5 * dot(localCoords, localCoords);
				localCoords.z = _BumpDepth;
				
				// Normal transpose matrix
				float3x3 local2WorldTranspose = float3x3(
					i.tangentWorld,
					i.binormalWorld,
					i.normalWorld
				);
				
				// calculate normal direction
				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));
				
				// Reflections
				
				float3 diffuseReflection = _LightColor0.rgb * attenuation * saturate(dot(normalDirection, lightDirection));
				float3 specularReflection = pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess) * _SpecColor.rgb * diffuseReflection;
				
				// Rim Lightning
				
				float rim = 1 - dot(viewDirection, normalDirection);
				float3 rimLightning = _RimColor.rgb * pow(rim, _RimPower) * diffuseReflection;
				
				// Final Light
				float3 lightFinal = diffuseReflection + specularReflection + rimLightning;
				
				// Now we blend everything together.
				// Lightning, texture and tint color
				return float4(lightFinal * _Color.rgb, 1.0);
			}
			
			ENDCG
		}
	}
	//Fallback "Specular"
}