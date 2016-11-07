using UnityEngine;
using System.Collections;

public class RedSpawner : MonoBehaviour
{
	public RedController dna;
	public GameController gameController;
	public BodyState bodystate;
	private float nextReprod = 0f;
	// Use this for initialization
	void Start ()
	{
		if (dna != null)
			makeone ();
	}

	void makeone ()
	{
		try {
			Vector3 v3 = transform.position + (Random.insideUnitSphere * dna.transform.localScale.x);
			v3.y = 1f;
			RedController cell = Instantiate (dna, v3, transform.rotation) as RedController;
			cell.setBodyState (bodystate);
			cell.updateHealthStats (100 - bodystate.redHealth ());
			cell.gameController = gameController;
			if (gameController == null || bodystate == null)
				Debug.LogError (name +" bodystate and gamecontroller are null !!!");
			nextReprod = Time.time + cell.get_bodystats_reprod () * bodystate.redReprodRate ();
			Debug.Log (cell.name + " Instantiated in RedSpawner");
		} catch (System.Exception) {
			Debug.LogError (name + " Instantiate failed in RedSpawnwer");
			nextReprod = Time.time + 2; // Try again in 2 seconds if it failed
		}

	}

	// Update is called once per frame
	void Update ()
	{
		CellController cell = null;
		if (dna != null) {  // Can reproduce
			if (Time.time > nextReprod) {
				makeone ();
				//Debug.Log("Red Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"} ");
			}
		}
	}
}