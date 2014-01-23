using UnityEngine;	
using System.Collections;
using Coherent.UI;

public class Navigation : MonoBehaviour {
	
	BrowserView browserView;
	DisplayController displayController;
	
	void Start () {
		displayController = GetComponent<DisplayController>();
		browserView = (BrowserView) displayController.View.View;
	}
	
	void Update () {
		browserView = (BrowserView) displayController.View.View;
		if(displayController.Focused && Input.GetButton("Super Button")){
			if(Input.GetKey(KeyCode.LeftArrow))
				browserView.GoBack();
			if(Input.GetKey(KeyCode.RightArrow))
				browserView.GoForward();
				
		}
	}
}
