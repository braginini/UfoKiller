using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class NewMenuButton : MonoBehaviour
{
	public AudioClip beep;
	public AudioClip hover;
	public float clickScale = 1.1f;
	public string optionsLevel = "Options_menu";
	public string mainMenuLevel = "Main_menu";

	private Vector3 normalScale;
	private string exitBtnTag = "exit_btn";
	private string startBtnTag = "start_btn";
	private string optionsBtnTag = "options_btn";
	private string optionsExitBtn = "options_exit_btn"; //dog -nail remove!!! did this because can't determine level i nandroid

	void Start() {
		normalScale = transform.localScale;
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
		yield return new WaitForSeconds(0.35f);
		if (exitBtnTag.Equals(gameObject.tag)) 
		{
			Application.Quit();
		}
		else if (startBtnTag.Equals(gameObject.tag)) 
		{

		}
		else if (optionsBtnTag.Equals(gameObject.tag) && !String.IsNullOrEmpty(optionsLevel)) 
		{
			Application.LoadLevel(optionsLevel);
		}
		else if (optionsExitBtn.Equals(gameObject.tag) && !String.IsNullOrEmpty(mainMenuLevel)) 
		{
			Application.LoadLevel(mainMenuLevel);	
		}
	}

	/*private string getCurrentLevelName() 
	{
		string[] split = EditorApplication.currentScene.Split(char.Parse("/"));
		string level = split[split.Length - 1];
		return level.Split(char.Parse("."))[0];

	}*/

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
