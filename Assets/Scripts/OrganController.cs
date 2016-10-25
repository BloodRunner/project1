using UnityEngine;
using System.Collections;

[System.Serializable]
public class OrganStats
{   // base level multiplied by ( 0 - 100% of base level )
	public float health, defense, regenRate;
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
		stats.health=100f;
		stats.defense = 100f;
		stats.regenRate = 100f;
	}

	// oxygenate means add power to health + defense to organ
	void oxygenate (float power) {
		stats.health += power;
		if (stats.health > 100f)
			stats.health = 100f;
		stats.defense += power;
		if (stats.defense > 100f)
			stats.defense = 100f;
	}

	public float health () {
		return ((stats.health/100.0f) * (float)bodyStats.health);
	}

	public float defense () {
		return ((stats.defense/100.0f) * (float)bodyStats.defense);
	}

	// Update player stats if collision
	// cleaner if stats has setters/getters
	public void updateStats(float health, float defense, float regenRate) {		
			stats.health += health;
			stats.defense += defense;
			stats.regenRate += regenRate;
	}

	// Each combatant lose 1% in defend after each combat
	public bool defend(CellController pathogen){
		if (pathogen.power() > defense ()) { // attack successful
			updateStats(defense()-pathogen.power(), -1f, 0f);
			pathogen.updateStats (0f, 0f, -1f,0f,0f); // lose a bit of defense
			return false;
		}
		else { // successful defense
			pathogen.updateStats(defense()-pathogen.power(), 0f,-1f,0f,0f); // pathogen is damaged
			updateStats(0f, -1f, 0f);
			return true;
		}
	}

	// Each organ damage does different damage to host stats
	public virtual void damageBody(){
		
	}

	// Collider for each object is called.
	// Only organ collision is dealt with here.
	void OnTriggerEnter(Collider other) {
		if (stats.health == 0) { // Organ is dead - destroy???
			return; 
		}
		// Infect/Attack/Oxygenate everything that enters the trigger
		if (other.tag == "Infection") { // do battle
			CellController infection = other.GetComponent(typeof(CellController)) as CellController;
			if (!defend(infection)) // if failed to defend, check for specific damage
				damageBody (); // Organ specific damage to the cell stats
		} else if (other.name == "red") {
			CellController red = other.GetComponent(typeof(CellController)) as CellController;
			oxygenate (red.power ());
		}
		//gameController.addScore (scoreValue);
	}
}
