using UnityEngine;
using System.Collections;
using System;

public class DisplayController : MonoBehaviour {
	#region Inspector Variables
	public float MouseBorder = 0.06f;
	public GameObject ClonePrefab;
	public bool MoveRelativeToRotation = true;
	#endregion
	
	#region Public Variables
	[HideInInspector]
	public GameObject Handle;
	[HideInInspector]
	public GameObject Cursor;
	[HideInInspector]
	public CoherentUIView View;
	[HideInInspector]
	public Rigidbody Rigidbody;
	[HideInInspector]
	public bool Dragging = false;
	
	private bool _focused = false;
	public bool Focused {
		get{
			return _focused;
		}
		set{
			_focused = value;
			View.ReceivesInput = value;
			if(value){
				Color color = Color.green;
				color.a = handleDefaultColor.a;
				Handle.renderer.material.color = color;
			} else {
				Handle.renderer.material.color = handleDefaultColor;
			}
		}
	}
	
	public string Location;
	#endregion
	
	#region Private Variables
	private Vector2 lastDisplayMouse = Vector2.zero;
	private VirtualCursor virtualCursor;
	private DisplayManager displayManager;
	private AppManager appManager;
	private EnvironmentManager environmentManager;
	private Color handleDefaultColor;
	#endregion
	
	#region Initialization
	void Awake () {
		Handle = transform.FindChild("Handle").gameObject;
		handleDefaultColor = Handle.renderer.material.color;
		Cursor = transform.FindChild("Cursor").gameObject;
		View = GetComponentInChildren<CoherentUIView>();
		Rigidbody = GetComponent<Rigidbody>();
		virtualCursor = GetComponent<VirtualCursor>();
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		appManager = GameObject.Find("/AppManager").GetComponent<AppManager>();
		environmentManager = GameObject.Find("/EnvironmentManager").GetComponent<EnvironmentManager>();
		displayManager.Displays.Add(gameObject);
	}
	
	void Start(){
		View.Listener.ViewCreated += HandleViewCreated;
		View.Listener.ReadyForBindings += HandleReadyForBindings;
		//AttachWebRTC();
	}
	#endregion
	
	#region Event Listeners
	
	void HandleViewCreated (Coherent.UI.View view){
		//AttachWebRTC();
	}

	void HandleReadyForBindings (int frameId, string path, bool isMainFrame){
		//Debug.Log("--------Ready For Bindings" + gameObject.name);
		
		virtualCursor.Width = View.Width;
		virtualCursor.Height = View.Height;
	}
	#endregion
	
