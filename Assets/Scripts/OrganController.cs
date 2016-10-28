using UnityEngine;
using System.Collections;
/*
[System.Serializable]
public class OrganStats
{   // base level multiplied by ( 0 - 100% of base level )
	public float health, defense, regenRate;
}
*/
// Tag = Host
// Name = organ name
public class OrganController : BodyController {
	public Rigidbody rb;
	public GameObject shot;

	//private OrganStats stats; // percentage of the body stats - from damage
	private float stats_health=100f;
	private float stats_defense=100f;
	private float stats_regenRate=100f;
	private float nextReGen=0f;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	// oxygenate means add power to health + defense to organ
	void oxygenate (float power) {
		stats_health += power;
		if (stats_health > 100f) // health goes up by oxygen power
			stats_health = 100f;
		stats_defense += power/2; // defense goes up by 1/2 oxygen power
		if (stats_defense > 100f)
			stats_defense = 100f;
	}

	public float health () {
		return ((stats_health/100.0f) * (float)organStats.health);
	}

	public float defense () {
		return ((stats_defense/100.0f) * (float)organStats.defense);
	}

	// Update player stats if collision
	// cleaner if stats has setters/getters
	public void updateStats(float health, float defense, float regenRate) {	
		stats_health += health;
		if (stats_health < 0) {
			// Gameover?
			Destroy (gameObject);
		} else if (stats_health > 100) {
			stats_health = 100f;
		}
		stats_defense += defense;
		if (stats_defense < 0) {
			stats_defense = 0;
		} else if (stats_defense > 100) {
			stats_defense = 100;
		}
		stats_regenRate += regenRate;
		if (stats_regenRate < myorganStats.regenRate)
			stats_regenRate = myorganStats.regenRate;
	}

	// Each combatant lose 1% in defend after each combat
	public bool defend(CellController pathogen){
		bool success;
		if (pathogen.power() > defense ()) { // attack successful
			updateStats(defense()-pathogen.power(), -1f, 0f);
			pathogen.updateStats (0f, 0f, -1f,0f,0f); // lose a bit of defense
			success= false;
		}
		else { // successful defense
			pathogen.updateStats(defense()-pathogen.power(), 0f,-1f,0f,0f); // pathogen is damaged
			updateStats(0f, -1f, 0f);
			success= true;
		}
		if (stats_health <= 0)
			Destroy (gameObject);
		return success;
	}

	// regenerate if damaged
	public void Update() {		
		if (nextReGen == 0f) {
			nextReGen = Time.time + stats_regenRate;
		}
		if (stats_health < 100) {
			if (Time.time > nextReGen) {
				stats_health += 1;	
			}
		}
	}

	// Each organ damage does different damage to host stats
	public virtual void damageBody(){
		
	}

	// Collider for each object is called.
	// Only organ collision is dealt with here.
	void OnTriggerEnter(Collider other) {
		if (stats_health == 0) { // Organ is dead - destroy???
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
		//gameController.updateScore (scoreValue);
	}
}
