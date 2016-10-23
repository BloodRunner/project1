using UnityEngine;
using System.Collections;

[System.Serializable]
public class OrganStats
{   // base level multiplied by ( 0 - 100% of base level )
	public int health, defense, regenRate;
}

// Tag = Host
// Name = organ name
public class OrganController : MonoBehaviour {
	public Rigidbody rb;
	public GameObject shot;
	public OrganStats bodyStats; // configured stats for the critter;
	private OrganStats stats; // percentage of the body stats - from damage

	void Start() {
		rb = GetComponent<Rigidbody>();
		stats = new OrganStats();   // Create base stats
		stats.health=100;
		stats.defense = 100;
		stats.regenRate = 100;
	}

	// oxygenate means add power to health + defense to organ
	void oxygenate (int power) {
		stats.health += power;
		if (stats.health > 100)
			stats.health = 100;
		stats.defense += power;
		if (stats.defense > 100)
			stats.defense = 100;
	}

	public int health () {
		return (int)((float)(stats.health/100.0f) * (float)bodyStats.health);
	}

	public int defense () {
		return (int)((float)(stats.defense/100.0f) * (float)bodyStats.defense);
	}

	// Update player stats if collision
	// cleaner if stats has setters/getters
	public void updateStats(int health, int defense, int regenRate) {		
			stats.health += health;
			stats.defense += defense;
			stats.regenRate += regenRate;
	}
	
	// Update is called once per frame
	void Update () {
	}

	// This should be overridden by individual organs
	void OnTriggerEnter(Collider other) {
		// Infect/Attack/Oxygenate everything that enters the trigger
		OrganController organ = other.GetComponent(typeof(OrganController)) as OrganController;
		if (other.tag == "Infection") { // do battle
			CellController infect = other.GetComponent(typeof(CellController)) as CellController;
			// other.gameObject
			// TODO: Write attack routine for each organ
			if (infect.power () > defense ()) { // if attacker is stronger
				updateStats((infect.power()-defense()), 0, 0); // health, defense, regen.
				//Instantiate (explosion, transform.position, transform.rotation); // If damaged - use graphics
			}
			if (stats.health == 0) {
				//gameController.GameOver ();
			}
		} else if (other.name == "red") {
			CellController red = other.GetComponent(typeof(CellController)) as CellController;
			oxygenate (red.power ());
		}
		//gameController.addScore (scoreValue);
	}
}
