using UnityEngine;
using System.Collections;

public class RangedAttackSpeedDrop : MonoBehaviour {

	private float buff = 0.1f;

	void OnTriggerEnter(Collider other){
		if(other.gameObject.name == "White" || other.gameObject.name == "KillerT"){
			GameObject.Find ("GameController").GetComponent<DropScript> ().buffRangedAttackSpeed (buff);
			//if(other.gameObject.GetComponent<PlayerMovement>().enabled == true){
				//if(0>=(other.gameObject.GetComponentInChildren<Shooter>().timeBetweenBullets - GameObject.Find ("GameController").GetComponent<DropScript> ().getRangedAttackSpeed() - GameObject.Find ("GameController").GetComponent<DropScript> ().getRangedAttackSpeed())){
					other.gameObject.GetComponentInChildren<Shooter>().timeBetweenBullets -= GameObject.Find ("GameController").GetComponent<DropScript> ().getRangedAttackSpeed();
				//}
			//}
			Debug.Log ("Ranged attack speed " + buff.ToString());
			Destroy (this.gameObject, 0f);
		}
	}
}
