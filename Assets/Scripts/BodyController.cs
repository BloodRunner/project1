using UnityEngine;
using System.Collections;

/* This allows Unity UI to see class and show it
 * */
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}
// Base stats for cells,
[System.Serializable]
public class Stats
{
	// base level multiplied by ( 0 - 100% of base level )
	public float health, speed, defense, reprodRate;
	// regen==reprod)
	public float power;
	// attack if white cell or infection, oxygenation if red cell
	public float delay;
	// Pause before it can follow a command
	public int level;
}
	
public class BodyController : MonoBehaviour
{
	public string myname;
	public GameController gameController;
	public BodyState bodystate; // Hold the global state of all the cells
	public Stats bodyStats;

	protected Hashtable inContact = new Hashtable ();
	protected float stats_health = 100.0f;
	protected float stats_speed = 100.0f;
	protected float stats_defense = 100.0f;
	protected float stats_reprodRate = 100.0f;
	protected float stats_power = 100.0f;
	protected float stats_delay = 0f;
	protected int stats_level = 1;

	public void setBodyState(BodyState bs) {
		bodystate=bs;
	}

	public virtual float health () {
		return ((stats_health/100.0f) * bodyStats.health);
	}

	public virtual float defense () {
		return ((stats_defense/100.0f) * bodyStats.defense);
	}

	public virtual float power () {
		return ((stats_power / 100.0f) * bodyStats.power);
	}
	// Lower number is faster
	public virtual float reprodRate () {
		if (bodyStats.reprodRate < 10) {
			//Debug.LogError (name +" !!!ReprodRate (<10) is messed up " + bodyStats.reprodRate);
			bodyStats.reprodRate = 10;
		}
		return (bodyStats.reprodRate);
	}
		
	// Drift speed
	public virtual float speed () { 
		return (stats_speed/100f) * bodyStats.speed;
	}

	public string showStats(){
		return name+" H:"+ health()+" D:"+ defense();
	}

	public float get_stats_power(){ return stats_power;
	}
	public float get_stats_health(){ return stats_health;
	}
	public float get_stats_defense(){ return stats_defense;
	}
	public void updateHealthStats(float point) {
		stats_health += point;
		if (stats_health > 100f) // health goes up by oxygen power
			stats_health = 100f;
		if (stats_health <= 0) {
			DestroyObject (gameObject);
		}
	}
	public void updatePowerStats(float point) {
		stats_power += point;
		if (stats_power > 100f) // health goes up by oxygen power
			stats_power = 100f;
		if (stats_power <= 0) {stats_power = 0;}
	}
	public void updateDefenseStats(float point) {
		stats_defense += point;
		if (stats_defense > 100f) // health goes up by oxygen power
			stats_defense = 100f;
		if (stats_defense <= 0) {stats_defense = 0; }
	}
}

