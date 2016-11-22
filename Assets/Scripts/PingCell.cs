using UnityEngine;
using System.Collections;

public class PingCell : MonoBehaviour {

	private Light myLight;
	private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		myLight = GetComponent<Light> ();
		coroutine = PingPing ();
	}

	private IEnumerator PingPing(){
		myLight.range = 3f;
		myLight.enabled = true;
		for(int i = 0; i < 5; i ++){
			myLight.color -= Color.red / 2.0F * Time.deltaTime;
			yield return new WaitForSeconds (0.1f);
		}
		myLight.enabled = false;
		StopCoroutine (coroutine);
	}
	
	public void PingIt(){
		StartCoroutine (coroutine);
	}
}
