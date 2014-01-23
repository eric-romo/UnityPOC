using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedDial : MonoBehaviour {
	private List<string> urls = new List<string>{
		"www.google.com",
		"http://www.chromeexperiments.com/webgl/",
		"http://youtu.be/Rsf35tugWkg?html5=1&autoplay=1&hd=1",
		"http://recode.net/",
		"http://www.script-tutorials.com/demos/372/index.html"
	};
	void Start () {
	
	}
	
	void Update () {
		for(int i = 0; i < urls.Count; i++){
			if(Input.GetButton("Super Button") && Input.GetKey((KeyCode)(49 + i))){
				GameObject.Find("/DisplayManager").GetComponent<DisplayManager>().FocusedDisplayController.View.Page = urls[i];
			}
		}
	}
}
