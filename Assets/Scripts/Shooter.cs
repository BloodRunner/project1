using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {
	ParticleSystem weapon;
	private RaycastHit hit;
	private Ray ray;

	void Awake() {
		weapon = GetComponent<ParticleSystem>();
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Fire1")) { 
			
			weapon.Play ();

			//audio.Play ();

			Vector2 screenCenterPoint = new Vector2 (Screen.width / 2, Screen.height / 2);

			//ray = Camera.main.ScreenPointToRay (screenCenterPoint);

			if (Physics.Raycast (ray, out hit, 5)) {
				/*
				Vector3 targetPoint = ray.GetPoint(hit);
			//	Vector3 distance = Vector3.Distance(hit.transform.position, transform.position); // Within shooting range?
				Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
				transform.rotation = targetRotation;
				Debug.Log ("Weapon hits " + hit.collider.tag);
				Vector3 bulletHolePosition = hit.point + hit.normal * 0.01f;
*/
				//Quaternion bulletHoleRotation = Quaternion.FromToRotation (-Vector3.forward, hit.normal);

				//GameObject hole = (GameObject)GameObject.Instantiate (bulletHolePrefab, bulletHolePosition, bulletHoleRotation);
			}
		}
	}
		
}
