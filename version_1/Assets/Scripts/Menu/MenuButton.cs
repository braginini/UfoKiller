using UnityEngine;
using System;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class MenuButton : MonoBehaviour
{
	public string levelToLoad;
	public Texture2D normalTexture;
	public Texture2D rollOverTexture;
	public Texture2D downTexture;
	public AudioClip beep;
	public AudioClip hover;
	
	void OnMouseEnter()
	{
		RollOverTexture();
		PlayHover();
	}
	
	void OnMouseExit()
	{
		NormalTexture();
	}
	
	IEnumerator OnMouseDown()
	{
		DownTexture();
		PlayBeep();
		yield return new WaitForSeconds(0.35f);
		if (!String.IsNullOrEmpty(levelToLoad))
			Application.LoadLevel(levelToLoad);
		else
			Application.Quit();
	}
	
	public void NormalTexture() {
		if (normalTexture)
			guiTexture.texture = normalTexture;
	}
	
	public void RollOverTexture() {
		if(rollOverTexture)
			guiTexture.texture = rollOverTexture;
	}
	
	public void DownTexture() {
		if(downTexture)
			guiTexture.texture = downTexture;
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
