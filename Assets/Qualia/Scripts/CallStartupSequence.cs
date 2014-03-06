using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class CallStartupSequence : MonoBehaviour {

	public bool WaitForLogin = true;
	
	private DisplayManager displayManager;

	private GameObject systemDisplay;
	private GameObject mainDisplay;
	private GameObject secondaryDisplay;
	
	// Use this for initialization
	void Start () {
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		
		systemDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		systemDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/SystemWindow/index.html");
		systemDisplay.name = "System Display";
		systemDisplay.SetActive(false);
		
		mainDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		mainDisplay.name = "Main Display";
		mainDisplay.SetActive(false);
		
		secondaryDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		secondaryDisplay.name = "Secondary Display";
		secondaryDisplay.SetActive(false);
		//secondaryDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/CallWindow/index.html");
		secondaryDisplay.GetComponent<DisplayController>().LoadUrl("http://www.gavanwilhite.com/test/webrtc");
		
		Sequence startupSequence = new Sequence();
		startupSequence.AppendInterval(2);
		startupSequence.AppendCallback(SpawnLogin);
		if(WaitForLogin){
			startupSequence.AppendInterval(10);
		} else {
			startupSequence.AppendInterval(0.5f);
		}
		startupSequence.AppendCallback(MinimizeLogin);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(SpawnSecondary);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(MinimizeSecondary);
		startupSequence.AppendInterval(1f);
		startupSequence.AppendCallback(SpawnMain);
		startupSequence.Play();
	}
	
	public void SpawnLogin(){
		systemDisplay.SetActive(true);
		displayManager.FocusedDisplay = systemDisplay;
		displayManager.MoveDisplayToLocation(systemDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(systemDisplay, "front", true);
	}
	
	public void MinimizeLogin(){
		displayManager.MoveDisplayToLocation(systemDisplay, "left", true);
	}
	
	public void SpawnSecondary(){
		secondaryDisplay.SetActive(true);
		displayManager.FocusedDisplay = secondaryDisplay;
		displayManager.MoveDisplayToLocation(secondaryDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(secondaryDisplay, "front", true);
	}
	
	public void SpawnMain(){
		mainDisplay.SetActive(true);
		displayManager.FocusedDisplay = mainDisplay;
		displayManager.MoveDisplayToLocation(mainDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(mainDisplay, "front", true);
	}
	
	public void MinimizeSecondary(){
		displayManager.MoveDisplayToLocation(secondaryDisplay, "down", true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
