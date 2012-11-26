using UnityEngine;
using System.Collections;
using System;


public class ConnectionScript : MonoBehaviour 
{
	//References to scripts used
	//http://www.youtube.com/watch?v=v_HR-wAdruk
	//http://www.youtube.com/watch?v=Y4evTLXXIYY&list=PL8853AD61DF0806F5&index=14&feature=plpp_video
	//Lookup potential Port # to change
	public GUISkin myskin;
	public GameObject player;
	
	private string serverName = "", maxPlayers = "", port = "25566";
	private Rect windowRect = new Rect(100, 0, 400, 400);
	
	private void OnGUI()
	{
		GUI.skin = myskin;
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			windowRect = GUI.Window(0, windowRect, windowFunction, "Servers");
			GUILayout.Label ("Server Name");
			serverName = GUILayout.TextField(serverName);
		
			GUILayout.Label ("Port");
			port = GUILayout.TextField(port);
	
			GUILayout.Label ("Max Players");
			maxPlayers = GUILayout.TextField(maxPlayers);
			
			if (GUILayout.Button ("Create Server"))
			{
				try
				{
					Network.InitializeSecurity();
					Network.InitializeServer(int.Parse(maxPlayers), int.Parse(port), !Network.HavePublicAddress());
					MasterServer.RegisterHost("NetworkTest", serverName);
				}	
				catch (Exception)
				{
					print("Please type in numbers for port and max players");
				}
			}	
		}
		else
		{
			if (GUILayout.Button("Spawn"))
			{
				Network.Instantiate(player, transform.position, transform.rotation, 0);
			}
			
			if (GUILayout.Button("Disconnect"))
			{
				Network.Disconnect();
			}
		}
	}
	
	void OnServerInitialized()
	{
		Application.LoadLevel("Orb");
	}
	
	void OnConnectedToServer()
	{
		Application.LoadLevel("Orb");
	}
	
	private void windowFunction(int id)
	{
		if (GUILayout.Button("Refresh"))
		{
			MasterServer.RequestHostList("NetworkTest");	
		}
		GUILayout.BeginHorizontal();
		
		GUILayout.Box("Server Name");
		
		GUILayout.EndHorizontal();
		
		if (MasterServer.PollHostList().Length != 0)
		{
			HostData[] data = MasterServer.PollHostList();
			foreach(HostData c in data)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Box(c.gameName);
				if (GUILayout.Button("Connect"))
				{
					Network.Connect(c);
				}
				GUILayout.EndHorizontal();
			}
		}
		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}
}
