using UnityEngine;
using System.Collections;
using System;

public class QJSController : MonoBehaviour {
	
	public GameObject defaultModel;
	
	private string qjsCode;
	private string coherentjsCode;
	
	DisplayController displayController;
	// Use this for initialization
	void Start () {
		displayController = GetComponent<DisplayController>();
		displayController.View.Listener.ReadyForBindings += HandleReadyForBindings;
		
		qjsCode = (Resources.Load("Q.js") as TextAsset).text;
		coherentjsCode = (Resources.Load("coherent.js") as TextAsset).text;
	}
	
	void HandleReadyForBindings (int frameId, string path, bool isMainFrame)
	{
		GetComponent<DisplayController>().View.View.BindCall("DisplayModel", (Action<DisplayModelOptions>)(DisplayModel));
	}
	
	private void DisplayModel(DisplayModelOptions options){
		Debug.Log("Displaying Model " + options.Url);
		HologramController hologramController = gameObject.AddComponent<HologramController>();
		hologramController.AddAssets(defaultModel);
	}
	
	public void InjectCoherent(){
		Debug.Log("Injecting Coherent");
		
		displayController.View.View.ExecuteScript(coherentjsCode);
	}
	
	public void InjectQJS(){
		Debug.Log("Injecting QJS");
		
		displayController.View.View.ExecuteScript(qjsCode);
	}
	
	
	
}
