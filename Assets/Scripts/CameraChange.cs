using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraChange : MonoBehaviour {

	private IEnumerator coroutine;
	public Camera topCamera; 
	public Camera followCamera;
	public Light highlightPLayer;
	private float zoom;
	private bool isOn;

	void OnMouseDown(){
		startFollow ();
	}



	public IEnumerator followThis(){
		while(true){
			if(zoom >=10){
				zoom = 10;
			}
			if(zoom <=1){
				zoom = 1;
			}
			zoom -= Input.GetAxis ("Mouse ScrollWheel");
			followCamera.transform.position = new Vector3(this.transform.position.x, zoom, this.transform.position.z);
			yield return null;
		}
	}

	void Start(){
		isOn = false;
		zoom = 3f;
		coroutine = followThis ();
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera>();
		highlightPLayer = this.GetComponent<Light> ();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			stopCamera ();
		}

	}

	public void stopCamera (){
		StopCoroutine (coroutine);
		followCamera.enabled = false;
		topCamera.enabled = true;
		highlightPLayer.enabled = false;
		isOn = false;
		Camera.SetupCurrent (topCamera);
		this.GetComponent<BloodFlow> ().stopPlayer ();
		this.GetComponent<PlayerMovement> ().enabled = false;
		if (this.GetComponentInChildren<Shooter> () != null) {
			this.GetComponentInChildren<Shooter>().enabled = false;
			this.SendMessage ("removeStats");
		}
	}

	public void startFollow(){
		this.GetComponent<PlayerMovement> ().enabled = true;
		if (this.GetComponent<Shooter> () != null) {
			Debug.Log ("Activate shooter to " + name + "." + tag);
			this.GetComponent<Shooter> ().enabled = true;

		}
		if (this.GetComponentInChildren<Shooter> () != null) {
			Debug.Log ("Activate shooter in children to " + name + "." + tag);
			this.GetComponentInChildren<Shooter>().enabled = true;
		}
		this.SendMessage ("addStats");
		topCamera.enabled = false;
		followCamera.enabled = true;
		highlightPLayer.enabled = true;
		isOn = true;
		Camera.SetupCurrent (followCamera);
		this.GetComponent<BloodFlow> ().startPlayer ();
		StartCoroutine (coroutine);
	}
	void OnMouseOver() {
		if (followCamera.enabled == false) {
			highlightPLayer.range = 3f;
			highlightPLayer.enabled = true;
		}

	}

	void OnMouseExit() {
		if (followCamera.enabled == false) {
			highlightPLayer.range = 0.3f;
			highlightPLayer.enabled = false;
		} else {
			highlightPLayer.range = 0.3f;
		}
	}

	public bool getIsOn(){
		return isOn;
	}
}
