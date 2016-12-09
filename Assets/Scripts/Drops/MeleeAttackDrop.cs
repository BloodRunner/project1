﻿using UnityEngine;
using System.Collections;

public class MeleeAttackDrop : MonoBehaviour {

	private float buff = 3f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
			GameObject.Find ("GameController").GetComponent<DropScript> ().buffMeleeAttack (buff);
			if(other.gameObject.GetComponent<PlayerMovement>().enabled == true)
				other.GetComponent<WhiteController>().bodyStats.power += buff;;
			Debug.Log ("Attack " + buff.ToString());
			Destroy (this.gameObject, 0.5f);
		}
	}
}