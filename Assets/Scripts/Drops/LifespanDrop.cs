﻿using UnityEngine;
using System.Collections;

public class LifespanDrop : MonoBehaviour {

	private float buff = 3f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
			GameObject.Find ("GameController").GetComponent<DropScript> ().buffLifeSpan (buff);
			other.GetComponent<WhiteController> ().buffLifeSpan (buff);
			Debug.Log ("Lifespan " + buff.ToString());
			Destroy (this.gameObject, 0f);
		}
	}
}
