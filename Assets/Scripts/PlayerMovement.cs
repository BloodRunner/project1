using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 1.0f;
	//NavMeshAgent agent;

	void Start() {


		//agent = GetComponent<NavMeshAgent>();
	}
	void Update() {

		if (Input.GetKeyDown (KeyCode.W)) {
			if (this.GetComponent<Rigidbody> ().velocity.z <= speed)
				this.GetComponent<Rigidbody> ().AddForce (0, 0, speed * .1f);
		}
			
		if (Input.GetKeyDown (KeyCode.S)) {
			if (this.GetComponent<Rigidbody> ().velocity.z >= speed * -1)
				this.GetComponent<Rigidbody> ().AddForce (0, 0, speed * .1f * -1);
		}
			
		if (Input.GetKeyDown (KeyCode.A)) {
			if (this.GetComponent<Rigidbody> ().velocity.x >= speed * -1)
				this.GetComponent<Rigidbody> ().AddForce (speed*.1f*-1,0,0);
		}
			
		if (Input.GetKeyDown (KeyCode.D)) {
			if (this.GetComponent<Rigidbody> ().velocity.x <= speed)
				this.GetComponent<Rigidbody> ().AddForce (speed * .1f,0,0);
		}
			
		/*if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;

			if (Physics.Raycast(Camera.current.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				agent.destination = hit.point;
			}
		}*/
		//Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		//transform.position += move * speed * Time.deltaTime;
	}


}
