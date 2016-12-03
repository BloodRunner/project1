using UnityEngine;
using System.Collections;

public class DropScript : MonoBehaviour {
	private float healDropRate = 5;
	private float meleeAttackDropRate = 1;
	private float rangedAttackDropRate = 1;
	private float healthIncreaseDropRate = 2;
	private float defenceIncreaseDropRate = 2;
	private float rangeAttackSpeedDropRate = 1;
	public GameObject[] drops = new GameObject[6];

	public void drop(GameObject me, float value){

		float rand = Random.Range (0f, 100f);
		float drop = 0;
		if(rand <= (healDropRate*value)){
			Instantiate (drops[0], me.transform.position, Quaternion.identity);
		}else if( rand <= (drop + (meleeAttackDropRate*value))){

		}else if( rand <= (drop + (rangedAttackDropRate*value))){

		}else if( rand <= (drop + (healthIncreaseDropRate*value))){

		}else if( rand <= (drop + (defenceIncreaseDropRate*value))){

		}else if( rand <= (drop + (rangeAttackSpeedDropRate*value))){

		}
	}
}
