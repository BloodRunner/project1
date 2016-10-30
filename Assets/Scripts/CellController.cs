﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This allows Unity UI to see class and show it
 * */

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class CellController : BodyController {
	const string Phage = "White";
	const int Distance = 2; // How far it is spawn from parent
	public string named;
	public float playerSpeed;  	// base movement speed
	public float tilt;
	public Boundary boundary; // Game boundary - if outside boundary - no control
	public GameObject dna; // What to spawn
	public Transform shotSpawn; // the transform for origin of the offspring spawn

	protected Rigidbody rb;
	private float nextReprod=0f;
	private float seconds_in_float = 1200f; // Cell life span in seconds float
	protected float stats_health=100.0f;
	protected float stats_speed = 100.0f;
	protected float stats_defense = 100.0f;
	protected float stats_reprodRate = 100.0f;
	protected float stats_power = 100.0f;
	protected float stats_delay = 0f;
	protected int stats_level = 1;



	void Start() { // Start and Awake don't seem to hold references
		rb = GetComponent<Rigidbody>();
		rb.angularVelocity = Random.insideUnitSphere * Random.Range(0,2); // rotate randomly
		Vector3 velo = Random.insideUnitSphere * speed();
		velo.y = 0;
		//rb.velocity = transform.forward * speed(); // base movement on blue axis
		rb.velocity = velo;
		Destroy(gameObject, seconds_in_float); // destroy objects automatically - cell death!
		//Debug.Log("START nextReprod= "+ nextReprod +"{"+ reprodRate() +"}");
		if (myname != null) name= myname;
	}

	// Update player stats if collision
	// cleaner if stats has setters/getters
	public void updateStats(float health, float speed, float defense, float reprodRate, float power) {		
		stats_health += health; 
		if (stats_health > 100)
			stats_health = 100;
		if (stats_health <= 0f) {
			Debug.Log (name + " dies! ");
			// Temp
			if (gameObject.name.Equals("Player") && gameController!= null)
				gameController.GameOver();
			this.GetComponent<CameraChange> ().stopCamera ();
			Destroy (gameObject); // No health - dies!
			return;
		}
		stats_speed += speed;
		if (stats_speed > 100)
			stats_speed = 100;
		else if (stats_speed < 0)
			stats_speed = 0;
		stats_defense += defense;
		if (stats_defense > 100)
			stats_defense = 100;
		else if (stats_defense < 0)
			stats_defense = 0;
		stats_power += power;
		if (stats_power > 100)
			stats_power = 100;
		else if (stats_power < 0)
			stats_power = 0;
		stats_reprodRate += reprodRate;
		if (stats_reprodRate <= 0f) stats_reprodRate=1;
	}

	// This changes the top level
	void addDelay(float sec) {
		mybodyStats.delay += sec; // when brain is damaged, all cells are slow to follow command
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

	// This cell defend against another
	public bool defendAgainst (CellController other) {
		float combat = other.power () - defense ();
		bool win;
		if (combat <= 0) { // successful defense against the others
			other.updateStats (combat, 0.0f, -1.0f, 0.0f, 0.0f);
			updateStats (0.0f, 0.0f, -1.0f, 0.0f, 0.0f);
			win= true;
		} else { // Lost
			updateStats (-combat, 0f, -1.0f, 0.0f, 0.0f);
			other.updateStats (0.0f, 0f, -1f, 0f, 0f);
			if (stats_health < 0)
				Destroy (rb);
			else
				inContact [other.GetInstanceID ()] = new Damage (combat, Time.time + 1);
			win= false;
			// Keeps track of the damage if contact continues; 

		}
		Debug.Log(gameObject.name+"."+gameObject.tag+" defends against "+ other.name+"."+other.tag+" health="+ stats_health+ " win? "+ win);
		return win;
	}


	// Reproduce once every N seconds - (reprod rate)
	void Update () {
		if (dna != null) {  // Can reproduce
			if (nextReprod == 0f) {
				nextReprod = Time.time + reprodRate ();
			}
			if (Time.time > nextReprod) {
				GameObject clone;
				Vector3 v3 = transform.position + (Random.insideUnitSphere * 0.2f);
				v3.y = 1f;

				if (shotSpawn != null) {// dna is the prefab
					clone = Instantiate (dna, shotSpawn.position, shotSpawn.rotation) as GameObject;
				} else {// Pop out a new one away from itself
					clone = Instantiate (dna, v3 , transform.rotation)as GameObject;
				}
				clone.name = gameObject.name;
				nextReprod = Time.time + reprodRate ();
				//Debug.Log("cell Ctrller nextReprod= "+ nextReprod +"{"+ reprodRate() +"}"+ clone.name);
			}
		}
		//if (defending>0) {Debug.Log ("Still defending "+defending+" critters");}
	}

	// Both Colliders are called, so only defense is coded
	// Combat rules:
	// 1) White cell attacks pathogen
	// 2) Pathogen attacks red cell
	void OnTriggerEnter(Collider other) {
		if ("Boundary".Equals(other.name) || gameObject.CompareTag(other.tag)) { // Same Team
			return;
		}
		CellController othercell = other.GetComponent(typeof(CellController)) as CellController;
		if ((gameObject.name.Equals("Red") && other.name.Equals("Pathogen")) ||  // Infection attacks Red (me)
			(gameObject.name.Equals("Pathogen") && (other.name.Equals("White") || other.name.Equals("Player"))) ) // White attacks pathogen (me)
		{
			Debug.Log(gameObject.name+"-"+gameObject.tag+" combat "+ other.name+"="+other.tag);
			defendAgainst(othercell);  // defend against white cell or pathogen
		}
		//if (!other.name.Equals("BloodFlow"))
		//Debug.Log(gameObject.name+"-"+gameObject.tag+" collided with "+ other.name+"="+other.tag);

	}	

	void OnTriggerExit(Collider other) {
		if ("Boundary" == other.name) {
			//Debug.Log(gameObject.name+" met boundary");
			rb.velocity *= -1;
			rb.position = new Vector3 (
				Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax));
			rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
			return;
		}
		if (gameObject.CompareTag(other.tag)) { // Same Team
			return;
		}
		GameObject otherObj = other.gameObject;
		// If sustaining damage - remove counter
		if (inContact.ContainsKey (otherObj.GetInstanceID ())) {
			inContact.Remove (otherObj.GetInstanceID ());
		}
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
				updateStats (damage.damage(), 0f, 0.0f, 0.0f, 0.0f);
				Debug.Log(gameObject.name+"-"+gameObject.tag+" damaged by contact with "+ other.name+"="+other.tag);
			}	
		}
	}
}
