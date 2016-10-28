using UnityEngine;
using System.Collections;

public class cellSpawner : MonoBehaviour {
	public CellController dna;
	private float nextReprod = 0f;
	private const float distance = 2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Instantiate (dna, transform.position + (Random.insideUnitSphere * distance), transform.rotation);
		if (dna != null) {  // Can reproduce
			if (nextReprod == 0f) {
				nextReprod = Time.time + dna.reprodRate ();
			}
			if (Time.time > nextReprod) {
				//GameObject clone =
				Instantiate (dna, transform.position + (Random.insideUnitSphere * dna.transform.localScale.x), transform.rotation);
				nextReprod = Time.time + dna.reprodRate ();
				//Debug.Log("nextReprod= "+ nextReprod +"{"+ reprodRate() +"}");
			}
		}
	}
}
