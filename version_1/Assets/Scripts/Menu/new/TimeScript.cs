using UnityEngine;
using System.Collections;

public class TimeScript : MonoBehaviour
{
	private UILabel minutesLabel = null;
	private UILabel secondsLabel = null;
	
	void Start()
	{
		minutesLabel = GameObject.Find("Minutes Label").GetComponent<UILabel>();
		secondsLabel = GameObject.Find("Seconds Label").GetComponent<UILabel>();
	}
	
	void Update()
	{
		var t = (int)Time.timeSinceLevelLoad;
		var seconds = t % 60;
		var minutes = (t / 60) % 100; // Just 2 digits for minutes
		minutesLabel.text = minutes.ToString("00");
		secondsLabel.text = seconds.ToString("00");
	}
}
