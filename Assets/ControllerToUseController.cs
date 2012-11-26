using UnityEngine;
using System.Collections;

public class ControllerToUseController : MonoBehaviour {

	
    public float speed = 2000F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
	private Vector3 spawnPosition = Vector3.zero;
	public float maxVel = 7f;
	
	public float idolSpeed=0.79f;
	private float startTime;
	public float slowRate = 0.15f;
	private float isMov=1;
	
	public float health = 100;
	public float energy = 100;
	public bool movementEnabled = false;
	
	void Start() {
		spawnPosition = transform.position;
		
		// This is our own player
		movementEnabled = networkView.isMine;
				
		if (movementEnabled) {
			Camera.mainCamera.GetComponent<CamFollowPlayer>().target = this.transform;
		}
	}
	
	void Update () {
	
		float hor = 0.0f;
		float ver = 0.0f;
		
		if (movementEnabled) {
			hor = Input.GetAxis("Horizontal");
			ver = Input.GetAxis("Vertical");
		}
		
		moveDirection = new Vector3(hor, 0.0f, ver);
		
        moveDirection = transform.TransformDirection(moveDirection);
		//moveDirection *= speed;
		//speed 6   maxvel =10
        moveDirection *= speed*Time.deltaTime;
       //speed 2000
		// max vel 7
   	
		Rigidbody rBod = GetComponent<Rigidbody>();
		rBod.AddForce(moveDirection);
		
		if(Mathf.Approximately(hor,0f) && Mathf.Approximately(ver,0f)){
			
			if(isMov==1){
				startTime=Time.time;
			}
		//	if(Time.time -startTime %slowRate==0 ){
			if(Time.time -startTime>slowRate){
				startTime=Time.time;
				//Debug.Log("Slowing Speed");
				
				rBod.velocity=rBod.velocity *idolSpeed;
			}
			
			//Debug.Log(Time.time -startTime);
			isMov=0;
		}
		else{
			isMov=1;
		}
		
		if(rBod.velocity.magnitude > maxVel){rBod.velocity =rBod.velocity.normalized*maxVel;}
		
		if(health <= 0){
			transform.position = spawnPosition;
			health = 100;
			energy = 100;
			}
		if(energy <= 100 && !Input.GetMouseButton(0)){
			energy += 0.25f;
		}
		
		else if(energy >= 0 && Input.GetMouseButton(0)){
			energy -= 0.5f;
		}
	}
	void OnCollisionStay(Collision collision) {
		if (collision.gameObject.tag == "Death") {
			health -= 0.5f;
		}
	}
	void Damage(){
		health -= 0.5f;
	}
}
