using System;
using UnityEngine;

namespace UnityEditor
{
	internal class WireframeGridToonShaderGUI : WireframeGridShaderGUI
	{

		private static class Styles
		{			
			public static GUIContent mainTex = new GUIContent("Main Texture ", "Main Color (RGB) Alpha (A) ");

			public static GUIContent outlineColor = new GUIContent("Outline Color", "Color of the outline");
			public static GUIContent outlineWidth = new GUIContent("Outline Size", "Width of the outline");

			public static GUIContent shadeTex = new GUIContent("Shade Texture", "Color (RGB)");
			public static GUIContent shadePower = new GUIContent("Shade Power", "Power");

			public static GUIContent shadow = new GUIContent("Shadow Enable", "Shadow");
			public static GUIContent shadowPower = new GUIContent("Power", "Power");

			public static GUIContent diffuse = new GUIContent("Diffuse Enable", "Diffuse");
			public static GUIContent diffusePower = new GUIContent("Power", "Power");

			public static GUIContent ambient = new GUIContent("Ambient Enable", "Ambient");
			public static GUIContent ambientPower = new GUIContent("Power", "Power");

		}			
		
		MaterialProperty mainTex = null;

		MaterialProperty outlineColor = null;
		MaterialProperty outlineWidth = null;

		MaterialProperty shadeTex = null;
		MaterialProperty shadePower = null;

		MaterialProperty shadow = null;
		MaterialProperty shadowPower = null;

		MaterialProperty diffuse = null;
		MaterialProperty diffusePower = null;

		MaterialProperty ambient = null;
		MaterialProperty ambientPower = null;

		override public void FindProperties (MaterialProperty[] props)
		{			
			mainTex = FindProperty ("_MainTex", props,false);

			outlineColor = FindProperty ("_OutlineColor", props,false);
			outlineWidth = FindProperty ("_OutlineWidth", props,false);

//			shade = FindProperty ("_Shade", props,false);
			shadeTex = FindProperty ("_ShadeTex", props,false);
			shadePower = FindProperty ("_ShadePower", props,false);

			shadow = FindProperty ("_Shadow", props,false);
			shadowPower = FindProperty ("_ShadowPower", props,false);

			diffuse = FindProperty ("_Diffuse", props,false);
			diffusePower = FindProperty ("_DiffusePower", props,false);

			ambient = FindProperty ("_Ambient", props,false);
			ambientPower = FindProperty ("_AmbientPower", props,false);

			base.FindProperties(props);
		}

		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] props)
		{				
			FindProperties (props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
			Material material = materialEditor.target as Material;

			ShaderPropertiesGUI (materialEditor,material);
		}

		public void ShaderPropertiesGUI (MaterialEditor materialEditor, Material material)
		{
			// Use default labelWidth
			EditorGUIUtility.labelWidth = 0f;

			// Detect any changes to the material
			EditorGUI.BeginChangeCheck();
			{
				EditorGUILayout.HelpBox("General Settings",MessageType.None);
				EditorGUILayout.Space();
				materialEditor.ShaderProperty(mainTex,Styles.mainTex.text);
				materialEditor.ShaderProperty(outlineColor,Styles.outlineColor.text);
				materialEditor.ShaderProperty(outlineWidth,Styles.outlineWidth.text);
//				GUILayout.Label ("Shading", EditorStyles.boldLabel);
				materialEditor.ShaderProperty(shadeTex,Styles.shadeTex.text);
				materialEditor.ShaderProperty(shadePower,Styles.shadePower.text);
				EditorGUILayout.Space();
				GUILayout.Label ("Lighting", EditorStyles.boldLabel);
				materialEditor.ShaderProperty(shadow,Styles.shadow.text);
				materialEditor.ShaderProperty(shadowPower,Styles.shadowPower.text);
//				GUILayout.Label ("Diffuse", EditorStyles.boldLabel);
				materialEditor.ShaderProperty(diffuse,Styles.diffuse.text);
				materialEditor.ShaderProperty(diffusePower,Styles.diffusePower.text);
//				GUILayout.Label ("Ambient", EditorStyles.boldLabel);
				materialEditor.ShaderProperty(ambient,Styles.ambient.text);
				materialEditor.ShaderProperty(ambientPower,Styles.ambientPower.text);

				// Wireframe			
				WireframePropertiesGUI(materialEditor,material);

			}
			if (EditorGUI.EndChangeCheck())
			{
//				foreach (var obj in blendMode.targets)
//					MaterialChanged((Material)obj, m_WorkflowMode);
			}
		}
	}
}

