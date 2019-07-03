using System;
using UnityEngine;

namespace UnityEditor
{
	internal class WireframeGridShaderGUI : ShaderGUI
	{		
		public static Texture2D logo;
		public static GUIStyle logoStyle;

		public enum AlphaMode
		{
			Normal,
			Alpha,
			AlphaInvert,
			Mask
		}

		private static class Styles
		{
			public static GUIContent wireframeBasicTitleText = new GUIContent("Basic", "");
			public static GUIContent wireframeColorTitleText = new GUIContent("Color/Texture", "");
			public static GUIContent wireframeAnimationTitleText = new GUIContent("Animation", "");

			public static GUIContent wireframeAlphaModeText = new GUIContent("Alpha Mode", "How the wireframe appear");
			public static GUIContent wireframeAlphaCutoffText = new GUIContent("Alpha Cutoff", "How the wireframe alpha cutoff");
			public static GUIContent[] wireframeAlphaModeNames = {new GUIContent("Color"),new GUIContent("Texture alpha"),new GUIContent("Invert texture alpha"),new GUIContent("Mask")};
			public static GUIContent wireframeColorText = new GUIContent("Color(RGB) Trans(A)", "Wireframe Color");
			public static GUIContent wireframeTexText = new GUIContent("Texture", "Wireframe Texture");
			public static GUIContent wireframeMaskTexText = new GUIContent("Mask", "Mask for wireframe");
			public static GUIContent wireframeAAText = new GUIContent("Anti Aliasing", "Enable Anti Aliasing");
			public static GUIContent wireframeSizeText = new GUIContent("Size", "Width of the wire");
			public static GUIContent wireframeDoubleSidedText = new GUIContent("Double-sided", "Enable double-sided");
			public static GUIContent wireframeLightingText = new GUIContent("Color affect by Light", "Wireframe color affect by light/lightmap");
			public static GUIContent wireframeTexAniSpeedXText = new GUIContent("Speed(U direction)", "Speed of animated uv");
			public static GUIContent wireframeTexAniSpeedYText = new GUIContent("Speed(V direction)", "Speed of animated uv");
			public static GUIContent wireframeUVText = new GUIContent("UV Channel", "UV channel for wireframe texture");

			public static GUIContent _GridSpacing = new GUIContent("Grid Spacing", "Relative space to the model");
			public static GUIContent _GridUseWorldspace = new GUIContent("Use world space", "Using world space for drawing the grid.");
			public static GUIContent _GridSpacingScale = new GUIContent("Spacing related to Object", "Grid Spacing Related to Object");
//			public static GUIContent _WireframeSizeScale = new GUIContent("Scale wireframe size", "Scale the grid related to object.");

			public static GUIContent wireframeEmissionColor = new GUIContent("Emission", "Wireframe Emission");
			public static GUIContent emissiveWarning = new GUIContent ("Emissive value is animated but the material has not been configured to support emissive. Please make sure the material itself has some amount of emissive.");
			public static GUIContent emissiveColorWarning = new GUIContent ("Ensure emissive color is non-black for emission to have effect.");
		}

		MaterialProperty wireframeAlphaMode = null;
		MaterialProperty wireframeAlphaCutoff = null;		
		MaterialProperty wireframeColor = null;
		MaterialProperty wireframeTex = null;
		MaterialProperty wireframeMaskTex = null;
		MaterialProperty wireframeSize = null;
		MaterialProperty wireframeAA = null;	
		MaterialProperty wireframeLighting = null;
		MaterialProperty wireframeDoubleSided = null;
		MaterialProperty wireframeTexAniSpeedX = null;
		MaterialProperty wireframeTexAniSpeedY = null;
		MaterialProperty wireframeUV = null;
		MaterialProperty _GridSpacing = null;
		MaterialProperty _GridUseWorldspace = null;
		MaterialProperty _GridSpacingScale = null;
//		MaterialProperty _WireframeSizeScale = null;
		MaterialProperty wireframeEmissionColor = null;


		virtual public void FindProperties (MaterialProperty[] props)
		{			
			wireframeAlphaMode = FindProperty ("_WireframeAlphaMode", props);
			wireframeAlphaCutoff = FindProperty ("_WireframeAlphaCutoff", props);
			wireframeColor = FindProperty ("_WireframeColor", props);
			wireframeTex = FindProperty ("_WireframeTex", props);
			wireframeMaskTex = FindProperty ("_WireframeMaskTex", props);
			wireframeSize = FindProperty ("_WireframeSize", props);
			wireframeAA = FindProperty ("_WireframeAA", props);
			wireframeLighting = FindProperty ("_WireframeLighting", props);
			wireframeDoubleSided = FindProperty ("_WireframeDoubleSided", props);
			wireframeTexAniSpeedX = FindProperty ("_WireframeTexAniSpeedX", props);
			wireframeTexAniSpeedY = FindProperty ("_WireframeTexAniSpeedY", props);
			wireframeUV = FindProperty ("_WireframeUV", props);

			_GridSpacing = FindProperty ("_GridSpacing", props);
			_GridUseWorldspace = FindProperty ("_GridUseWorldspace", props);
			_GridSpacingScale = FindProperty ("_GridSpacingScale", props);
//			_WireframeSizeScale = FindProperty ("_WireframeSizeScale", props);
			wireframeEmissionColor = FindProperty("_WireframeEmissionColor",props,false);

		}

