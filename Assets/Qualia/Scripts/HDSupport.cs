using UnityEngine;
using System.Collections;

public class HDSupport : MonoBehaviour {
	
	public bool IsHD = false;
	
	// Use this for initialization
	void Start () {
		if(IsHD){
			Screen.orientation = ScreenOrientation.Portrait;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
