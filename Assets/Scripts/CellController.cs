using UnityEngine;
using System.Collections;

/* This allows Unity UI to see class and show it
 * */
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

[System.Serializable]
public class Stats
{   // base level multiplied by ( 0 - 100% of base level )
	public int health, speed, defense, reprodRate;
	public int power; // attack if white cell or infection, oxygenation if red cell
	public int delay; // Pause before it can follow a command
	public int level;
}

public class CellController : MonoBehaviour {
	public Rigidbody rb;
	public float speed;  	// base movement speed
	public float tilt;
	public Boundary boundary; // Game boundary - if outside boundary - no control
	public GameObject shot;
	public Stats bodyStats; // configured stats for the critter;
	private Stats stats; // percentage of the body stats - from damage
	// public Transform shotSpawn; // is the transform

	void Start() {
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed; // base movement

		stats = new Stats();   // Create base stats
		stats.health=100;
		stats.speed = 100;
		stats.defense = 100;
		stats.reprodRate = 100;
		stats.power = 100;
		stats.level = 1;
		// Destroy(gameObject, seconds_in_float); // destroy objects automatically
	}

	// Update player stats if collision
	// cleaner if stats has setters/getters
	public void updateStats(int health, int speed, int defense, int reprodRate, int power) {		
			stats.health += health;
			stats.speed += speed;
			stats.defense += defense;
			stats.reprodRate += reprodRate;
			stats.power += power;
	}
	void addDelay(int sec) {
		stats.delay += sec;
	}
	public int health () {
		return (int)((float)(stats.health/100.0f) * (float)bodyStats.health);
	}

	public int defense () {
		return (int)((float)(stats.defense/100.0f) * (float)bodyStats.defense);
	}
	public int power () {
		return (int)((float)(stats.power / 100.0f) * (float)bodyStats.power);
	}

	public float reprodRate () {
		return ((float)(stats.reprodRate/100.0f) * (float)bodyStats.reprodRate);
	}

	public float movementSpeed () {
		return speed - stats.delay;
	}

	// Update is called once per frame
	// Save this for generating attack later
	void Update () {
		/*
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
//			GameObject clone =
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);// as GameObject; 
			GetComponent<AudioSource>().Play ();
		}
		*/

	}
		
}
