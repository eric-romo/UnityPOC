using UnityEngine;
using System.Collections;

public class TextureReference : MonoBehaviour {
	
	TextureCapture ReferenceTextureCapture;
	
	private GameObject screen;
	
	private DisplayManager displayManager;
	
	// Use this for initialization
	void Start () {
		displayManager = GameObject.Find("/DisplayManager").GetComponent<DisplayManager>();
		GameObject referenceDisplay = displayManager.Displays[0];
		ReferenceTextureCapture = referenceDisplay.GetComponent<TextureCapture>();
		
		screen = transform.Find("Screen").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		screen.renderer.material.mainTexture = ReferenceTextureCapture.CaptureTexture;
	}
}
