using UnityEngine;
using System.Collections;
using System;

public class MenuLogic : MonoBehaviour {
	public GameObject optionsPopUp;
	public GameObject exitBtn;
	public string demoLevel = "demo_level";

	// Use this for initialization
	void Start () 
	{
		if (!optionsPopUp) 
		{
			Debug.LogError("Options popup not found");
			Application.Quit();
		}

		EnableOptionsPopUp(false);
	}

	public void EnableOptionsPopUp(bool enable) 
	{
		optionsPopUp.SetActive(enable);
	}

	public void EnableExitBtn(bool enable) 
	{
		exitBtn.SetActive(enable);
	}

	public void SwitchToOptionsMenu() 
	{
		EnableOptionsPopUp(true);
		EnableExitBtn(false);
	}

	public void SwitchToMainMenu() 
	{
		EnableOptionsPopUp(false);
		EnableExitBtn(true);
	}

	public void LoadDemoLevel() 
	{
		if (!String.IsNullOrEmpty(demoLevel))
			Application.LoadLevel(demoLevel);
	}
}
