using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour {

	public EnvironmentManifest[] Environments;
	
	private DisplayManager displayManager;
	
	public void Awake(){
		displayManager = GameObject.Find("DisplayManager").GetComponent<DisplayManager>();
	}
	
	public void SwitchEnvironment(string name){
		for(int i = 0; i < Environments.Length; i++){
			EnvironmentManifest envManifest = Environments[i];
			if(envManifest.Name == name){
				GameObject.Destroy(GameObject.Find("Environment"));
				GameObject environment = GameObject.Instantiate(envManifest.UnityAssetsPrefab) as GameObject;
				environment.name = "Environment";
				
				//HACK
				if(name == "Timbuk2"){
					RenderSettings.ambientLight = new Color(1, 1, 1);
				} else {
					RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.2f);
				}
				return;
			}
		}
	}
}
