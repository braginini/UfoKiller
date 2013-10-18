using UnityEngine;
using System.Collections;

public class FinalScoreIniter : MonoBehaviour
{
	public GUIText score;
	
	void Start()
	{
		score.text = GlobalVariable.Instance.finalScore.ToString();
	}	
}
