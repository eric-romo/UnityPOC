using UnityEngine;
using System.Collections;

public class CoherentViewMaterialSetter : MonoBehaviour {
	public Material material;
	
	void Awake() {
		GetComponent<CoherentUIView>().OnViewCreated += SetMaterial;
	}
	
	void SetMaterial(Coherent.UI.View view) {
		renderer.material.shader = material.shader;
	}
}