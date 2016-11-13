using UnityEngine;
using System.Collections;

public class WhiteSpawner : MonoBehaviour
{
	public WhiteController dna;
	public BodyState bodystate;
	public GameController gameController;
	private float nextReprod = 0f;

	void Start ()
	{
		if (gameController==null)
			gameController = GameObject.FindObjectOfType (typeof(GameController)) as GameController;
		if (bodystate == null)
			bodystate = GameObject.FindObjectOfType (typeof(BodyState)) as BodyState;
		if (dna != null)
			makeone ();
		
	}

	void makeone ()
	{
		try {
			Vector3 v3 = transform.position + (Random.insideUnitSphere * dna.transform.localScale.x);
			v3.y = 1f;
			WhiteController cell = Instantiate (dna, v3, transform.rotation) as WhiteController;
			cell.setBodyState (bodystate);
			cell.updateHealthStats (100 - bodystate.whiteHealth ());
			cell.gameController = gameController;
			nextReprod = Time.time + cell.get_bodystats_reprod () * bodystate.whiteReprodRate ();
			//Debug.Log (cell.name + " Instantiated in Whitespawner");
		} catch {
			Debug.LogError ("Can't spawn in WhiteSpawnwer");
			nextReprod = Time.time + 2;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (dna != null) {  // Can reproduce
			if (Time.time > nextReprod && ! gameController.isGameOver()) {
				makeone ();
			}
		}
	}
}
