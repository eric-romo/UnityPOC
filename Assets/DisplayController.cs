using UnityEngine;
using System.Collections;

public class DisplayController : MonoBehaviour {
	
	public GameObject Handle;
	public GameObject Cursor;
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
			
			Vector3 displayMouse = normalizedMouse;
			//Cursor.transform.localPosition = displayMouse;
			Vector2 viewMouse;
			viewMouse.x = normalizedMouse.x;
			viewMouse.y = normalizedMouse.y;
			viewMouse.x *= View.Width;
			viewMouse.y *= View.Height;
			viewMouse.y = View.Height - viewMouse.y;
			View.SetMousePosition(Mathf.FloorToInt(viewMouse.x), Mathf.FloorToInt(viewMouse.y));
		}
	}
	
	
}
