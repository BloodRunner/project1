﻿using UnityEngine;
using System.Collections;

public class PlayerController : WhiteController {
	

	// Make banked turns if moved by player - 3D right now
	void FixedUpdate () {
		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");
		Vector3 movement =new Vector3(moveHorizontal, 0.0f, moveVertical);
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.velocity = movement * (playerSpeed-stats_delay); // delayed by brain damage
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
		//Debug.Log("movex = "+ moveHorizontal); // inheritance works!
	}

}
