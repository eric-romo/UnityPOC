using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class StartupSequence : MonoBehaviour {
	
	private DisplayManager displayManager;

	private GameObject systemDisplay;
	private GameObject mainDisplay;
	
	// Use this for initialization
	void Start () {
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		
		systemDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		systemDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/SystemWindow/index.html");
		//systemDisplay.GetComponent<DisplayController>().LoadUrl("www.engadget.com");
		systemDisplay.SetActive(false);
		
		Sequence startupSequence = new Sequence();
		startupSequence.AppendInterval(2);
		startupSequence.AppendCallback(SpawnLogin);
		startupSequence.AppendInterval(6);
		startupSequence.AppendCallback(MinimizeLogin);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(SpawnMain);
		startupSequence.AppendInterval(1);
		startupSequence.AppendCallback(MinimizeMain);
		startupSequence.Play();
	}
	
	public void SpawnLogin(){
		systemDisplay.SetActive(true);
		displayManager.FocusedDisplay = systemDisplay;
		displayManager.MoveDisplayToLocation(systemDisplay, "single-primary-spawn", false);
		displayManager.MoveDisplayToLocation(systemDisplay, "single-primary", true);
	}
	
	public void MinimizeLogin(){
		displayManager.MoveDisplayToLocation(systemDisplay, "shared-left", true);
		mainDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		mainDisplay.SetActive(false);
	}
	
	public void SpawnMain(){
		mainDisplay.SetActive(true);
		displayManager.FocusedDisplay = mainDisplay;
		displayManager.MoveDisplayToLocation(mainDisplay, "single-primary-spawn", false);
		displayManager.MoveDisplayToLocation(mainDisplay, "single-primary", true);
	}
	
	public void MinimizeMain(){
		displayManager.MoveDisplayToLocation(mainDisplay, "shared-mine", true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
