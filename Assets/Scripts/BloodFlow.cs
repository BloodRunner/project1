using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {

	public float pressureMulty;
	public Vector3 target;

	void OnTriggerStay(Collider blood){
		if (blood.CompareTag ("Cell")|blood.CompareTag ("Host")|blood.CompareTag ("Infection")) {
			blood.transform.position = Vector3.MoveTowards (blood.transform.position, target, GameObject.Find("GameController").GetComponent<GameController>().pressure * pressureMulty);

		}
	}
}
