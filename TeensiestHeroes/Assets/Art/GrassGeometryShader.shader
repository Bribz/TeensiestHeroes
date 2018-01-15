// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Low Poly Shader developed as part of World of Zero: http://youtube.com/worldofzerodevelopment
// Based upon the example at: http://www.battlemaze.com/?p=153

Shader "Custom/Grass Geometry Shader" 
{
	Properties
	{
		[HDR]_BackgroundColor("Background Color", Color) = (1,0,0,1)
		[HDR]_ForegroundColor("Foreground Color", Color) = (0,0,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Cutoff("Cutoff", Range(0,1)) = 0.25
		_GrassHeight("Grass Height", Float) = 0.25
		_GrassWidth("Grass Width", Float) = 0.25
		_WindSpeed("Wind Speed", Float) = 100
		_WindStrength("Wind Strength", Float) = 0.05
	}

	SubShader
	{
		Tags{ "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" } //TEST SHADOWCASTER "IgnoreProjector" = "True"
		LOD 200

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			//CULL OFF

			CGPROGRAM
			#include "UnityCG.cginc" 
			#include "Autolight.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			//#pragma alphatest : _Cutoff

			// Use shader model 4.0 target, we need geometry shader support
			#pragma target 4.0

			sampler2D _MainTex;

			struct v2g
			{
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float2 uv : TEXCOORD0;
				float3 color : TEXCOORD1;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float3 norm : NORMAL;
				float2 uv : TEXCOORD0;
				float3 diffuseColor : TEXCOORD1;
				//float3 specularColor : TEXCOORD2;
				LIGHTING_COORDS(2, 3)
				UNITY_FOG_COORDS(4)
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _BackgroundColor;
			fixed4 _ForegroundColor;
			half _GrassHeight;
			half _GrassWidth;
			half _Cutoff;
			half _WindStrength;
			half _WindSpeed;
			uniform float4 _LightColor0;

			v2g vert(appdata_full v)
			{
				float3 v0 = v.vertex.xyz;

				v2g OUT;
				OUT.pos = v.vertex;
				OUT.norm = v.normal;
				OUT.uv = v.texcoord;
				OUT.color = v.color; //tex2Dlod(_MainTex, v.texcoord).rgb;
		
				//TEST
				//OUT.pos = mul(UNITY_MATRIX_MVP, v.position);
				

				return OUT;
			}

			[maxvertexcount(4)]
			void geom(point v2g IN[1], inout TriangleStream<g2f> triStream)
			{
				float3 lightPosition = _WorldSpaceLightPos0;

				float3 perpendicularAngle = float3(-1, 0, 0);
				float3 tiltAngle = float3(0, 0, 0.15);
				float3 faceNormal = cross(perpendicularAngle, IN[0].norm);

				float3 v0 = IN[0].pos.xyz;
				float3 v1 = IN[0].pos.xyz + tiltAngle + IN[0].norm * _GrassHeight;

				float3 wind = float3(sin(_Time.x * _WindSpeed + v0.x) + sin(_Time.x * _WindSpeed + v0.z * 2) + sin(_Time.x * _WindSpeed * 0.1 + v0.x), 0,
					cos(_Time.x * _WindSpeed + v0.x * 2) + cos(_Time.x * _WindSpeed + v0.z));
				v1 += wind * _WindStrength;

				float3 color = (IN[0].color);

				float sin30 = 0.5;
				float sin60 = 0.866f;
				float cos30 = sin60;
				float cos60 = sin30;

				g2f OUT;

				// Quad 1

				OUT.pos = UnityObjectToClipPos(v0 + (perpendicularAngle * (_GrassWidth * 0.4) * _GrassHeight));
				OUT.norm = faceNormal;
				OUT.diffuseColor = color;
				OUT.uv = float2(0, 0);
				TRANSFER_VERTEX_TO_FRAGMENT(OUT);

				triStream.Append(OUT);

				OUT.pos = UnityObjectToClipPos(v1 + (perpendicularAngle * (_GrassWidth * 0.4) * _GrassHeight));
				OUT.norm = faceNormal;
				OUT.diffuseColor = color;
				OUT.uv = float2(0, 1);
				TRANSFER_VERTEX_TO_FRAGMENT(OUT);

				triStream.Append(OUT);

				OUT.pos = UnityObjectToClipPos(v0 + perpendicularAngle * (_GrassWidth * -0.6));
				OUT.norm = faceNormal;
				OUT.diffuseColor = color;
				OUT.uv = float2(1, 0);
				TRANSFER_VERTEX_TO_FRAGMENT(OUT);

				triStream.Append(OUT);

				OUT.pos = UnityObjectToClipPos(v1 + perpendicularAngle * (_GrassWidth * -0.6));
				OUT.norm = faceNormal;
				OUT.diffuseColor = color;
				OUT.uv = float2(1, 1);
				TRANSFER_VERTEX_TO_FRAGMENT(OUT);

				triStream.Append(OUT);

			}

			half4 frag(g2f IN) : COLOR
			{
				float4 c = tex2D(_MainTex, IN.uv);
				clip(c.a - _Cutoff);
				//if (color.a < _Cutoff)
				//	discard;

				float atten = LIGHT_ATTENUATION(IN);
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				float3 lambert = float(max(0.0, dot(float3(0, 1, 0), lightDirection)));
		
				float3 Lighting = (ambient + lambert * atten) * _LightColor0.rgb;
				//fixed4 color = fixed4(IN.diffuseColor.rgb * Lighting, 1.0);
				//c *= color;

				//TEST
				c = fixed4(c.rgb * IN.diffuseColor * Lighting, 1.0f);
		
				return c;
			}

			ENDCG
		}

	
		// shadow caster
		Pass
		{
			//Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

			Fog{ Mode Off }
			ZWrite On ZTest LEqual

			CGPROGRAM

			#pragma vertex	vertexShader
			#pragma geometry geometryShader
			#pragma fragment fragmentShader

			//#pragma multi_compile_shadowcaster
			//#pragma only_renderers d3d11
			//#define SHADOW_CASTER_PASS

			#include "UnityCG.cginc"
			#include "HLSLSupport.cginc"

			struct VS_INPUT
			{
				float4 position : POSITION;
				float3 norm : NORMAL;
				//float4 uv_Noise : TEXCOORD0;
				//fixed sizeFactor : TEXCOORD1;
			};

			struct SHADOW_VERTEX
			{
				float4 vertex : POSITION; // has to be called this way because of unity macro
			};

			struct GS_INPUT
			{
				float4 worldPosition : TEXCOORD0;
				float3 norm : NORMAL;
				//fixed2 parameters : TEXCOORD1;	// .x = noiseValue, .y = sizeFactor
			};

			struct FS_INPUT
			{
				float2 uv_MainTexture : TEXCOORD0;
				V2F_SHADOW_CASTER;
			};

			uniform sampler2D _MainTex, _NoiseTexture;

			// for billboard
			half _Cutoff;
			uniform float _MinSize, _MaxSize;
			uniform float _MaxCameraDistance;
			uniform float _Transition;
			half _Glossiness;
			half _Metallic;
			fixed4 _BackgroundColor;
			fixed4 _ForegroundColor;
			half _GrassHeight;
			half _GrassWidth;
			half _WindStrength;
			half _WindSpeed;

			GS_INPUT vertexShader(VS_INPUT v)
			{
				GS_INPUT vOut;

				// set output values
				vOut.worldPosition = mul(unity_ObjectToWorld, v.position);
				vOut.norm = v.norm;
				//vOut.parameters.x = tex2Dlod(_NoiseTexture, float4(v.uv_Noise.xyz, 0)).r;
				//vOut.parameters.y = v.sizeFactor;

				return vOut;
			}

			// Geometry Shader
			[maxvertexcount(4)]
			void geometryShader(point GS_INPUT p[1], inout TriangleStream<FS_INPUT> triStream)
			{
				// cutout trough a transition area
				float cameraDistance = length(_WorldSpaceCameraPos - p[0].worldPosition);

				float3 perpendicularAngle = float3(-1, 0, 0);
				float3 tiltAngle = float3(0, 0, 0.15f);
				float3 faceNormal = cross(perpendicularAngle, float3(0,1,0));

				float3 v0 = p[0].worldPosition.xyz;
				float3 v1 = p[0].worldPosition.xyz + tiltAngle + p[0].norm * _GrassHeight;

				float3 wind = float3(sin(_Time.x * _WindSpeed + v0.x) + sin(_Time.x * _WindSpeed + v0.z * 2) + sin(_Time.x * _WindSpeed * 0.1 + v0.x), 0,
					cos(_Time.x * _WindSpeed + v0.x * 2) + cos(_Time.x * _WindSpeed + v0.z));
				v1 += wind * _WindStrength;

				float sin30 = 0.5f;
				float sin60 = 0.866f;
				float cos30 = sin60;
				float cos60 = sin30;

				// Quad 1
				float4 vertices[4];
				vertices[0] = float4(v0 + (perpendicularAngle * (_GrassWidth * 0.4) * _GrassHeight), 1.0);
				vertices[1] = float4(v1 + (perpendicularAngle * (_GrassWidth * 0.4) * _GrassHeight), 1.0);
				vertices[2] = float4(v0 + perpendicularAngle * (_GrassWidth * -0.6), 1.0);
				vertices[3] = float4(v1 + perpendicularAngle * (_GrassWidth * -0.6), 1.0);
			
				FS_INPUT fIn;

				SHADOW_VERTEX v;
				v.vertex = mul(unity_WorldToObject, vertices[0]);
				fIn.uv_MainTexture = float2(0.0f, 0.0f);
				TRANSFER_SHADOW_CASTER(fIn)

				triStream.Append(fIn);

				v.vertex = mul(unity_WorldToObject, vertices[1]);
				fIn.uv_MainTexture = float2(0.0f, 1.0f);
				TRANSFER_SHADOW_CASTER(fIn)

				triStream.Append(fIn);

				v.vertex = mul(unity_WorldToObject, vertices[2]);
				fIn.uv_MainTexture = float2(1.0f, 0.0f);
				TRANSFER_SHADOW_CASTER(fIn)

				triStream.Append(fIn);

				v.vertex = mul(unity_WorldToObject, vertices[3]);
				fIn.uv_MainTexture = float2(1.0f, 1.0f);
				TRANSFER_SHADOW_CASTER(fIn)

				triStream.Append(fIn);
			}

			fixed4 fragmentShader(FS_INPUT fIn) : COLOR
			{
				fixed4 color = tex2D(_MainTex, fIn.uv_MainTexture);
				clip(color.a - _Cutoff);

				SHADOW_CASTER_FRAGMENT(fIn);
			}

			ENDCG
		}
		
	}
}
