using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class OnShootSoundEffects : MonoBehaviour {
	
	//public AudioClip laserSounds;

	
	void Update () {
	
		if(Input.GetMouseButton(0)&& transform.GetComponent<ControllerToUseController>().energy >= 0){
			if(!audio.isPlaying){
				audio.Play(0);
				
			}
		}
		else{
			audio.Pause();
		}
	}
}
