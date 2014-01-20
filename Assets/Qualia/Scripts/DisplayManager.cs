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
		}
	}
	CoherentUIView _focusedView = null;
	CoherentUIView FocusedView {
		get{
			return _focusedView;
		}
	}
	
	// Use this for initialization
	void Start () {
		Displays.AddRange(GameObject.FindGameObjectsWithTag("display"));
		FocusedDisplay = Displays[0];
		FocusedView.ReceivesInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		ProcessInput();
	}
	
	private void ProcessInput(){
		FocusedView.ReceivesInput = true;
		FocusedView.SetMousePosition((int)Input.mousePosition.x, FocusedView.Height - (int)Input.mousePosition.y);
		
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
			}*/
		}
		
		
		// Activate input processing for the view below the mouse cursor
		/*RaycastHit hitInfo;
		if (Physics.Raycast(m_MainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
		{
			//Debug.Log (hitInfo.collider.name);
			
			CoherentUIView viewComponent = hitInfo.collider.gameObject.GetComponent(typeof(CoherentUIView)) as CoherentUIView;
			if (viewComponent == null)
			{
				viewComponent = hitInfo.collider.gameObject.GetComponentInChildren(typeof(CoherentUIView)) as CoherentUIView;
			}
			
			if (viewComponent != null && !viewComponent.ClickToFocus)
			{
				viewComponent.ReceivesInput = true;
				viewComponent.SetMousePosition(
					(int)(hitInfo.textureCoord.x * viewComponent.Width),
					(int)(hitInfo.textureCoord.y * viewComponent.Height));
			}
		}
	}*/
}
