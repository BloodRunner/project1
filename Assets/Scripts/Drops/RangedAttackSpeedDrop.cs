using UnityEngine;
using System.Collections;

public class RangedAttackSpeedDrop : MonoBehaviour {

	private float buff = 0.1f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
			
				GameObject.Find ("GameController").GetComponent<DropScript> ().buffRangedAttackSpeed (buff);
				other.GetComponent<WhiteController> ().buffRangedAttackSpeed (buff);
				Debug.Log ("Ranged attack speed " + buff.ToString());
				Destroy (this.gameObject, 0f);
				//other.GetComponent<updatePlayerStats> ().enabled = true;


		}
	}
}
