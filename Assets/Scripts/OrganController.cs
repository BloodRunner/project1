using UnityEngine;
using System.Collections;

// Tag = Host
// Name = organ name
public class OrganController : BodyController {
	public Rigidbody rb;
	public GameObject shot;

	//private OrganStats stats=new OrganStats(); // percentage of the body stats - from damage
	private float stats_health=100f;
	private float stats_defense=100f;
	private float stats_regenRate=100f;
	private float nextReGen=0f;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	// oxygenate means add power to health + defense to organ
	// TODO: modify by regenRate
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
	// Organ can reflect the attack (1/2 defense - attack on the pathogen
	public bool defend(CellController pathogen){
		float combat = pathogen.power () - defense ();
		bool successful;
		if (combat <= 0) { // successful defense against the pathogen
			pathogen.updateHealthStats (-combat/2); // pathogen is damaged
			pathogen.updateDefenseStats(-1f);
			updateDefenseStats ( -1.0f);// organ loses a bit of defense
			successful= true;
		} else { // Lost
			updateHealthStats (-combat);
			updateDefenseStats ( -1.0f);
			pathogen.updateDefenseStats (-1f);// pathogen loses a bit of defense
			if (stats_health <= 0)
				Destroy (rb); // Keep game object!?
			else
				inContact [pathogen.GetInstanceID ()] = new Damage (combat, Time.time + 1);
			successful= false;
			// Keeps track of the damage if contact continues; 
		}
		Debug.Log(gameObject.name+"."+gameObject.tag+" defends against "+ pathogen.name+"."+pathogen.tag+" health="+ stats_health+ (successful?" success":"failed"));
		return successful;
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
	// Collider for each object is called.
	// Only organ collision is dealt with here.
	void OnTriggerExit(Collider other) {
		if (stats_health == 0) { // Organ is dead - destroy???
			return; 
		}
		// Infect/Attack/Oxygenate everything that enters the trigger
		if (other.tag == "Infection") { // do battle
			CellController infection = other.GetComponent(typeof(CellController)) as CellController;
			if (inContact.ContainsKey (infection.GetInstanceID ())) {
				inContact.Remove (infection.GetInstanceID ());
			}
		} else if (other.name == "red") {
			CellController red = other.GetComponent(typeof(CellController)) as CellController;
			oxygenate (red.power ());
		}
		//gameController.updateScore (scoreValue);
	}

	void OnTriggerStay(Collider other) {
		if (gameObject.tag == other.tag ) { // Same Team
			return;
		}
		GameObject gameObj = other.gameObject;
		// If sustaining damage - apply damage
		if (gameObj && inContact.ContainsKey (gameObj.GetInstanceID ())) {
			Damage damage = (Damage)inContact[gameObj.GetInstanceID()];
			if (damage.nextAttack (Time.time)) {
				updateStats (damage.damage(),0.0f, 0.0f);
				Debug.Log(gameObject.name+"-"+gameObject.tag+" damaged by contact with "+ other.name+"="+other.tag);
			}	
		}
	}

}
