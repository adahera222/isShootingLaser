using UnityEngine;
using System.Collections;
 
public class OrbController : MonoBehaviour {
 
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
	private Vector3 spawnPosition = Vector3.zero;
	public float maxVel = 10f;
	
	public float movDrag = 10f;
	public float staDrag = 100f;
	
	void Start() {
		spawnPosition = transform.position;
	}
	
	void Update() {
		var hor = Input.GetAxis("Horizontal");
		var ver = Input.GetAxis("Vertical");
		
	    moveDirection = new Vector3(hor, 0, ver);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
    
        moveDirection.y -= gravity * Time.deltaTime;
        //controller.Move(moveDirection * Time.deltaTime);
   	
		Rigidbody rBod = GetComponent<Rigidbody>();
		rBod.AddForce(moveDirection);
		
		if(Mathf.Approximately(hor,0f) && Mathf.Approximately(ver,0f)){rBod.drag=staDrag;}
		else{rBod.drag=movDrag;}
		
		if(rBod.velocity.magnitude > maxVel){rBod.velocity =rBod.velocity.normalized*maxVel;}
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Death") {
			transform.position = spawnPosition;
		}
	}
}