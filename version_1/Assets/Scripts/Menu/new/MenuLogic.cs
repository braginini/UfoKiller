using UnityEngine;
using System.Collections;

public class MenuLogic : MonoBehaviour {
	public GameObject optionsPopUp;
	public GameObject exitBtn;

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
}
