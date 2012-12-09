using UnityEngine;
using System.Collections;
using System;

public class MenuControl : MonoBehaviour {
	
	public GameObject tutorialController;
	
	void Start() {
		tutorialController.GetComponent<ControllerToUseController>().movementEnabled = true;
		tutorialController.GetComponent<LaserShootRecursiveBetter>().useRPC = false;
	}
	
	void OnGUI() {		
		if (GUI.Button(new Rect(Screen.width*0.65f, Screen.height*0.55f, 150, 80), "Start Game")) {
			Application.LoadLevel(1);
		}   
	}
}