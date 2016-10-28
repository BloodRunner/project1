using UnityEngine;
using System.Collections;

public class RandomMove : MonoBehaviour {

	public Rigidbody rb;
	public float interval = 3f;
	public float erratic = 4f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		StartCoroutine(BrownianMotion ());
	}

	// Update is called once per frame
	void Update () {

	}


	private IEnumerator BrownianMotion (){
		while (true) {
			rb.AddForce (new Vector3(Random.Range (erratic * -1f, erratic),0, Random.Range (erratic * -1f, erratic)));
			yield return new WaitForSeconds (Random.Range(interval -0.1f,interval +0.1f));
		}	
	}
}
