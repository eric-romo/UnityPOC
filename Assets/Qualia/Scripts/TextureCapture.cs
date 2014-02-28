using UnityEngine;
using System.Collections;
using System.IO;

public class TextureCapture : MonoBehaviour {

	public float CaptureInterval = 1.0f;

	DisplayController displayController;
	GameObject screen;
	
	private Texture2D _captureTexture;
	public Texture2D CaptureTexture{
		get{return _captureTexture;}
	}
	
	// Use this for initialization
	void Start () {
		displayController = GetComponent<DisplayController>();
		screen = transform.Find("Screen").gameObject;
		_captureTexture = new Texture2D(screen.renderer.material.mainTexture.width, screen.renderer.material.mainTexture.height);
		StartCoroutine("Capture");
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public IEnumerator Capture()
	{
		while ( true )
		{
			RenderTexture renderTexture = screen.renderer.material.mainTexture as RenderTexture;
			RenderTexture.active = renderTexture;
			_captureTexture.ReadPixels(new Rect(0, 0, _captureTexture.width, _captureTexture.height), 0, 0);
			//byte[] png = _captureTexture.EncodeToPNG();
			//File.WriteAllBytes(Application.dataPath + "/../SavedScreen_" + displayController.Location + ".png", png);
			RenderTexture.active = null;
			
			yield return new WaitForSeconds(CaptureInterval);
		}
	}
}
