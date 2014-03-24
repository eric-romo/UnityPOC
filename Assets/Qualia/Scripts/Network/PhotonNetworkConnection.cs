using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

public class PhotonNetworkConnection : MonoBehaviour {
	/*
		This class should contain most of the boilerplate junk to enable networking. Actual behavior level stuff should be executed in Network Mananger. 
	*/
	
	private NetworkMananger networkManager;
	
	#region CONNECTION HANDLING
	
	public void Awake()
	{
		networkManager = GetComponent<NetworkMananger>();
		
		if (!PhotonNetwork.connected)
		{
			PhotonNetwork.autoJoinLobby = false;
			PhotonNetwork.ConnectUsingSettings("1");
		}
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnConnectedToMaster()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null, true, true, 4);
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnJoinedRoom()
	{
		Debug.Log("Joined Room, with " + PhotonNetwork.room.playerCount + " players");
		networkManager.AddMyself();
	}
	
	// This is one of the callback/event methods called by PUN (read more in PhotonNetworkingMessage enumeration)
	public void OnCreatedRoom()
	{
		Debug.Log("Created Room");
		//Application.LoadLevel(Application.loadedLevel);
	}
	
	#endregion
	
	#region Player Handling
	
	void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		Debug.Log("Player connected: " + player);
		//networkManager.AddUser(player);
	}
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("Player disconnected: " + player);
		networkManager.RemoveUser(player);
		
	}
	
	#endregion
}
