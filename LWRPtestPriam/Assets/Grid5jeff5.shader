Shader "Custom/XZGridJeff1"
{
	Properties
	{
		_Color("Color", Color) = (0.5,1,1,0)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_GridStep("Grid size", Float) = 10
		_GridTiling("Grid Tiling", Float) = 1
		_GridWidth("Grid width", Float) = 1
		_GridOffset("Grid Offset", Vector) = (0,0,0,0)
		_GridAngle("Grid Angle", Float) = 0

		_GridStep2("Grid size2", Float) = 10
		_GridTiling2("Grid Tiling2", Float) = 1
		_GridWidth2("Grid width2", Float) = 5
		_GridOffset2("Grid Offset2", Vector) = (0,0,0,0)
		_GridAngle2("Grid Angle2", Float) = 1
	}
		SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows alpha:blend

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;

	struct Input
	{
		float2 uv_MainTex;
		float2 uv_MainTex2;
		float3 worldPos;
		float3 worldPos2;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	fixed4 _Color2;
	float _GridStep;
	float _GridWidth;
	float2 _GridOffset;
	float _GridAngle;
	float _GridTiling;
	float _GridStep2;
	float _GridWidth2;
	float2 _GridOffset2;
	float _GridAngle2;
	float _GridTiling2;


	void surf(Input IN, inout SurfaceOutputStandard o) {
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

		// grid overlay
		float2 gridPos = IN.worldPos.xz;
		float2 gridRot = gridPos;
		gridPos.x = gridRot.x * cos(_GridAngle) - gridRot.y * sin(_GridAngle);
		gridPos.y = gridRot.x * sin(_GridAngle) + gridRot.y * cos(_GridAngle);
		gridPos += _GridOffset;
		float2 pos = gridPos / _GridStep;
		pos *= floor(_GridTiling) * 2 - 1;
		float2 f = abs(frac(pos) - .5);
		float2 df = fwidth(pos) * _GridWidth;
		float2 g = smoothstep(-df ,df , f);
		float grid = 1.0 - saturate(g.x * g.y);
		//c.rgb = lerp(c.rgb, float3(.2, .2, .2), grid); //OLD
		float3 gridColor = float3(.2, .2, .2);	//NEW
		c.rgb += gridColor * grid;				//NEW
		c.a += grid;							//NEW


		// grid 2 overlay
		float2 gridPos2 = IN.worldPos.xz;
		float2 gridRot2 = gridPos2;
		gridPos2.x = gridRot2.x * cos(_GridAngle2) - gridRot2.y * sin(_GridAngle2);
		gridPos2.y = gridRot2.x * sin(_GridAngle2) + gridRot2.y * cos(_GridAngle2);
		gridPos2 += _GridOffset2;
		//float2 pos2 = IN.worldPos.xz / _GridStep;	//OLD
		float2 pos2 = gridPos2 / _GridStep;			//NEW
		pos2 *= floor(_GridTiling2) * 2 - 1;
		float2 f2 = abs(frac(pos2) - .5);
		float2 df2 = fwidth(pos2) * _GridWidth2;
		float2 g2 = smoothstep(-df2, df2, f2);
		float grid2 = 1.0 - saturate(g2.x * g2.y);
		//c.rgb = lerp(c.rgb, float3(.5, .5, .7), grid2);
		float3 gridColor2 = float3(.5, .5, .7);	//NEW
		c.rgb += gridColor2 * grid2;			//NEW
		c.a += grid2;							//NEW


		//c.a = grid;

		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}
	ENDCG
	}
	FallBack "Diffuse"
}
