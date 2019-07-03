// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Digicrafts/WireframeGrid/Unlit/Texture" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("Main Color", Color) = (0.5,0.5,0.5,1)

	// Wireframe Properties
	[HDR]_WireframeColor ("_WireframeColor", Color) = (1,1,1,1)
	_WireframeTex ("_WireframeTex", 2D) = "white" {}
	[Enum(UV0,0,UV1,1)] _WireframeUV ("_WireframeUV", Float) = 0
	_WireframeSize ("_WireframeSize", Range(0.0, 10.0)) = 1.5
	[Toggle(_WIREFRAME_LIGHTING)]_WireframeLighting ("_WireframeLighting", Float) = 0
	[Toggle(_WIREFRAME_AA)]_WireframeAA ("_WireframeAA", Float) = 1
	[Toggle]_WireframeDoubleSided ("2 Sided", Float) = 0
	_WireframeMaskTex ("_WireframeMaskTex", 2D) = "white" {}
	_WireframeTexAniSpeedX ("_WireframeTexAniSpeedX", Float) = 0
	_WireframeTexAniSpeedY ("_WireframeTexAniSpeedY", Float) = 0
	_GridSpacing ("_GridSpacing", Float) = 0.5
	[Toggle]_GridSpacingScale ("_GridRelatedScale", Float) = 0
	[Toggle]_GridUseWorldspace ("_GridUseWorldspace", Float) = 0

	_WireframeAlphaCutoff ("_WireframeAlphaCutoff", Range(0.0, 1.0)) = 0
	[HideInInspector] _WireframeAlphaMode ("__WireframeAlphaMode", Float) = 0
	[HideInInspector] _WireframeCull ("__WireframeCull", Float) = 2
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	Cull [_WireframeCull]

	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma shader_feature _WIREFRAME_LIGHTING
			#pragma shader_feature _WIREFRAME_AA
			#pragma shader_feature _WIREFRAME_ALPHA_NORMAL _WIREFRAME_ALPHA_TEX_ALPHA _WIREFRAME_ALPHA_TEX_ALPHA_INVERT _WIREFRAME_ALPHA_MASK
			#pragma target 3.0

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			#include "UnityCG.cginc"
			#include "Assets/Digicrafts/WireframeGrid/Shaders/Core/Core.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv4 : TEXCOORD3;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				half2 tex : TEXCOORD0;
				DC_WIREFRAME_COORDS(1,2)
				UNITY_FOG_COORDS(3)
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex = TRANSFORM_TEX(v.uv0, _MainTex);
				DC_WIREFRAME_TRANSFER_COORDS(o);
				UNITY_TRANSFER_FOG(o,o.pos);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{						
				fixed4 c = tex2D(_MainTex, i.tex)*_Color;
				DC_APPLY_WIREFRAME(c.rgb,c.a,i)
				UNITY_APPLY_FOG(i.fogCoord, c);
				UNITY_OPAQUE_ALPHA(c.a);
//				c=fixed4(mass,1);
				return c;
			}

		ENDCG
	}
}
CustomEditor "WireframeGridGeneralShaderGUI"
}
