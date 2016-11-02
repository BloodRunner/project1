using UnityEngine;
using System.Collections;

public class RedSpawner : MonoBehaviour {
	public RedController dna;
	public GameController gameController;
	public BodyState bodystate;
	private float nextReprod = 0f;
	// Use this for initialization
	void Awake () {
		nextReprod = Time.time + dna.reprodRate ();
		Debug.Log("Red DNA Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"}");
	}

	// Update is called once per frame
	void Update () {
		RedController cell=null;
		if (dna != null) {  // Can reproduce
			if (Time.time > nextReprod) {
				
				Vector3 v3 = transform.position + (Random.insideUnitSphere * dna.transform.localScale.x);
				v3.y = 1f;
				nextReprod = Time.time + dna.reprodRate ();

				try {
					cell = Instantiate (dna, v3, transform.rotation) as RedController;
					if (!bodystate) {
						Debug.LogError ("BodyState is missing in RedSpawnwer");
					}
					cell.setBodyState (bodystate);
					cell.gameController = gameController;
				
				}catch(System.Exception) {
					Debug.LogError (name + " Instantiate failed in RedSpawnwer");
				}

				//Debug.Log("Red Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"} ");
			}
		}
	}
}