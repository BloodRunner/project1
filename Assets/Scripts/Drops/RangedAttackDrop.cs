﻿using UnityEngine;
using System.Collections;

public class RangedAttackDrop : MonoBehaviour {

	private float buff = 3f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){

				GameObject.Find ("GameController").GetComponent<DropScript> ().buffRangedAttack (buff);
				other.GetComponent<WhiteController> ().buffRangedAttack (buff);
				Debug.Log ("Ranged Attack " + buff.ToString());
				Destroy (this.gameObject, 0f);
				//other.GetComponent<updatePlayerStats> ().enabled = true;


		}
	}
}
