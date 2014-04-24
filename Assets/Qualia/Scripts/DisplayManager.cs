using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

[RequireComponent(typeof(PhotonView))]
public class DisplayManager : MonoBehaviour {

	#region Editor Fields
	public GameObject DisplayPrefab;
	public float AnimationTime = 1.0f;
	#endregion
	
	#region Location Transforms
	public Dictionary<string, StoredTransform> LocationTransforms;
	
	public Dictionary<string, Dictionary<string, StoredTransform>> LTLayouts = new Dictionary<string, Dictionary<string, StoredTransform>>{
		{"original", new Dictionary<string, StoredTransform>
			{
				{"front", new StoredTransform(new Vector3(-0.6f, 2.15f, -0.35f), new Vector3(0, 0, 0), new Vector3(2.5f, 2.5f, 2.5f)) },
				{"left", new StoredTransform(new Vector3(-1.5f, 1.8f, 2.8f), new Vector3(0, -90, -20), new Vector3(1.15f, 1.15f, 1.15f)) },
				{"right", new StoredTransform(new Vector3(-1.5f, 1.8f, -2.8f), new Vector3(0, 90, -20), new Vector3(1.15f, 1.15f, 1.15f)) },
				{"spawn", new StoredTransform(new Vector3(-0.6f, 2.15f, 0), new Vector3(0, 90, 0), new Vector3(0.5f, 0.5f, 0.5f)) },
				{"down", new StoredTransform(new Vector3(-1.6f, 0.65f, -0.35f), new Vector3(0, 0, 300), new Vector3(2f, 2f, 2f)) }
			}
		},
		{"table", new Dictionary<string, StoredTransform>
			{
				{"front", new StoredTransform(new Vector3(1f, 3.77f, -0.5f), new Vector3(0, 0, 0), new Vector3(5.4f, 5.4f, 5.4f)) },
				{"left", new StoredTransform(new Vector3(-0.22f, 1.8f, 2.8f), new Vector3(0, -90, -20), new Vector3(1.15f, 1.15f, 1.15f)) },
				{"right", new StoredTransform(new Vector3(-0.22f, 1.8f, -2.8f), new Vector3(0, 90, -20), new Vector3(1.15f, 1.15f, 1.15f)) },
				{"spawn", new StoredTransform(new Vector3(-0.6f, 2.15f, 0), new Vector3(0, 90, 0), new Vector3(0.5f, 0.5f, 0.5f)) },
				{"down", new StoredTransform(new Vector3(-0.13f, 1.2f, -0.35f), new Vector3(0, 0, 270), new Vector3(2.45f, 2.45f, 2.45f)) }
			}
		}
	};
	
	/*public Dictionary<string, DisplayLayout> Layouts = new Dictionary<string, DisplayLayout>{
		{"original", new DisplayLayout(
			front: 
		)},
	}*/
	
	#endregion
	
	#region Public Properties
	GameObject _focusedDisplay = null;
	public GameObject FocusedDisplay {
		get{
			return _focusedDisplay;
		}
		set{
			_focusedDisplay = value;
			if(_focusedDisplayController != null){
				_focusedDisplayController.Focused = false;
			}
			if(value != null){
				_focusedView = value.GetComponentInChildren<CoherentUIView>();
				_focusedDisplayController = value.GetComponent<DisplayController>();
				_focusedDisplayController.Focused = true;
			} else {
				_focusedView = null;
				_focusedDisplayController = null;
			}
		}
	}
	DisplayController _focusedDisplayController = null;
	public DisplayController FocusedDisplayController {
		get{
			return _focusedDisplayController;
		}
	}
	CoherentUIView _focusedView = null;
	CoherentUIView FocusedView {
		get{
			return _focusedView;
		}
	}
	
	public Dictionary<string, GameObject> Locations = new Dictionary<string, GameObject>();
	
	[HideInInspector]
	public Camera MainCamera = null;
	
	[HideInInspector]
	public List<GameObject> Displays = new List<GameObject>();
	
	public string Homepage = "coui://UIResources/Qualia/LaunchScreen/index.html";
	#endregion
	
	#region Private Variables
	CoherentUISystem coherentUISytem;
	NetworkMananger networkManager;
	#endregion
	
	#region Initialization
	void Awake(){
		coherentUISytem = GetComponent<CoherentUISystem>();
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkMananger>();
		
		MainCamera = GameObject.Find("CameraRight").GetComponent<Camera>();
		HOTween.Init( true, false, true );
		LocationTransforms = LTLayouts["table"];
	}
	
	void Start () {
		//Displays.AddRange(GameObject.FindGameObjectsWithTag("Display"));
	}
	#endregion
	
	#region Update and Input
	void Update () {
		ProcessInput();
	}
	
