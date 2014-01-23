using UnityEngine;
using System.Collections;

public class DisableMouselook : MonoBehaviour {

	private OVRPlayerController playerController;
	// Use this for initialization
	void Start () {
		playerController = GetComponent<OVRPlayerController>();
		playerController.SetAllowMouseRotation(false);
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		Screen.lockCursor = true;
	}
	
	void OnApplicationPause(bool focus){
		if(focus){
			Screen.lockCursor = true;
		}
	}
}
