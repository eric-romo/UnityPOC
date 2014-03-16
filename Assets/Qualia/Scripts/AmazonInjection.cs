using UnityEngine;
using System.Collections;

public class AmazonInjection : MonoBehaviour {

	DisplayController displayController;
	QJSController jslibraryController;
	// Use this for initialization
	void Start () {
		displayController = GetComponent<DisplayController>();
		jslibraryController = GetComponent<QJSController>();
		//displayController.View.Listener.NavigateTo += NavigateTo;
		displayController.View.Listener.FinishLoad += HandleFinishLoad;;
	}

	void HandleFinishLoad (int frameId, string validatedPath, bool isMainFrame, int statusCode, Coherent.UI.HTTPHeader[] headers)
	{
		if(validatedPath.Contains("http://www.amazon.com/")){
			Debug.Log("On Amazon!");
			
			jslibraryController.InjectCoherent();
			jslibraryController.InjectQJS();
			Inject3DPreview();
		}
	}

	/*void NavigateTo (string path)
	{
		//Debug.Log("NavigateTo!");
		if(path.Contains("http://www.amazon.com/")){
			Debug.Log("On Amazon!");
			Inject3DPreview();
		}
	}*/
	// Update is called once per frame
	void Update () {
	
	}
	
	void Inject3DPreview(){
		string code = @"
		$li = jQuery('<li id=""3d-preview"" class=""a-spacing-small item""><span class=""a-list-item""> <span class=""a-declarative"" ""><span class=""a-button a-button-thumbnail a-button-toggle""><span class=""a-button-inner""><input class=""a-button-input"" type=""submit""><span class=""a-button-text""><img src=""http://placehold.it/40x40/ffffff&text=3D""></img></span></span></span></span></span></li>');
		$li.click(function(){Q.Holo.displayModel('camera.obj')});
		if(jQuery('#3d-preview').length > 0){
			return;
		}
		jQuery(jQuery("".a-button-list"")[0]).append($li);";
		
		displayController.View.View.ExecuteScript(code);
		
	}
}
