using UnityEngine;
using System.Collections;

public class HealthBonusDrop : MonoBehaviour {

	private float buff = 20f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
				other.GetComponent<WhiteController> ().updateHealthStats (buff);
				Debug.Log ("Healed " + buff.ToString());
				//other.GetComponent<updatePlayerStats> ().enabled = true;
			Destroy (this.gameObject, 0f);
		}
	}
}
