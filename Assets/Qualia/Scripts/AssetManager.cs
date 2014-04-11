using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
		string assetHandle = assetGuid.ToString();
		
		
		StartCoroutine(LoadModelAsync(url, assetHandle, hasMtl));
		
		Debug.Log("9");
		return assetHandle;
	}
	
	public IEnumerator LoadModelAsync(string url, string assetHandle, bool hasMtl = false){
		Debug.Log("2");
		ObjReader.ObjData objData = ObjReader.use.ConvertFileAsync(url, hasMtl, DefaultMaterial);
		while(!objData.isDone){
			yield return null;
		}
		
		GameObject[] gameObjects = objData.gameObjects;
		Debug.Log("3");
		
		if(gameObjects == null){
			throw new Exception("No models found in OBJ");
		}
			
		GameObject root = gameObjects[0];
			
		Debug.Log("Loaded GameObjects: " + gameObjects);
		//root.SetActive(false);
		models[assetHandle] = root;
		
		
		Debug.Log("4");
		//callback(modelId);
	}
	
	public IEnumerable LoadingModel(ObjReader.ObjData objData){
		while(!objData.isDone){
			yield return 0;
		}
	}
	
	public GameObject GetModel(string assetHandle){
		GameObject model = models[assetHandle];
		return model;
	}
}
