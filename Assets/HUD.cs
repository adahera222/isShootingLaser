using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
    public float healthDisplay, energyDisplay, enemyDisplay; 
    public Vector2 size = new Vector2(100,20);
    public Texture2D energyFull = new Texture2D(145, 35);
    public Texture2D energyEmpty = new Texture2D(145, 35);
	public Texture2D healthFull = new Texture2D(145, 35);
    public Texture2D healthEmpty = new Texture2D(145, 35);
	
	public GameObject player;
	
	public GUIStyle style;
	
    void OnGUI() {
       
		if (player.GetComponent<NetworkView>().isMine) {
		  //Draw player energy bar
	       GUI.BeginGroup(new Rect(Screen.width - 140, Screen.height - 50, 128, 50));
	         GUI.Box(new Rect(0,0, 155, 50), energyEmpty, style);
	         	GUI.BeginGroup(new Rect(0,0, energyDisplay, 27));
	          		GUI.Box(new Rect(4,4,124, 30), energyFull, style);
	         	GUI.EndGroup();
	       GUI.EndGroup();
			
		   //Draw player health bar
		   GUI.BeginGroup(new Rect(13, Screen.height - 50, 128, 50));
	         GUI.Box(new Rect(0,0, 155, 50), healthEmpty, style);
	         	GUI.BeginGroup(new Rect(0,0, healthDisplay, 27));
	          		GUI.Box(new Rect(4,4,124, 30), healthFull, style);
	         	GUI.EndGroup();
	       GUI.EndGroup();	
		} else {

			//Draw enemy health bar
		   GUI.BeginGroup(new Rect(Screen.width - 140, 50 , 128, 50));
	         GUI.Box(new Rect(0,0, 155, 50), healthEmpty, style);
	         	GUI.BeginGroup(new Rect(0,0, enemyDisplay, 27));
	          		GUI.Box(new Rect(4,4,124, 30), healthFull, style);
	         	GUI.EndGroup();
	       GUI.EndGroup();	
		}
    }

    void Update() {
       healthDisplay = transform.GetComponent<ControllerToUseController>().health;
	   energyDisplay = transform.GetComponent<ControllerToUseController>().energy;
	   enemyDisplay = player.transform.GetComponent<ControllerToUseController>().health;
    }
}