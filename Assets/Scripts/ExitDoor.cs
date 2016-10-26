using UnityEngine;
using System.Collections;

public class ExitDoor : MonoBehaviour {

	private bool open;
	public float pullForce = 1;
	public Light lt;
	public BoxCollider bc;

	// Use this for initialization
	void Start () {
		lt = GetComponent<Light> ();
		bc = GetComponent<BoxCollider> ();
		open = false;
	}

	void OnMouseDown() {
		print ("clicked");
		changeState ();
	}

	void changeState(){
		if (open == false) {
			open = true;
			lt.color = Color.green;
			bc.isTrigger = true;
		} else {
			open = false;
			lt.color = Color.red;
			bc.isTrigger = false;
		}
	}
}
