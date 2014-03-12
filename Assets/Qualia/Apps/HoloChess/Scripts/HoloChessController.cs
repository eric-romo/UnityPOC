using UnityEngine;
using System;
using System.Collections;
using Coherent.UI.Binding;
using Holoville.HOTween;

public class HoloChessController : MonoBehaviour, IAppController {
	
	private GameObject holoChessPrefab;
	private GameObject holoChess;
	private GameObject cursor;
	
	private GameObject[] pieces;
	private GameObject dragPiece;
	
	private CoherentUIView view;
	
	// Use this for initialization
	void Start () {
		cursor = transform.Find("Cursor").gameObject;
		view = GetComponent<DisplayController>().View;
		view.Listener.ReadyForBindings += HandleReadyForBindings;
	}

	void HandleReadyForBindings (int frameId, string path, bool isMainFrame)
	{
		GetComponent<DisplayController>().View.View.BindCall("Play", (Action)(OnPlay));
		GetComponent<DisplayController>().View.View.BindCall("NewGame", (Action)(OnNewGame));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && pieces != null && pieces.Length > 0){
			foreach(GameObject piece in pieces){
				GameObject pieceMesh = piece.transform.GetChild(0).gameObject;
				if(cursor.renderer.bounds.Intersects(pieceMesh.renderer.bounds)){
					Debug.Log("Grabbing piece: " + pieceMesh.name);
					dragPiece = piece;
				}
			}
		}
		
		if(Input.GetMouseButtonUp(0)){
			dragPiece = null;
		}
		
		if(Input.GetMouseButton(0) && dragPiece != null){
			Vector3 piecePosition = cursor.transform.position;
			//Debug.Log("dragPiece pos: " + piecePosition);
			
			Vector3 pieceLocalPosition = dragPiece.transform.localPosition;
			dragPiece.transform.position = piecePosition;
			
			pieceLocalPosition.x = dragPiece.transform.localPosition.x;
			pieceLocalPosition.z = dragPiece.transform.localPosition.z;
			
			dragPiece.transform.localPosition = pieceLocalPosition;
		}
	}
	
	public void AddAssets(GameObject holoChessPrefab){
		this.holoChessPrefab = holoChessPrefab;
	
	}
	
	private void OnPlay(){
		Debug.Log("Playing!");
		ShowBoard(true);
	}
	
	public void ShowBoard(bool animate = true){
		holoChess = GameObject.Instantiate(holoChessPrefab) as GameObject;
		holoChess.name = "HoloChess";
		holoChess.transform.parent = gameObject.transform;
		holoChess.transform.localPosition = new Vector3(-0.01913294f, -0.009256481f, 0.02416356f);
		holoChess.transform.localScale = new Vector3(0.4081632f, 0.4081632f, 0.4081632f);
		holoChess.transform.localEulerAngles = new Vector3(0,0,90f);
		holoChess.SetActive(true);
		
		holoChess.transform.Find("ChessBoard/BoardTop").gameObject.renderer.sharedMaterial.renderQueue = 2999;
		holoChess.transform.Find("ChessBoard/BoardBottom").gameObject.renderer.sharedMaterial.renderQueue = 2999;
		
		
		if(animate){
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
			showSequence.AppendCallback(FindAllPices);
			showSequence.Play();
		} else {
			holoChess.SetActive(true);
			FindAllPices();
		}
	}
	
	public void OnNewGame(){
		GameObject.Destroy(holoChess);
		Sequence newGameSequence = new Sequence();
		newGameSequence.AppendInterval(0.01f);
		newGameSequence.AppendCallback( () => ShowBoard(false) );
		newGameSequence.Play();
		pieces =  null;
	}
	
	private void FindAllPices(){
		pieces = GameObject.FindGameObjectsWithTag("Draggable");
	}
}
