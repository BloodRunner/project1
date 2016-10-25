using UnityEngine;
using System.Collections;

/* This allows Unity UI to see class and show it
 * */
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

// Base stats for cells,
[System.Serializable]
public class Stats
{   // base level multiplied by ( 0 - 100% of base level )
	public float health, speed, defense, reprodRate;
	public float power; // attack if white cell or infection, oxygenation if red cell
	public float delay; // Pause before it can follow a command
	public int level;
}

public class CellController : MonoBehaviour {
	const string Phage = "White";
	const int Distance = 2; // How far it is spawn from parent

	public float playerSpeed;  	// base movement speed
	public float tilt;
	public Boundary boundary; // Game boundary - if outside boundary - no control
	public GameObject dna; // What to spawn
	public Transform shotSpawn; // the transform for origin of the offspring spawn
	public Stats bodyStats; // configured stats for the cell body;

	protected Stats stats; // percentage of the body stats - from damage
	protected Rigidbody rb;
	private float nextReprod=0f;
	private int defending = 0;
	private float seconds_in_float = 1200f; // Cell life span in seconds float
	protected float stats_health=100.0f;
	protected float stats_speed = 100.0f;
	protected float stats_defense = 100.0f;
	protected float stats_reprodRate = 100.0f;
	protected float stats_power = 100.0f;
	protected float stats_delay = 0f;
	protected int stats_level = 1;

	void Start() {
		rb = GetComponent<Rigidbody>();
		rb.angularVelocity = Random.insideUnitSphere * Random.Range(0,2); // rotate randomly
		Vector3 velo = Random.insideUnitSphere * speed();
		velo.y = 0;
		//rb.velocity = transform.forward * speed(); // base movement on blue axis
		rb.velocity = velo;
		Destroy(gameObject, seconds_in_float); // destroy objects automatically - cell death!
		//Debug.Log("START nextReprod= "+ nextReprod +"{"+ reprodRate() +"}");
	}

	// Update player stats if collision
	// cleaner if stats has setters/getters
	public void updateStats(float health, float speed, float defense, float reprodRate, float power) {		
		stats_health += health;
		stats_speed += speed;
		stats_defense += defense;
		stats_reprodRate += reprodRate;
		if (stats_reprodRate <= 0f) stats_reprodRate=1;
		stats_power += power;

		if (stats_health <= 0f) {
			Destroy (gameObject); // No health - dies!
		}
	}

	// This changes the top level
	void addDelay(float sec) {
		bodyStats.delay += sec; // when brain is damaged, all cells are slow to follow command
	}

	public float health () {
		return ((stats_health/100.0f) * bodyStats.health);
	}

	public float defense () {
		return ((stats_defense/100.0f) * bodyStats.defense);
	}

	public float power () {
		return ((stats_power / 100.0f) * bodyStats.power);
	}

	// Lower number is faster
	public float reprodRate () {
		return (bodyStats.reprodRate);
	}


	// Drift speed
	public float speed () { 
		return (stats_speed/100f) * bodyStats.speed;
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
			win= false;
		}
		Debug.Log(gameObject.name+" defends ("+ defending+") against"+ other.name+" health="+ stats_health);
		defending += 1;
		return win;

	}

	// Reproduce once every N seconds - (reprod rate)
	void Update () {
		if (dna != null) {  // Can reproduce
			if (nextReprod == 0f) {
				nextReprod = Time.time + reprodRate ();
			}
			if (Time.time > nextReprod) {
				//GameObject clone =
				if (shotSpawn != null) // dna is the prefab
					Instantiate (dna, shotSpawn.position, shotSpawn.rotation);// as GameObject; 
				else // Pop out a new one away from itself
					Instantiate (dna, transform.position + (Random.insideUnitSphere * Distance), transform.rotation);
				nextReprod = Time.time + reprodRate ();
				//Debug.Log("nextReprod= "+ nextReprod +"{"+ reprodRate() +"}");
			}
		}
		//if (defending>0) {Debug.Log ("Still defending "+defending+" critters");}
	}

	// Both Colliders are called (I think), so only defend is coded
	// Combat rules:
	// 1) White cell attacks pathogen
	// 2) Pathogen attacks red cell
	void OnTriggerEnter(Collider other) {
		if ("Boundary" == other.name || gameObject.tag == other.tag ) { // Same Team
			return;
		}
		//Debug.Log(gameObject.name+"-"+gameObject.tag+" collided with "+ other.name+"="+other.tag);

		CellController othercell = gameObject.GetComponent(typeof(CellController)) as CellController;
		if ((other.tag == "Infection" && gameObject.name == "Red") ||  // Infection attacks Red (me)
			(gameObject.tag == "Infection" && other.name == "White" || other.name == "Player")) // White attacks pathogen (me)
		{
				defendAgainst(othercell);  // defend against white cell or pathogen
		}
	}	

	void OnTriggerExit(Collider other) {
		if ("Boundary" == other.name) {
			//Debug.Log(gameObject.name+" met boundary");
			rb.velocity *= -1;
			rb.position = new Vector3 (
				Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
			rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
			return;
		}
		if (gameObject.tag == other.tag ) { // Same Team
			return;
		}
		//Debug.Log(gameObject.name+" separated from "+ other.name);
		//CellController othercell = gameObject.GetComponent(typeof(CellController)) as CellController;
		if (other.tag == "Infection") { // Infection attacks Red (me)
			if (gameObject.name == "Red") {
				Debug.Log(gameObject.name+" left "+ other.name+" health="+ stats_health);
				defending--;
			}
		}
		else if (gameObject.tag == "Infection") {// White attacks pathogen(me)
			if (other.name == "White" || other.name == "Player") {
				Debug.Log(gameObject.name+" left "+ other.name+" health="+ stats_health);
				defending--;
			}
		}

	}
}
