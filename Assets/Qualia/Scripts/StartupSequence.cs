using UnityEngine;
using System.Collections;

public class StartupSequence : MonoBehaviour {
	
	private DisplayManager displayManager;
	
	// Use this for initialization
	void Start () {
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		
		GameObject mainDisplay = Instantiate(displayManager.DisplayPrefab) as GameObject;
		displayManager.MoveDisplayToLocation(mainDisplay, "single-primary-spawn", false);
		displayManager.MoveDisplayToLocation(mainDisplay, "single-primary", true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
