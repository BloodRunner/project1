using UnityEngine;
using System.Collections;

public class DropScript : MonoBehaviour {
	private float healDropRate = 5;
	private float meleeAttackDropRate = 1;
	private float rangedAttackDropRate = 1;
	private float healthIncreaseDropRate = 2;
	private float defenceIncreaseDropRate = 2;
	private float rangeAttackSpeedDropRate = 1;
	private float lifeSpan = 2;
	public GameObject[] drops = new GameObject[6];

	public void drop(GameObject me, float value){

		float rand = Random.Range (0f, 100f);
		float drop = 0;
		if(rand <= (healDropRate*value)){
			Instantiate (drops[0], me.transform.position, Quaternion.identity);
		}else if( rand <= (drop + (meleeAttackDropRate*value))){
			Instantiate (drops[1], me.transform.position, Quaternion.identity);
		}else if( rand <= (drop + (rangedAttackDropRate*value))){
			Instantiate (drops[2], me.transform.position, Quaternion.identity);
		}else if( rand <= (drop + (healthIncreaseDropRate*value))){
			Instantiate (drops[3], me.transform.position, Quaternion.identity);
		}else if( rand <= (drop + (defenceIncreaseDropRate*value))){
			Instantiate (drops[4], me.transform.position, Quaternion.identity);
		}else if( rand <= (drop + (rangeAttackSpeedDropRate*value))){
			Instantiate (drops[5], me.transform.position, Quaternion.identity);
		}else if( rand <= (drop + (lifeSpan*value))){
			Instantiate (drops[6], me.transform.position, Quaternion.identity);
		}
	}
}
