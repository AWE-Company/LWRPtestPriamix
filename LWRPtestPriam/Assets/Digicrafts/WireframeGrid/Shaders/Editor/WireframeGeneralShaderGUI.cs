using System;
using UnityEngine;

namespace UnityEditor
{
	internal class WireframeGridGeneralShaderGUI : WireframeGridShaderGUI
	{

		private static class Styles
		{
			public static GUIContent colorText = new GUIContent("Main Color", "Main Color (RGB)");
			public static GUIContent mainTexText = new GUIContent("Main Texture ", "Main Color (RGB) Alpha (A) ");
			public static GUIContent bumpMapText = new GUIContent("Normalmap ", "Normalmap");
			public static GUIContent bump = new GUIContent("Bump Power ", "Bump Power");
			public static GUIContent shininessText = new GUIContent("Shininess ", "Shininess");
			public static GUIContent cutoffText = new GUIContent("Alpha cutoff ", "Alpha cutoff");
		}
			
		MaterialProperty color = null;
		MaterialProperty mainTex = null;
		MaterialProperty bumpMap = null;
		MaterialProperty bump = null;
		MaterialProperty shininess = null;
		MaterialProperty cutoff = null;

		override public void FindProperties (MaterialProperty[] props)
		{
			color = FindProperty ("_Color", props,false);
			mainTex = FindProperty ("_MainTex", props,false);
			bumpMap = FindProperty ("_BumpMap", props,false);
			bump = FindProperty ("_Bump", props,false);
			shininess = FindProperty ("_Shininess", props,false);
			cutoff = FindProperty ("_Cutoff", props,false);

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
//			EditorGUI.BeginChangeCheck();
//			{
				EditorGUILayout.HelpBox("General Settings",MessageType.None);
				EditorGUILayout.Space();
				if(color!=null)
					materialEditor.ShaderProperty(color,Styles.colorText.text);
				if(mainTex!=null)
					materialEditor.ShaderProperty(mainTex,Styles.mainTexText.text);
				if(bumpMap!=null)
					materialEditor.ShaderProperty(bumpMap,Styles.bumpMapText.text);
				if(bump!=null)
					materialEditor.ShaderProperty(bump,Styles.bump.text);
				if(cutoff!=null)
					materialEditor.ShaderProperty(cutoff,Styles.cutoffText.text);
				if(shininess!=null)
					materialEditor.ShaderProperty(shininess,Styles.shininessText.text);

				// Wireframe			
				WireframePropertiesGUI(materialEditor,material);

//			}
//			if (EditorGUI.EndChangeCheck())
//			{
////				foreach (var obj in blendMode.targets)
////					MaterialChanged((Material)obj, m_WorkflowMode);
//			}
		}
	}
}

