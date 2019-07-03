// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit shader. Simplest possible colored shader.
// - no lighting
// - no lightmap support
// - no texture

Shader "Digicrafts/WireframeGrid/Unlit/Color" {
Properties {
	
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

			#pragma shader_feature _DebugUV
			uniform fixed4 _Color;
																					
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
				DC_WIREFRAME_COORDS(0,1)
				UNITY_FOG_COORDS(2)
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				DC_WIREFRAME_TRANSFER_COORDS(o);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c = _Color;
				UNITY_APPLY_FOG(i.fogCoord, c);
				UNITY_OPAQUE_ALPHA(c.a);
				DC_APPLY_WIREFRAME(c.rgb,c.a,i)

				#if _DebugUV
					return i.mass;
				#else 
					return c;
				#endif
			}
		ENDCG
	}
}
CustomEditor "WireframeGridGeneralShaderGUI"
}
