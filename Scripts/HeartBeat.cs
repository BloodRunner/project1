using UnityEngine;
using System.Collections;

public class HeartBeat : MonoBehaviour {


	private float delay;
	private float hold;
	private IEnumerator coroutine;
	// Use this for initialization
	void Start () {
		delay = 2f;
		hold = 1f;
		coroutine = pump ();
		StartCoroutine (coroutine);
	}


	public IEnumerator pump(){
		while (true) {
			this.GetComponent<BoxCollider> ().enabled = false;
			this.GetComponent<MeshRenderer> ().enabled = false;
			yield return new WaitForSeconds (delay);
			this.GetComponent<BoxCollider> ().enabled = true;
			this.GetComponent<MeshRenderer> ().enabled = true;
			yield return new WaitForSeconds (hold);
			this.GetComponent<BoxCollider> ().enabled = false;
			this.GetComponent<MeshRenderer> ().enabled = false;
		}
	}


	public void setDelay(float setTo){
		delay = setTo;
	}

	public void setHold(float setTo){
		hold = setTo;
	}

}
