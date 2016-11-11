using UnityEngine;
using System.Collections;

public class AttackCell : MonoBehaviour {

	private IEnumerator coroutine;
	public float range;
	public GameObject target;
	public bool hasTarget;
	private float thex;
	private float thez;
	private Vector3 dest;
	private float dist;
	private float speed;
	Collider[] collisions;
	// Use this for initialization
	void Start () {
		hasTarget = false;
		target = null;
		range = 2f;
		speed = 1f;
		coroutine = Hunting ();
		StartCoroutine (coroutine);
	}


	private IEnumerator Hunting(){
		while (true) {
			collisions = Physics.OverlapSphere (this.GetComponent<Transform> ().position, range);
			dist = 10f;
			for (int i = 0; i < collisions.Length; i++) {
				//print (collisions [i].tag);
				if (Vector3.Distance (collisions [i].GetComponent<Transform> ().position, this.GetComponent<Transform> ().position) < dist) {
					if (collisions [i].CompareTag ("Host")) {
						dist = Vector3.Distance (collisions [i].GetComponent<Transform> ().position, this.GetComponent<Transform> ().position);
						target = collisions [i].gameObject;
						//print ("target");
					}
				}
			}
			yield return new WaitForSeconds (0.3f);
		}
	}

	void Update(){
		if(target!=null){
			if ((this.GetComponent<Transform> ().position.x - target.GetComponent<Transform> ().position.x) >= 0f) {
				thex = speed * -1f;
			} else {
				thex = speed;
			}
			if ((this.GetComponent<Transform> ().position.z - target.GetComponent<Transform> ().position.z) >= 0f) {
				thez = speed * -1f;
			} else {
				thez = speed;
			}
			dest = new Vector3 (thex,0,thez);
			this.transform.position += dest * speed * Time.deltaTime;
		}
	}


}
