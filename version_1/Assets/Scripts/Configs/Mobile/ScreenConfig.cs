using UnityEngine;

public class ScreenConfig : MonoBehaviour
{

    //public ScreenOrientation Orientation = ScreenOrientation.LandscapeLeft;

	// Use this for initialization
	void Start () {

        Screen.orientation = ScreenOrientation.LandscapeLeft;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
