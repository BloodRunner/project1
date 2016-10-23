using UnityEngine;
using System.Collections;

public class DamageByContact : MonoBehaviour {
	//public GameObject explosion;
	public int scoreValue;
	private GameController gameController;

	void Start() {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject!=null) 
			{
				gameController = gameControllerObject.GetComponent <GameController>();
			}
			if (gameController == null)
			{
				Debug.Log ("Cannot find 'GameController' script");
			}
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.tag == "boundary") {
			return;
		}
		if (gameObject.name == other.name || gameObject.tag == other.tag ) { // Same Team
			return;
		}
		if (gameObject.tag == "Host" && other.tag == "Infection") { 
			// For cells - white attacks Infection
			// Infection attacks Red and Organ
		}
		if (gameObject.tag == "Infection" && other.tag == "Host") {
			CellController infect = gameObject.GetComponent(typeof(CellController)) as CellController;
			if (other.name == "Organ") {
				OrganController organ = other.GetComponent(typeof(OrganController)) as OrganController;
				organ.updateStats (-infect.power(), 0, 0);
			}
			else {
				CellController bodycell = other.GetComponent(typeof(CellController)) as CellController;
				if (infect.power() > bodycell.defense()) { // White cell or red cell
					bodycell.updateStats (infect.power (), 0, 0, 0, 0);
				}
			}
		}
		// TODO: Score
	}
}
