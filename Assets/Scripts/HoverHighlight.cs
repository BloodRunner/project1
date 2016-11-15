using UnityEngine;
using System.Collections;

public class HoverHighlight : MonoBehaviour {

	public Light highlightPLayer;

	void Start(){
		highlightPLayer = this.GetComponent<Light> ();

	}

	void OnMouseOver() {
		if (this.GetComponent<CameraChange> ().followCamera == false) {
			highlightPLayer.range = 3f;
			highlightPLayer.enabled = true;
		}

	}

	void OnMouseExit() {
		if (this.GetComponent<CameraChange> ().enabled == false) {
			highlightPLayer.range = 0.3f;
			highlightPLayer.enabled = false;
		} else {
			highlightPLayer.range = 0.3f;
		}
	}
}
