using UnityEngine;
using System.Collections;

/* This allows Unity UI to see class and show it
 * */

// Base stats for cells,
[System.Serializable]
public class Stats
{   // base level multiplied by ( 0 - 100% of base level )
	public float health, speed, defense, reprodRate;
	public float power; // attack if white cell or infection, oxygenation if red cell
	public float delay; // Pause before it can follow a command
	public int level;
}

// Base stats for Organ,
[System.Serializable]
public class OrganStats
{   // base level multiplied by ( 0 - 100% of base level )
	public float health, defense, regenRate;
}

public class BodyController : MonoBehaviour {
	public string myname;
	public GameController gameController;
	public Stats bodyStats; // configured max stats for the cell body;
	public OrganStats organStats; // configured max stats for the organ;
	protected Stats mybodyStats; // configured stats for the cell body;
	protected OrganStats myorganStats; // configured stats for the organ;
	const float EMPTY = -999f;
	const float MAX = 999999f;
	protected float mybodyStats_power; // new() in Start/Awake doesn't work consistently
	protected float mybodyStats_health;
	protected float mybodyStats_reprodRate;
	protected float mybodyStats_defense;
	protected float mybodyStats_speed;
	protected float mybodyStats_delay;
	public void Awake() { // Initialize my status from defaults
		//mybodyStats = new Stats();
		//myorganStats = new OrganStats();
		updateCellStats(MAX, MAX, MAX, 0f,MAX);
		//updateOrganStats(MAX, MAX,0f);
	}

    // Update cell stats for the whole body
	public void updateCellStats(float health, float speed, float defense, float reprodRate, float power) {
        if (health != EMPTY) {
	        mybodyStats_health = health; 
			if (mybodyStats_health > bodyStats.health)
                mybodyStats_health = bodyStats.health;
            if (mybodyStats_health <= 0f) {
                // Game Over!
                return;
            }
        }
        if (speed != EMPTY) {
            mybodyStats_speed = speed;
            if (mybodyStats_speed > bodyStats.speed)
                mybodyStats_speed = bodyStats.speed;
            else if (mybodyStats_speed < 0)
                mybodyStats_speed = 0;
        }
        if (defense != EMPTY) {
            mybodyStats_defense = defense;
            if (mybodyStats_defense > bodyStats.defense)
                mybodyStats_defense = bodyStats.defense;
            else if (mybodyStats_defense < 0)
                mybodyStats_defense = 0;
        }
        if (power != EMPTY) {
            mybodyStats_power = power;
            if (mybodyStats_power > bodyStats.power)
                mybodyStats_power = bodyStats.power;
            else if (mybodyStats_power < 0)
                mybodyStats_power = 0;
        }
        if ( reprodRate != EMPTY) {
            mybodyStats_reprodRate = reprodRate;
            if (mybodyStats_reprodRate < bodyStats.reprodRate )  // smaller is better
                mybodyStats_reprodRate = bodyStats.reprodRate;
			//Debug.Log (gameObject.name+" reprodRate="+reprodRate +" -> "+ bodyStats.reprodRate +"=>"+mybodyStats_reprodRate);
        }
    }

	// Update cell stats for the whole body
	public void updateOrganStats(float health, float defense, float regenRate) {
		if (health!=EMPTY) {
			myorganStats.health = health; 
			if (myorganStats.health > organStats.health)
				myorganStats.health = organStats.health;
			if (myorganStats.health <= 0f) {
				// Game Over!
				return;
			}
		}
		if (defense != EMPTY) {
			myorganStats.defense = defense;
			if (myorganStats.defense > organStats.defense)
				myorganStats.defense = organStats.defense;
			else if (myorganStats.defense < 0)
				myorganStats.defense = 0;
		}
		if ( regenRate != EMPTY) {
			myorganStats.regenRate = regenRate;
			if (myorganStats.regenRate < organStats.regenRate )  // smaller is better
				myorganStats.regenRate = organStats.regenRate;
		}
	}

    // If heart is damaged - all cells will slow down to the percentage of damage of the heart
	public void slowDownAllCells(float percentage){ 
        float speed = bodyStats.speed * percentage;
        updateCellStats(EMPTY, speed, EMPTY, EMPTY, EMPTY);
	}

	public void reduceAllCellDefense(float percent){
        float defense = bodyStats.defense * percent;
        updateCellStats(EMPTY, EMPTY, defense, EMPTY, EMPTY);
	}

    // The number of seconds before a cell divides
    public void reduceReprodRate(float reprod){
        if (reprod <= 0f) {
            reprod= 0.1f;
        }
        float reprodRate = bodyStats.reprodRate * 100/reprod;
        updateCellStats(EMPTY, EMPTY, EMPTY, reprodRate, EMPTY);
    }

}

