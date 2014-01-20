using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayManager : MonoBehaviour {
	
	List<GameObject> Displays = new List<GameObject>();
	
	GameObject _focusedDisplay = null;
	GameObject FocusedDisplay {
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
	DisplayController FocusedDisplayController {
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
	
	public Camera CursorCamera = null;
	
	void Awake(){
		CursorCamera = GameObject.Find("CameraRight").GetComponent<Camera>();
	}
	
	// Use this for initialization
	void Start () {
		Displays.AddRange(GameObject.FindGameObjectsWithTag("Display"));
		if(Displays.Count > 0){
			FocusedDisplay = Displays[0];
		}
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput();
	}
	
	private void ProcessInput(){
		
		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Tab)){
			int index = (Displays.IndexOf(FocusedDisplay) + 1);
			index %= Displays.Count;
			Debug.Log("Index: " + index);
			FocusedDisplay = Displays[index];
		}
		
		//FocusedView.SetMousePosition((int)Input.mousePosition.x, FocusedView.Height - (int)Input.mousePosition.y);
		
		/*var cameraView = m_MainCamera.gameObject.GetComponent<CoherentUIView>();
		if (cameraView && !cameraView.ClickToFocus)
		{
			var view = cameraView.View;
			if (view != null)
			{
				var normX = Input.mousePosition.x / cameraView.Width;
				var normY = 1 - Input.mousePosition.y / cameraView.Height;
				if (normX >= 0 && normX <= 1 && normY >= 0 && normY <= 1)
				{
					view.IssueMouseOnUIQuery(normX, normY);
					view.FetchMouseOnUIQuery();
					if (view.IsMouseOnView())
					{
						cameraView.ReceivesInput = true;
						cameraView.SetMousePosition((int)Input.mousePosition.x, cameraView.Height - (int)Input.mousePosition.y);
						return;
					}
				}
			}
		}*/
		
		
		// Activate input processing for the view below the mouse cursor
		/*RaycastHit hitInfo;
		if (Physics.Raycast(CursorCamera.transform.position, CursorCamera.transform.forward, out hitInfo))
		{
			Debug.DrawRay(CursorCamera.transform.position, CursorCamera.transform.forward, Color.green);
			Debug.Log (hitInfo.collider.name);
			
			CoherentUIView viewComponent = hitInfo.collider.gameObject.GetComponent(typeof(CoherentUIView)) as CoherentUIView;
			if (viewComponent == null)
			{
				viewComponent = hitInfo.collider.gameObject.GetComponentInChildren(typeof(CoherentUIView)) as CoherentUIView;
			}
			
			if (viewComponent != null && !viewComponent.ClickToFocus)
			{
				FocusedDisplay = viewComponent.gameObject.transform.parent.gameObject;
				viewComponent.SetMousePosition(
					(int)(hitInfo.textureCoord.x * viewComponent.Width),
					(int)(hitInfo.textureCoord.y * viewComponent.Height));
				Debug.Log("HitInfo: x: " + hitInfo.textureCoord.x + " y: " + hitInfo.textureCoord.y);
			}
		}*/
	}
}
