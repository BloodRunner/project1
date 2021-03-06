﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraChange : MonoBehaviour {

	private IEnumerator coroutine;
	public Camera topCamera; 
	public Camera followCamera;
	public float zoom;

	void OnMouseDown(){
		startFollow ();
	}



	public IEnumerator followThis(){
		while(true){
			if(zoom >=8){
				zoom = 8;
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
		zoom = 3f;
		coroutine = followThis ();
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera>();
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
		this.GetComponent<PlayerMovement> ().enabled = false;
	}

	public void startFollow(){
		this.GetComponent<PlayerMovement> ().enabled = true;
		topCamera.enabled = false;
		followCamera.enabled = true;
		StartCoroutine (coroutine);
	}
}