		public void WireframePropertiesGUI(MaterialEditor materialEditor, Material material)
		{	
			AlphaMode alphaMode = (AlphaMode)wireframeAlphaMode.floatValue;

			EditorGUILayout.Space();
			if(WireframeGridShaderGUI.logo==null) WireframeGridShaderGUI.logo=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Digicrafts/WireframeGrid/Shaders/Editor/logo.png");
			if(WireframeGridShaderGUI.logoStyle==null) {
				WireframeGridShaderGUI.logoStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
				WireframeGridShaderGUI.logoStyle.alignment = TextAnchor.UpperCenter;
				WireframeGridShaderGUI.logoStyle.normal.background=WireframeGridShaderGUI.logo;
			}		
			GUILayout.Label("",WireframeGridShaderGUI.logoStyle,GUILayout.Height(50));
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Wireframe Settings",MessageType.None);
			EditorGUILayout.Space();
			GUILayout.Label(Styles.wireframeBasicTitleText,EditorStyles.boldLabel);
			materialEditor.ShaderProperty(wireframeSize,Styles.wireframeSizeText.text);
//			materialEditor.ShaderProperty(_WireframeSizeScale,Styles._WireframeSizeScale.text);
			materialEditor.ShaderProperty(_GridSpacing,Styles._GridSpacing.text);
			materialEditor.ShaderProperty(_GridSpacingScale,Styles._GridSpacingScale.text);
			materialEditor.ShaderProperty(_GridUseWorldspace,Styles._GridUseWorldspace.text);
			materialEditor.ShaderProperty(wireframeDoubleSided,Styles.wireframeDoubleSidedText.text);
			materialEditor.ShaderProperty(wireframeAA,Styles.wireframeAAText.text);
			materialEditor.ShaderProperty(wireframeLighting,Styles.wireframeLightingText.text);
			EditorGUILayout.Space();
			GUILayout.Label(Styles.wireframeColorTitleText,EditorStyles.boldLabel);
			materialEditor.TexturePropertyWithHDRColor(Styles.wireframeTexText, wireframeTex, wireframeColor,new ColorPickerHDRConfig(0,2,0,2),true);
			if(alphaMode==AlphaMode.Mask){
				materialEditor.TexturePropertySingleLine(Styles.wireframeMaskTexText, wireframeMaskTex);
			}
			WireframeAlphaModePopup(materialEditor, material);
			materialEditor.ShaderProperty(wireframeAlphaCutoff,Styles.wireframeAlphaCutoffText.text);
			if(wireframeEmissionColor!=null){
				materialEditor.ShaderProperty(wireframeEmissionColor,Styles.wireframeEmissionColor.text);
			}
			EditorGUILayout.Space();
			materialEditor.TextureScaleOffsetProperty(wireframeTex);
			materialEditor.ShaderProperty(wireframeUV, Styles.wireframeUVText.text);
			EditorGUILayout.Space();
			GUILayout.Label(Styles.wireframeAnimationTitleText,EditorStyles.boldLabel);
			materialEditor.ShaderProperty(wireframeTexAniSpeedX, Styles.wireframeTexAniSpeedXText.text);
			materialEditor.ShaderProperty(wireframeTexAniSpeedY, Styles.wireframeTexAniSpeedYText.text);
			EditorGUILayout.Space();

			// Double Sided
			if(wireframeDoubleSided.floatValue>0)
				material.SetInt("_WireframeCull",(int)UnityEngine.Rendering.CullMode.Off);
			else
				material.SetInt("_WireframeCull",(int)UnityEngine.Rendering.CullMode.Back);

		}

		void WireframeAlphaModePopup(MaterialEditor materialEditor, Material material)
		{
			EditorGUI.showMixedValue = wireframeAlphaMode.hasMixedValue;
			var mode = (AlphaMode)wireframeAlphaMode.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (AlphaMode)EditorGUILayout.Popup(Styles.wireframeAlphaModeText, (int)mode, Styles.wireframeAlphaModeNames);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Alpha Mode");
				wireframeAlphaMode.floatValue = (float)mode;

				switch (mode)
				{
				case AlphaMode.Normal:				
					material.EnableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK");
					break;
				case AlphaMode.Alpha:
					material.DisableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.EnableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK");
					break;
				case AlphaMode.AlphaInvert:
					material.DisableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.EnableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK");
					break;
				case AlphaMode.Mask:
					material.DisableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.EnableKeyword("_WIREFRAME_ALPHA_MASK");
					break;
				}
			}
			EditorGUI.showMixedValue = false;
		}

		static public bool ShouldEmissionBeEnabled (Color color)
		{
			return color.maxColorComponent > (0.1f / 255.0f);
		}

		static public void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
				m.EnableKeyword (keyword);
			else
				m.DisableKeyword (keyword);
		}

	}

} // namespace UnityEditor
