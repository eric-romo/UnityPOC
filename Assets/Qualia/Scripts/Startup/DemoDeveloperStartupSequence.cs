using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DemoDeveloperStartupSequence : MonoBehaviour {
	
	public bool WaitForLogin = false;
	
	public bool Autoplay = true;
	
	private bool Initialized = false;
	
	private DisplayManager displayManager;
	private EnvironmentManager environmentManager;
	
	private GameObject editorDisplay;
	private GameObject previewDisplay;
	private GameObject jobsDisplay;
	private GameObject sharedDisplay;
	
	void Awake(){
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		environmentManager = GameObject.Find("/EnvironmentManager").GetComponent<EnvironmentManager>();
	}
	
	void Start () {
		environmentManager.SwitchEnvironment("Empty");
		displayManager.LocationTransforms = displayManager.LTLayouts["empty"];
		
		editorDisplay = displayManager.CreateDisplay("Editor Display", "coui://UIResources/Qualia/LoadingScreen/index.html", "spawn");
		previewDisplay = displayManager.CreateDisplay("Preview Display", "https://c9.io/qualiademo/demos/workspace/model-viewer/model-viewer.html", "spawn");
		jobsDisplay = displayManager.CreateDisplay("Jobs Display", "http://qualia3d.com/jobs.html", "spawn");
		sharedDisplay = displayManager.CreateDisplay("Shared Display", "http://piratepad.net/o27r6xuk94", "spawn");
		
		editorDisplay.SetActive(false);
		previewDisplay.SetActive(false);
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
		startupSequence.AppendCallback(SpawnPreview);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(SpawnJobs);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(SpawnShared);
		if(WaitForLogin){
			startupSequence.AppendInterval(7);
		} else {
			startupSequence.AppendInterval(0.5f);
		}
		startupSequence.AppendCallback(NavigateToCloud9);
		startupSequence.Play();
	}
	
	public void SpawnLogin(){
		editorDisplay.SetActive(true);
		displayManager.FocusedDisplay = editorDisplay;
		displayManager.MoveDisplayToLocation(editorDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(editorDisplay, "front", true);
	}
	public void SpawnPreview(){
		previewDisplay.SetActive(true);
		displayManager.MoveDisplayToLocation(previewDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(previewDisplay, "right", true);
	}
	public void SpawnJobs(){
		jobsDisplay.SetActive(true);
		displayManager.MoveDisplayToLocation(jobsDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(jobsDisplay, "left", true);
	}
	public void SpawnShared(){
		sharedDisplay.SetActive(true);
		displayManager.MoveDisplayToLocation(sharedDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(sharedDisplay, "down", true);
	}
	
	private void NavigateToCloud9(){
		editorDisplay.GetComponent<DisplayController>().LoadUrl("https://c9.io/qualiademo/demos");
	}
	
	void Update () {
		if(!Autoplay && !Initialized && Input.anyKeyDown){
			Play();
		}
	}
}
