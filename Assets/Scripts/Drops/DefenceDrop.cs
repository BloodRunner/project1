using UnityEngine;
using System.Collections;

public class DefenceDrop : MonoBehaviour {

	private float buff = 3f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
				GameObject.Find ("GameController").GetComponent<DropScript> ().buffDefence (buff);
				other.GetComponent<WhiteController>().buffDefence (buff);
				Debug.Log ("Defense" + buff.ToString());
				Destroy (this.gameObject, 0f);
				//other.GetComponent<updatePlayerStats> ().enabled = true;
		}
	}
}
