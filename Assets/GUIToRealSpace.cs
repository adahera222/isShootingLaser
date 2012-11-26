using UnityEngine;
using System.Collections;

public enum TutorialPhase {
	TutPhaseMove = 1,
	TutPhaseAim, 
	TutPhaseComplete
};

public class GUIToRealSpace : MonoBehaviour {
	
	private Vector3 initialPosition;
	public Texture aimTutorial;
	private TutorialPhase phase = TutorialPhase.TutPhaseMove;
	
	void Start () {
		Vector3 dest = gameObject.transform.parent.gameObject.transform.position;
		initialPosition = Camera.main.WorldToViewportPoint(dest);
	}
	
	void Update () {
		Vector3 dest = gameObject.transform.parent.gameObject.transform.position;
		transform.position = Camera.main.WorldToViewportPoint(dest);
		
		if (phase == TutorialPhase.TutPhaseMove) {
			if (Vector3.Distance(initialPosition, transform.position) > 0.2) {
				guiTexture.texture = aimTutorial;
				phase++;
			}
		} else if (phase == TutorialPhase.TutPhaseAim) {
			if (Input.GetMouseButtonDown(0)) {
				guiTexture.texture = null;
				phase = TutorialPhase.TutPhaseComplete;
			}	
		}
	}
}
