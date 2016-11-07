using UnityEngine;
using System.Collections;

// Tag = Host
// Name = organ name
public abstract class OrganController : BodyController {
	public Rigidbody rb;
	public GameObject shot;

	private float stats_regenRate=100f;
	private float nextReGen=0f;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	// oxygenate means add power to health + defense to organ
	// TODO: modify by regenRate
	void oxygenate (float power) {
		//Debug.Log(name +" before oxygenate ("+ power+")"+ showStats());
		updateHealthStats (power); // health goes up by oxygen power
		updateDefenseStats (power / 2f); // defense goes up by 1/2 oxygen power
		//Debug.Log(name +" after oxygenate "+ showStats());
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
			damageBody();
			if (stats_health <= 0)
				Destroy (gameObject); // Die!
			else
				inContact [pathogen.GetInstanceID ()] = new Damage (combat, Time.time + 1);
			successful= false;
			// Keeps track of the damage if contact continues; 
		}
		//Debug.Log(gameObject.name+"."+defense()+" defends against "+ pathogen.name
		//	+"."+pathogen.power()+ " "+ showStats()+ (successful?" success":"failed"));
			
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
		if (other.tag.Equals("Infection")) { // do battle
			CellController infection = other.GetComponent(typeof(CellController)) as CellController;
			if (!defend(infection)) // if failed to defend, check for specific damage
				damageBody (); // Organ specific damage to the cell stats
		} else if (other.name.Equals( "Red")) {
			RedController red = other.GetComponent(typeof(RedController)) as RedController;
			//Debug.Log(other.name+" bodyStats "+ red.bodystate.showStats());
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
				updateHealthStats (damage.damage());
				//Debug.Log(gameObject.name+"-"+gameObject.tag+" damaged by contact with "+ other.name+"="+other.tag);
			}	
		}
	}
		
}
