using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Digicrafts.WireframeGrid
{	
	public class WireframeGridSettings : ScriptableObject {		

		[MenuItem("Tools/Digicrafts/WireframeGrid/Install Shader Graph Plugin")]
		public static void InstallPlayMakerPlugin ()
		{
#if UNITY_2019
            AssetDatabase.ImportPackage(Application.dataPath + "/Digicrafts/WireframeGrid/Extra/WireframeGridShaderGraph2019Support.unitypackage",false);
#else
			AssetDatabase.ImportPackage(Application.dataPath + "/Digicrafts/WireframeGrid/Extra/WireframeGridShaderGraphSupport.unitypackage", false);
#endif			
		}

	}
}
