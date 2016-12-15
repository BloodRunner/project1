﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraChange : MonoBehaviour {

	private IEnumerator coroutine;
	public Camera topCamera; 
	public Camera followCamera;
	public BloodFlow bf;
	public Light highlightPLayer;
	private float zoom;
	private bool isOn;
	private GameObject[] boxes;

	void OnMouseDown(){
		if (followCamera.enabled == false) {
			startFollow ();
		} else {
			/*
			if (this.GetComponent<Shooter> () != null) {
				Debug.Log ("Activate shooter to " + name + "." + tag);
				this.GetComponent<Shooter> ().enabled = true;

			}*/
			if (this.GetComponentInChildren<Shooter> () != null) {
				Debug.Log ("Activate shooter in children to " + name + "." + tag);
				this.GetComponentInChildren<Shooter>().enabled = true;
			}
		}

	}

	public IEnumerator followThis(){
		while(true){
			if(zoom >=14){
				zoom = 14;
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
		boxes = GameObject.FindGameObjectsWithTag ("directional");
		bf = this.gameObject.GetComponent<BloodFlow> ();
		this.gameObject.GetComponent<PlayerMovement> ().enabled = false;
		isOn = false;
		zoom = 8f;
		coroutine = followThis ();
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera>();
		highlightPLayer = this.GetComponent<Light> ();
	}

	/*void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			if(isOn == false){
				stopCamera ();
			}
		}
	}*/

	public void stopCamera (){
		for(int i = 0; i < boxes.Length; i++){
			boxes [i].gameObject.SetActive (false);
		}
		//bool wasIBound = this.GetComponent<BloodFlow> ().amIBound ();
		//Vector3 dest = this.GetComponent<BloodFlow> ().getMyDestV ();
		StopCoroutine (coroutine);
		followCamera.enabled = false;
		topCamera.enabled = true;
		highlightPLayer.enabled = false;
		isOn = false;
		debuffPlayer ();
		Camera.SetupCurrent (topCamera);
		//this.GetComponent<BloodFlow> ().stopPlayer ();
		this.GetComponent<PlayerMovement> ().enabled = false;
		if (this.GetComponentInChildren<Shooter> () != null) {
			this.GetComponentInChildren<Shooter>().enabled = false;
			this.SendMessage ("removeStats");
		}
		bf.stopPlayer ();
		/*if (wasIBound) {
			this.GetComponent<BloodFlow> ().destOveride (dest);
			this.GetComponent<BloodFlow> ().setMyBound (true);

		}*/
	}

	public void startFollow(){
		for(int i = 0; i < boxes.Length; i++){
			boxes [i].gameObject.SetActive (true);
		}
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
		highlightPLayer.range = 0.3f;
		bf.startPlayer ();
		isOn = true;
		Camera.SetupCurrent (followCamera);
		//this.GetComponent<BloodFlow> ().startPlayer ();
		StartCoroutine (coroutine);
		buffPlayer ();
	}
	/*void OnMouseOver() {
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
	}*/

	public bool getIsOn(){
		return isOn;
	}

	public void buffPlayer(){
		this.gameObject.GetComponent<WhiteController> ().bodyStats.defense += GameObject.Find ("GameController").GetComponent<DropScript> ().getDefence ();
		this.gameObject.GetComponent<WhiteController> ().bodyStats.power += GameObject.Find ("GameController").GetComponent<DropScript> ().getMeleeAttack ();
		this.gameObject.GetComponent<WhiteController> ().bodyStats.health += GameObject.Find ("GameController").GetComponent<DropScript> ().getBonusHealth ();
		this.gameObject.GetComponentInChildren<Shooter>().damagePerShot += (int)GameObject.Find ("GameController").GetComponent<DropScript> ().getRangeAttack ();
		this.gameObject.GetComponentInChildren<Shooter>().timeBetweenBullets -= GameObject.Find ("GameController").GetComponent<DropScript> ().getRangedAttackSpeed ();
	}

	public void debuffPlayer(){
		this.gameObject.GetComponent<WhiteController> ().bodyStats.defense -= GameObject.Find ("GameController").GetComponent<DropScript> ().getDefence ();
		this.gameObject.GetComponent<WhiteController> ().bodyStats.power -= GameObject.Find ("GameController").GetComponent<DropScript> ().getMeleeAttack ();
		this.gameObject.GetComponent<WhiteController> ().bodyStats.health -= GameObject.Find ("GameController").GetComponent<DropScript> ().getBonusHealth ();
		this.gameObject.GetComponentInChildren<Shooter>().damagePerShot -= (int)GameObject.Find ("GameController").GetComponent<DropScript> ().getRangeAttack ();
		this.gameObject.GetComponentInChildren<Shooter>().timeBetweenBullets += GameObject.Find ("GameController").GetComponent<DropScript> ().getRangedAttackSpeed ();
	}
}
