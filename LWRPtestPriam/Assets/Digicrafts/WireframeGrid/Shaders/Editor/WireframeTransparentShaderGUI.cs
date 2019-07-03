using System;
using UnityEngine;

namespace UnityEditor
{
	internal class WireframeGridTransparentShaderGUI : WireframeGridShaderGUI
	{		

		private static class Styles
		{
			public static GUIContent _ProjectorTex = new GUIContent("Clip Mask", "Mask the area when projecting");
			public static GUIContent _ProjectorFalloffTex = new GUIContent("Falloff Mask (z-position)", "Mask the z-distance when projecting");

		}

		MaterialProperty _ProjectorTex = null;
		MaterialProperty _ProjectorFalloffTex = null;

		override public void FindProperties (MaterialProperty[] props)
		{
			_ProjectorTex = FindProperty ("_ProjectorTex", props,false);
			_ProjectorFalloffTex = FindProperty ("_ProjectorFalloffTex", props,false);

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
				// Wireframe			
				WireframePropertiesGUI(materialEditor,material);
				EditorGUILayout.Space();
				if(_ProjectorTex!=null){
					EditorGUILayout.HelpBox("Projector Settings",MessageType.None);
					EditorGUILayout.Space();
					materialEditor.ShaderProperty(_ProjectorTex,Styles._ProjectorTex.text);
				}
				if(_ProjectorFalloffTex!=null)
					materialEditor.ShaderProperty(_ProjectorFalloffTex,Styles._ProjectorFalloffTex.text);

			}
			if (EditorGUI.EndChangeCheck())
			{
//				foreach (var obj in blendMode.targets)
//					MaterialChanged((Material)obj, m_WorkflowMode);
			}
		}
	}
}

