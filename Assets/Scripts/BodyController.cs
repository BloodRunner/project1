using UnityEngine;
using System.Collections;

/* This allows Unity UI to see class and show it
 * */

// Base stats for cells,
[System.Serializable]
public class Stats
{
	// base level multiplied by ( 0 - 100% of base level )
	public float health, speed, defense, reprodRate;
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
	public OrganStats organStats;
	// configured max stats for the organ;
	protected Stats mybodyStats = new Stats ();
	// configured stats for the cell body;
	protected OrganStats myorganStats = new OrganStats ();
	// configured stats for the organ;
	protected Hashtable inContact=new Hashtable();

	public void Awake ()
	{ // Initialize my status from defaults
		initCellStats ();
		initOrganStats ();
	}

	public void initCellStats ()
	{
		mybodyStats.health = bodyStats.health;
		mybodyStats.speed = bodyStats.speed;
		mybodyStats.defense = bodyStats.defense;
		mybodyStats.power = bodyStats.power;
		mybodyStats.reprodRate = bodyStats.reprodRate;
	}

	public void initOrganStats ()
	{
		myorganStats.health = organStats.health;
		myorganStats.defense = organStats.defense;
		myorganStats.regenRate = organStats.regenRate;
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

	// Update cell stats for the whole body
	public void updateOrganHealthStats (float health)
	{
		myorganStats.health = health;
		if (myorganStats.health > organStats.health)
			myorganStats.health = organStats.health;
		if (myorganStats.health <= 0f) {
			// Game Over!
			return;
		}
	}

	public void updateOrganDefenseStats (float defense)
	{
		myorganStats.defense = defense;
		if (myorganStats.defense > organStats.defense)
			myorganStats.defense = organStats.defense;
		else if (myorganStats.defense < 0)
			myorganStats.defense = 0;
	}

	public void updateOrganHRegenStats (float regenRate)
	{
		myorganStats.regenRate = regenRate;
		if (myorganStats.regenRate < organStats.regenRate)  // smaller is better
				myorganStats.regenRate = organStats.regenRate;

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

}

