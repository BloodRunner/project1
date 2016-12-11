using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed = 2.0f;
	private GameObject[] directionals;
	private string[] waypoints;

	void Awake (){
		directionals = GameObject.FindGameObjectsWithTag("directionals");
	}





	/*void Update(){
		Vector3 move = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		transform.position += move * speed * Time.deltaTime;
	}*/

}
