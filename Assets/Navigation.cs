using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftArrow)){
			CoherentUIView view = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>().FocusedDisplayController.View;
			//Do back with BrowserView when availiable
		}
	}
}
