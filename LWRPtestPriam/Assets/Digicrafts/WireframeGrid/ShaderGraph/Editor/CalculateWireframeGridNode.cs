using UnityEngine;
using UnityEditor.ShaderGraph;
using System.Reflection;

[Title("Digicrafts", "Calculate Wireframe Grid")]
public class CalculateWireframeGridNode : CodeFunctionNode
{
	public CalculateWireframeGridNode()
	{
		name = "Calculate Wireframe Grid";
	}

	protected override MethodInfo GetFunctionToConvert()
	{        
		return GetType().GetMethod("CalculateWireframeGrid",
			BindingFlags.Static | BindingFlags.NonPublic);
	}

	static string CalculateWireframeGrid(		
		[Slot(0, Binding.None)] Boolean UseWorldspace,	
		[Slot(2, Binding.None)] Vector3 VertexPosition,
		[Slot(3, Binding.None)] Vector3 WorldPosition,
		[Slot(9, Binding.None)] Matrix4x4 WorldToObject,
		[Slot(10, Binding.None)] Matrix4x4 ObjectToWorld,
		[Slot(4, Binding.None)] Vector1 Width,
		[Slot(5, Binding.None)] Vector1 GridSpacing,
		[Slot(8, Binding.None)] Vector3 Offset,
		[Slot(6, Binding.None)] Boolean UseObjectSpacing,				
		[Slot(7, Binding.None)] out Vector1 Out)
	{
        Out=new Vector1();
		return
			@"
{		
	float lwidth = Width*1.9f;
	float gridScale = 1.0f;
	GridSpacing+=0.00001f;
	if(UseObjectSpacing==1.0f||UseWorldspace==1.0f){ 
		if(UseObjectSpacing==UseWorldspace) {
			float scale = length(float3(ObjectToWorld[0].x, ObjectToWorld[1].x, ObjectToWorld[2].x));
			gridScale=scale*GridSpacing;
		} else {
			gridScale=GridSpacing;
		} 
	}else{
		float scale = length(float3(WorldToObject[0].x, WorldToObject[1].x, WorldToObject[2].x));
		gridScale=scale*GridSpacing;
	}

	float3 space = (UseWorldspace==0.0f)?VertexPosition.xyz:WorldPosition.xyz;	
	space += Offset.xyz;
	float3 mass = abs(frac(space/gridScale));
	// mass=mass*mass;
	if(mass.x<=0.5f) mass.x=mass.x*2.0f; else mass.x=(1.0f-mass.x)*2.0f;
	if(mass.y<=0.5f) mass.y=mass.y*2.0f; else mass.y=(1.0f-mass.y)*2.0f;
	if(mass.z<=0.5f) mass.z=mass.z*2.0f; else mass.z=(1.0f-mass.z)*2.0f;	
	float3 w = fwidth(abs(mass.xyz));
	float check1 = _ProjectionParams.y*0.1f;
	float check2 = _ProjectionParams.y*0.001f;
	if(w.y<=check1&&w.y>check2) w.y=check1;	
	if(w.x<=check1&&w.x>check2) w.x=check1;
	if(w.z<=check1&&w.z>check2) w.z=check1;	
	float ww = (lwidth-1.0f);
	ww=(ww>1.0f)?ww:0.0f;
	half3 steps = smoothstep(w*ww,w*lwidth,mass.xyz);
	Out = 1.0f-min(min(steps.y, steps.z), steps.x);		
} 
";


	}
}