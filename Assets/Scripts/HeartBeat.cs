using UnityEngine;
using System.Collections;

public class HeartBeat : MonoBehaviour {


	private float delay;
	private float hold;
	private IEnumerator coroutine;
	private BoxCollider bc;
	private MeshRenderer mr;
	// Use this for initialization
	void Start () {
		delay = 2f;
		hold = 1f;
		coroutine = pump ();
		StartCoroutine (coroutine);
		bc = this.GetComponent<BoxCollider> ();
		mr = this.GetComponent<MeshRenderer> ();
	}


	public IEnumerator pump(){
		while (true) {
			yield return new WaitForSeconds (delay);
			bc.enabled = true;
			mr.enabled = true;
			yield return new WaitForSeconds (hold);
			bc.enabled = false;
			mr.enabled = false;
		}
	}


	public void setDelay(float setTo){
		delay = setTo;
	}

	public void setHold(float setTo){
		hold = setTo;
	}

}
