using UnityEngine;
using System.Collections;

public class PlayerController : CellController {
	
	// Make banked turns if moved by player
	void FixedUpdate () {
		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");
		Vector3 movement =new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
		//Debug.Log("health = "+ health()); // inheritance works!
	}
		

}
