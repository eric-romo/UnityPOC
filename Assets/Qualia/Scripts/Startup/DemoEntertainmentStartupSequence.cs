using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DemoEntertainmentStartupSequence : MonoBehaviour {
	
	public bool WaitForLogin = false;
	
	public bool Autoplay = true;
	
	private bool Initialized = false;
	
	private DisplayManager displayManager;
	private EnvironmentManager environmentManager;
	
	private GameObject gameDisplay;
	private GameObject netflixDisplay;
	private GameObject jobsDisplay;
	private GameObject sharedDisplay;
	
	void Awake(){
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		environmentManager = GameObject.Find("/EnvironmentManager").GetComponent<EnvironmentManager>();
	}
	
	void Start () {
		environmentManager.SwitchEnvironment("Greenhouse");
		displayManager.LocationTransforms = displayManager.LTLayouts["entertainment"];
		
		gameDisplay = displayManager.CreateDisplay("Game Display", "coui://UIResources/Qualia/LoadingScreen/index.html", "spawn");
		netflixDisplay = displayManager.CreateDisplay("Netflix Display", "http://www.netflix.com/WiPlayer?movieid=70153380&trkid=7728649&tctx=-99%2C-99%2Cc8f2efb8-1052-42af-a945-cadd1946453a-8519625", "spawn");
		jobsDisplay = displayManager.CreateDisplay("Jobs Display", "http://qualia3d.com/hiring.html", "spawn");
		sharedDisplay = displayManager.CreateDisplay("Shared Display", "http://piratepad.net/o27r6xuk94", "spawn");
		
		gameDisplay.SetActive(false);
		netflixDisplay.SetActive(false);
		jobsDisplay.SetActive(false);
		sharedDisplay.SetActive(false);
		
		if(Autoplay)
			Play();
			
	}
	
	public void Play(){
		Initialized = true;
		
		Sequence startupSequence = new Sequence();
		startupSequence.AppendCallback(SpawnLogin);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(SpawnNetflix);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(SpawnJobs);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(SpawnShared);
		if(WaitForLogin){
			startupSequence.AppendInterval(7);
		} else {
			startupSequence.AppendInterval(0.5f);
		}
		startupSequence.AppendCallback(NavigateToGame);
		startupSequence.Play();
	}
	
	public void SpawnLogin(){
		gameDisplay.SetActive(true);
		displayManager.FocusedDisplay = gameDisplay;
		displayManager.MoveDisplayToLocation(gameDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(gameDisplay, "front", true);
	}
	public void SpawnNetflix(){
		netflixDisplay.SetActive(true);
		displayManager.MoveDisplayToLocation(netflixDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(netflixDisplay, "left", true);
	}
	public void SpawnJobs(){
		jobsDisplay.SetActive(true);
		displayManager.MoveDisplayToLocation(jobsDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(jobsDisplay, "right", true);
	}
	public void SpawnShared(){
		sharedDisplay.SetActive(true);
		displayManager.MoveDisplayToLocation(sharedDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(sharedDisplay, "down", true);
	}
	
	private void NavigateToGame(){
		gameDisplay.GetComponent<DisplayController>().LoadUrl("http://hexgl.bkcore.com/");
	}
	
	void Update () {
		if(!Autoplay && !Initialized && Input.anyKeyDown){
			Play();
		}
	}
}
