using UnityEngine;
using System.Collections;

public class DisplayController : MonoBehaviour {

	public float MouseBorderRatio = 1.05f;
	
	[HideInInspector]
	public GameObject Handle;
	[HideInInspector]
	public GameObject Cursor;
	[HideInInspector]
	public CoherentUIView View;
	
	private bool _focused = false;
	public bool Focused {
		get{
			return _focused;
		}
		set{
			_focused = value;
			View.ReceivesInput = value;
			if(value){
				Handle.renderer.material.color = Color.green;
			} else {
				Handle.renderer.material.color = Color.black;
			}
		}
	}
	
	// Use this for initialization
	void Awake () {
		Handle = transform.FindChild("Handle").gameObject;
		Cursor = transform.FindChild("Cursor").gameObject;
		View = GetComponentInChildren<CoherentUIView>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Focused){
			Vector3 normalizedMouse = Input.mousePosition;
			normalizedMouse.x /= Screen.width;
			normalizedMouse.y /= Screen.height;
			normalizedMouse.x *= MouseBorderRatio;
			normalizedMouse.y *= MouseBorderRatio;
			normalizedMouse.x = Mathf.Min(normalizedMouse.x, MouseBorderRatio);
			normalizedMouse.y = Mathf.Min(normalizedMouse.y, MouseBorderRatio);
			
			Vector3 displayMouse = normalizedMouse;
			float displayWidth = 1.6f;
			float displayHeight = 0.9f;
			displayMouse.x *= displayWidth;
			displayMouse.y *= displayHeight;
			displayMouse.x -= displayWidth / 2;
			displayMouse.y -= displayHeight / 2;
			displayMouse.z = -displayMouse.x;
			displayMouse.x = -0.03138549f;
			Cursor.transform.localPosition = displayMouse;
			
			Vector2 viewMouse;
			viewMouse.x = normalizedMouse.x;
			viewMouse.y = normalizedMouse.y;
			viewMouse.x *= View.Width;
			viewMouse.y *= View.Height;
			viewMouse.y = View.Height - viewMouse.y;
			
			//Debug.Log("viewMouse x: " + viewMouse.x + " y: " + viewMouse.y);
			
			
			
			/* What quadrant, centered on the upper right corner of the display, is the cursor over */
			bool UR = viewMouse.x > View.Width && viewMouse.y < 0;
			bool UL = viewMouse.x > View.Width / MouseBorderRatio && viewMouse.y < 0;
			bool LR = viewMouse.x > View.Width && viewMouse.y < View.Height * MouseBorderRatio - View.Height;
			//Debug.Log("UR: " + UR + " UL: " + UL + " LR: " + LR);
			
			bool isOverHandle = UR || UL || LR;
			
			if(!isOverHandle){
				View.SetMousePosition(Mathf.FloorToInt(viewMouse.x), Mathf.FloorToInt(viewMouse.y));
			}
			View.ReceivesInput = Focused && !isOverHandle;
			
			if(isOverHandle){
				if(Input.GetMouseButtonDown(0)){
					Debug.Log("Handle Clicked! Button 0");
				}
				if(Input.GetMouseButtonDown(1)){
					Debug.Log("Handle Clicked! Button 1");
				}
				if(Input.GetMouseButtonDown(2)){
					Debug.Log("Handle Clicked! Button 2");
				}
				
				float scrollDelta =  Input.GetAxis("Mouse ScrollWheel");
				Debug.Log("Input.GetAxis(scroll);" + scrollDelta);
				float scaleDelta = 1 - scrollDelta * 0.05f;
				Vector3 scale = transform.localScale;
				scale.Scale(new Vector3(scaleDelta, scaleDelta, scaleDelta));
				transform.localScale = scale;
			}
		}
	}
	
	
}
