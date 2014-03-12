using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class HoloChessStartupSequence : MonoBehaviour {
	
	public bool WaitForLogin = true;
	
	private DisplayManager displayManager;
	
	private GameObject systemDisplay;
	private GameObject holographicDisplay;
	
	public GameObject HoloChessPrefab;
	
	// Use this for initialization
	void Start () {
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		
		systemDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		systemDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/SystemWindow/index.html");
		systemDisplay.name = "System Display";
		systemDisplay.SetActive(false);
		
		holographicDisplay = Instantiate (displayManager.DisplayPrefab) as GameObject;
		holographicDisplay.GetComponent<DisplayController>().LoadUrl("coui://UIResources/Qualia/HoloChess/index.html");
		holographicDisplay.name = "Holographic Display";
		holographicDisplay.SetActive(false);
		
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
		startupSequence.AppendCallback(SpawnHologram);
		startupSequence.AppendInterval(0.5f);
		startupSequence.AppendCallback(MinimizeHologram);
		startupSequence.AppendInterval(2f);
		startupSequence.AppendCallback(InitHoloChess);
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
	
	public void SpawnHologram(){
		holographicDisplay.SetActive(true);
		displayManager.FocusedDisplay = holographicDisplay;
		displayManager.MoveDisplayToLocation(holographicDisplay, "spawn", false);
		displayManager.MoveDisplayToLocation(holographicDisplay, "front", true);
	}
	
	public void InitHoloChess(){
		HoloChessController holoChessController = holographicDisplay.AddComponent<HoloChessController>();
		holoChessController.AddAssets(HoloChessPrefab);
	}
	
	public void MinimizeHologram(){
		displayManager.MoveDisplayToLocation(holographicDisplay, "down", true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
