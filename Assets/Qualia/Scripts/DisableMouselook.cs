using UnityEngine;
using System.Collections;

public class DisableMouselook : MonoBehaviour {

	private OVRPlayerController playerController;
	// Use this for initialization
	void Start () {
		playerController = GetComponent<OVRPlayerController>();
		playerController.SetAllowMouseRotation(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
