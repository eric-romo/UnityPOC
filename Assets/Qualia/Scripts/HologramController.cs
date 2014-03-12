using UnityEngine;
using System;
using System.Collections;
using Coherent.UI.Binding;
using Holoville.HOTween;

public class HologramController : MonoBehaviour, IAppController {
	
	public GameObject Prefab;
	
	public float RotationRate = -5.0f;
	private GameObject cursor;
	
	private GameObject[] pieces;
	private GameObject dragPiece;
	
	private GameObject hologram;
	
	// Use this for initialization
	void Start () {
		cursor = transform.Find("Cursor").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(RotationRate != 0 && hologram != null){
			hologram.transform.Rotate(new Vector3(0, RotationRate * Time.smoothDeltaTime));
		}
		
	}
	
	public void AddAssets(GameObject assetPrefab){
		hologram = GameObject.Instantiate(assetPrefab) as GameObject;
		hologram.name = "Hologram";
		hologram.transform.parent = gameObject.transform;
		hologram.SetActive(true);
		
		hologram.transform.localPosition = new Vector3(-0.1f,0,0);
		hologram.transform.localEulerAngles = new Vector3(0,0,90);
		
		
		Sequence showSequence = new Sequence();
		showSequence.AppendInterval(0.01f);
		showSequence.AppendCallback( () => hologram.SetActive(false));
		showSequence.AppendInterval(0.2f);
		showSequence.AppendCallback( () => hologram.SetActive(true));
		showSequence.AppendInterval(0.01f);
		showSequence.AppendCallback( () => hologram.SetActive(false));
		showSequence.AppendInterval(0.05f);
		showSequence.AppendCallback( () => hologram.SetActive(true));
		showSequence.Play();
	}
}

