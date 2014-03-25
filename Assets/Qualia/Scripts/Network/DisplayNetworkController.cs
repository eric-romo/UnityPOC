using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class DisplayNetworkController : MonoBehaviour {
	
	private DisplayManager displayManager;
	
	void Awake(){
		displayManager = GameObject.Find("DisplayManager").GetComponent<DisplayManager>();
	}

	[RPC]
	public void Init(string name, string url, string locationName){
		Debug.Log("Display Initing over network. name: " + name + " url: " + url + " locationName: " + locationName);
		
		gameObject.GetComponent<DisplayController>().LoadUrl(url);
		gameObject.name = name;
		
		displayManager.MoveDisplayToLocation(gameObject, locationName, false);
	}
}
