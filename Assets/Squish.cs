using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Squish : MonoBehaviour {
	
	public AudioClip SquishSound;
	
	public float maxSquish = 0.5f;
	public float maxSpeed = 8.5f;
	public float elasticity = 6f;
	public Transform visualRepresentation;
	private float sphereScale=1;
	//For Debuging with gizmoes
	private List<ContactPoint> Normal;
	
	void Start(){
		sphereScale = visualRepresentation.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
	    Vector3 ones= new Vector3(1,1,1);
		
		visualRepresentation.localScale += (ones*sphereScale- visualRepresentation.localScale)*elasticity *   Time.deltaTime;
		
}
	void OnCollisionEnter(Collision other) {
		
		if(other.gameObject.tag.Equals("CauseSquish")){
						
			var contact = other.contacts[0];
			float magOfSquish = Vector3.Dot(other.relativeVelocity, contact.normal);
			Normal=other.contacts.ToList();
			
			var squish =  maxSquish*magOfSquish/maxSpeed;
			if(squish>maxSquish){
				squish=maxSquish;
			}
					
			visualRepresentation.transform.rotation = Quaternion.LookRotation(contact.normal, Vector3.up);
			visualRepresentation.localScale += new Vector3(visualRepresentation.transform.localScale.x*squish,0,-visualRepresentation.transform.localScale.z*squish);
			
			audio.PlayOneShot(SquishSound);
		}
	}
    /*void OnDrawGizmos() {
		if(Normal == null)
			return;
        Gizmos.color = Color.red;
		foreach(var contact in Normal){
			Gizmos.DrawRay(transform.position, contact.normal*100);
		}
        
    }*/
}