	#region Update
	void Update () {
		if(Focused){
			Vector2 normalizedMouse = virtualCursor.NormalizedPosition;
			normalizedMouse.y = Utilities.mapRange(0, 1, -MouseBorder, 1f, normalizedMouse.y);
			
			Vector2 displayMouse = normalizedMouse;
			float displayWidth = 1.6f;
			float displayHeight = 0.9f;
			displayMouse.y = 1 - displayMouse.y;
			displayMouse.x *= displayWidth;
			displayMouse.y *= displayHeight;
			displayMouse.x -= displayWidth / 2;
			displayMouse.y -= displayHeight / 2;
			
			Vector2 viewMouse;
			viewMouse.x = normalizedMouse.x;
			viewMouse.y = normalizedMouse.y;
			viewMouse.x *= View.Width;
			viewMouse.y *= View.Height;
			
			bool isOverTopBar = viewMouse.y < 0;
			
			bool isOverCloseButton = isOverTopBar && normalizedMouse.x > 1 - MouseBorder;
			bool isOverNewButton = isOverTopBar && normalizedMouse.x < MouseBorder;
			bool isOverBackButton = isOverTopBar && normalizedMouse.x < MouseBorder * 2 && normalizedMouse.x > MouseBorder * 1;
			bool isOverForwardButton = isOverTopBar && normalizedMouse.x < MouseBorder * 3 && normalizedMouse.x > MouseBorder * 2;
			bool isOverReloadButton = isOverTopBar && normalizedMouse.x < MouseBorder * 4 && normalizedMouse.x > MouseBorder * 3;
			bool isOverMoveHandle = isOverTopBar && !isOverCloseButton && !isOverNewButton && !isOverBackButton && !isOverForwardButton && !isOverReloadButton;
			
			//Debug.Log("isOverTopBar" + isOverTopBar + "isOverCloseButton" + isOverCloseButton + " isOverNewButton" + isOverNewButton + " isOverMoveHandle" + isOverMoveHandle);
			//Debug.Log("ViewMouse: " + viewMouse + "displayMouse: " + displayMouse + "normalizedMouse: " + normalizedMouse);
			
			if(!isOverTopBar){
				View.SetMousePosition(Mathf.FloorToInt(viewMouse.x), Mathf.FloorToInt(viewMouse.y));
			}
			View.ReceivesInput = Focused && !isOverMoveHandle;
			
			Cursor.transform.localPosition = new Vector3(-0.03138549f, displayMouse.y, -displayMouse.x);
			
			#region Dragging
			bool draggingPosition = Input.GetMouseButton(0) && isOverMoveHandle;
			bool draggingRotation = Input.GetMouseButton(1) && isOverMoveHandle;
			Dragging = draggingPosition || draggingRotation;
			
			if(Dragging){
				virtualCursor.Locked = true;
			} else {
				virtualCursor.Locked = false;
			}
			
			if(draggingPosition){
				if(MoveRelativeToRotation || Input.GetKey(KeyCode.LeftShift)){
					transform.position += transform.TransformDirection(new Vector3(0, -virtualCursor.Delta.y * 8, -virtualCursor.Delta.x * 8));
				} else {
					transform.position += new Vector3(0, -virtualCursor.Delta.y * 8, -virtualCursor.Delta.x * 8);
				}
			}
			
			if(draggingRotation){
				Vector3 rotation = new Vector3(0, -virtualCursor.Delta.x * 100, virtualCursor.Delta.y * 100);
				transform.Rotate(rotation);
				transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
			}
			
			if((draggingPosition || draggingRotation) && Location != null){
				displayManager.Locations[Location] = null;
				Location = null;
			}
			#endregion
			
			#region Buttons
			if(isOverCloseButton && Input.GetMouseButtonDown(0)){
				displayManager.Close(gameObject);
			}
			if(isOverNewButton && Input.GetMouseButtonDown(0)){
				displayManager.CreateDisplay(name + "(Clone)", displayManager.Homepage, "front");
			}
			if(isOverBackButton && Input.GetMouseButtonDown(0)){
				View.View.ExecuteScript("history.back();");
			}
			if(isOverForwardButton && Input.GetMouseButtonDown(0)){
				View.View.ExecuteScript("history.forward();");
			}
			if(isOverReloadButton && Input.GetMouseButtonDown(0)){
				View.View.ExecuteScript("location.reload(true);");
			}
			if(isOverNewButton && Input.GetMouseButtonDown(1)){
				displayManager.CreateDisplay(name + "(Clone)", View.Page, "front");
			}
			#endregion
			
			#region Zooming
			if(isOverMoveHandle){
				float scrollDelta =  Input.GetAxis("Mouse ScrollWheel");
				float scaleDelta = 1 - scrollDelta * 0.05f;
				Vector3 scale = transform.localScale;
				scale.Scale(new Vector3(scaleDelta, scaleDelta, scaleDelta));
				transform.localScale = scale;
			}
			#endregion;
			
			#region Closing
			if(Focused && Input.GetButton("Super Button")){
				if(Input.GetKey(KeyCode.W))
					displayManager.Close(gameObject);
				
			}
			#endregion
		}
	}
	#endregion
	
	#region Navigation
	private string urlWaitingToLoad;
	public void LoadUrl (string url)
	{
		if(View.View != null){
			View.Page = url;
		} else {
			urlWaitingToLoad = url;
			View.OnViewCreated += DelayedLoadUrl;
		}
	}
	
	void DelayedLoadUrl (Coherent.UI.View view)
	{
		LoadUrl(urlWaitingToLoad);
	}
	#endregion
	
	#region WebRTC
	void AttachWebRTC(){
		View.Listener.RequestMediaStream += (request) => {
			var devices = request.Devices;
			request.Respond(new uint[]{0});
			return;
			for (var i = 0; i != devices.Length; ++i)
			{
				if (devices[i].Type == Coherent.UI.MediaStreamType.MST_DEVICE_AUDIO_CAPTURE)
				{
					if (i > 0)
					{
						// respond with first video and last audio device
						Debug.Log(string.Format("Using audio device {0} {1}", devices[i-1].DeviceId, devices[i-1].Name));
						Debug.Log(string.Format("Using video device {0} {1}", devices[i].DeviceId, devices[i].Name));
						request.Respond(new uint[] { (uint)i - 1});
						return;
					}
					else
					{
						Debug.LogError("No audio devices detected?");
					}
				}
			}
			Debug.LogError("No audio or video devices detected?");
		};
	}
	#endregion
	
	
}
