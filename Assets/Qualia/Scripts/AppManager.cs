﻿using UnityEngine;
using System.Collections;

public class AppManager : MonoBehaviour {

	public AppManifest[] Apps;
	
	private DisplayManager displayManager;
	
	public void Awake(){
		displayManager = GameObject.Find("DisplayManager").GetComponent<DisplayManager>();
	}
	
	public void LaunchApp(string name, bool launchInNewDisplay){
		for(int i = 0; i < Apps.Length; i++){
			AppManifest appManifest = Apps[i];
			if(Apps[i].Name == name){
				GameObject display;
				if(launchInNewDisplay){
					display = displayManager.Create(appManifest.Name, appManifest.URL, "spawn");
				} else {
					display = displayManager.FocusedDisplay;
					//TODO clean up any components on focusedDisplay
					display.name = appManifest.Name;
					display.GetComponent<DisplayController>().LoadUrl(appManifest.URL);
				}
				
				if(appManifest.ControllerScriptName != null){
					IAppController appController = display.AddComponent(appManifest.ControllerScriptName) as IAppController;
					if(appManifest.UnityAssetsPrefab){
						appController.AddAssets(appManifest.UnityAssetsPrefab);
					}
				}
				
				displayManager.MoveDisplayToLocation(display, appManifest.DefaultLocation, true);
				
				return;
			}
		}
	}
}