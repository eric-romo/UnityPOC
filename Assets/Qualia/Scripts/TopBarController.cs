using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopBarController : MonoBehaviour {
	
	public GameObject CloseButton;
	public GameObject NewButton;
	public GameObject BackButton;
	public GameObject ForwardButton;
	public GameObject ReloadButton;
	
	public List<GameObject> Buttons;
	
	public GameObject Handle;
	
	public Color Deselected = new Color(0.2f, 0.2f, 0.2f);
	public Color Selected = new Color(0, 0, 1f);
	
	private DisplayController displayController;
	
	void Awake(){
		CloseButton = transform.Find("CloseButton").gameObject;
		NewButton = transform.Find("NewButton").gameObject;
		BackButton = transform.Find("BackButton").gameObject;
		ForwardButton = transform.Find("ForwardButton").gameObject;
		ReloadButton = transform.Find("ReloadButton").gameObject;
		
		Handle = transform.Find("Handle").gameObject;
		
		Buttons = new List<GameObject>{CloseButton, NewButton, BackButton, ForwardButton, ReloadButton};
		
		displayController = GetComponent<DisplayController>();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	#region Highlighting

	public void ClearHighlights(){
	
		foreach(GameObject button in Buttons){
			SetButtonHighlight(button, false);
		}
		
		Handle.renderer.material.color = displayController.Focused ? displayController.FocusedColor : displayController.UnfocusedColor;
		
	}
	
	public void SetButtonHighlight(GameObject button, bool isHighlighted){
		MeshRenderer[] meshRenderers = button.transform.GetComponentsInChildren<MeshRenderer>();
		for(int i = 0; i < meshRenderers.Length; i++){
			if(isHighlighted){
				meshRenderers[i].material.color = Selected;
			} else {
				meshRenderers[i].material.color = Deselected;
			}
		}
	}
	
	public void HighlightBar(){
		Handle.renderer.material.color = Selected;
	}
	
	#endregion
}
