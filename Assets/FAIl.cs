using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FAIl : MonoBehaviour {
	
	public Camera camera;
	
	public float laserRange=20f;
	
	public Color laserColor;
	public int refNum=3;
	
	public float laserIntenseity = 4f;
	
	private float widthLaser=0.15f;
	
	public float lightDist=0.5f;
	
	public Shader laserShader; 
	
	private List<GameObject> refLaserCyl= new List<GameObject>();
	private float SphereRadius=0.5f;
    void Start(){
	//	laserShader = Shader.Find("Self-Illuminated Specular");
		
		//SphereCollider col = (SphereCollider) collider.;
	//	SphereRadius = col.radius;
	}
	
	void Update () {
		foreach( GameObject i in refLaserCyl){
			Destroy(i);
		}
		refLaserCyl.Clear();
		if(Input.GetMouseButton(0)){
		
			
			Ray click = camera.ScreenPointToRay(Input.mousePosition);
			Vector3 mousePos = click.GetPoint(25f);
			mousePos.y=0;
			Vector3 laserDir = mousePos - transform.position;
			laserDir.y=0;
			laserDir=laserDir.normalized;
			ReflectLaser(laserRange,laserDir,transform.position+laserDir*SphereRadius,transform.up,1);
		}
		
		
	}
	
	void ReflectLaser(float len, Vector3 refDir,Vector3 originPos,Vector3 Up,float NumRec){
		if(refNum<NumRec){return;}
		RaycastHit laserHit;
		float lengthofRefLaser;
		if(Physics.Raycast(originPos, refDir, out laserHit,len)){
			Vector3 refletLaser = Vector3.Reflect(refDir,laserHit.normal);
			refletLaser=refletLaser.normalized;
			lengthofRefLaser =len-(originPos-laserHit.point).magnitude;
			ReflectLaser(lengthofRefLaser,refletLaser,laserHit.point,Up,NumRec+1);
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
			
			//laserHit.collider.renderer.
		}
		else
		{
			lengthofRefLaser=0;
		}
		
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
