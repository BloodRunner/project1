using UnityEngine;
using System.Collections;

public class HealthBonusDrop : MonoBehaviour {

	private float buff = 20f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
			if (other.gameObject.GetComponent<PlayerMovement> ().enabled == true) {
				other.GetComponent<WhiteController> ().updateHealthStats (buff);
				Destroy (this.gameObject, 0.5f);
			}
			Debug.Log ("Healed " + buff.ToString());
			//other.GetComponent<updatePlayerStats> ().enabled = true;

		}
	}
}
