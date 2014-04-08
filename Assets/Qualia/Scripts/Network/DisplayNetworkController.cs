using UnityEngine;
using System.Collections;
using Coherent.UI.Binding;

[RequireComponent(typeof(PhotonView))]
public class DisplayNetworkController : MonoBehaviour {
	
	private DisplayManager displayManager;
	public PhotonView PhotonView;
	private NetworkMananger networkManager;
	
	void Awake(){
		PhotonView = GetComponent<PhotonView>();
		displayManager = GameObject.Find("DisplayManager").GetComponent<DisplayManager>();
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkMananger>();
		
		if(!networkManager.Networked){
			Destroy(this);
			Destroy(GetComponent<TransformLerp>());
			Destroy(GetComponent<PhotonView>());
			Destroy(transform.Find("Cursor").gameObject.GetComponent<TransformLerp>());
			Destroy(transform.Find("Cursor").gameObject.GetComponent<PhotonView>());
		}
	}

	[RPC]
	public void Init(string name, string url, string locationName){
		Debug.Log("Display Initing over network. name: " + name + " url: " + url + " locationName: " + locationName);
		
		gameObject.GetComponent<DisplayController>().LoadUrl(url);
		gameObject.name = name;
		
		displayManager.MoveDisplayToLocation(gameObject, locationName, false);
	}
	
	public void Update(){
		bool left = Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0);
		bool middle = Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1);
		bool right = Input.GetMouseButtonDown(2) || Input.GetMouseButtonUp(2);
		
		string button = left ? "left" : middle ? "middle" : right ? "right" : null;
		
		bool up = Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2);
		bool down = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
		
		string type = up ? "up" : down ? "down" : null;
		
		if(up || down){
			Vector2 position = GetComponent<VirtualCursor>().Position;
			int x = (int) position.x;
			int y = GetComponent<VirtualCursor>().Height - (int) position.y;
			
			Debug.Log("Sending mouse rpc, button: " + button + " type: " + type + " x: " + x + " y: " + y);
			PhotonView.RPC("MouseEvent", PhotonTargets.Others, new object[]{button, type, x, y});
		}
	}
	
	[RPC]
	public void MouseEvent(string button, string type, int x, int y){ //TODO: Replace with JS version. Scroll position is too flakey for rapid movement
		Coherent.UI.MouseEventData.MouseButton cButton;
		Coherent.UI.MouseEventData.EventType cType;
		
		switch(button){
			case "left":
				cButton = Coherent.UI.MouseEventData.MouseButton.ButtonLeft;
			break;
			case "middle":
				cButton = Coherent.UI.MouseEventData.MouseButton.ButtonMiddle;
				break;
			case "right":
				cButton = Coherent.UI.MouseEventData.MouseButton.ButtonRight;
				break;
			default:
				cButton = Coherent.UI.MouseEventData.MouseButton.ButtonNone;
				break;
		}
		
		switch(type){
			case "up":
				cType = Coherent.UI.MouseEventData.EventType.MouseUp;
				break;
			case "down":
				cType = Coherent.UI.MouseEventData.EventType.MouseDown;
				break;
			default:
				cType = Coherent.UI.MouseEventData.EventType.MouseMove;
				break;
		}
		
		
		Coherent.UI.MouseEventData mouseEvent = new Coherent.UI.MouseEventData();
		
		mouseEvent.Button = cButton;
		mouseEvent.Type = cType;
		
		mouseEvent.X = x;
		mouseEvent.Y = y;
		
		GetComponent<DisplayController>().View.View.MouseEvent(mouseEvent);
		
		/*mouseEvent.Type = Coherent.UI.MouseEventData.EventType.MouseUp;
		
		GetComponent<DisplayController>().View.View.MouseEvent(mouseEvent);*/
	}
	
	[RPC]
	public void ReceiveScroll(int scrollTop){
		Debug.Log("Receiving scrolling of " + scrollTop);
		ScrollOptions options = new ScrollOptions();
		options.ScrollTop = scrollTop;
		GetComponent<DisplayController>().View.View.ExecuteScript("Q.Sync.receiveScroll({ScrollTop: "+ scrollTop +"})");//HACK
		//GetComponent<DisplayController>().View.View.TriggerEvent("ReceiveScroll", options);
		
	}
}
