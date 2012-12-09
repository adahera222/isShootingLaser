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
	
	public float health = 124;
	public float energy = 124;
	public bool movementEnabled = false;
	public bool rematchAccepted = false;
	public bool playerAcceptedRematch = false;
	
	void OnGUI () {
		
		if (health <= 0) { // The player has died.
			if (networkView.isMine) {
				GUI.Label(new Rect(Screen.width/2, Screen.height/2-20, 200, 200), "You lose");
			} else {
				GUI.Label(new Rect(Screen.width/2, Screen.height/2-20, 200, 200), "You Win!");
			}

			if (!playerAcceptedRematch && rematchAccepted) {
			
				GUI.Label(new Rect(Screen.width/2, Screen.height/2-50, 200, 200), "Waiting for player...");				
			
			}
			
			if (playerAcceptedRematch && rematchAccepted) {
						
				if (networkView.isMine) {
					movementEnabled = true;
				}
				
				transform.position = spawnPosition;
				health = 124;
				energy = 124;	
				return;
			}
				
			if (GUI.Button(new Rect(Screen.width/2-110, Screen.height/2, 220, 100), "Rematch!")) {
				
				rematchAccepted = true;
				NetworkViewID viewID = Network.AllocateViewID();
				networkView.RPC("recieveRematchRequest", RPCMode.OthersBuffered, viewID);
			}
		} else {
			rematchAccepted = false;
			playerAcceptedRematch = false;
		}	
	}
	
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
		
		if (rBod.velocity.magnitude > maxVel) {
			rBod.velocity = rBod.velocity.normalized*maxVel;
		}
		
		if (energy <= 124 && !Input.GetMouseButton(0)){
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
		
		if (!networkView.isMine) {
			NetworkViewID viewID = Network.AllocateViewID();
			networkView.RPC("updateOpponentHealth", RPCMode.OthersBuffered, health);	
		}
	}
	
	[RPC]
	void recieveRematchRequest(NetworkViewID viewID) {
		playerAcceptedRematch = true;	
	}
	
	[RPC]
	void updateOpponentHealth(NetworkViewID viewID, float newHealth) {
		health = newHealth;
	}
}
