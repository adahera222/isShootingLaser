using UnityEngine;
using System.Collections;

public class EnemyFollowCollider : MonoBehaviour {
	
	public float followSpeed = 5.0f;
	public float returnSpeed = 0f;
	
	private bool isFollowingPlayer = false;
	private GameObject player;
	private Vector3 spawnPosition;
	
	void Start(){
		spawnPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		returnSpeed = (spawnPosition+transform.position).magnitude;
		if (isFollowingPlayer) {
			transform.parent.GetComponent<Rigidbody>().AddForce((player.transform.position-transform.position).normalized * followSpeed);
		}
		else {
			transform.parent.GetComponent<Rigidbody>().AddForce((spawnPosition-transform.position).normalized * 2 * returnSpeed);
			transform.parent.GetComponent<Rigidbody>().AddForce(-(transform.parent.GetComponent<Rigidbody>().GetPointVelocity(spawnPosition)).normalized * returnSpeed);
		}
	}
	
   void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.tag == "Player") {
        	isFollowingPlayer = true;
			player = other.gameObject;
		}
    }
	
	void OnTriggerExit(Collider other) {
		
		if (other.gameObject.tag == "Player") {
        	isFollowingPlayer = false;
		}		
	}
}
