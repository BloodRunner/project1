using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This allows Unity UI to see class and show it
 * */


public class CellController :  BodyController{
	public float playerSpeed;  	// base movement speed
	public float tilt;
	public Boundary boundary; // Game boundary - if outside boundary - no control
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

	void Start() { 
		rb = GetComponent<Rigidbody>();
		rb.angularVelocity = Random.insideUnitSphere * Random.Range(0,2); // rotate randomly
		Vector3 velo = Random.insideUnitSphere * speed();
		velo.y = 0;
		rb.velocity = velo;
		nvagt = gameObject.GetComponent<NavMeshAgent> ();
		if (gameController ==null)
			gameController = gameObject.GetComponent<GameController> ();
		if (bodystate ==null)
			bodystate = gameObject.GetComponent<BodyState> ();
		if (lifespan_in_seconds == 0)
			lifespan_in_seconds = 1200f;
		Destroy(gameObject, lifespan_in_seconds); // destroy objects automatically - cell death!
		//Debug.Log("START nextReprod= "+ nextReprod +"{"+ reprodRate() +"}");
		if (myname != null) name= myname;
		hitParticles = GetComponentInChildren <ParticleSystem> ();
		//if (hitParticles!=null)
		//	hitParticles.Stop ();
	}

	public void setVelocity(float v) {
		if (nvagt == null)
			nvagt = gameObject.GetComponent<NavMeshAgent> ();
		nvagt.speed = speed();
	}

	// This changes the top level
	void addDelay(float sec) {
		mybodyStats.delay += sec; // when brain is damaged, all cells are slow to follow command
	}
	public void TakeDamage(int points, Vector3 hitPt) {
		updateHealthStats (-points);
	}

	// This cell defend against another
	public bool defendAgainst (CellController other){
		//Debug.Log (name + " defends against " + other.name);
		float combat = other.power () - defense ();
		bool win;
		if (combat <= 0) { // successful defense against the others
			other.updateHealthStats (combat);
			other.updateDefenseStats(-1.0f);
			updateDefenseStats (-1.0f);
			win= true;
		} else { // Lost
			updateHealthStats (-combat);
			updateDefenseStats (-1.0f);
			other.updateDefenseStats (-1.0f);
			if (stats_health > 0) 
				inContact [other.GetInstanceID ()] = new Damage (combat, Time.time + 1);
			win= false;
			// Keeps track of the damage if contact continues; 

		}
		Debug.Log(gameObject.name+"."+gameObject.tag+" defended against "+ other.name+"."+other.tag + " "+
			showStats()+ (win?"succeeded":"failed") );
		return win;
	}
	// Special Power of this cell, whatever it is
	public virtual void special(){
	}

	public override void deathHandler (){
		//Debug.Log (name + " dies ");
		if (hitParticles != null) {
			hitParticles.Play (); // Play explosion
		}
	
		if (gameController) {
			if (tag.Equals ("Infection"))
				gameController.UpdateScore (points);
			gameController.showMessage ("Poor " + name + " dies ", 3);
		} else
			Debug.Log (name + " dies - gameController empty");
		DestroyObject (gameObject);
	}

	// Reproduce once every N seconds - (reprod rate)
	protected void Update () {
		special (); // If it has special powers, use it first
		if (dna != null) {  // Can reproduce - do it
			if (nextReprod == 0f) { // Add a random so that reproduction is staggered
				nextReprod = Time.time + reprodRate () + Random.Range(0,0.5f);
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
				Debug.Log(name + " nextReprod= "+ nextReprod +"{"+ reprodRate() +"}"+ clone.name);
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
		if ((gameObject.tag.Equals("Infection") && other.name.Equals("Red")) ||
			(gameObject.name.StartsWith("White") && other.tag.Equals("Infection"))){

			if (!nvagt)
				nvagt= gameObject.GetComponent<NavMeshAgent> ();
			if (nvagt)
				nvagt.Move(other.transform.position - transform.position);

		}
		//Debug.Log(gameObject.name+"-"+gameObject.tag+" collided with "+ other.name+"="+other.tag);
	}	
		
	void OnCollisionStay(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Collider other = contact.otherCollider;
			GameObject gameObj = other.gameObject;
			// If sustaining damage - apply damage
			if (gameObj && inContact.ContainsKey (gameObj.GetInstanceID ())) {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
				Damage damage = (Damage)inContact[gameObj.GetInstanceID()];
				if (damage.nextAttack (Time.time)) {
					updateHealthStats (damage.damage());
					Debug.Log(gameObject.name+"-"+gameObject.tag+" damaged by contact with "+ other.name+"="+other.tag);
				}	
			}
		}
	}
	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			
			Collider other = contact.otherCollider;
			if (!tag.Equals (other.tag) &&
			    (other.tag.Equals ("Host") || other.tag.Equals ("Infection"))) {
				//Debug.Log (name + "-" + tag + " collided with " + other.name + "=" + other.tag);
				if (gameObject.name.Equals ("Red") && other.tag.Equals ("Infection")){
					CellController othercell = other.GetComponent (typeof(PathogenController)) as PathogenController;
					defendAgainst (othercell); 
				}
				if (gameObject.tag.Equals ("Infection") && (other.name.StartsWith ("White") || other.name.Equals ("Player"))) { // White attacks pathogen (me)
						CellController othercell = other.GetComponent (typeof(WhiteController)) as WhiteController;
						defendAgainst (othercell);  // defend against white cell or pathogen
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
			}
		}
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
				Debug.Log(gameObject.name+"-"+gameObject.tag+" damaged by tcontact with "+ other.name+"="+other.tag);
			}	
		}
	}
}
