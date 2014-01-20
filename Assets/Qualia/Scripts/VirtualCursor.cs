using UnityEngine;
using System.Collections;

public class VirtualCursor : MonoBehaviour {

	public bool Acceleration = false;
	
	public Vector2 Sensitivity = new Vector2(0.01f, 0.01f);

	public Vector2 NormalizedPosition;
	
	public Vector2 Delta;
	
	public Vector2 Position;
	
	public bool Locked = false;
	
	public int Width = -1;
	public int Height = -1;
	
	void Awake(){
	}

	void Start () {
		
	}
	
	void Update () {
		Delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		if(Acceleration){
			Delta.x = Delta.x * Mathf.Abs(Delta.x);
			Delta.y = Delta.y * Mathf.Abs(Delta.y);
		}
		Delta.Scale(new Vector2(Time.deltaTime, Time.deltaTime));
		Delta.Scale(Sensitivity);
		
		if(!Locked){
			NormalizedPosition += Delta;
			NormalizedPosition.x = Mathf.Clamp(NormalizedPosition.x, 0, 1.0f);
			NormalizedPosition.y = Mathf.Clamp(NormalizedPosition.y, 0, 1.0f);
			
			
			if(Width > 0 && Height > 0){
				Position = Vector2.Scale (NormalizedPosition, new Vector2(Width, Height));
			}
		}
	}
	
	void Init(){
	}
}
