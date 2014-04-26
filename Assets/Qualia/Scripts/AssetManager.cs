using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Coherent.UI.Binding;//VERY IMPORTANT

public class AssetManager : MonoBehaviour {
	
	public Material DefaultMaterial;
	
	private Dictionary<string, GameObject> models = new Dictionary<string, GameObject>(); 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
//	public string LoadModel(string url, bool hasMtl = false){ //Returns a model ID so that we can reference it via JS
////		GameObject[] gameObjects;
////		
//		StartCoroutine(
//		
//		if(gameObjects == null){
//			throw new Exception("No models found in OBJ");
//		}
//		
//		GameObject root = gameObjects[0];
//		/*foreach(GameObject go in gameObjects){
//			go.SetActive(false);
//		}*/
//		Debug.Log("Loaded GameObjects: " + gameObjects);
//		root.SetActive(false);
//		
//		models.Add(root);
//		
//		return models.IndexOf(root).ToString();
//		return "";
//	}

	public string LoadModel(string url, bool hasMtl = false){ //This returns a Id before loading happens, because of the absurd nature of coroutines we can't do anything about this AFAIK. No way of waiting for a coroutine in a syncronous function.
		Debug.Log("1");
		
		System.Guid assetGuid = System.Guid.NewGuid();
		string assetId = assetGuid.ToString();
		
		
		StartCoroutine(LoadModelAsync(url, assetId, hasMtl));
		
		Debug.Log("9");
		return assetId;
	}
	
	public IEnumerator LoadModelAsync(string url, string assetId, bool hasMtl = false){
		Debug.Log("2");
		ObjReader.ObjData objData = ObjReader.use.ConvertFileAsync(url, hasMtl, DefaultMaterial);
		while(!objData.isDone){
			yield return null;
		}
		
		GameObject[] gameObjects = objData.gameObjects;
		Debug.Log("3");
		
		if(gameObjects == null || gameObjects.Length == 0){
			string error = "Could not find/load .obj file";
			GameObject.Find("DisplayManager").GetComponent<DisplayManager>().FocusedDisplayController.View.View.TriggerEvent<string, string>("modelLoaded", assetId, error);
			//throw new Exception("No models found in OBJ");
		} else {
			GameObject root = gameObjects[0];
			
			Debug.Log("Loaded GameObjects: " + gameObjects);
			root.SetActive(false);
			models[assetId] = root;
			
			
			Debug.Log("4");
			
			GameObject.Find("DisplayManager").GetComponent<DisplayManager>().FocusedDisplayController.View.View.TriggerEvent<string, string>("modelLoaded", assetId, null);
		}
	}
	
	public GameObject GetModel(string assetId){
		GameObject model = models[assetId];
		return model;
	}
}
