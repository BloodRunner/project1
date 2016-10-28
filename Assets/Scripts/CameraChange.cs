using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraChange : MonoBehaviour {

	private IEnumerator coroutine;
	public Camera topCamera; 
	public Camera followCamera;

	void OnMouseDown(){
		topCamera.enabled = false;
		followCamera.enabled = true;
		StartCoroutine (coroutine);
	}



	public IEnumerator followThis(){
		while(true){
			followCamera.transform.position = new Vector3(this.transform.position.x, 3, this.transform.position.z);
			yield return null;
		}
	}

	void Start(){
		coroutine = followThis ();
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera>();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			StopCoroutine (coroutine);
			followCamera.enabled = false;
			topCamera.enabled = true;
		}

	}

	public void stopCamera (){
		StopCoroutine (coroutine);
		followCamera.enabled = false;
		topCamera.enabled = true;
	}
}
