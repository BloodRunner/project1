﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This allows Unity UI to see class and show it
 * */


public class CellController :  BodyController{
	protected string nickname="";
	public float playerSpeed;  	// base movement speed
	public float tilt;
	//public Boundary boundary; // Game boundary - if outside boundary - no control
	public GameObject dna; // What to spawn
	public Transform shotSpawn; // the transform for origin of the offspring spawn
	static Stats mybodyStats=new Stats(); // applies to the whole class
	protected Rigidbody rb;
	// On killerT cells - it is the number of points to get a new one
	// On pathogens - it is the number of point you get for killing one
	public int points;	// score points and T cell bonus - applies to pathogen
	private float nextReprod=0f;
	public float lifespan_in_seconds ; // Cell life span in seconds float
	private NavMeshAgent nvagt;
	private GameObject target; // for attackers
	private Vector3 dest;
	ParticleSystem hitParticles; // Death Indication
	float birthtime;
	AudioSource deathSound=null;
	bool dead=false;


	void Awake() { 
		rb = GetComponent<Rigidbody>();
		rb.angularVelocity = Random.insideUnitSphere * Random.Range(0,2); // rotate randomly
		Vector3 velo = Random.insideUnitSphere * speed();
		velo.y = 0;
		rb.velocity = velo;
		nvagt = gameObject.GetComponent<NavMeshAgent> ();
		if (gameController ==null)
			gameController = GameObject.FindObjectOfType<GameController> ();
		if (bodystate ==null)
			bodystate = GameObject.FindObjectOfType<BodyState> ();
		if (lifespan_in_seconds == 0)
			lifespan_in_seconds = 1200f;
		AudioSource [] audiosources = gameObject.GetComponentsInChildren<AudioSource> ();
		for (int i =0; i < audiosources.Length; i++) {
			if (audiosources[i].name.Equals("deathSound"))
				deathSound = audiosources[i];
			//Debug.Log (" deathSound found for " + name);
		}
		birthtime = Time.time;
		Destroy(gameObject, lifespan_in_seconds); // destroy objects automatically - cell death!
		//Debug.Log("START nextReprod= "+ nextReprod +"{"+ reprodRate() +"}");
		if (myname != null) name= myname;
		hitParticles = GetComponentInChildren <ParticleSystem> ();
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Hostcell"),LayerMask.NameToLayer("Walls"));
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Hostcell"),LayerMask.NameToLayer("Ground"));
	}

	public float time_left_to_live() {
		return	lifespan_in_seconds - (Time.time - birthtime);
	}

	public void setVelocity(float v) {
		if (nvagt == null)
			nvagt = gameObject.GetComponent<NavMeshAgent> ();
		nvagt.updateRotation = true;
		nvagt.speed = speed();
	}

	// This changes the top level
	void addDelay(float sec) {
		mybodyStats.delay += sec; // when brain is damaged, all cells are slow to follow command
	}
	public void TakeDamage(float points, Vector3 hitPt) {
		if (hitParticles != null) {
			hitParticles.Play (); // Play explosion
		}
		updateHealthStats (-points);
	}

	// This cell defend against another
	public bool defendAgainst (CellController other){
		//Debug.Log (name + " defends against " + other.name);
		float combat = other.power () - defense ();
		bool win;
		if (combat <= 0) { // successful defense against the others
			other.updateHealthStats (-combat);
			other.updateDefenseStats(-1.0f);
			updateDefenseStats (-1.0f);
			updateHealthStats (-0.75f); // Even winning loses some health
			win= true;
		} else { // Lost
			if (hitParticles != null) {
				hitParticles.Play (); // Play explosion
			}
			updateHealthStats (-combat);
			updateDefenseStats (-1.0f);
			other.updateDefenseStats (-1.0f);
			other.updateHealthStats (-0.75f); // Even winner loses some health
			win= false;
			// Keeps track of the damage if contact continues; 
		}
		/*
		if (tag.Equals("Infection"))
			Debug.Log(gameObject.name+"."+gameObject.tag+" defended against "+ other.nickname+"."+other.name + " "+
				showStats()+ other.GetComponent <CellController> ().showStats()+(win?" succeeded":" failed") );
		*/
		return win;
	}
	// Special Power of this cell, whatever it is
	public virtual void special(){
	}

	public override void deathHandler (){
		//Debug.Log (name + " dies ");
		if (dead) return;
		dead = true;
		if (deathSound) deathSound.Play();
		//GetComponent<MeshRenderer>().enabled = false;
		if (gameController == null)
			gameController = GetComponent<GameController> ();
		if (gameController) {
			if (tag.Equals ("Infection")) {
				GameObject.Find ("GameController").GetComponent<DropScript> ().drop (this.gameObject, 5);
				gameController.UpdateScore (points);
			}
		}
		DestroyObject (gameObject, 0.2f);
	}

