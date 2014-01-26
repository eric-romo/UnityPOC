using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class DisplayManager : MonoBehaviour {

	#region Editor Fields
	public GameObject DisplayPrefab;
	public float AnimationTime = 1.0f;
	#endregion
	
	#region Location Transforms
	public Dictionary<string, StoredTransform> LocationTransforms = new Dictionary<string, StoredTransform>{
		{"single-primary", new StoredTransform(new Vector3(-0.6f, 2.15f, -0.35f), new Vector3(0, 0, 0), new Vector3(2.5f, 2.5f, 2.5f)) },
		{"shared-left", new StoredTransform(new Vector3(-1.5f, 1.8f, 2.8f), new Vector3(0, -90, -20), new Vector3(1.15f, 1.15f, 1.15f)) },
		{"shared-right", new StoredTransform(new Vector3(-1.5f, 1.8f, -2.8f), new Vector3(0, 90, -20), new Vector3(1.15f, 1.15f, 1.15f)) },
		{"single-primary-spawn", new StoredTransform(new Vector3(-0.6f, 2.15f, 0), new Vector3(0, 90, 0), new Vector3(0.5f, 0.5f, 0.5f)) },
		{"shared-mine", new StoredTransform(new Vector3(-1.6f, 0.65f, -0.35f), new Vector3(0, 0, 300), new Vector3(2f, 2f, 2f)) },
	};
	#endregion
	
	#region Public Properties
	GameObject _focusedDisplay = null;
	public GameObject FocusedDisplay {
		get{
			return _focusedDisplay;
		}
		set{
			_focusedDisplay = value;
			_focusedView = value.GetComponentInChildren<CoherentUIView>();
			if(_focusedDisplayController != null)
				_focusedDisplayController.Focused = false;
			_focusedDisplayController = value.GetComponent<DisplayController>();
			_focusedDisplayController.Focused = true;
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
	#endregion
	
	#region Initialization
	void Awake(){
		MainCamera = GameObject.Find("CameraRight").GetComponent<Camera>();
		HOTween.Init( true, false, true );
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
		
		if(Input.GetButton("Super Button") ){
			if(Input.GetKeyDown(KeyCode.Tab)){
				int index = (Displays.IndexOf(FocusedDisplay) + 1);
				index %= Displays.Count;
				Debug.Log("Index: " + index);
				FocusedDisplay = Displays[index];
			}
			
			if(Input.GetKeyDown(KeyCode.W)){
				MoveDisplayToLocation(FocusedDisplay, "single-primary", true);
			}
			if(Input.GetKeyDown(KeyCode.A)){
				MoveDisplayToLocation(FocusedDisplay, "shared-left", true);
			}
			if(Input.GetKeyDown(KeyCode.S)){
				MoveDisplayToLocation(FocusedDisplay, "shared-mine", true);
			}
			if(Input.GetKeyDown(KeyCode.D)){
				MoveDisplayToLocation(FocusedDisplay, "shared-right", true);
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
			}
		}
	}
	#endregion
	
	public void MoveDisplayToLocation(GameObject display, string locationName, bool animate = false){//TODO If a display already exists there, swap
		
		Debug.Log("Moving to location: " + locationName);
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
		
		if(displayIsAtTargetLocation){
			if(displayAtTargetLocation != null && displayAtTargetLocation != display && currentLocation != null){
				Debug.Log("Swapping");
				MoveDisplayToLocation(displayAtTargetLocation, currentLocation, animate);
			}
		}
		
		Debug.Log("Locations: " + Locations.Keys.ToString());
		
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
