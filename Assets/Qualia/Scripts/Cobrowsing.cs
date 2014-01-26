using UnityEngine;
using System.Collections;
using Coherent.UI;

public class Cobrowsing : MonoBehaviour {
	
	CoherentUIView cuiView;
	View view;
	BrowserView browserView;
	BrowserViewListener browserViewListener;
	DisplayController displayController;
	
	string currentURL;
	
	// Use this for initialization
	void Start () {
		displayController = GetComponent<DisplayController>();
		cuiView = displayController.View;
		view  = cuiView.View;
		browserView = (BrowserView) browserView;
		//cuiView.Listener.FinishLoad += FinishLoad;
		cuiView.Listener.NavigateTo += NavigateTo;
	}

	void NavigateTo (string path)
	{
		Debug.Log("NavigateTo!");
		InjectTogetherJS();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void FinishLoad(int frameId, string validatedPath, bool isMainFrame, int statusCode, HTTPHeader[] headers){
		Debug.Log("currentURL: " + currentURL + " cuiView.Page: ");
		if(currentURL == cuiView.Page){
			return;
		}
		Debug.Log("FinishedLoad!");
		InjectTogetherJS();
		currentURL = cuiView.Page;
	}
	
	void InjectTogetherJS(){
		string makeBodyBlue = @"
		document.getElementsByTagName('body')[0].style.backgroundColor = 'blue';";
		
		//string loadTogetherJS = MakeScriptToLoadLibrary(@"https://togetherjs.com/togetherjs-min.js");
		string runTogetherJS = @"
			TogetherJSConfig_findRoom = 'coherent_test_room';
			
			var callback = function(){
				TogetherJS();
			};
			
			var script = document.createElement('script');
	
			script.type = 'text/javascript';
			script.src = 'https://togetherjs.com/togetherjs-min.js';
			
			script.onreadystatechange = callback;
    		script.onload = callback;
    		
   			var head = document.getElementsByTagName('head')[0];
   			head.appendChild(script);
   			
		";
		view  = cuiView.View;
		view.ExecuteScript(runTogetherJS);
	}
	
	string MakeScriptToLoadLibrary(string url){
		return @"
			var js = document.createElement('script');
	
			js.type = 'text/javascript';
			js.src = " + url + @";
			
			document.body.appendChild(js);
			";
	}
}
