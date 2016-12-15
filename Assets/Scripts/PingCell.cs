using UnityEngine;
using System.Collections;

public class PingCell : MonoBehaviour {

	private Light myLight;
	private IEnumerator coroutine;
	private IEnumerator coroutine2;
	private BloodFlow me;

	// Use this for initialization
	void Start () {
		myLight = GetComponent<Light> ();
		coroutine = PingPing ();
		coroutine2 = defendPing ();
		me = this.gameObject.GetComponent<BloodFlow> ();
	}

	private IEnumerator PingPing(){
		myLight.range = 3f;
		myLight.enabled = true;
		for(int i = 0; i < 10; i ++){
			myLight.color -= Color.red / 2.0F * Time.deltaTime;
			yield return new WaitForSeconds (0.1f);
		}
		myLight.enabled = false;
		StopCoroutine (coroutine);
	}

	private IEnumerator defendPing(){
		while(true){
			myLight.range = 1.5f;
			myLight.enabled = true;
			myLight.color = Color.green;
			if(me.onADefend() == false){
				myLight.enabled = false;
				myLight.color = Color.white;
				myLight.range = 0.3f;
			}
			yield return new WaitForSeconds (1.5f);
		}
	}
	
	public void PingIt(){
		StartCoroutine (coroutine);
	}
}
