using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Demo : MonoBehaviour {

	public GameObject obj;

	public Slider sizeSlider;

	public ColorPicker gridColorPicker;

	public Button colorButton;

	public Material mat;

	private Color gridColor;


	// Use this for initialization
	void Start () {
	
//		sizeSlider.value=obj.GetComponentInChildren<MeshRenderer>().material.GetFloat("_WireframeSize");

		gridColorPicker.gameObject.SetActive(false);

		gridColor = new Color(0,1,1);

		gridColorPicker.CurrentColor=gridColor;

		mat = new Material(mat);

		sizeSlider.value = mat.GetFloat("_GridSpacing");

		MeshRenderer[] m = obj.GetComponentsInChildren<MeshRenderer>();

//		foreach(int i=0;i<m.Length;i++){
//
//		}

		foreach(MeshRenderer mc in m){
			mc.material=mat;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeStrength(Slider target){

		mat.SetFloat("_GridSpacing",target.value);
	}		

	public void toggleColor() {				
		gridColorPicker.gameObject.SetActive(!gridColorPicker.gameObject.activeSelf);
	}
		
	public void setTopColor(Color c) {

		gridColor.r=c.r;
		gridColor.g=c.g;
		gridColor.b=c.b;

		mat.SetColor("_WireframeColor",gridColor);

		ColorBlock cb = colorButton.colors;
		cb.normalColor=cb.highlightedColor=cb.pressedColor=gridColor;
		colorButton.colors=cb;
	}		

}
