using UnityEngine;
using System.Collections;

public class VirtualCursor : MonoBehaviour {
	
	public Vector2 Sensitivity = new Vector2(0.01f, 0.01f);

	public Vector2 NormalizedPosition;
	
	public Vector2 Delta;
	
	public Vector2 Position;
	
	public bool Locked = false;
	
	public int Width = -1;
	public int Height = -1;
	
	void Awake(){
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		Delta.x = Delta.x * Mathf.Abs(Delta.x);
		Delta.y = Delta.y * Mathf.Abs(Delta.y);
		Delta.Scale(new Vector2(Time.deltaTime, Time.deltaTime));
		Delta.Scale(Sensitivity);
		
		if(!Locked){
			NormalizedPosition += Delta;
			NormalizedPosition.x = Mathf.Clamp(NormalizedPosition.x, 0, 1.0f);
			NormalizedPosition.y = Mathf.Clamp(NormalizedPosition.y, 0, 1.0f);
			
			
			if(Width > 0 && Height > 0){
				Position = Vector2.Scale (NormalizedPosition, new Vector2(Width, Height));
			}
			
			//Debug.Log("Norm x: " + NormalizedPosition.x + " y: " + NormalizedPosition.y);
			//Debug.Log("scaledDelta x: " + scaledDelta.x + " y: " + scaledDelta.y);
		}
	}
	
	void Init(){
	}
}
