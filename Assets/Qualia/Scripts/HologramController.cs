using UnityEngine;
using System;
using System.Collections;
using Coherent.UI.Binding;
using Holoville.HOTween;

public class HologramController : MonoBehaviour {
	
	public GameObject Prefab;
	
	public float RotationRate = 0.0f;
	private GameObject cursor;
	
	private GameObject[] pieces;
	private GameObject dragPiece;
	
	// Use this for initialization
	void Start () {
		cursor = transform.Find("Cursor").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetMouseButtonDown(0) && pieces != null && pieces.Length > 0){
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
			Debug.Log("dragPiece pos: " + piecePosition);
			
			Vector3 pieceLocalPosition = dragPiece.transform.localPosition;
			dragPiece.transform.position = piecePosition;
			
			pieceLocalPosition.x = dragPiece.transform.localPosition.x;
			pieceLocalPosition.z = dragPiece.transform.localPosition.z;
			
			dragPiece.transform.localPosition = pieceLocalPosition;
		}*/
		if(RotationRate != 0){
			gameObject.transform.Rotate(new Vector3(0, RotationRate * Time.smoothDeltaTime));
		}
		
	}
}
	
