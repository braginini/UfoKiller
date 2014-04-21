using UnityEngine;
using System.Collections;

/**
 * Script used to load proper atlas based on device resolution
 * 
**/
public class ResolutionController : MonoBehaviour {

	public GameObject replacementAtlas;
	public string fullAtlasName = "Atlas - UI Background Full";
	public string smallAtlasName = "Atlas - UI Background Small";
	public UIAtlas referenceAtlas;
	public bool loadFull;//for test


	// Use this for initialization
	void Start () {
		if (!referenceAtlas)
		{
			Debug.LogError("Reference Atlas was not found");
			Application.Quit();
		}


		string chosenAtlas = loadFull ? fullAtlasName : smallAtlasName;
		Debug.Log("Chose Atlas " + chosenAtlas);

		replacementAtlas = Resources.Load(chosenAtlas, typeof (GameObject)) as GameObject;

		if (replacementAtlas) 
		{
			referenceAtlas.replacement = replacementAtlas.GetComponent<UIAtlas>();
		}
		else 
		{
			Debug.LogError("No Replacement Atlas was found");
		}
	
	}
}