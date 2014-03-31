using UnityEngine;
using System.Collections;
using System;

public class DisplayController : MonoBehaviour {
	#region Inspector Variables
	public float MouseBorderRatio = 1.05f;
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
		displayManager.Displays.Add(gameObject	);
		Debug.Log(View);
		Debug.Log(View.Listener);
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
		Debug.Log("--------Ready For Bindings" + gameObject.name);
		
		virtualCursor.Width = View.Width;
		virtualCursor.Height = View.Height;
	}
	#endregion
	
	#region Update
	void Update () {
		if(Focused){
			Vector2 normalizedMouse = virtualCursor.NormalizedPosition;
			normalizedMouse.x *= MouseBorderRatio;
			normalizedMouse.y *= MouseBorderRatio;
			normalizedMouse.x = Mathf.Min(normalizedMouse.x, MouseBorderRatio);
			normalizedMouse.y = Mathf.Min(normalizedMouse.y, MouseBorderRatio);
			
			Vector2 displayMouse = normalizedMouse;
			float displayWidth = 1.6f;
			float displayHeight = 0.9f;
			displayMouse.x *= displayWidth;
			displayMouse.y *= displayHeight;
			displayMouse.x -= displayWidth / 2;
			displayMouse.y -= displayHeight / 2;
			
			Vector2 viewMouse;
			viewMouse.x = normalizedMouse.x;
			viewMouse.y = normalizedMouse.y;
			viewMouse.x *= View.Width;
			viewMouse.y *= View.Height;
			viewMouse.y = View.Height - viewMouse.y;
			
			/* What quadrant, centered on the upper right corner of the display, is the cursor over */
			bool UR = viewMouse.x > View.Width && viewMouse.y < 0;
			bool UL = viewMouse.x > View.Width / MouseBorderRatio && viewMouse.y < 0;
			bool LR = viewMouse.x > View.Width && viewMouse.y < View.Height * MouseBorderRatio - View.Height;
			
			bool isOverHandle = UR || UL || LR;
			
			if(!isOverHandle){
				View.SetMousePosition(Mathf.FloorToInt(viewMouse.x), Mathf.FloorToInt(viewMouse.y));
			}
			View.ReceivesInput = Focused && !isOverHandle;
			
			Cursor.transform.localPosition = new Vector3(-0.03138549f, displayMouse.y, -displayMouse.x);
			
			#region Dragging
			bool draggingPosition = Input.GetMouseButton(0) && isOverHandle;
			bool draggingRotation = Input.GetMouseButton(1) && isOverHandle;
			Dragging = draggingPosition || draggingRotation;
			
			if(Dragging){
				virtualCursor.Locked = true;
			} else {
				virtualCursor.Locked = false;
			}
			
			if(draggingPosition){
				if(MoveRelativeToRotation || Input.GetKey(KeyCode.LeftShift)){
					transform.position += transform.TransformDirection(new Vector3(0, virtualCursor.Delta.y * 15, -virtualCursor.Delta.x * 15));
				} else {
					transform.position += new Vector3(0, virtualCursor.Delta.y * 15, -virtualCursor.Delta.x * 15);
				}
			}
			
			if(draggingRotation){
				Vector3 rotation = new Vector3(0, -virtualCursor.Delta.x * 100, -virtualCursor.Delta.y * 100);
				transform.Rotate(rotation);
				transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
			}
			
			if((draggingPosition || draggingRotation) && Location != null){
				displayManager.Locations[Location] = null;
				Location = null;
			}
			
			
			#endregion
			
			#region Cloning
			if(Input.GetMouseButtonDown(2) || Input.GetButton("Super Button") && Input.GetKeyDown(KeyCode.C)){
				Vector3 position = transform.position;
				position.x -= 0.5f;
				Vector3 scale = transform.localScale;
				//scale = scale * 0.5f;
				GameObject clone = GameObject.Instantiate(ClonePrefab, position, transform.rotation) as GameObject;
				clone.transform.localScale = scale;
				GameObject duplicateRenderCamera = clone.GetComponent<DisplayController>().View.transform.GetChild(0).gameObject;
				GameObject.Destroy(duplicateRenderCamera);
				displayManager.Displays.Add(clone);
				displayManager.FocusedDisplay = clone;
			}
			#endregion
			
			#region Zooming
			if(isOverHandle){
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
