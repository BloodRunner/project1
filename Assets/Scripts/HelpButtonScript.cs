using UnityEngine;
using System.Collections;

public class HelpButtonScript : MonoBehaviour
{
	private RaycastHit hit;
	private Ray ray;
	Ray shootRay;
	int buttonMask; // Only care for Organ sword/shield buttons
	int organLayer;
	//Color white = new Color(255,255,255);
	//Color black = new Color(0,0,0);
	float timer = 0f;
	void Awake ()
	{
		buttonMask = LayerMask.GetMask ("SwordShield") ;
		organLayer = LayerMask.NameToLayer ("Organ");

	}

	// Button click
	void flash (GameObject button) {
		SpriteRenderer sr;
		sr = button.GetComponent<SpriteRenderer> ();
		StartCoroutine(FlashSprite(sr));
	}

	IEnumerator FlashSprite(SpriteRenderer sr)
	{
		sr.enabled = false;
		yield return new WaitForSeconds(.2f);
		sr.enabled = true;
	}

	void Update ()
	{
		timer += Time.deltaTime;
		if (Input.GetButton ("Fire1")) {
			Camera cx = Camera.current;
			Camera[] cxs = Camera.allCameras;
			if (cx == null) {
				cx = cxs [0]; 
			}
			Ray ray = cx.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			string callMesg = "";
		
			if (Physics.Raycast (ray, out hit, 1000, buttonMask)) {
				//Debug.Log ("Help Button Script" + hit.collider.tag);
				if (hit.collider.tag.Equals ("UI")) {
					GameObject button = hit.collider.gameObject;
					if (button.name.Equals("CallButton")) 
						callMesg="callForSupport";
					else
						Debug.Log ("button " + button.name + " fired for support ");
					GameObject parent = button.transform.parent.gameObject;
					if (parent.layer == organLayer) {	
						if (timer > 2f) {
							parent.SendMessage (callMesg);
							Debug.Log ("Hit organ " + parent.name + " " + callMesg);
						}
					}
					flash (button);
				}
				timer = 0;
			}
		}
	}
			
}
