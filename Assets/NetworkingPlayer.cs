using UnityEngine;
using System.Collections;
using System;

public class NetworkingPlayer : MonoBehaviour {

	public GameObject player;
	
	public GUIText networkStatusText;
	public GUITexture networkStatusView;
	private string serverName = "MadBubbleSmashGameWINNER"; 
	private int maxPlayers = 1; 
	private int port = 25567;
	private Rect windowRect = new Rect(100, 0, 400, 400);
	private bool isGameAvailable = false;
	private bool gameActive = false;
	private bool playerInstantiated = false;
		
	void Start () {
		MasterServer.RequestHostList(serverName);						
	}
	
	void OnGUI () {
		
		if (!gameActive) {
			if (GUI.Button(new Rect(Screen.width/2-110, Screen.height/2, 220, 100), "Debug Start")) {
				player.GetComponent<ControllerToUseController>().movementEnabled = true;
				networkStatusView.enabled = false;
				networkStatusText.enabled = false;		
				Instantiate(player, player.transform.position, Quaternion.identity);
				gameActive = true;
			}   	
		}
	}
	
	void Update () {
	
	}
	
	void OnServerInitialized() {
		
	}
	
	void OnConnectedToServer() {
		Debug.Log("Connected to server");		
		NetworkViewID viewID = Network.AllocateViewID();
        networkView.RPC("SpawnPlayer", RPCMode.AllBuffered, viewID, transform.position);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent) {
        
		Debug.Log("Server event " + msEvent);
		
		if (msEvent == MasterServerEvent.RegistrationSucceeded) {
        
			networkStatusText.text = "Server registered. Waiting...";
		
		} else if (msEvent == MasterServerEvent.HostListReceived) {
			
			bool foundServer = false;
			
			// Connect to a game if there's one available with less than 1 player
			if (MasterServer.PollHostList().Length != 0) {

				HostData[] data = MasterServer.PollHostList();
				
				for (int i = 0; i < data.Length; i++) {

					HostData host = data[i];					

					if (host.connectedPlayers < 2) {
						networkStatusText.text = "Connecting to host...";	
						foundServer = true;
						Network.Connect(host);						
						break;
					}
				}				
			} 
			
			if (!foundServer) {
				networkStatusText.text = "Creating server...";	
				createServer();
			}
		}
    }
	
	void createServer() {
		
		int randomGameNumber = UnityEngine.Random.Range(0,10000);
		
		String gameName = "win" + randomGameNumber;
		
		try {
			Network.InitializeSecurity();
			Network.InitializeServer(maxPlayers, port, !Network.HavePublicAddress());
			MasterServer.RegisterHost(serverName, serverName, gameName);
			Debug.Log("Created " + gameName);
		} catch (Exception) {
			print("Please type in numbers for port and max players");
		}				
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
	
		Vector3 position = Vector3.zero;
		Debug.Log("serializing");
		if (stream.isWriting) {
			position = player.transform.position;
			stream.Serialize(ref position);
		} else {
			stream.Serialize(ref position);
			player.transform.position = position;
		}
	}
	
    void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
        networkStatusText.text = ("Could not connect to master server: " + info);
    }
	
	[RPC]
    void SpawnPlayer(NetworkViewID viewID, Vector3 location) {
		networkStatusView.enabled = false;
		networkStatusText.enabled = false;
		Debug.Log("Instantiating player");
		if (!playerInstantiated) {
			Network.Instantiate(player, player.transform.position, Quaternion.identity, 0);
		}
		playerInstantiated = true;
		gameActive = true;
	}
}