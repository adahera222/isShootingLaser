using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CamFollowPlayerOld : MonoBehaviour {

	public Transform target;
	
	public float maxCharScreenSpace = 0.66f;
	public float minCharScreenSpace = 0.33f;
	
	public float correctionSpeed = 1f; 
	
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 pos = transform.position;
		
		pos.z+=(correctionSpeed*(target.position.z-transform.position.z))*Time.deltaTime;
		
		pos.x+=(correctionSpeed*(target.position.x-transform.position.x))*Time.deltaTime;
		
		transform.position = pos;
	}
}
	// Find the extents of the character from the renderers
	/*	var listOfRenderersInTarget = target.GetComponentsInChildren<Renderer>().ToList<Renderer>();
		var minimumx = listOfRenderersInTarget.ConvertAll<float>((x)=> x.bounds.min.y).Min();
		var maximumx = listOfRenderersInTarget.ConvertAll<float>((x)=> x.bounds.max.y).Max();
		
		var minimumz = listOfRenderersInTarget.ConvertAll<float>((x)=> x.bounds.min.y).Min();
		var maximumz = listOfRenderersInTarget.ConvertAll<float>((x)=> x.bounds.max.y).Max();

		Camera cam = GetComponent<Camera>();
		Vector2 topOfCharacterScreenSpace = cam.WorldToViewportPoint(new Vector3(target.position.x, maximumx,target.position.z));
		Vector2 botOfCharacterScreenSpace = cam.WorldToViewportPoint(new Vector3(target.position.x, minimumx,target.position.z));
		
		Vector2 topOfCharacterScreenSpacez = cam.WorldToViewportPoint(new Vector3(target.position.x, target.position.y, maximumz));
		Vector2 botOfCharacterScreenSpacez = cam.WorldToViewportPoint(new Vector3(target.position.x, target.position.y, minimumz));
		
		if(topOfCharacterScreenSpace.y > maxCharScreenSpace){
			pos.z += correctionSpeed*Time.deltaTime;
		}
		if(botOfCharacterScreenSpace.y < minCharScreenSpace){
			pos.z -= correctionSpeed* Time.deltaTime;
		}
		if(topOfCharacterScreenSpacez.x > maxCharScreenSpace){
			pos.x += correctionSpeed*Time.deltaTime;
		}
		if(botOfCharacterScreenSpacez.x < minCharScreenSpace){
			pos.x -= correctionSpeed* Time.deltaTime;
		}*/