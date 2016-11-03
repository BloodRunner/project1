using UnityEngine;
using System.Collections;

public class WhiteSpawner : MonoBehaviour {
	public WhiteController dna;
	public BodyState bodystate;
	public GameController gameController;
	private float nextReprod = 0f;

	void Awake () {
		nextReprod = Time.time + dna.reprodRate ();
		Debug.Log("DNA Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"}");
	}

	// Update is called once per frame
	void Update () {
		if (dna != null) {  // Can reproduce
			if (Time.time > nextReprod) {
				Vector3 v3 = transform.position + (Random.insideUnitSphere * dna.transform.localScale.x);
				v3.y = 1f;
				WhiteController cell;
				try {
					cell = Instantiate (dna, v3, transform.rotation) as WhiteController;

					//cell.velocity = transform.forward * 10;
					if (!cell) {
						Debug.LogError ("Controller is missing in WhiteSpawnwer");
					} else
						cell.setBodyState (bodystate);
				}
				catch {
					Debug.LogError ("Can't spawn in WhiteSpawnwer");
				}

				if (!bodystate) {
					Debug.LogError ("BodyState is missing in WhiteSpawnwer");
				} else {
				//	cell.setBodyState (bodystate);
				//	cell.gameController = gameController;
				}
				nextReprod = Time.time + dna.reprodRate ();
				//Debug.Log("Red Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"} ");
			}
		}
	}
}
