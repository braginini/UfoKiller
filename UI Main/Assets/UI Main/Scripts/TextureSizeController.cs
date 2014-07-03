using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TextureSizeController : MonoBehaviour {

	public UITexture texture;
	public List<UITextureDefenition> textures;
	public UITextureDefenition defaultTexture;
	public bool widthBased = true;
	public GameObject testLabel;
	public bool showTestLabel = true;

	// Use this for initialization
	void Start () {
		Screen.fullScreen = true;
		if (textures == null)
			return;

		//sort the textures array by resolution to be in ascending order
		//List<UITextureDefenition> l = new List<UITextureDefenition>(textures);
		textures.Sort(delegate(UITextureDefenition x, UITextureDefenition y) {
			float xValue = widthBased ? x.triggerWidth : x.triggerHeight;
			float yValue = widthBased ? y.triggerWidth : y.triggerHeight;

			if (xValue > yValue)
				return 1;
			else if (xValue < yValue)
				return -1;
			else
				return 0;

		});


		if(defaultTexture == null) 
		{
			Debug.LogError("No default texture found");
			return;
		}

		float currValue = widthBased ? Screen.width : Screen.height;
		UITextureDefenition chosenTexture = defaultTexture;
		foreach (UITextureDefenition udef in textures) 
		{
			Debug.Log(udef.name);
			float trigger = widthBased ? udef.triggerWidth : udef.triggerHeight;
			if (currValue >= trigger)
				chosenTexture = udef;
		}



		if (!texture.name.Equals(chosenTexture.name)) 
		{
			if (texture.mainTexture != null)
				Resources.UnloadAsset(texture.mainTexture);
		
			texture.mainTexture = Resources.Load(chosenTexture.path) as Texture;
			texture.MakePixelPerfect();

			//Debug.Log(chosenTexture.name);
			if (!showTestLabel)
				testLabel.SetActive(false);
			else
				testLabel.GetComponent<UILabel>().text = "Texture: " + chosenTexture.path + " " + texture.mainTexture.width + "x" + texture.mainTexture.height + 
					" Screen: " + Screen.width + "x" + Screen.height;

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[System.Serializable]
	public class UITextureDefenition { 

		//name of the resource to be loaded
		public string name;
		public float triggerHeight;
		public float triggerWidth;
		//path of the resource relative to Resources folder
		public string path;


		
	}
}
