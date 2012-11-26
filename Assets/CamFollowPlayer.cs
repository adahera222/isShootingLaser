using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CamFollowPlayer : MonoBehaviour {

	public Transform target = null;
	
	public float maxCharScreenSpace = 0.66f;
	public float minCharScreenSpace = 0.33f;
	
	public float correctionSpeed = 1f; 
	
	void setTargetPlayer (GameObject player) {
		target = player.transform;	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (target != null) {
			Vector3 pos = transform.position;
			
			pos.z+=(correctionSpeed*(target.position.z-transform.position.z))*Time.deltaTime;
			
			pos.x+=(correctionSpeed*(target.position.x-transform.position.x))*Time.deltaTime;
			
			transform.position = pos;
		}
	}
}