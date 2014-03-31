using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class QJSController : MonoBehaviour {
	
	public GameObject defaultModel;
	
	private string qjsScript;
	private string coherentjsScript;
	private List<string> vendorScripts = new List<string>();
	private AppManager appManager;
	private EnvironmentManager environmentManager;
	private List<Coherent.UI.BoundEventHandle> boundEvents = new List<Coherent.UI.BoundEventHandle>();
	
	DisplayController displayController;
	
	#region Initialization
	void Start () {
		displayController = GetComponent<DisplayController>();
		displayController.View.Listener.ReadyForBindings += HandleReadyForBindings;
		
		qjsScript = (Resources.Load("Q.js") as TextAsset).text;
		coherentjsScript = (Resources.Load("coherent.js") as TextAsset).text;
		
		vendorScripts.Add((Resources.Load("jquery.js") as TextAsset).text);
		vendorScripts.Add((Resources.Load("underscore-min.js") as TextAsset).text);
		
		displayController.View.Listener.NavigateTo += HandleNavigateTo;
		
		appManager = GameObject.Find("/AppManager").GetComponent<AppManager>();
		environmentManager = GameObject.Find("/EnvironmentManager").GetComponent<EnvironmentManager>();
	}
	#endregion
	
	#region Events
	void HandleReadyForBindings (int frameId, string path, bool isMainFrame)
	{
		
		Debug.Log("---QJS bindings" + frameId);
		
	}
	
	private void SendScroll(ScrollOptions options){
		Debug.Log("Sending scrolling of " + options.ScrollTop);
		if(GetComponent<DisplayNetworkController>().PhotonView.isMine){
			GetComponent<DisplayNetworkController>().PhotonView.RPC("ReceiveScroll", PhotonTargets.OthersBuffered, new object[]{options.ScrollTop});
		}
	}
	
	private void DisplayModel(DisplayModelOptions options){
		Debug.Log("Displaying Model " + options.Url);
		HologramController hologramController = gameObject.AddComponent<HologramController>();
		hologramController.AddAssets(defaultModel);
	}
	
	private void HandleLaunchApp(LaunchAppOptions options){
		Debug.Log("Launching " + options.Name);
		appManager.LaunchApp(options.Name, false, gameObject);
	}
	
	private void HandleSwitchEnvironment(SwitchEnvironmentOptions options){
		Debug.Log("Switching Environment to " + options.Name);
		
		environmentManager.SwitchEnvironment(options.Name);
	}
	#endregion
	
	
	#region Script Injection
	
	void HandleNavigateTo (string path)
	{
		InjectCoherent();
		InjectVendorScripts();
		InjectQJS();
		
		foreach(Coherent.UI.BoundEventHandle bind in boundEvents){
			GetComponent<DisplayController>().View.View.UnbindCall(bind);
		}
		
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("DisplayModel", (Action<DisplayModelOptions>)(DisplayModel)));
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("SendScroll", (Action<ScrollOptions>)(SendScroll)));
        boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("LaunchApp", (Action<LaunchAppOptions>)(HandleLaunchApp)));
        boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("SwitchEnvironment", (Action<SwitchEnvironmentOptions>)(HandleSwitchEnvironment)));
	}
	
	public void InjectCoherent(){
		Debug.Log("Injecting Coherent");
		
		displayController.View.View.ExecuteScript(coherentjsScript);
	}
	
	public void InjectVendorScripts(){
		foreach(string script in vendorScripts){
			displayController.View.View.ExecuteScript(script);
		}
	}
	
	public void InjectQJS(){
		Debug.Log("Injecting QJS");
		
		displayController.View.View.ExecuteScript(qjsScript);
	}
	#endregion
	
	
	
}
