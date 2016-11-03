using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {

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




	/* public float pressureMulty;
	public Vector3 target;
	public float xSpeed;
	public float zSpeed;
	public float maxSpeed;

	void OnTriggerStay(Collider blood){
		if (blood.CompareTag ("Cell")|blood.CompareTag ("Host")|blood.CompareTag ("Infection")) {
			if(blood.attachedRigidbody.velocity.magnitude <= maxSpeed){
				blood.attachedRigidbody.AddForce (xSpeed * pressureMulty, 0f, zSpeed * pressureMulty);
			}
		}
	} */
}
