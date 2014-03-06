using UnityEngine;
using System;
using System.Collections;
using Coherent.UI.Binding;
using Holoville.HOTween;

public class HoloChessController : MonoBehaviour {
	
	private GameObject holoChessPrefab;
	private GameObject holoChess;
	private GameObject cusor;
	
	private delegate void CoherentDelegate();
	
	private GameObject whiteQueen;
	
	// Use this for initialization
	void Start () {
		cusor = transform.Find("Cursor").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0) && whiteQueen != null){
			Vector3 queenPosition = cusor.transform.position;
			Debug.Log("queen pos: " + queenPosition);
			
			Vector3 queenLocalPosition = whiteQueen.transform.localPosition;
			whiteQueen.transform.position = queenPosition;
			
			queenLocalPosition.x = whiteQueen.transform.localPosition.x;
			queenLocalPosition.z = whiteQueen.transform.localPosition.z;
			
			whiteQueen.transform.localPosition = queenLocalPosition;
		}
	}
	
	public void Init(GameObject holoChessPrefab){
		this.holoChessPrefab = holoChessPrefab;
		GetComponent<DisplayController>().View.View.BindCall("Play", (Action)(ShowBoard));
		//PlayHoloChess();
	
	}
	
	public void ShowBoard(){
		Debug.Log("Playing!");
		holoChess = GameObject.Instantiate(holoChessPrefab) as GameObject;
		holoChess.name = "HoloChess";
		holoChess.transform.localPosition = new Vector3(-0.15f, 1.26f, -0.27f);
		holoChess.SetActive(true);
		holoChess.transform.parent = gameObject.transform;
		
		holoChess.transform.Find("ChessBoard/BoardTop").gameObject.renderer.sharedMaterial.renderQueue = 2999;
		holoChess.transform.Find("ChessBoard/BoardBottom").gameObject.renderer.sharedMaterial.renderQueue = 2999;
		
		Sequence showSequence = new Sequence();
		showSequence.AppendInterval(0.01f);
		showSequence.AppendCallback( () => holoChess.SetActive(false));
		showSequence.AppendInterval(0.2f);
		showSequence.AppendCallback( () => holoChess.SetActive(true));
		showSequence.AppendInterval(0.01f);
		showSequence.AppendCallback( () => holoChess.SetActive(false));
		showSequence.AppendInterval(0.05f);
		showSequence.AppendCallback( () => holoChess.SetActive(true));
		showSequence.AppendInterval(0.01f);
		showSequence.AppendCallback(NewGame);
		showSequence.Play();
	}
	
	public void NewGame(){
		whiteQueen = GameObject.Find("ChessPieceQueenWhite").gameObject;
	}
	
	private void FindAllPices(){
		
	}
}
