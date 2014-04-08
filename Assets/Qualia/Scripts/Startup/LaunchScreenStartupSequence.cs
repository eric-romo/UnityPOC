using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class LaunchScreenStartupSequence : MonoBehaviour {
	
	public bool WaitForLogin = true;
	
	public bool StartMinimized = true;
	
	public bool Autoplay = false;
	
	private DisplayManager displayManager;
	
	private GameObject mainDisplay;
	
	void Awake(){
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
	}
	
	void Start () {
		if(Autoplay)
			Play();
	}
	
	public void Play(){
		/*mainDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		mainDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/LoadingScreen/index.html");
		mainDisplay.name = "System Display";
		mainDisplay.SetActive(false);*/
		mainDisplay = displayManager.CreateDisplay("System Display", "coui://UIResources/Qualia/LoadingScreen/index.html", "spawn");
		
		Sequence startupSequence = new Sequence();
		startupSequence.AppendInterval(2);
		startupSequence.AppendCallback(SpawnLogin);
		if(WaitForLogin){
			startupSequence.AppendInterval(7);
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
		if(StartMinimized)
			displayManager.MoveDisplayToLocation(mainDisplay, "left", true);
		else
			displayManager.MoveDisplayToLocation(mainDisplay, "front", true);
	}
	
	private void NavigateToLaunchScreen(){
		mainDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/LaunchScreen/index.html");
	}
	
	void Update () {
		
	}
}
