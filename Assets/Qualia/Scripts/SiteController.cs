using UnityEngine;
using System.Collections;

public class SiteController : MonoBehaviour {

	private DisplayController displayController;
	private QJSController jslibraryController;
	
	public SiteScripts Scripts;
	
	public string Prefix = null;
	
	// Use this for initialization
	public void Start () {
		displayController = GetComponent<DisplayController>();
		jslibraryController = GetComponent<QJSController>();
		displayController.View.Listener.NavigateTo += HandleNavigateTo;
		displayController.View.Listener.FinishLoad += HandleFinishLoad;
	}

	void HandleNavigateTo (string path)
	{
		Debug.Log("HNT");
		if(Prefix != null && path.Contains(Prefix)){
			InjectScript(Scripts.NavigateTo);
			
			string innerScript = Scripts.Head.Replace(System.Environment.NewLine, "");
			string headScript = @"$('body').append(""<script>" + innerScript + @"</script>"")";
			Debug.Log(headScript);
			InjectScript(headScript);
			
		}
	}

	void HandleFinishLoad (int frameId, string validatedPath, bool isMainFrame, int statusCode, Coherent.UI.HTTPHeader[] headers)
	{
		Debug.Log("HFL");
		if(Prefix != null && validatedPath.Contains(Prefix)){
			InjectScript(Scripts.FinishLoad);
		}
	}
	
	void Update () {
	
	}
	
	void InjectScript(string script){
		if(script != null){
			Debug.Log("Injecting script on " + Prefix);
			displayController.View.View.ExecuteScript(script);
		}
	}
	
}

public struct SiteScripts {
	public string FinishLoad;
	public string NavigateTo;
	public string Head;
}
