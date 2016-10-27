using UnityEngine;
using System.Collections;

public class cellSpawner : MonoBehaviour {
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
				GameObject clone = Instantiate (dna, transform.position + (Random.insideUnitSphere * dna.transform.localScale.x), transform.rotation) as GameObject;
				nextReprod = Time.time + dna.reprodRate ();
				//Debug.Log("Red Reprod= "+ nextReprod +"{"+ dna.reprodRate() +"} ");
			}
		}
	}
}
