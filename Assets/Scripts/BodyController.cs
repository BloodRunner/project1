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

// Base stats for Organ,
[System.Serializable]
public class OrganStats
{
	// base level multiplied by ( 0 - 100% of base level )
	public float health, defense, regenRate;
}

public class BodyController : MonoBehaviour
{
	public string myname;
	public GameController gameController;
	public Stats bodyStats;
	// configured max stats for the cell body;
	//public OrganStats organStats;
	// configured max stats for the organ;
	protected Stats mybodyStats = new Stats ();
	// configured stats for the cell body;
	protected OrganStats myorganStats = new OrganStats ();
	// configured stats for the organ;
	protected Hashtable inContact = new Hashtable ();
	protected float stats_health = 100.0f;
	protected float stats_speed = 100.0f;
	protected float stats_defense = 100.0f;
	protected float stats_reprodRate = 100.0f;
	protected float stats_power = 100.0f;
	protected float stats_delay = 0f;
	protected int stats_level = 1;

	public void Awake ()
	{ // Initialize my status from defaults
		initStats ();
		//initOrganStats ();
	}

	// Use for both cell and organ - ignore power and speed in organ for now
	public void initStats ()
	{
		mybodyStats.health = bodyStats.health;
		mybodyStats.speed = bodyStats.speed;
		mybodyStats.defense = bodyStats.defense;
		mybodyStats.power = bodyStats.power;
		mybodyStats.reprodRate = bodyStats.reprodRate;
	}
		
	// Update cell stats for the whole body
	public void updateCellHealthStats (float health)
	{
		mybodyStats.health = health;
		if (mybodyStats.health > bodyStats.health)
			mybodyStats.health = bodyStats.health;
		if (mybodyStats.health <= 0f) {
			// Game Over!
			return;
		}
	}

	public void updateCellSpeedStats (float speed)
	{
		mybodyStats.speed = speed;
		if (mybodyStats.speed > bodyStats.speed)
			mybodyStats.speed = bodyStats.speed;
		else if (mybodyStats.speed < 0)
			mybodyStats.speed = 0;
	}

	public void updateCellDefenseStats (float defense)
	{

		mybodyStats.defense = defense;
		if (mybodyStats.defense > bodyStats.defense)
			mybodyStats.defense = bodyStats.defense;
		else if (mybodyStats.defense < 0)
			mybodyStats.defense = 0;

	}

	public void updateCellPowerStats (float power)
	{
		mybodyStats.power = power;
		if (mybodyStats.power > bodyStats.power)
			mybodyStats.power = bodyStats.power;
		else if (mybodyStats.power < 0)
			mybodyStats.power = 0;
	}

	public void updateCellReprodStats (float reprodRate)
	{
		mybodyStats.reprodRate = reprodRate;
		if (mybodyStats.reprodRate < bodyStats.reprodRate)  // smaller is better
                mybodyStats.reprodRate = bodyStats.reprodRate;
		Debug.Log (gameObject.name + " reprodRate=" + reprodRate + " -> " + bodyStats.reprodRate + "=>" + mybodyStats.reprodRate);
	}		

	// If heart is damaged - all cells will slow down to the percentage of damage of the heart
	public void slowDownAllCells (float percentage)
	{
		float speed = bodyStats.speed * percentage;
		updateCellSpeedStats (speed);
	}

	public void reduceAllCellDefense (float percent)
	{
		float defense = bodyStats.defense * percent;
		updateCellDefenseStats (defense);
	}

	// The number of seconds before a cell divides
	public void reduceReprodRate (float reprod)
	{
		if (reprod <= 0f) {
			reprod = 0.1f;
		}
		float reprodRate = bodyStats.reprodRate * 100 / reprod;
		updateCellReprodStats (reprodRate);
	}

	// Update player stats if collision
	// cleaner if stats has setters/getters
	public void updateHealthStats (float health)
	{		
		stats_health += health; 
		if (stats_health > 100)
			stats_health = 100;
		if (stats_health <= 0f) {
			Debug.Log (name + " dies! ");
			// Temp
			if (gameObject.name.Equals ("Player") && gameController != null)
				gameController.GameOver ();
			if (name.Equals("White"))
				gameObject.GetComponent<CameraChange> ().stopCamera ();
			Destroy (gameObject); // No health - dies!
			return;
		}
	}

	public void updateSpeedStats (float speed)
	{
			
		stats_speed += speed;
		if (stats_speed > 100)
			stats_speed = 100;
		else if (stats_speed < 0)
			stats_speed = 0;
	}

	public void updateDefenseStats (float defense)
	{
		
		stats_defense += defense;
		if (stats_defense > 100)
			stats_defense = 100;
		else if (stats_defense < 0)
			stats_defense = 0;
	}

	public void updatePowerStats (float power)
	{
		stats_power += power;
		if (stats_power > 100)
			stats_power = 100;
		else if (stats_power < 0)
			stats_power = 0;
	}

	public void updateReprodStats (float reprodRate)
	{
		stats_reprodRate += reprodRate;
		if (stats_reprodRate <= 0f)
			stats_reprodRate = 1;
	}

	public float health () {
		return ((stats_health/100.0f) * mybodyStats.health);
	}

	public float defense () {
		return ((stats_defense/100.0f) * mybodyStats.defense);
	}

	public float power () {
		return ((stats_power / 100.0f) * mybodyStats.power);
	}
	// Lower number is faster
	public float reprodRate () {
		if (mybodyStats.reprodRate < 10) {
			Debug.LogError (name +" !!!ReprodRate (<10) is messed up " + mybodyStats.reprodRate);
			mybodyStats.reprodRate = 10;
		}
		return (mybodyStats.reprodRate);
	}


	// Drift speed
	public float speed () { 
		return (stats_speed/100f) * mybodyStats.speed;
	}

	public string showStats(){
		return name+" H:"+ health()+" D:"+ defense();
	}

}

