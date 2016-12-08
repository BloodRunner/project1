using UnityEngine;
using System.Collections;

public class RangedAttackSpeedDrop : MonoBehaviour {

	private float buff = 0.01f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
			GameObject.Find ("GameController").GetComponent<DropScript> ().buffRangedAttackSpeed (buff);
			if(other.gameObject.GetComponent<PlayerMovement>().enabled == true){
				if((GameObject.Find ("GameController").GetComponent<DropScript> ().getRangedAttackSpeed() + buff) < 0.08f)
					other.gameObject.GetComponentInChildren<Shooter>().timeBetweenBullets -= buff;
			}
			Debug.Log ("Ranged attack speed " + buff.ToString());
			Destroy (this.gameObject, 0.5f);
		}
	}
}
