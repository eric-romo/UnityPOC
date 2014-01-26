using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayManager : MonoBehaviour {

	#region Editor Fields
	public GameObject DisplayPrefab;
	#endregion
	
	#region Location Transforms
	public Dictionary<string, StoredTransform> LocationTransforms = new Dictionary<string, StoredTransform>{
		{"single-primary", new StoredTransform(new Vector3(-0.6f, 2.15f, -0.35f), new Vector3(0, 0, 0), new Vector3(2.5f, 2.5f, 2.5f)) },
		{"shared-left", new StoredTransform(new Vector3(-1.5f, 1.8f, 2.8f), new Vector3(0, 270, 340), new Vector3(1.15f, 1.15f, 1.15f)) },
		{"single-primary-spawn", new StoredTransform(new Vector3(-0.6f, 2.15f, 0), new Vector3(0, 90, 0), new Vector3(0.5f, 0.5f, 0.5f)) },
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
	
	[HideInInspector]
	public Camera MainCamera = null;
	
	[HideInInspector]
	public List<GameObject> Displays = new List<GameObject>();
	#endregion
	
	#region Initialization
	void Awake(){
		MainCamera = GameObject.Find("CameraRight").GetComponent<Camera>();
	}
	
	void Start () {
		Displays.AddRange(GameObject.FindGameObjectsWithTag("Display"));
		if(Displays.Count > 0){
			FocusedDisplay = Displays[0];
		}
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
				MoveDisplayToLocation(FocusedDisplay, "single-primary");
			}
			if(Input.GetKeyDown(KeyCode.A)){
				MoveDisplayToLocation(FocusedDisplay, "shared-left");
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
				if(FocusedDisplay != display){//Don't spam focus updates
					if(!FocusedDisplay.GetComponent<DisplayController>().Dragging){
						FocusedDisplay = display;
					}
				}
			}
		}
	}
	#endregion
	
	public void MoveDisplayToLocation(GameObject display, string locationName, bool animate = false){
		if(animate){
			StoredTransform storedTransform = LocationTransforms[locationName];
			display.transform.position = storedTransform.position;
			display.transform.eulerAngles = storedTransform.rotation;
			display.transform.localScale = storedTransform.scale;
		} else {
			StoredTransform storedTransform = LocationTransforms[locationName];
			display.transform.position = storedTransform.position;
			display.transform.eulerAngles = storedTransform.rotation;
			display.transform.localScale = storedTransform.scale;
		}
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
