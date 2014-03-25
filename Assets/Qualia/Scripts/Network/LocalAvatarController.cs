using UnityEngine;
using System.Collections;

public class LocalAvatarController : MonoBehaviour {
	
	private NetworkMananger networkManager;
	
	private GameObject cameraRight;
	private GameObject avatar;
	private GameObject lookDirection;
	private GameObject head;
	
	private bool initialized = false;
	
	// Use this for initialization
	void Awake () {
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkMananger>();
		cameraRight = transform.Find("OVRCameraController/CameraRight").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(networkManager.LocalUser != null && !initialized){
			avatar = networkManager.LocalUser.Avatar;
			lookDirection = avatar.transform.Find ("LookDirection").gameObject;
			initialized = true;
		}
		
		if(initialized){
			lookDirection.transform.rotation = cameraRight.transform.rotation;
			//Vector3 localEulerAngles = lookDirection.transform.localEulerAngles;
			//lookDirection.transform.localEulerAngles = localEulerAngles;
			
			lookDirection.transform.position = cameraRight.transform.position;
			
			avatar.transform.position = avatar.transform.position; //Bug, but looks kinda good. A bit more dynamic avatar.
		}
	}
}
