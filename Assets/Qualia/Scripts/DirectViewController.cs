using UnityEngine;
using System.Collections;

public class DirectViewController : MonoBehaviour {
	
	private GameObject directCamera;
	private GameObject	ovrView;
	
	private DisplayController displayController;
	private DisplayManager displayManager;
	
	private string lastLocation;
	
	private bool _isDirect = false;
	public bool IsDirect{
		get{
			return _isDirect;
		}
		set{
			_isDirect = value;
			
			directCamera.SetActive(value);
			ovrView.SetActive(!value);
			
			/*if(value){
				lastLocation = displayController.Location;
				displayManager.MoveDisplayToLocation(gameObject, "direct", false);
			} else {
				if(lastLocation != null){
					displayManager.MoveDisplayToLocation(gameObject, lastLocation, false);
				} else {
					displayManager.MoveDisplayToLocation(gameObject, "front", false);
				}
			}*/
		}
	}
	
	void Awake() {
		directCamera = transform.Find("DirectCamera").gameObject;
		ovrView = GameObject.Find("LocalOVRView");
		displayController = GetComponent<DisplayController>();
		displayManager = GameObject.Find("DisplayManager").GetComponent<DisplayManager>();
		IsDirect = false;
		
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(displayController.Focused && Input.GetButton("Super Button")){
			if(Input.GetKeyDown(KeyCode.D))
				IsDirect = !IsDirect;
		}
	}
	
}
