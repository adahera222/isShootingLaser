using UnityEngine;
using System.Collections;
using System;

public class MenuControl : MonoBehaviour {
	void OnGUI() {		
		if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 220, 100), "Start Game")) {
			Application.LoadLevel(1);
		}   
	}
}