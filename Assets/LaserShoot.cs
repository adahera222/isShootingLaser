using UnityEngine;
using System.Collections;

public class LaserShoot : MonoBehaviour {
	
	public Camera camera;
	
	public float laserRange=10f;
	public Color laserColor;
	//public float timeLaserVisible=0.1f;
	GameObject cylin;
	GameObject refCylin;
	private float laserShotTime;
	
	private float widthLaser=0.15f;
	
	private Vector3 laserDirStor=Vector3.zero;
	private float laserScale=0;
	
	private Vector3 laserRefStor=Vector3.zero;
	private Vector3 refPoint;
	void Update () {
		Destroy (refCylin);
		Destroy(cylin);
		if(Input.GetMouseButton(0)){
		
			
			Ray click = camera.ScreenPointToRay(Input.mousePosition);
			Vector3 mousePos = click.GetPoint(25f);
			mousePos.y=0;
			Vector3 laserDir = mousePos - transform.position;
			laserDir.y=0;
			laserDir=laserDir.normalized;
			
			
			cylin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			cylin.collider.enabled=false;
			laserScale=laserRange/2;
			cylin.transform.position=transform.position+laserScale*laserDir;
			cylin.transform.localScale=new Vector3(widthLaser,laserScale,widthLaser);
			cylin.renderer.material.color=laserColor;
			
			RaycastHit laserHit;
			if(Physics.Raycast(transform.position, laserDir, out laserHit,laserRange)){
				Vector3 refletLaser = Vector3.Reflect(laserDir,laserHit.normal);
				refletLaser=refletLaser.normalized;
				laserRefStor=refletLaser;
				
				refPoint=laserHit.point;
				float lengthofRefLaser =laserRange-(transform.position-laserHit.point).magnitude;
				
				refCylin=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				refCylin.transform.localScale=new Vector3(widthLaser,lengthofRefLaser/2,widthLaser);
				refCylin.transform.position=laserHit.point+ refletLaser*(lengthofRefLaser/2);//-(refletLaser*(laserRange/2));
				refCylin.collider.enabled=false;
				
				Vector3 refRightOfLaserDir = Vector3.Cross(transform.up,refletLaser);
				Vector3 refCylinForward = Vector3.Cross (refRightOfLaserDir,refletLaser);
				refCylin.transform.rotation= Quaternion.LookRotation(refCylinForward, refletLaser);
				
				laserScale=(laserRange-lengthofRefLaser)/2;
				
				cylin.transform.localScale=new Vector3(widthLaser,laserScale,widthLaser);
				cylin.transform.position=transform.position+(laserScale)*laserDir.normalized;
			}
			
	
			
			Vector3 rightOfLaserDir = Vector3.Cross(transform.up,laserDir);
			Vector3 cylinForward = Vector3.Cross (rightOfLaserDir,laserDir);
			
			cylin.transform.rotation= Quaternion.LookRotation(cylinForward, laserDir);
			laserShotTime=Time.time;
			laserDirStor=laserDir;
		}
		
		
		
		//if(cylin){
			//cylin.transform.position=transform.position+laserScale*laserDirStor;
///		}
//		if(Time.time-laserShotTime > timeLaserVisible){
		//	Destroy(cylin);
		//	laserDirStor=Vector3.zero;
		//}
	}
    void OnDrawGizmos() {
		if(laserDirStor == null)
			return;
        Gizmos.color = Color.red;
		
		Gizmos.DrawRay(refPoint, laserRefStor*10);
		
        
    }
}
