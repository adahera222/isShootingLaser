using UnityEngine;
using System.Collections;

public class Energy : MonoBehaviour {
    public float barDisplay; //current progress
    public Vector2 pos = new Vector2(650,400);
    public Vector2 size = new Vector2(100,20);
    public Texture2D emptyTex;
    public Texture2D fullTex;

    void OnGUI() {
       //draw the background:
       GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
         GUI.Box(new Rect(0,0, size.x, size.y), fullTex);

         //draw the filled-in part:
         GUI.BeginGroup(new Rect(0,0, barDisplay, size.y));
          GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
         GUI.EndGroup();
       GUI.EndGroup();
    }

    void Update() {
       barDisplay = transform.GetComponent<ControllerToUseController>().energy;
    }
}