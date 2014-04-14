using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class NewMenuButton : MonoBehaviour
{
	public AudioClip beep;
	public AudioClip hover;
	public float clickScale = 1.1f;
	public MenuLogic menuLogicScript;
	public static float clickTimeout = 0.2f;

	private Vector3 normalScale;
	private static string exitBtnTag = "exit_btn";
	private static string startBtnTag = "start_btn";
	private static string optionsBtnTag = "options_btn";
	private static string menuLogicTag = "menu_logic";
	private static string optionsExitBtn = "options_exit_btn"; //dog -nail remove!!! did this because can't determine level i nandroid

	//Should be invoked after GUIAspectRatioScale.Start() method to scale GUI elements in proper way
	void Start() {
		normalScale = transform.localScale;

		menuLogicScript = GameObject.FindGameObjectWithTag(menuLogicTag).GetComponent<MenuLogic>();
		if (!menuLogicScript) 
		{
			Debug.LogError("Menu logic script not found");
			Application.Quit();
		}
	}

	void OnMouseEnter()
	{

	}
	
	void OnMouseExit()
	{
		Normal();
	}
	
	IEnumerator OnMouseDown()
	{
		Click();
		PlayBeep();
		yield return new WaitForSeconds(clickTimeout);
		if (exitBtnTag.Equals(gameObject.tag)) 
		{
			Application.Quit();
		}
		else if (startBtnTag.Equals(gameObject.tag)) 
		{

		}
		else if (optionsBtnTag.Equals(gameObject.tag)) //clicked options btn on main menu
		{
			menuLogicScript.SwitchToOptionsMenu();
		}
		else if (optionsExitBtn.Equals(gameObject.tag)) //clicked exit btn on options popup
		{
			menuLogicScript.SwitchToMainMenu();	
		}
	}

	IEnumerator OnMouseUp()
	{
		Normal();
		yield return new WaitForSeconds(0.35f);
	}

	public void Normal() {
		transform.localScale = normalScale;
	}
	
	public void Click() {
		transform.localScale = normalScale * clickScale;
	}
	
	public void PlayBeep() {
		if (beep)
			audio.PlayOneShot(beep);
	}
	
	public void PlayHover() {
		if (hover)
			audio.PlayOneShot(hover);
	}
	
}
