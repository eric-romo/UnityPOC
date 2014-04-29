using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class LaunchScreenStartupSequence : MonoBehaviour {
	
	public bool WaitForLogin = false;
	
	public bool Autoplay = true;
	
	private DisplayManager displayManager;
	private EnvironmentManager environmentManager;
	
	private GameObject editorDisplay;
	private GameObject previewDisplay;
	
	void Awake(){
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		environmentManager = GameObject.Find("/EnvironmentManager").GetComponent<EnvironmentManager>();
	}
	
	void Start () {
		
		if(Autoplay)
			Play();
			
	}
	
	public void Play(){
		environmentManager.SwitchEnvironment("Empty");
		displayManager.LocationTransforms = displayManager.LTLayouts["empty"];
		
		editorDisplay = displayManager.CreateDisplay("Editor Display", "coui://UIResources/Qualia/LoadingScreen/index.html", "spawn");
		previewDisplay = displayManager.CreateDisplay("Preview Display", "https://c9.io/qualiademo/demos/workspace/model-viewer/model-viewer.html", "spawn");
		
		Sequence startupSequence = new Sequence();
		startupSequence.AppendInterval(2);
		startupSequence.AppendCallback(SpawnLogin);
		if(WaitForLogin){
			startupSequence.AppendInterval(7);
		} else {
			startupSequence.AppendInterval(0.5f);
		}
		startupSequence.AppendCallback(NavigateToCloud9);
		startupSequence.AppendCallback(SpawnPreview);
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
		displayManager.MoveDisplayToLocation(previewDisplay, "down", true);
	}
	
	private void NavigateToCloud9(){
		editorDisplay.GetComponent<DisplayController>().LoadUrl("https://c9.io/qualiademo/demos");
	}
	
	void Update () {
		
	}
}
