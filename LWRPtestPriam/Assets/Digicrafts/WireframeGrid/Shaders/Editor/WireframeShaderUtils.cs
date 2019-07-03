using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Digicrafts.WireframePro {

	/// <summary>
	/// Color gradient shader helper.
	/// </summary>
	public class WireframeShaderUtils {	

		private static Vector2 b1 = new Vector2(0,1);
		private static Vector2 b2 = new Vector2(0,2);
		private static Vector2 b3 = new Vector2(0,4);

		/// <summary>
		/// Updates the mesh.
		/// </summary>
		/// <param name="mesh">Mesh.</param>
		/// <param name="optimize">If set to <c>true</c> optimize.</param>
		public static void updateMesh(Mesh mesh, bool optimize = true)
		{
//			Debug.Log("update Mesh");
			List<Vector3> vertices = new List<Vector3>(mesh.vertices);
			List<Vector2> m = new List<Vector2>(new Vector2[mesh.vertices.Length]);

			List<Vector3> normal = null;
			List<Vector4> tangents = null;
			List<Vector2> uv = null;
			List<Vector2> uv2 = null;
			List<Vector2> uv3 = null;
			List<Color> colors = null;
			List<BoneWeight> bone = null;
			//			List<Matrix4x4> bindposes = null;

//			Debug.Log("blenshape: " + mesh.blendShapeCount);

			bool hasNormal = false;
			bool hasTangents = false;
			bool hasUV = false;
			bool hasUV2 = false;
			bool hasUV3 = false;
			bool hasColor = false;
			bool hasBone = false;
			//			bool hasBindposes = false;

			if(mesh.normals!=null&&mesh.normals.Length>0){
				normal = new List<Vector3>(mesh.normals);
				hasNormal=true;
			}
			if(mesh.tangents!=null&&mesh.tangents.Length>0){
				tangents = new List<Vector4>(mesh.tangents);
				hasTangents=true;
			}
			if(mesh.uv!=null&&mesh.uv.Length>0){
				uv = new List<Vector2>(mesh.uv);
				hasUV=true;
			}
			if(mesh.uv2!=null&&mesh.uv2.Length>0){
				uv2 = new List<Vector2>(mesh.uv2);
				hasUV2=true;
			}
			if(mesh.uv3!=null&&mesh.uv3.Length>0){
				uv3 = new List<Vector2>(mesh.uv3);
				hasUV3=true;
			}
			if(mesh.colors!=null&&mesh.colors.Length>0){
				colors = new List<Color>(mesh.colors);
				hasColor=true;
			}

			if(mesh.boneWeights!=null&&mesh.boneWeights.Length>0){
				bone = new List<BoneWeight>(mesh.boneWeights);
				hasBone=true;
			}

			//			if(mesh.bindposes!=null&&mesh.bindposes.Length>0){
			//				bindposes = new List<Matrix4x4>(mesh.bindposes);
			//				hasBindposes=true;			
			//			}
//			Debug.Log("bind pose: " + mesh.bindposes.Length);

			// Loop each submesh
			for (int k=0; k < mesh.subMeshCount; k++) {		
				//				int k=2;
				// Get triangles
				int[] index = mesh.GetTriangles(k);

				// Check if index
				if(index.Length>0){

					// init
					int numOfTri = index.Length/3;				

					// Loop the triangles
					for (int i=0; i < numOfTri; i++) {

						// Get index
						int idx1 = index[i*3];
						int idx2 = index[i*3+1];
						int idx3 = index[i*3+2];

						// Get the triangle verties
						Vector2 uv_1 = m[idx1];
						Vector2 uv_2 = m[idx2];
						Vector2 uv_3 = m[idx3];

						// Calculate mass
						int mass = (uv_1.y>0?1:0)*4+(uv_2.y>0?1:0)*2+(uv_3.y>0?1:0);

						if(mass!=0 && (uv_1.y==uv_2.y||uv_2.y==uv_3.y||uv_1.y==uv_3.y)){

							m.Add(b1);
							m.Add(b2);
							m.Add(b3);

							vertices.Add(vertices[idx1]);
							vertices.Add(vertices[idx2]);
							vertices.Add(vertices[idx3]);

							// Add the normal
							if(hasNormal){
								normal.Add(normal[idx1]);
								normal.Add(normal[idx2]);
								normal.Add(normal[idx3]);
							}
							// Add the mass
							if(hasTangents){
								tangents.Add(tangents[idx1]);
								tangents.Add(tangents[idx2]);
								tangents.Add(tangents[idx3]);
							}
							// Add the mass
							if(hasUV){
								uv.Add(uv[idx1]);
								uv.Add(uv[idx2]);
								uv.Add(uv[idx3]);
							}
							// Add the mass
							if(hasUV2){
								uv2.Add(uv2[idx1]);
								uv2.Add(uv2[idx2]);
								uv2.Add(uv2[idx3]);
							}
							// Add the mass
							if(hasUV3){
								uv3.Add(uv3[idx1]);
								uv3.Add(uv3[idx2]);
								uv3.Add(uv3[idx3]);
							}
							// Add the mass
							if(hasColor){
								colors.Add(colors[idx1]);
								colors.Add(colors[idx2]);
								colors.Add(colors[idx3]);
							}
							// Add the mass
							if(hasBone){
								bone.Add(bone[idx1]);
								bone.Add(bone[idx2]);
								bone.Add(bone[idx3]);
							}
							//							// Add the mass
							//							if(hasBindposes){
							//								bindposes.Add(bindposes[idx1]);
							//								bindposes.Add(bindposes[idx2]);
							//								bindposes.Add(bindposes[idx3]);
							//							}

							idx3=vertices.Count-1;
							idx2=idx3-1;
							idx1=idx3-2;
							//
							index[i*3]=idx1;
							index[i*3+1]=idx2;
							index[i*3+2]=idx3;

						} else {

							// Select mass uv
							switch(mass){
							case 0: // 000
								m[idx1]=b1;
								m[idx2]=b2;
								m[idx3]=b3;
								break;
							case 1: // 001
								if(uv_3.y==1){								
									m[idx1]=b3;
									m[idx2]=b2;
								} else if(uv_3.y==2){								
									m[idx1]=b1;
									m[idx2]=b3;
								} else if(uv_3.y==4){								
									m[idx1]=b1;
									m[idx2]=b2;
								}
								break;
							case 2: // 010							
								if(uv_2.y==1){																
									m[idx1]=b3;
									m[idx3]=b2;
								} else if(uv_2.y==2){								
									m[idx1]=b3;
									m[idx3]=b1;
								} else if(uv_2.y==4){								
									m[idx1]=b2;
									m[idx3]=b1;
								}
								break;
							case 3: // 011
								if((uv_2.y==2 && uv_3.y==4)||(uv_2.y==4 && uv_3.y==2)){																
									m[idx1]=b1;
								} else if((uv_2.y==1 && uv_3.y==4)||(uv_2.y==4 && uv_3.y==1)){								
									m[idx1]=b2;
								} else if((uv_2.y==1 && uv_3.y==2)||(uv_2.y==2 && uv_3.y==1)){																
									m[idx1]=b3;
								} else {
									Debug.Log("011");
								}
								break;
							case 4: // 100
								if(uv_1.y==1){																
									m[idx2]=b2;
									m[idx3]=b3;
								} else if(uv_1.y==2){																
									m[idx2]=b1;
									m[idx3]=b3;
								} else if(uv_1.y==4){																
									m[idx2]=b1;
									m[idx3]=b2;
								}
								break;
							case 5: // 101
								if((uv_1.y==2 && uv_3.y==4)||(uv_1.y==4 && uv_3.y==2)){								
									m[idx2]=b1;
								} else if((uv_1.y==1 && uv_3.y==4)||(uv_1.y==4 && uv_3.y==1)){								
									m[idx2]=b2;
								} else if((uv_1.y==1 && uv_3.y==2)||(uv_1.y==2 && uv_3.y==1)){								
									m[idx2]=b3;
								} else {
									Debug.Log("101");
								}
								break;
							case 6: // 110							
								if((uv_1.y==2 && uv_2.y==4)||(uv_1.y==4 && uv_2.y==2)){								
									m[idx3]=b1;
								} else if((uv_1.y==1 && uv_2.y==4)||(uv_1.y==4 && uv_2.y==1)){								
									m[idx3]=b2;
								} else if((uv_1.y==1 && uv_2.y==2)||(uv_1.y==2 && uv_2.y==1)){								
									m[idx3]=b3;
								} else {
									Debug.Log("110");
								}
								break;
							case 7: // 111

								//								Debug.Log("111");

								break;	
							}
						} // if
					}
					if(vertices.Count<65000) {
						mesh.vertices=vertices.ToArray();
						mesh.SetTriangles(index,k);

					} else {
						Debug.Log("error too large");
					}
				} // if index.Length
			} // loop subMesh
			mesh.uv4=m.ToArray();
			if(hasNormal) mesh.normals=normal.ToArray();
			if(hasTangents) mesh.tangents=tangents.ToArray();
			if(hasUV) mesh.uv=uv.ToArray();
			if(hasUV2) mesh.uv2=uv2.ToArray();
			if(hasUV3) mesh.uv3=uv3.ToArray();
			if(hasColor) mesh.colors=colors.ToArray();
			if(hasBone) mesh.boneWeights=bone.ToArray();
			//			if(hasBindposes) mesh.bindposes=bindposes.ToArray();
			// if(optimize) ;
		}	

	}

}