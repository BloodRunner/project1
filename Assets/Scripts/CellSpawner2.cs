using UnityEngine;
using System.Collections;

public class CellSpawner2 : MonoBehaviour {
	public CellController dna;
	private float nextReprod = 0f;
	// Use this for initialization
	void Awake () {
		nextReprod = Time.time + dna.reprodRate ();
		Debug.Log("DNA Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"}");
	}

	// Update is called once per frame
	void Update () {
		if (dna != null) {  // Can reproduce
			if (Time.time > nextReprod) {
				Vector3 v3 = transform.position + (Random.insideUnitSphere * dna.transform.localScale.x);
				v3.y = 0f;
				GameObject clone = Instantiate (dna, v3 , transform.rotation) as GameObject;
				nextReprod = Time.time + dna.reprodRate ();
				//Debug.Log("Red Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"} ");
			}
		}
	}
}