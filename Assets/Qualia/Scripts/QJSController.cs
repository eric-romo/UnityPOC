using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class QJSController : MonoBehaviour {
	
	public GameObject defaultModel;
	
	public bool AutoInjectQJS = false;
	public bool AutoInjectVendor = true;
	public bool AutoInjectFixes = true;
	public bool AutoInjectCoherent = true;
	
	private string qjsScript;
	private string coherentjsScript;
	private List<string> vendorScripts = new List<string>();
	private List<string> fixScripts = new List<string>();
	private AppManager appManager;
	private EnvironmentManager environmentManager;
	private AssetManager assetManager;
	private List<Coherent.UI.BoundEventHandle> boundEvents = new List<Coherent.UI.BoundEventHandle>();
	
	private List<GameObject> appContent = new List<GameObject>();
	
	[HideInInspector]
	public bool VERBOSE = false;

	NetworkMananger networkManager;
	
	DisplayController displayController;
	
	#region Initialization
	void Start () {
		assetManager = GameObject.Find("AssetManager").GetComponent<AssetManager>();
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkMananger>();
		displayController = GetComponent<DisplayController>();
		displayController.View.Listener.ReadyForBindings += HandleReadyForBindings;
		displayController.View.Listener.NavigateTo += HandleNavigateTo;
		
		qjsScript = (Resources.Load("Q.js") as TextAsset).text;
		coherentjsScript = (Resources.Load("coherent.js") as TextAsset).text;
		
		vendorScripts.Add("window.Qualia = window.Qualia || {};");
		vendorScripts.Add((Resources.Load("jquery.js") as TextAsset).text);
		vendorScripts.Add("window.Qualia.$ = $.noConflict();");
		vendorScripts.Add((Resources.Load("underscore-min.js") as TextAsset).text);
		vendorScripts.Add("window.Qualia._ = _.noConflict();");
		
		
		fixScripts.Add("window.Qualia = window.Qualia || {};");
		fixScripts.Add("Qualia.conflicts = {window:{}};");
		fixScripts.Add(@"Qualia.$(document).on('click', 'a[target=""_blank""]', function(e){e.currentTarget.target = ""_self"";});"); //redirect _blank links to open in the current window
		fixScripts.Add(@"
			if(!window.nativeOpen){
			  window.nativeOpen = window.open;
			  window.nativeOpen.bind(window);
			  window.open = function(url, name, features, replace){
			    return window.nativeOpen(url, '_self', features, replace);
			  };
			}"); //redirect window.open to open in the current window
		
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
		modelReturn.AssetId = "";
		modelReturn.Error = "";
		
		Debug.Log("0");
		modelReturn.AssetId = assetManager.LoadModel(options.Url, options.HasMtl);
		Debug.Log("10");
		
//		try{
//			modelReturn.Id = assetManager.LoadModelAsync(options.Url, options.HasMtl);
//		} catch(Exception e){
//			modelReturn.Error = e.Message;
//		}
		
		
		return modelReturn;
	}
	
	private AddModelReturn AddModel(SOptions options){
		Debug.Log("Adding Model: " + options.String0);
		string assetId = options.String0;
		GameObject modelAsset = assetManager.GetModel(assetId);
		GameObject model = Instantiate(modelAsset) as GameObject;
		model.SetActive(true);
		model.transform.parent = gameObject.transform;
		model.transform.localPosition = Vector3.zero;
		model.transform.localRotation = Quaternion.identity;
		model.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		model.name = System.Guid.NewGuid().ToString();
		appContent.Add(model);
		
		AddModelReturn ret = new AddModelReturn();
		ret.ModelId = model.name;
		return ret;
	}
	
	private void TransformModel(ModelTransformOptions options){
		GameObject model = GameObject.Find(options.ModelId);
		
		if(options.Duration == 0){
			if(options.IsRelative){
				switch(options.TransformType){
				case "move":
					//Debug.Log("Moving model " + options.ModelId);
					model.transform.localPosition += new Vector3(-options.Z, options.Y, -options.X); 
					break;
				case "rotate": 
					//Debug.Log("Rotating model " + options.ModelId);
					model.transform.localEulerAngles += new Vector3(options.Z * 200, options.X * 200, -options.Y * 200);
					break;
				case "scale": 
					//Debug.Log("Scaling model " + options.ModelId);
					model.transform.localScale += new Vector3(-options.Z, options.Y, -options.X);
					break;
				}
			} else {
				/*switch(options.TransformType){
				case "move":
					if(VERBOSE)
						Debug.Log("Moving model to" + options.ModelId);
					model.transform.localPosition = new Vector3(-options.Z, options.Y, -options.X); 
					break;
				case "rotate": 
					if(VERBOSE)
						Debug.Log("Rotating model to" + options.ModelId);
					model.transform.localRotation = Quaternion.LookRotation(new Vector3(options.X, options.Y, options.Z), Vector3.up);
					break;
				case "scale": 
					if(VERBOSE)
						Debug.Log("Scaling model to" + options.ModelId);
					model.transform.localScale = new Vector3(-options.Z, options.Y, -options.X);
					break;
				}*/
			}
		}
	}
	
	#endregion
	
	private void DestroyApp(){
		foreach(GameObject go in appContent){
			GameObject.Destroy(go);
		}
	}
	
	#region Script Injection
	
	void HandleNavigateTo (string path)
	{
		DestroyApp();
		if(AutoInjectCoherent) InjectCoherent();
		if(AutoInjectVendor) InjectVendorScripts();
		if(AutoInjectFixes) InjectFixScripts();
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
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("addModel", (Func<SOptions, AddModelReturn>)(AddModel)));
		boundEvents.Add(GetComponent<DisplayController>().View.View.BindCall("transformModel", (Action<ModelTransformOptions>)(TransformModel)));
	}
	
	public void InjectCoherent(){
		Debug.Log("Injecting Coherent");
		
		displayController.View.View.ExecuteScript(coherentjsScript);
	}
	
	public void InjectFixScripts(){
		foreach(string script in fixScripts){
			displayController.View.View.ExecuteScript(script);
		}
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
