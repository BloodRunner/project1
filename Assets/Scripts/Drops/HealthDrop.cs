﻿using UnityEngine;
using System.Collections;

public class HealthDrop : MonoBehaviour {

	private float buff = 10f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){

				GameObject.Find ("GameController").GetComponent<DropScript> ().buffHealth (buff);
				other.GetComponent<WhiteController> ().buffHealth (buff);
				Debug.Log ("Health " + buff.ToString());
				Destroy (this.gameObject, 0f);
				//other.GetComponent<updatePlayerStats> ().enabled = true;


		}
	}
}
