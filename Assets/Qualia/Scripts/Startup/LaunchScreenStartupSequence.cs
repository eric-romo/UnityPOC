using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class LaunchScreenStartupSequence : MonoBehaviour {
	
	public bool WaitForLogin = true;
	
	private DisplayManager displayManager;
	
	private GameObject mainDisplay;
	
	// Use this for initialization
	void Start () {
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		
		mainDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		mainDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/SystemWindow/index.html");
		mainDisplay.name = "System Display";
		mainDisplay.SetActive(false);
		
		Sequence startupSequence = new Sequence();
		startupSequence.AppendInterval(2);
		startupSequence.AppendCallback(SpawnLogin);
		if(WaitForLogin){
			startupSequence.AppendInterval(10);
		} else {
			startupSequence.AppendInterval(0.5f);
		}
		startupSequence.AppendCallback(NavigateToLaunchScreen);
		startupSequence.Play();
		
	}
	
	public void SpawnLogin(){
		mainDisplay.SetActive(true);
		displayManager.FocusedDisplay = mainDisplay;
		displayManager.MoveDisplayToLocation(mainDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(mainDisplay, "front", true);
	}
	
	private void NavigateToLaunchScreen(){
		mainDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/LaunchScreen/index.html");
	}
	
	void Update () {
		
	}
}
