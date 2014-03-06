using UnityEngine;
using System.Collections;

public class HoloChessController : MonoBehaviour {
	
	private GameObject holoChessPrefab;
	private GameObject holoChess;
	
	private delegate void CoherentDelegate();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Init(GameObject holoChessPrefab){
		this.holoChessPrefab = holoChessPrefab;
		GetComponent<DisplayController>().View.View.BindCall("Play", (CoherentDelegate)(PlayHoloChess));
		PlayHoloChess();
	
	}
	
	public void PlayHoloChess(){
		Debug.Log("Playing!");
		holoChess = GameObject.Instantiate(holoChessPrefab) as GameObject;
		holoChess.transform.localPosition = new Vector3(-0.15f, 1.26f, -0.27f);
		holoChess.SetActive(true);
		//holoChess.transform.parent = gameObject.transform;
	}
}
