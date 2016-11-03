using UnityEngine;
using System.Collections;

public class RandomMove : MonoBehaviour {

	public Rigidbody rb;
	public float interval = 1f;
	public float erratic = 1f;

	// Use this for initialization
	void Start () {
		//rb = GetComponent<Rigidbody>();
		//StartCoroutine(BrownianMotion ());
	}

	// Update is called once per frame
	void Update () {
		Vector3 randomMove = new Vector3 (Random.Range(interval * -1,interval), 0, Random.Range(interval * -1,interval));
		transform.position += randomMove * erratic * Time.deltaTime;
	}


	/*private IEnumerator BrownianMotion (){
		while (true) {
			this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (Random.Range (0, erratic), 0, Random.Range (0, erratic)),interval);
			yield return new WaitForSeconds (Random.Range(interval -1f,interval +1f));
		}	
	}*/
}
