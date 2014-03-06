using UnityEngine;
using System.Collections;

public class RotateMe : MonoBehaviour {
	public float rotationXSpeed;
	public float rotationYSpeed;
	public float rotationZSpeed;
	public bool AxisX = false;
	public bool AxisY = true;
	public bool AxisZ = false;

	// Update is called once per frame
	void Update () {
	
			if(AxisX == true){
				transform.Rotate(Vector3.right * Time.deltaTime * rotationXSpeed);
			}
			if(AxisY == true){
				transform.Rotate(Vector3.up * Time.deltaTime * rotationYSpeed);
			}
			if(AxisZ == true){
				transform.Rotate(Vector3.forward * Time.deltaTime * rotationZSpeed);
			}
		
	}
}
