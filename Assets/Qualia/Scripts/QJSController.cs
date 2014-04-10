using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class QJSController : MonoBehaviour {
	
	public GameObject defaultModel;
	
	public bool AutoInjectQJS = false;
	public bool AutoInjectVendor = false;
	public bool AutoInjectCoherent = true;
	
	private string qjsScript;
	private string coherentjsScript;
	private List<string> vendorScripts = new List<string>();
	private AppManager appManager;
	private EnvironmentManager environmentManager;
	private List<Coherent.UI.BoundEventHandle> boundEvents = new List<Coherent.UI.BoundEventHandle>();

	NetworkMananger networkManager;
	
	DisplayController displayController;
	
	#region Initialization
	void Start () {
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkMananger>();
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
		
		//Debug.Log("---QJS bindings" + frameId);
		
	}
	
	private void SendScroll(ScrollOptions options){
		Debug.Log("Sending scrolling of " + options.ScrollTop);
		if(networkManager.Networked && GetComponent<DisplayNetworkController>().PhotonView.isMine){
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
	
	private void Ping(){
		Debug.Log("Pinged from JS!");
		GetComponent<DisplayController>().View.View.TriggerEvent("ping");
	}
	
	private void Log(SOptions options){
		Debug.Log("Log from Q.js: " + options.String0);
	}
	
	private LoadModelReturn LoadModel(LoadModelOptions options){
		Debug.Log("Loading Model: " + options.Url);
		
		LoadModelReturn modelReturn = new LoadModelReturn();
		modelReturn.Id = "";
		modelReturn.Error = "Cannot find model";
		
		
		return modelReturn;
	}
	
	#endregion
	
	#region Script Injection
	
	void HandleNavigateTo (string path)
	{
		if(AutoInjectCoherent) InjectCoherent();
		if(AutoInjectVendor) InjectVendorScripts();
		if(AutoInjectQJS) InjectQJS();
		
		foreach(Coherent.UI.BoundEventHandle bind in boundEvents){
			GetComponent<DisplayController>().View.View.UnbindCall(bind);
		}
		
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("DisplayModel", (Action<DisplayModelOptions>)(DisplayModel)));
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("SendScroll", (Action<ScrollOptions>)(SendScroll)));
        boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("LaunchApp", (Action<LaunchAppOptions>)(HandleLaunchApp)));
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("SwitchEnvironment", (Action<SwitchEnvironmentOptions>)(HandleSwitchEnvironment)));
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("log", (Action<SOptions>)(Log)));
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("ping", (Action)(Ping)));
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("loadModel", (Func<LoadModelOptions, LoadModelReturn>)(LoadModel)));
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