	private void ProcessInput(){
		
		if(Displays.Count == 0 && Input.GetMouseButtonDown(0)){
			CreateDisplay("New Display", Homepage, "front");
		}
		
		if(Input.GetButton("Super Button") ){
			if(Input.GetKeyDown(KeyCode.Tab)){
				int index = (Displays.IndexOf(FocusedDisplay) + 1);
				index %= Displays.Count;
				Debug.Log("Index: " + index);
				FocusedDisplay = Displays[index];
			}
			
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				MoveDisplayToLocation(FocusedDisplay, "front", true);
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				MoveDisplayToLocation(FocusedDisplay, "left", true);
			}
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				MoveDisplayToLocation(FocusedDisplay, "down", true);
			}
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				MoveDisplayToLocation(FocusedDisplay, "right", true);
			}
			if(Input.GetKeyDown(KeyCode.N)){
				GameObject display = CreateDisplay("New Screen", "coui://UIResources/Qualia/LaunchScreen/index.html", "spawn");
				MoveDisplayToLocation(display, "front", true);
			}
		}
		

		// Activate input processing for the view being looked at
		RaycastHit hitInfo;
		if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hitInfo))
		{
			CoherentUIView viewComponent = hitInfo.collider.gameObject.GetComponent(typeof(CoherentUIView)) as CoherentUIView;
			if (viewComponent == null)
			{
				viewComponent = hitInfo.collider.gameObject.GetComponentInChildren(typeof(CoherentUIView)) as CoherentUIView;
			}
			
			if (viewComponent != null && !viewComponent.ClickToFocus)
			{
				GameObject display = viewComponent.gameObject.transform.parent.gameObject;
				if(FocusedDisplay != null && FocusedDisplay != display){//Don't spam focus updates
					if(!FocusedDisplay.GetComponent<DisplayController>().Dragging){
						FocusedDisplay = display;
					}
				}
				if(FocusedDisplay == null){
					FocusedDisplay = display;
				}
			}
		}
	}
	#endregion
	
	public void MoveDisplayToLocation(GameObject display, string locationName, bool animate = false, bool sendRPC = false){//TODO If a display already exists there, swap
		
		//Debug.Log("Moving to location: " + locationName);
		if(animate){
			StoredTransform storedTransform = LocationTransforms[locationName];
			HOTween.To(display.transform, AnimationTime, "position", storedTransform.position);
			HOTween.To(display.transform, AnimationTime, "rotation", Quaternion.Euler(storedTransform.rotation));
			HOTween.To(display.transform, AnimationTime, "localScale", storedTransform.scale);
		} else {
			StoredTransform storedTransform = LocationTransforms[locationName];
			display.transform.position = storedTransform.position;
			display.transform.eulerAngles = storedTransform.rotation;
			display.transform.localScale = storedTransform.scale;
		}
		
		string currentLocation = display.GetComponent<DisplayController>().Location;
		GameObject displayAtTargetLocation = null;
		bool displayIsAtTargetLocation = Locations.TryGetValue(locationName, out displayAtTargetLocation);
		
		Locations[currentLocation] = null;
		display.GetComponent<DisplayController>().Location = locationName;
		Locations[locationName] = display;
		
		/*if(displayIsAtTargetLocation){
			if(displayAtTargetLocation != null && displayAtTargetLocation != display && currentLocation != null){
				//Debug.Log("Swapping");
				MoveDisplayToLocation(displayAtTargetLocation, currentLocation, animate);
			}
		}*/
		/*if(sendRPC){
			GetComponent<PhotonView>().RPC("MoveDisplayToLocation", PhotonTargets.OthersBuffered, new object[]{display, locationName, animate});
		}*/
		
	}

	public void Close (GameObject display)
	{
		DisplayController displayController = display.GetComponent<DisplayController>();
		if(displayController.Location != null){
			Locations[displayController.Location] = null;
		}
		if(displayController.Focused){
			FocusedDisplay = null;
		}
		GameObject.Destroy(display);
	}
	
	public GameObject CreateDisplay(string name, string url, string locationName){
		if(!networkManager.Networked){
			GameObject display;
			display = Instantiate(DisplayPrefab) as GameObject;
			display.GetComponent<DisplayController>().LoadUrl(url);
			//display.GetComponent<DisplayController>().Location = locationName;
			display.name = name;
			//The display will add itself to Displays
			
			MoveDisplayToLocation(display,locationName, false);
			
			return display;
		} else {
			return CreateNetworkDisplay(name, url, locationName);
		}
	}
	
	private GameObject CreateNetworkDisplay(string name, string url, string locationName){
	
		GameObject display;
		
		display = PhotonNetwork.Instantiate(DisplayPrefab.name, new Vector3(0, -1000, 0), Quaternion.identity, 0);
		
		PhotonView photonView = display.GetComponent<PhotonView>();
		
		object[] args = new object[]{name, url, locationName};
		
		photonView.RPC("Init", PhotonTargets.AllBuffered, args);
		
		return display;
	}
}

#region StoredTransform
public struct StoredTransform{
	public Vector3 position;
	public Vector3 rotation;
	public Vector3 scale;
	
	public StoredTransform(Vector3 position, Vector3 rotation, Vector3 scale){
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
	}
}
#endregion
