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
		if(displayController.Focused && Input.GetButton("Super Button")){
			browserView = (BrowserView) displayController.View.View;
			if(Input.GetKey(KeyCode.Comma))
				browserView.GoBack();
			if(Input.GetKey(KeyCode.Period))
				browserView.GoForward();
			if(Input.GetKey(KeyCode.R))
				displayController.View.View.ExecuteScript("location.reload(true);");
		}
	}
}
