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

	public float speed;  	// base movement speed
	public float tilt;
	public Boundary boundary; // Game boundary - if outside boundary - no control
	public GameObject dna; // What to spawn
	public Transform shotSpawn; // the transform for origin of the offspring spawn
	public Stats bodyStats; // configured stats for the cell body;

	protected Stats stats; // percentage of the body stats - from damage
	protected Rigidbody rb;
	private float nextReprod=Time.time;
	private int defending = 0;
	private float seconds_in_float = 2000; // Cell life span in float
	protected float stats_health=100.0f;
	protected float stats_speed = 100.0f;
	protected float stats_defense = 100.0f;
	protected float stats_reprodRate = 100.0f;
	protected float stats_power = 100.0f;
	protected float stats_delay = 0f;
	protected int stats_level = 1;

	void Start() {
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed; // base movement
		/*
		stats = new Stats();   // Create base stats
		stats.health=100.0f;
		stats.speed = 100.0f;
		stats.defense = 100.0f;
		stats.reprodRate = 100.0f;
		stats.power = 100.0f;
		stats.level = 1;
*/
		Destroy(gameObject, seconds_in_float); // destroy objects automatically - cell death!
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

	public float movementSpeed () {
		return speed - stats_delay;
	}

	// This cell defend against another
	public bool defend (CellController other) {
		float combat = other.power () - defense ();
		if (combat <= 0) { // successful defense against the others
			other.updateStats (combat, 0.0f, -1.0f, 0.0f, 0.0f);
			updateStats (0.0f, 0.0f, -1.0f, 0.0f, 0.0f);
			return true;
		} else { // Lost
			updateStats (-combat, 0f, -1.0f, 0.0f, 0.0f);
			other.updateStats (0.0f, 0f, -1f, 0f, 0f);
			return false;
		}
	}

	// Reproduce once every N seconds - (reprod rate)
	void Update () {
		if (Time.time > nextReprod) {
			//GameObject clone =
			if (shotSpawn!=null) // dna is the prefab
				Instantiate (dna, shotSpawn.position, shotSpawn.rotation);// as GameObject; 
			else // Pop out a new one away from itself
				Instantiate (dna, transform.position+( Random.insideUnitSphere * Distance ), transform.rotation);
			nextReprod = Time.time + reprodRate();
			Debug.Log("nextReprod= "+ nextReprod +"{"+ reprodRate() +"}");
		}
		if (defending>0) {Debug.Log ("Still defending "+defending+" critters");
			
		}
	}

	void FixedUpdate () {
		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");
		Vector3 movement =new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}   


	// Both Colliders are called (I think), so only defend is coded
	// Combat rules:
	// 1) White cell attacks pathogen
	// 2) Pathogen attacks red cell
	void OnTriggerEnter(Collider other) {
		if (gameObject.tag == other.tag ) { // Same Team
			return;
		}
		CellController othercell = gameObject.GetComponent(typeof(CellController)) as CellController;
		if (other.tag == "Infection") { // Infection attacks Red (me)
			if (gameObject.name == "Red") {
				defend (othercell); // defend against infection
				Debug.Log(gameObject.name+" defends against "+ other.name);
				defending += 1;
			}
		}
		else if (gameObject.tag == "Infection") {// White attacks pathogen(me)
			if (other.name == "White") {
				defend (othercell);  // defend against white cell
				Debug.Log(gameObject.name+" defends against "+ other.name);
				defending += 1;
			}
		}
	}	

	void OnTriggerExit(Collider other) {
		if (gameObject.tag == other.tag ) { // Same Team
			return;
		}
		//CellController othercell = gameObject.GetComponent(typeof(CellController)) as CellController;
		if (other.tag == "Infection") { // Infection attacks Red (me)
			if (gameObject.name == "Red") {
				Debug.Log(gameObject.name+" left "+ other.name);
				defending--;
			}
		}
		else if (gameObject.tag == "Infection") {// White attacks pathogen(me)
			if (other.name == "White") {
				Debug.Log(gameObject.name+" left "+ other.name);
				defending--;
			}
		}
	}	
}
