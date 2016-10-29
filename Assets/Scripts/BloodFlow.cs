using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {

	public float pressureMulty;
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
	}
}
