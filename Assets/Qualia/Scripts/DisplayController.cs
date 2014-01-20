﻿using UnityEngine;
using System.Collections;

public class DisplayController : MonoBehaviour {
	#region Inspector Variables
	public float MouseBorderRatio = 1.05f;
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
	#endregion
	
	#region Private Variables
	private Vector2 lastDisplayMouse = Vector2.zero;
	private VirtualCursor virtualCursor;
	#endregion
	
	// Use this for initialization
	void Awake () {
		Handle = transform.FindChild("Handle").gameObject;
		Cursor = transform.FindChild("Cursor").gameObject;
		View = GetComponentInChildren<CoherentUIView>();
		Rigidbody = GetComponent<Rigidbody>();
		virtualCursor = GetComponent<VirtualCursor>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Focused){
			/*Vector3 normalizedMouse = Input.mousePosition;
			normalizedMouse.x /= Screen.width;
			normalizedMouse.y /= Screen.height;*/
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
			//Debug.Log("UR: " + UR + " UL: " + UL + " LR: " + LR);
			
			bool isOverHandle = UR || UL || LR;
			
			if(!isOverHandle){
				View.SetMousePosition(Mathf.FloorToInt(viewMouse.x), Mathf.FloorToInt(viewMouse.y));
			}
			View.ReceivesInput = Focused && !isOverHandle;
			
			/*Vector2 displayMouseDelta;
			if(lastDisplayMouse == Vector2.zero){//If it hasn't been set yet.. or if we are at the top left, but thats a really tiny edge case
				displayMouseDelta = Vector2.zero;
			} else {
				displayMouseDelta = displayMouse - lastDisplayMouse;
			} 
			lastDisplayMouse = displayMouse;*/
			
			Cursor.transform.localPosition = new Vector3(-0.03138549f, displayMouse.y, -displayMouse.x);
			
			#region Dragging
			bool draggingPosition = Input.GetMouseButton(0) && isOverHandle;
			bool draggingRotation = Input.GetMouseButton(1) && isOverHandle;
			bool dragging = draggingPosition || draggingRotation;
			
			if(dragging){
				virtualCursor.Locked = true;
			} else {
				virtualCursor.Locked = false;
			}
			
			if(draggingPosition){
				transform.position += new Vector3(0, virtualCursor.Delta.y * 15, -virtualCursor.Delta.x * 15);
			}
			
			if(draggingRotation){
				Vector3 rotation = new Vector3(0, -virtualCursor.Delta.x * 100, -virtualCursor.Delta.y * 100);
				transform.Rotate(rotation);
				transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
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
		}
	}
	
	
}
