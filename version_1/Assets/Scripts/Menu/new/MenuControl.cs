using UnityEngine;
using System.Collections;
using System;

public class MenuControl : MonoBehaviour {

	public GameObject optionsPopUp;
	public GameObject closeBtn;
	public GameObject startBtn;
	public GameObject optionsBtn;
	public GameObject mainPanel;

	public string demoLevelName = "demo_level";

	public float uiWaitTimeout = .35f;

	// Use this for initialization
	void Start ()
	{
		if (!optionsPopUp) 
		{
			Debug.LogError("Options pop up not found");
			Application.Quit();
		}

		if (!closeBtn) 
		{
			Debug.LogError("Close button not found");
			Application.Quit();
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log(Screen.height + "x" + Screen.width);
	}

	void LoadLevel(string level) 
	{

	}

	void Exit() 
	{
		Application.Quit();
	}

	IEnumerator HideOptionsPopUp() 
	{
		yield return new WaitForSeconds(uiWaitTimeout);
		closeBtn.SetActive(true);
		optionsPopUp.SetActive(false);
		DisableMainPanel(true);
	}

	IEnumerator ShowOptionsPopUp() 
	{
		DisableMainPanel(false);
		yield return new WaitForSeconds(uiWaitTimeout);
		closeBtn.SetActive(false);
		optionsPopUp.SetActive(true);
	}

	//disables/enables all input by disabling colliders
	void DisableMainPanel(bool disable) 
	{
		var colliders = mainPanel.GetComponentsInChildren<Collider>();
		foreach (var collider in colliders) 
		{
			collider.enabled = disable;
		}
	}

	void LoadDemoLevel() 
	{
		if (!String.IsNullOrEmpty(demoLevelName)) 
		{
			Application.LoadLevel(demoLevelName);
		}
	}

}
