using UnityEngine;
using System.Collections;

// Tag = Host
// Name = organ name
public abstract class OrganController : BodyController {
	public string mission;
	public int[] order;
	public BloodFlowController bfctrl;
	public Rigidbody rb;
	public int mask;
	private Camera followerCamera;
	//public GameObject shot;
	protected bool isSpawner=false;
	protected Blink exclamation = null;

	// in organs reprodRate is the number of seconds it uses up 1% of it's oxygen/health
	// oxygen depletion of N points per N seconds
	private float nextOxygenDepletion=0f;

	void Start() {
		bfctrl = GameObject.Find ("GameController").GetComponent<BloodFlowController> ();
		followerCamera = GameObject.Find ("followCamera").GetComponent<Camera>();
		rb = GetComponent<Rigidbody>();
		exclamation = GetComponentInChildren<Blink> ();
		if (exclamation) exclamation.gameObject.SetActive (false);
		nextOxygenDepletion = Time.time;
		if (gameController==null)
			gameController = GameObject.FindObjectOfType (typeof(GameController)) as GameController;
		if (bodystate == null)
			bodystate = GameObject.FindObjectOfType (typeof(BodyState)) as BodyState;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Hostcell"),LayerMask.NameToLayer("Organ"));
	}

	// oxygenate means add power to health + defense to organ
	// TODO: modify by regenRate
	void oxygenate (float power) {
		//string preoxy = name +" before oxygenate("+ power+")="+ showStats();
		updateHealthStats (power); // health goes up by oxygen power
		updateDefenseStats (power / 2f); // defense goes up by 1/2 oxygen power
		// Debug.Log(preoxy +" after= "+ showStats());
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

			if (stats_health > 0) {
				inContact [pathogen.GetInstanceID ()] = new Damage (combat, Time.time + 1);
				if (isSpawner && stats_health > 25) {
					if (exclamation) exclamation.gameObject.SetActive (false);
					isSpawner = false;
					swapAudioTracks ();
					Debug.Log (name + " is revived "); // Add points??
				} else {
					if (stats_health < 25 && !isSpawner) {
						Debug.Log (name + " health=" + stats_health + " " + health ());
						if (exclamation) exclamation.gameObject.SetActive (true);
					}
				}
			} else
				gameController.checkGameOver ();
			successful= false;
			// Keeps track of the damage if contact continues; 
		}
	/*	Debug.Log(name+"."+defense()+" defends against "+ pathogen.name
			+"."+pathogen.power()+ " "+ showStats()+ (successful?" success":"failed"));
	*/		
		return successful;
	}

	// lose health slowly - depends on red cell for oxygenation
	// Loses 1% per N seconds
	public void Update() {		
		if (Time.time > nextOxygenDepletion) {
			updateHealthStats(-1f);
			//Debug.Log (name + " Oxygen depletion ("+reprodRate ()+")" + showStats());
			nextOxygenDepletion = Time.time + reprodRate (); //reprod is used for oxygen use rate -
		}
	}

	// Each organ damage has different effects on the host stats
	public virtual void damageBody(){
		
	}

	// New Game behaviour - organ turns into undead enemy spawner
	public override void deathHandler (){
		if (!isSpawner) {
			gameController.showMessage (name + " is now an infection factory! Save it by sending red cells!!", 5);
			isSpawner = true;
			//renderer.material.color = Color.blue;
			swapAudioTracks();
		} 
	}

	// Swap audio tracks from unique live to dead sound
	public void swapAudioTracks(){
		AudioSource[] tracks = GetComponentsInChildren<AudioSource>() as AudioSource[];
		foreach (AudioSource track in tracks) { 
			if (track.isPlaying) {
				track.Stop ();
			} else {
				track.Play ();
			}
		}
	}
	// Collider for each object is called.
	// Only organ collision is dealt with here.
	void OnTriggerEnter(Collider other) {
		
		// Infect/Attack/Oxygenate everything that enters the trigger
		if (other.tag.Equals("Infection")) { // do battle
			if (stats_health == 0) { // Organ is dead 
				return; 
			}
			CellController infection = other.GetComponent(typeof(CellController)) as CellController;
			if (!defend(infection)) // if failed to defend, check for specific damage
				damageBody (); // Organ specific damage to the cell stats
		} else if (other.name.Equals( "Red")) {
			RedController red = other.GetComponent(typeof(RedController)) as RedController;
			// Debug.Log(name+"+red" + red.bodystate.showStats());
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
			//if (!isSpawner)
			//	Debug.Log (other.name+ " exits " + gameObject.name);
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

	/* Call the closest white cell to come help */
	public void callForSupport() {
		if (followerCamera.enabled == false) {
			bfctrl.makeMission (order, mission);
		} else {
			WhiteController[] cells = GameObject.FindObjectsOfType (typeof(WhiteController)) as WhiteController[];
			//GameObject[] cells = GameObject.FindGameObjectsWithTag ("Host");
			for (int i = 0; i < cells.Length; i++) {
				if(cells[i].name == "White"){
					if(cells[i].GetComponent<CameraChange>().getIsOn()){
						print (this.myname);
						break;
					}
				} else if(cells[i].name == "KillerT"){
					if(cells[i].GetComponent<CameraChange>().getIsOn()){

						print (this.myname);
						break;
					}
				}
			}
		}
		//Debug.Log (name + " needs backup");
	}
		
}
