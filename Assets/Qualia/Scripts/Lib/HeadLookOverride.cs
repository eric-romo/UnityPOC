using UnityEngine;
using System.Collections;

public class HeadLookOverride : MonoBehaviour {

	private GameObject forwardDirection;
	
	public float RotationSpeed = 50.0f;
	// Use this for initialization
	void Start () {
		forwardDirection = GameObject.Find("ForwardDirection");
		OVRCameraController ovrCameraController = FindObjectOfType(typeof(OVRCameraController)) as OVRCameraController;
		ovrCameraController.FollowOrientation = forwardDirection.transform;
	}
	
	// Update is called once per frame
	void Update () {
		float friRotationSpeed = RotationSpeed * Time.deltaTime;
		if(Input.GetButton("Super Button")){
			if(Input.GetKey(KeyCode.I)){
				forwardDirection.transform.Rotate(-friRotationSpeed, 0, 0);
			}
			if(Input.GetKey(KeyCode.K)){
				forwardDirection.transform.Rotate(friRotationSpeed, 0, 0);
			}
			if(Input.GetKey(KeyCode.J)){
				forwardDirection.transform.Rotate(0, -friRotationSpeed, 0);
			}
			if(Input.GetKey(KeyCode.L)){
				forwardDirection.transform.Rotate(0, friRotationSpeed, 0);
			}
			
			Vector3 eulerAngles = forwardDirection.transform.eulerAngles;
			eulerAngles.z = 0;
			forwardDirection.transform.eulerAngles = eulerAngles;
		}
	}
}
