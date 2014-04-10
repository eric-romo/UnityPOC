using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedDial : MonoBehaviour {
	private List<string> urls = new List<string>{
		"http://localhost:8080/",
		"http://gavanwilhite.com/test/qualia3d/72ea6abe-c44a-49cb-9d3c-3861675eeb0d/q-js/test/test.html",
		"https://c9.io/gavanq/q-js/workspace/test/test.html",
		@"http://www.netflix.com/WiPlayer?movieid=70140361&trkid=7728649&tctx=-99%2C-99%2Ceeb0f1be-0d82-4082-b63f-fbdde3fefb31-2201399",
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
