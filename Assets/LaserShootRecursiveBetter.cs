using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LaserShootRecursiveBetter : MonoBehaviour {
	
	public Camera camera;
	public Color laserColor;
	public int refNum = 3;
	
	public float lightDist = 0.5f;
	public float laserRange = 20f;
	public float laserIntenseity = 4f;
	public Shader laserShader; 
	public GameObject sparks;
	public bool useRPC = true;
	
	private float widthLaser = 0.15f;
	private List<GameObject> refLaserCyl= new List<GameObject>();
	private float SphereRadius=0.5f;
	
    void Start(){
	//	laserShader = Shader.Find("Self-Illuminated Specular");
		
		//SphereCollider col = (SphereCollider) collider.;
	//	SphereRadius = col.radius;
		
		camera = Camera.mainCamera;
	}
	
	void Update () {

		foreach( GameObject i in refLaserCyl){
			Destroy(i);
		}
		refLaserCyl.Clear();

		if (GetComponent<ControllerToUseController>().movementEnabled) {
		
			if(Input.GetMouseButton(0)&& transform.GetComponent<ControllerToUseController>().energy >= 0){
			
				
				Ray click = camera.ScreenPointToRay(Input.mousePosition);
				Vector3 mousePos = click.GetPoint(25f);
				mousePos.y=0;
				Vector3 laserDir = mousePos - transform.position;
				laserDir.y=0;
				laserDir=laserDir.normalized;
				NetworkViewID viewID = Network.AllocateViewID();
				
				if (useRPC) {
					networkView.RPC("ReflectLaser", RPCMode.AllBuffered, viewID, laserRange,laserDir,transform.position+laserDir*SphereRadius,transform.up,1.0f);
				} else {
					ReflectLaser(viewID, laserRange,laserDir,transform.position+laserDir*SphereRadius,transform.up,1.0f);
				}
			}	
		}
	}
	
	[RPC]
	void ReflectLaser(NetworkViewID viewID, float len, Vector3 refDir,Vector3 originPos,Vector3 Up,float NumRec){
		
		if(refNum<NumRec){return;}
		
		RaycastHit laserHit;
		float lengthofRefLaser=0;
		
		if(Physics.Raycast(originPos, refDir, out laserHit,len)){
		
			laserHit.collider.gameObject.SendMessage("Damage",SendMessageOptions.DontRequireReceiver);
			
			if(laserHit.collider.gameObject.tag != "Player"){
			
				Vector3 refletLaser = Vector3.Reflect(refDir,laserHit.normal);
				refletLaser=refletLaser.normalized;
				lengthofRefLaser =len-(originPos-laserHit.point).magnitude;
				
				ReflectLaser(viewID, lengthofRefLaser,refletLaser,laserHit.point,Up,NumRec+1);
				
				GameObject glowEffect = new GameObject();
				Light glowLight = glowEffect.AddComponent<Light>();
				//glowEffect.AddComponent<>();
				glowLight.enabled=true;
				glowLight.type=LightType.Point;
				glowLight.intensity=(laserIntenseity/Mathf.Pow(3,NumRec));
				glowLight.color=laserColor;
				//glowLight.drawHalo=true;
				//glowLight.flare= new Flare();
				glowEffect.transform.position=laserHit.point+lightDist*laserHit.normal;
				refLaserCyl.Add(glowEffect);
				
				GameObject sparksIns = (GameObject) Instantiate(sparks,laserHit.point, Quaternion.LookRotation(laserHit.normal));
				Destroy(sparksIns,1f);
				
				//laserHit.collider.renderer.
			}
			else
				len= (laserHit.point-originPos).magnitude;
		}
		else{lengthofRefLaser=0;}
		
		GameObject laserSeg = new GameObject();
		LineRenderer lineRenderer = laserSeg.AddComponent<LineRenderer>();
		//lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.material.color=laserColor;
        lineRenderer.SetWidth(widthLaser, widthLaser);
        lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0,originPos);
		lineRenderer.SetPosition(1,originPos+refDir*(len-lengthofRefLaser));
	    lineRenderer.material.shader = laserShader;
		lineRenderer.material.color=laserColor;
		
		refLaserCyl.Add(laserSeg);
			
	}
}
