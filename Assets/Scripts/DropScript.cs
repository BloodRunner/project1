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
	private float bonusMeleeAttackPower =0;
	private float bonusRangedAttackPower=0;
	private float bonusLifeSpan=0;
	private float bonusRangedAttackSpeed=0;
	private float bonusHealth=0;
	private float bonusDefence=0;
	public GameObject[] drops = new GameObject[7];

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

	public void buffMeleeAttack(float val){
		bonusMeleeAttackPower += val;
	}
	public void buffRangedAttack(float val){
		bonusRangedAttackPower += val;
	}
	public void buffRangedAttackSpeed(float val){
		bonusRangedAttackSpeed += val;
	}
	public void buffHealth(float val){
		bonusHealth += val;
	}
	public void buffDefence(float val){
		bonusDefence += val;
	}
	public void buffLifeSpan(float val){
		bonusLifeSpan += val;
	}
	public float getMeleeAttack(){
		return bonusMeleeAttackPower;
	}public float getRangeAttack(){
		return bonusRangedAttackPower;
	}public float getRangedAttackSpeed(){
		return bonusRangedAttackSpeed;
	}public float getDefence(){
		return bonusDefence;
	}public float getLifeSpan(){
		return bonusLifeSpan;
	}public float getBonusHealth(){
		return bonusHealth;
	}
}
