using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {
	private Rigidbody rb;
	public float tumble;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.angularVelocity = Random.insideUnitSphere * tumble;
			//new Vector3 (1.4f, .2f, 5.7f);
	}
	
	// Update is called once per frame
	//void Update () {}
}