	// Reproduce once every N seconds - (reprod rate)
	protected void Update () {
		special (); // If it has special powers, use it first
		if (dna != null) {  // Can reproduce - do it
			if (nextReprod == 0f) { // Add a random so that reproduction is staggered
				nextReprod = Time.time + reprodRate () + Random.Range(0,0.5f);
			}
			if (Time.time > nextReprod && ! gameController.isGameOver()) {
				GameObject clone;
				Vector3 v3 = transform.position + (Random.insideUnitSphere * 0.2f);
				v3.y = 1f;

				if (shotSpawn != null) {// dna is the prefab
					clone = Instantiate (dna, shotSpawn.position, shotSpawn.rotation) as GameObject;
				} else {  // Pop out a new one away from itself
					clone = Instantiate (dna, v3 , transform.rotation)as GameObject;
				}
				nvagt = gameObject.GetComponent<NavMeshAgent> ();
				CellController cell = clone.GetComponent(typeof(CellController)) as CellController;
				cell.gameController = gameController;
				cell.bodystate = bodystate;
				if (!nvagt)
				 	nvagt= gameObject.GetComponent<NavMeshAgent> ();
				if (nvagt) {
					clone.GetComponent<NavMeshAgent> ().SetDestination (nvagt.destination);
				} else {
					Debug.Log(name +" Missing nav agent:");
				}
				if (myname!=null)
					clone.name = myname; // name by configuration
				else {
					clone.name = name; // name after the parent
				}
				nextReprod = Time.time + reprodRate ();
				//Debug.Log(name + " nextReprod= "+ nextReprod +"{"+ reprodRate() +"}"+ clone.name);
			}
		}
		//if (defending>0) {Debug.Log ("Still defending "+defending+" critters");}
	}

	// Both Colliders are called, so only defense is coded
	// Combat rules:
	// 1) White cell attacks pathogen
	// 2) Pathogen attacks red cell

	void OnCollisionStay(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Collider other = contact.otherCollider;
			GameObject gameObj = other.gameObject;
			// If sustaining damage - apply damage

			if (gameObj && inContact.ContainsKey (gameObj.GetInstanceID ())) {
				Debug.DrawRay (contact.point, contact.normal, Color.white);
				Damage damage = (Damage)inContact [gameObj.GetInstanceID ()];
				if (damage.nextAttack (Time.time)) {
					updateHealthStats (damage.damage ());
					//Debug.Log (gameObject.name + "-" + gameObject.tag + " damaged by contact with " + other.name + "=" + other.tag);
				}	
			} /* else {
				if (!tag.Equals (other.tag)
				    && (other.tag.Equals ("Infection") && (name.Equals ("Red") ))
					|| (tag.Equals ("Infection") && (other.name.StartsWith ("White")))) {
					//Debug.Log (gameObject.name + "-" + gameObject.tag + " collision stay with " + other.name + "=" + other.tag + " id=" + gameObj.GetInstanceID ());
				
					Debug.Log (gameObject.name + " defends against " + gameObj.name + " " + gameObj.GetInstanceID () + " not found in contact" + inContact.ContainsKey (gameObj.GetInstanceID ()));
				}
			}*/
		}
	}

	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Collider other = contact.otherCollider;
			/*
			if (!tag.Equals (other.tag)
				&& ( other.tag.Equals("Infection")  &&  name.Equals("Red"))
				|| ( tag.Equals("Infection")  && other.name.Equals("White") ) 
			) {
				Debug.Log (gameObject.name + "-" + gameObject.tag + " collisionEnter with " + other.name + "=" + other.tag +" id="+ other.gameObject.GetInstanceID ());
			}*/
			bool battle = true;
			if (!tag.Equals (other.tag) &&
			    (other.tag.Equals ("Host") || other.tag.Equals ("Infection"))) {
				//Debug.Log (name + "-" + tag + " collided with " + other.name + "=" + other.tag);
				if (gameObject.name.Equals ("Red") && other.tag.Equals ("Infection")){
					CellController othercell = other.GetComponent (typeof(PathogenController)) as PathogenController;
					battle = defendAgainst (othercell); 
				}
				if (gameObject.tag.Equals ("Infection") && (other.name.Equals ("White")||other.name.Equals ("KillerT"))) { // White attacks pathogen 
						CellController othercell = other.GetComponent (typeof(WhiteController)) as WhiteController;
						battle = defendAgainst (othercell);  // defend against white cell or pathogen
				}
				if (stats_health > 0 && battle == false) {
					if (!inContact.ContainsKey(other.gameObject.GetInstanceID ())) {
						inContact [other.gameObject.GetInstanceID ()] = new Damage (-2, Time.time + 1);
						//Debug.Log ("InContact damage start from "+ other.name+ "  "+ other.gameObject.GetInstanceID () );
					}
				}
			}
		}
		//if (collision.relativeVelocity.magnitude > 2)			audio.Play();
	}

	void OnCollisionExit(Collision collisionInfo) {
		if (!tag.Equals (collisionInfo.transform.tag) &&
		    (collisionInfo.transform.tag.Equals ("Host") ||
		    collisionInfo.transform.tag.Equals ("Infection"))) {
			//Debug.Log (name + " leaves " + collisionInfo.transform.name+"("+inContact.Count+")");
			GameObject otherObj = collisionInfo.transform.gameObject;
			// If sustaining damage - remove counter
			if (inContact.ContainsKey (otherObj.GetInstanceID ())) {
				inContact.Remove (otherObj.GetInstanceID ());
				//Debug.Log ("Collision Exit "+ otherObj.GetInstanceID ());
			}
		}
	}
		

}
