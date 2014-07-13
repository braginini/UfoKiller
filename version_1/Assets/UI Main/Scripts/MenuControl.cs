using UnityEngine;
using System.Collections;
using System;

public class MenuControl : MonoBehaviour {

	public GameObject optionsPopUp;
	public GameObject closeBtn;
	public GameObject startBtn;
	public GameObject optionsBtn;
	public GameObject mainPanel;
	public GameObject touchInput;
	public AudioClip pauseAudio;
	public AudioClip gameAudio;

	public string demoLevelName = "demo_level";

	public float uiWaitTimeout = .35f;

	public bool paused = false;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log(Screen.height + "x" + Screen.width);
	}

	public void LoadLevel(string level) 
	{

	}

	public void ExitApplication() 
	{
		Application.Quit();
	}

	public IEnumerator HideOptionsPopUp() 
	{
		yield return new WaitForSeconds(uiWaitTimeout);
		closeBtn.SetActive(true);
		optionsPopUp.SetActive(false);
		DisableMainPanel(true);
	}

	public IEnumerator ShowOptionsPopUp() 
	{
		DisableMainPanel(false);
		yield return new WaitForSeconds(uiWaitTimeout);
		closeBtn.SetActive(false);
		optionsPopUp.SetActive(true);
	}

	//visibale delegate
	public void OpenOptionPopUp() 
	{
		StartCoroutine(ShowOptionsPopUp());
	}

	public void CloseOptionPopUp() 
	{
		StartCoroutine(HideOptionsPopUp());
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

	public void LoadDemoLevel() 
	{
		if (!String.IsNullOrEmpty(demoLevelName)) 
		{
			Application.LoadLevel(demoLevelName);
		}
	}

	public void Pause() 
	{
		if (paused)
		{
			
			
			if (touchInput)
				touchInput.SetActive(true);

			if (gameAudio) 
			{
				audio.clip = gameAudio;
				audio.volume = .6f;
				audio.Play();
				audio.loop = true;
			}

			Time.timeScale = 1;
			paused = false;
		}
		else
		{
			if (touchInput)
				touchInput.SetActive(false);

			if (pauseAudio) 
			{
				audio.clip = pauseAudio;
				audio.volume = 1f;
				audio.Play();
				audio.loop = true;
			}

			Time.timeScale = 0;
			paused = true;
		}
	}

}
