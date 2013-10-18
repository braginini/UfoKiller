using UnityEngine;
using System;
using System.Collections;

public class GestureMenu : MonoBehaviour {
	
	private PXCUPipeline pp = null;
	
	public GameObject closeButton;
	
	public GameObject startButton;
	
	public string levelToLoad;

	void Start () {
		
		PXCUPipeline.Mode mode = PXCUPipeline.Mode.GESTURE;
		
		pp=new PXCUPipeline();
		
		if (!pp.Init(mode)) {
			print("Unable to initialize the PXCUPipeline");
			return;
		}		
	
	}
	
	void Update () {
		
		if (pp == null) return;
		if (!pp.AcquireFrame(false)) return;
		
		PXCMGesture.Gesture gestureHandData;
		pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_ANY, out gestureHandData);
		
		if (gestureHandData.label.Equals(PXCMGesture.Gesture.Label.LABEL_POSE_BIG5)) {
			
			if (startButton) {
			
				MenuButton menuBtnScript = startButton.GetComponent<MenuButton>();
			
				menuBtnScript.RollOverTexture();
				menuBtnScript.PlayBeep();
			
				if (!String.IsNullOrEmpty(levelToLoad))
					Application.LoadLevel(levelToLoad);
				else
					Application.Quit();
			} else {
				Application.Quit();
			}
		}
		
		if (gestureHandData.label.Equals(PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN)) {
			if (closeButton) {
				MenuButton menuBtnScript = closeButton.GetComponent<MenuButton>();
				menuBtnScript.PlayBeep();
				menuBtnScript.RollOverTexture();
				Application.Quit();
			}
		}
		
		pp.ReleaseFrame();
	}
	
	void OnDisable() {
		if (pp==null) return;
		pp.Dispose();
		pp=null;
    }
}
