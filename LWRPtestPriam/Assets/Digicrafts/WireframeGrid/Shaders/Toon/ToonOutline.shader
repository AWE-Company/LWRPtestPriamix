// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Digicrafts/WireframeGrid/CartoonOutline"
{
	Properties
	{			
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline Color", Color) = (0,0,0,0)
		_OutlineWidth ("Outline Width", Range (0, 10)) = 0.5
		[Toggle] _Shade ("Eanble", Float) = 1
		_ShadeTex ("Texture", CUBE) = "gray" {}
      	_ShadePower ("Power",Range(0,1)) = 1
		[Toggle] _Shadow ("Eanble", Float) = 0
		_ShadowPower ("Shadow Power",Range(0,1)) = 0.2
		[Toggle] _Diffuse ("Diffuse", Float) = 0
		_DiffusePower ("Power",Range(0,1)) = 1
		[Toggle] _Ambient ("Ambient", Float) = 0
		_AmbientPower ("Power",Range(0,1)) = 1

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
	SubShader
	{		
		// Outline pass
		Pass
		{
            Name "OUTLINE"
            Cull Front                       	           	
           
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag	
            		
 								
			uniform fixed _OutlineWidth;			
 			uniform fixed4 _OutlineColor;

           	struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
                UNITY_FOG_COORDS(0)
            };
            v2f vert(appdata v)
            {
                v2f o;
				if(_OutlineWidth!=0){	                
					o.pos = UnityObjectToClipPos (v.vertex);
	                float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	                float2 offset = normalize(TransformViewToProjection(norm.xy));
	                float ortho = 100;
	                float persp = 10;               
	                if(UNITY_MATRIX_P[3][3]==0){
	                	o.pos.xy += offset  *  _OutlineWidth / persp;
	                }else{
	                	o.pos.xy += offset * _OutlineWidth  / ortho;
	            	}         	
				} else {
					o.pos = fixed4(0,0,0,0);
	                o.color = _OutlineColor;    
				}
				UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
           
            half4 frag(v2f i) :COLOR
            {          
        		fixed4 c = fixed4(i.color.rgb,i.color.a);
        		UNITY_APPLY_FOG(i.fogCoord,c);
            	return c;
            }
            ENDCG
        }
        // base
		Pass
		{
			Name "BASE"
			Tags { 
				"LightMode"="ForwardBase"
				"RenderType"="Opaque" 
			}
			LOD 100
			Cull [_WireframeCull]

			CGPROGRAM
			#pragma target 3.0

			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#pragma multi_compile_fog
			#pragma shader_feature _DIFFUSE_ON
        	#pragma shader_feature _AMBIENT_ON
        	#pragma shader_feature _SHADOW_ON
        	#pragma shader_feature _SHADE_ON
        	#pragma shader_feature _WIREFRAME_LIGHTING
        	#pragma shader_feature _WIREFRAME_AA
			#pragma shader_feature _WIREFRAME_ALPHA_NORMAL _WIREFRAME_ALPHA_TEX_ALPHA _WIREFRAME_ALPHA_TEX_ALPHA_INVERT _WIREFRAME_ALPHA_MASK

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "AutoLight.cginc"
			#include "Assets/Digicrafts/WireframeGrid/Shaders/Core/Core.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float3 normal : NORMAL;
				fixed2 uv4 : TEXCOORD3;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				half3 objNormal : TEXCOORD1;
				half3 worldNormal : TEXCOORD2;

				#if _DIFFUSE_ON
				fixed3 diff : COLOR3;
				#endif

				#if _AMBIENT_ON
                fixed3 ambient : COLOR2;
                #endif

				#if _SHADOW_ON
				SHADOW_COORDS(3)
                #endif

                #if _SHADE_ON
				float3 cubenormal : TEXCOORD4;
				#endif

				UNITY_FOG_COORDS(5)
				DC_WIREFRAME_COORDS(6,7)	
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			#if _DIFFUSE_ON
			uniform fixed4 _DiffuseColor;
			uniform float _DiffusePower;
			#endif

			#if _AMBIENT_ON
			uniform float _AmbientPower;
			#endif

			#if _SHADOW_ON
			uniform fixed4 _ShadowColor;
			uniform fixed _ShadowPower;
			#endif

			#if _SHADE_ON
			samplerCUBE _ShadeTex;
         	uniform fixed _ShadePower;
         	uniform fixed _SpecToonEffectPower;
			#endif

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv0, _MainTex);
				o.objNormal = v.normal;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				#if _SHADE_ON
					o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
				#endif

				#if _DIFFUSE_ON
					half nl = max(0, dot(o.worldNormal, _WorldSpaceLightPos0.xyz));
					o.diff = nl * _LightColor0 * _DiffusePower;
				#endif

				#if _AMBIENT_ON
					o.ambient = ShadeSH9(half4(o.worldNormal,1)) * _AmbientPower;
				#endif

				#if _SHADOW_ON
					TRANSFER_SHADOW(o)
				#endif

				UNITY_TRANSFER_FOG(o,o.pos);
				DC_WIREFRAME_TRANSFER_COORDS(o);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{				
				fixed4 c = tex2D(_MainTex, i.uv);

				#if _WIREFRAME_LIGHTING
				DC_APPLY_WIREFRAME(c.rgb,c.a,i)
				#endif

				#if _SHADOW_ON
					fixed shadow = clamp(SHADOW_ATTENUATION(i)+1-_ShadowPower,0,1);
					#if _SHADOWTOONEFFECT_ON
					shadow = round(shadow*_ShadowToonEffectPower)/_ShadowToonEffectPower;
					#endif
				#else
					fixed shadow = 1;
				#endif
								           
				#if _SHADE_ON
					fixed4 cube = clamp(texCUBE(_ShadeTex, i.cubenormal),(1-_ShadePower),1);
					c.rgb = cube.rgb*c.rgb;
	            #endif
	            	                            
                #if _DIFFUSE_ON && _AMBIENT_ON
                	c.rgb=c.rgb*i.diff*shadow+i.ambient;					
				#else
					#if _DIFFUSE_ON
						c.rgb*=i.diff*shadow;
					#else 
						#if _AMBIENT_ON
							c.rgb=c.rgb*shadow+i.ambient;
						#else
							c.rgb=c.rgb*shadow;
						#endif
					#endif
				#endif									

				#if _SHADOW_ON
					c.rgb = c.rgb + (1-shadow)*_ShadowColor.rgb*_ShadowPower;
				#endif

				#ifndef _WIREFRAME_LIGHTING
				DC_APPLY_WIREFRAME(c.rgb,c.a,i)
				#endif

				UNITY_APPLY_FOG(i.fogCoord,c);

				return c;
			}
			ENDCG
		}

		// shadow pass
	    Pass {
	        Name "ShadowCaster"
	        Tags { "LightMode" = "ShadowCaster" }
	       
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
			 
			struct v2f {
			    V2F_SHADOW_CASTER;
			};
			 
			v2f vert( appdata_base v )
			{
			    v2f o;
			    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			    return o;
			}
			 
			float4 frag( v2f i ) : SV_Target
			{
			    SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
			 
	    }
	}
CustomEditor "WireframeGridToonShaderGUI"
}
