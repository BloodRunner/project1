using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {

	public Vector3[] points;
	private int destPoint;
	private NavMeshAgent agent;
	private IEnumerator coroutine;
	private float delay;

	void Start () {
		points = new Vector3[4];
		points [0] = new Vector3(15.24f,0f,-7.84f); // Vector3(15.24f,0f,-7.84f);
		points [1] = new Vector3(14.77f,0f,8.24f); //Vector3(14.77f,0f,8.24f);
		points [2] = new Vector3(0.26f,0f,0.23f) ;//Vector3(0.26f,0f,0.23f);
		points [3] = new Vector3(1.01f,0f,-16f) ;//Vector3(1.01f,0f,-16f);
		delay = 0;
		destPoint = 0;
		agent = GetComponent<NavMeshAgent> ();
		agent.autoBraking = false;
		coroutine = patrol ();
		StartCoroutine (coroutine);
	}



	void BloodFlowing(){
		if (points.Length == 0)
			return;
		else if (destPoint == points.Length - 1) {
			agent.destination = points [destPoint];
			print (agent.destination.ToString());
			destPoint = 0;

		} else {
			agent.destination = points [destPoint];
			print (agent.destination.ToString());
			destPoint = (destPoint + 1);
		}

	}

	public IEnumerator patrol(){
		while (true) {
			if (agent.remainingDistance < 0.5f) {
				BloodFlowing ();
				yield return new WaitForSeconds (0.1f);
			} else {
				yield return new WaitForSeconds (0.1f);
			}
		}
	}

	public float getDelay(){
		return delay;
	}

	public void setDelay(float setTo){
		delay = setTo;
	}

	public void setAPatrollPoint(Vector3 thePoint, int place){
		points [place] = thePoint;
	}

	//inserts a patroll point at the designated part of the list and pushes everything at and following that point back.
	public void insertPatrollPoint(Vector3 thePoint, int place){
		Vector3[] newpoints = new Vector3[points.Length + 1];
		for (int i = 0; i < newpoints.Length; i++) {
			if (i == place) {
				newpoints [place] = thePoint;
			} else if (i >= place) {
				newpoints [i] = points [i-1];
			} else {
				newpoints [i] = points [i];
			}
		}
		points = newpoints;
	}

	public void removePoint(int place){
		Vector3[] newpoints = new Vector3[points.Length - 1];
		for (int i = 0; i < points.Length -1; i++){
			if (i >= place) {
				newpoints [i - 1] = points [i];
			} else {
				newpoints [i] = points [i];
			}
		}
		points = newpoints;
	}




	/*
	public Vector3 target1;
	public Vector3 target2;
	public Vector3 target3;
	public float chance1;
	public float chance2;
	public float chance3;
	private float random;
	public BoxCollider interaction;


	//the most not elegant way of dealing with this.
	void OnTriggerStay(Collider collision) {
		
		if (collision.CompareTag ("Host") || collision.CompareTag ("Infection")) {
			random = Random.Range (0,100);
			if(random <= chance1){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target1);
			}
			else if(random <= chance2){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target2);
			}
			else if(random <= chance3){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target3);
			}
		}
	}
	*/

}
