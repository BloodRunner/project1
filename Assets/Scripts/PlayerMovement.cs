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
			
		}
			
		if (Input.GetKeyDown (KeyCode.S)) {

		}
			
		if (Input.GetKeyDown (KeyCode.A)) {

		}
			
		if (Input.GetKeyDown (KeyCode.D)) {

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
