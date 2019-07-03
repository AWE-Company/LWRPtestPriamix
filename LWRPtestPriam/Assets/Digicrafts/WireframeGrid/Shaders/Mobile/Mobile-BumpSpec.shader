// Simplified Bumped Specular shader. Differences from regular Bumped Specular one:
// - no Main Color nor Specular Color
// - specular lighting directions are approximated per vertex
// - writes zero to alpha channel
// - Normalmap uses Tiling/Offset of the Base texture
// - no Deferred Lighting support
// - no Lightmap support
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Digicrafts/WireframeGrid/Mobile/Bumped Specular" {
Properties {
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}

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
	[HDR]_WireframeEmissionColor("Color", Color) = (0,0,0)

	_WireframeAlphaCutoff ("_WireframeAlphaCutoff", Range(0.0, 1.0)) = 0
	[HideInInspector] _WireframeAlphaMode ("__WireframeAlphaMode", Float) = 0
	[HideInInspector] _WireframeCull ("__WireframeCull", Float) = 2

}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 250
	Cull [_WireframeCull]
CGPROGRAM
#pragma surface surf MobileBlinnPhong exclude_path:prepass noforwardadd halfasview interpolateview vertex:vert //alpha nolightmap
#pragma target 3.0
#pragma shader_feature _WIREFRAME_LIGHTING
#pragma shader_feature _WIREFRAME_AA
#pragma shader_feature _WIREFRAME_ALPHA_NORMAL _WIREFRAME_ALPHA_TEX_ALPHA _WIREFRAME_ALPHA_TEX_ALPHA_INVERT _WIREFRAME_ALPHA_MASK
#include "Assets/Digicrafts/WireframeGrid/Shaders/Core/Core.cginc"
#include "Assets/Digicrafts/WireframeGrid/Shaders/Core/Mobile.cginc"

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput_t s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	#if _WIREFRAME_LIGHTING
		c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
	#else
		c.rgb = lerp((s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten,s.Base,s.w);
	#endif
	c.a=s.Alpha;
	return c;
}

sampler2D _MainTex;
sampler2D _BumpMap;
half _Shininess;

struct Input {	
	DC_WIREFRAME_COORDS_MOBILE
};
void vert (inout appdata_full_t v, out Input o) {
      UNITY_INITIALIZE_OUTPUT(Input,o);
      DC_WIREFRAME_TRANSFER_COORDS_MOBILE(o)
}
void surf (Input i, inout SurfaceOutput_t o) {
	fixed4 c = tex2D(_MainTex, i.uv_MainTex);
	DC_APPLY_WIREFRAME_MOBILE(c.rgb,c.a,i,w)
	o.Emission = _WireframeEmissionColor*w;
	#if _WIREFRAME_LIGHTING
		o.Gloss = c.a;
		o.Albedo = c.rgb;
		o.Specular = _Shininess;
		o.Normal = UnpackNormal (tex2D(_BumpMap, i.uv_MainTex));
	#else 
		o.Base=c.rgb;
		o.Gloss = c.a;
		o.w=w;
		o.Albedo = lerp(c.rgb,float3(0,0,0),w);
		o.Specular = _Shininess;
		o.Normal = lerp(UnpackNormal(tex2D(_BumpMap, i.uv_MainTex)),half3(0,0,1),w);		
	#endif
	o.Alpha = c.a;
}
ENDCG
}
FallBack "Mobile/VertexLit"
CustomEditor "WireframeGridGeneralShaderGUI"
}
