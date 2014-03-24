using UnityEngine;
using System.Collections;

public class User {
	public User (string id, GameObject avatar, PhotonPlayer player)
	{
		ID = id;
		Avatar = avatar;
		PhotonPlayer = player;
	}

	public PhotonPlayer PhotonPlayer;
	
	public GameObject Avatar;
	
	public string ID;
	
}
