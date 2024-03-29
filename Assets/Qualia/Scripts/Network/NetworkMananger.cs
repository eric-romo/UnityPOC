﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class NetworkMananger : MonoBehaviour {
	/* It would be nice if this was network platform agnostic, but this is a POC, let's try, but mostly let's just get it done */
	
	public GameObject NetworkVRUserPrefab;
	
	private DisplayManager displayManager;
	
	public Dictionary<string, User> Users = new Dictionary<string, User>();
	
	public User LocalUser;
	
	public PhotonView PhotonView;
	
	public bool Networked = false;
	
	void Awake(){
		displayManager = GameObject.Find("DisplayManager").GetComponent<DisplayManager>();
		PhotonView = GetComponent<PhotonView>();
	}
	
	void Start(){
		
		if(!Networked)
			InitializeSpace();
	}
	
	/*public void AddUser(PhotonPlayer player){
		Debug.Log("Adding User");
		
		Transform spawnPoint = GameObject.Find("SpawnPoints").transform.GetChild(1);
		GameObject avatar = Instantiate(NetworkVRUserPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		
		User user = new User();
		user.ID = player.ID.ToString();
		user.Avatar = avatar;
		user.PhotonPlayer = player;
		
		Users.Add(user.ID, user);
		
	}*/
	
	public void AddMyself(){
		if(!Networked)
			return;
			
		Transform spawnPoint = GetNextSpawnPoint();
		GameObject avatar = PhotonNetwork.Instantiate(NetworkVRUserPrefab.name, spawnPoint.position, spawnPoint.rotation, 0);
		
		avatar.transform.Find("Body").gameObject.layer = 9;
		avatar.transform.Find("LookDirection/Head").gameObject.layer = 9;
		
		GameObject localOVRView = GameObject.Find("LocalOVRView") as GameObject;
		localOVRView.transform.position = spawnPoint.position;
		localOVRView.transform.rotation = spawnPoint.rotation;
		
		
		LocalUser = new User(PhotonNetwork.player.ID.ToString(), avatar, PhotonNetwork.player);
	}
	
	public void RemoveUser(PhotonPlayer player){
		if(!Networked)
			return;
			
		GameObject avatar = Users[player.ID.ToString()].Avatar;
		GameObject.Destroy(avatar);
		Users[player.ID.ToString()] = null;
	}
	
	private Transform GetNextSpawnPoint(){
		return GameObject.Find("SpawnPoints").transform.GetChild(PhotonNetwork.room.playerCount - 1);
	}
	
	public void InitializeSpace(){
		//GameObject.Find("DisplayManager").GetComponent<LaunchScreenStartupSequence>().Play();
	}
	
}
