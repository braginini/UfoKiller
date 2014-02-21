using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
	public int sheepLeft = 0;
	public int ufoKilled = 0;
	public GUIText sheepLextText;
	public GUIText ufoKilledText;
	
	void Start()
	{
		Screen.showCursor = false;
		ufoKilled = 0;
		GlobalVariable.Instance.finalScore = ufoKilled;
	}
	
	public void updateSheepCount(int count)
	{
		sheepLeft = count;
		if (sheepLeft >= 0)
			sheepLextText.text = sheepLeft.ToString();
	}
	
	public void decrementSheep()
	{
		if (sheepLeft > 0)
		{
			--sheepLeft;
			updateSheepCount(sheepLeft);
			if (sheepLeft == 0)
				StartCoroutine(finishGame());
		}
	}
	
	public void incrementUfoKilled()
	{
		++ufoKilled;
		GlobalVariable.Instance.finalScore = ufoKilled;
		ufoKilledText.text = ufoKilled.ToString();
	}
	
	IEnumerator finishGame()
	{
		var fader = GameObject.FindWithTag("fader");
		if (fader != null)
		{
			fader.animation.Play();
			yield return new WaitForSeconds(2f);
		}

		var player = GameObject.FindWithTag("Player");
		if (player)
		{
			var music = player.GetComponentInChildren<AudioSource>();
			if (music)
				music.Stop();
			yield return new WaitForSeconds(2f);
		}
		
		Debug.Log("Moving to finish scene");
		Application.LoadLevel("andrey_end_game");
	}
}
