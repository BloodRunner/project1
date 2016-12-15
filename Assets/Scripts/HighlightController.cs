using UnityEngine;
using System.Collections;

public class HighlightController : MonoBehaviour {

	Vector3 mouse;
	Vector3 worldPoint;
	private float range;
	private IEnumerator coroutine;
	//private Camera topCamera;
	private GameObject obj;
	private GameObject objOld;
	private GameObject objCurrent;
	private bool started;
	private RaycastHit hit;
	//int groundMask;  

	// Use this for initialization
	void Start () {
		//topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		//coroutine = mouseHovering ();
		obj = GameObject.Find ("GameController");
		objOld  = GameObject.Find ("GameController");
		objCurrent = GameObject.Find ("GameController");
		//StartCoroutine (coroutine);
		//started = true;
	}


	void Update(){
		//shamelessly copying of Phoebe's code
		if (GameObject.Find ("topCamera").GetComponent<Camera> ().enabled == true) {
			Camera cx = Camera.current;
			Camera[] cxs = Camera.allCameras;
			if (cx == null) {
				cx = cxs [0]; // Why is current null half the time
				//Debug.Log("Camera Current is null!");
			}
			Ray ray = cx.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			//DrawLine (transform.position, ray.GetPoint(30), Color.red, 0.1f);
			if (Physics.Raycast (ray, out hit)) {
				//Debug.DrawLine(ray,hit);
				Vector3 adjusted = ray.GetPoint (hit.distance - 0.1f);
				Vector3 end = new Vector3 (adjusted.x, 0.1f, adjusted.z);
				//GameObject.Find ("TheGrandFollower").transform.position = end;

				Collider[] search = Physics.OverlapSphere (end, 3f);
				range = 3;
				objOld = objCurrent;
				if (search.Length > 0) {
					for (int i = 0; i < search.Length; i++) {
						obj = search [i].gameObject;
						if (obj.name == "White" || obj.name == "KillerT") {
							if (Vector3.Distance (end, search [i].transform.position) < range) {
								//print ("Ping");
								range = Vector3.Distance (end, search [i].transform.position);
								objCurrent = obj;
							}
						}
					}
				}
				if (objOld != null && (objOld.name == "White" || objOld.name == "KillerT")) {
					if (objOld != objCurrent || Vector3.Distance (end, objOld.transform.position) > 3f) {
						objOld.GetComponent<Light> ().range = 0.3f;
						objOld.GetComponent<Light> ().enabled = false;
						//print ("rob");
					}
				}
				if (objCurrent != null && (objCurrent.name == "White" || objCurrent.name == "KillerT")) {
					if (Vector3.Distance (end, obj.transform.position) <= 3f) {
						objCurrent.GetComponent<Light> ().range = 3f;
						objCurrent.GetComponent<Light> ().enabled = true;
						//print ("sob");
					}
				}

			}
			if (Input.GetMouseButtonDown (0)) {
				if (objCurrent != null && (objCurrent.name == "White" || objCurrent.name == "KillerT")) {
					if (cx == null) {
						cx = cxs [0];
					}
					if (Physics.Raycast (ray, out hit)) {
						Vector3 adjusted = ray.GetPoint (hit.distance - 0.1f);
						Vector3 end = new Vector3 (adjusted.x, 0.1f, adjusted.z);
						//GameObject.Find ("TheGrandFollower").transform.position = end;
						if (Vector3.Distance (end, objCurrent.transform.position) < 3) {
							//print (objCurrent.name);
							objCurrent.GetComponent<CameraChange> ().startFollow ();
							objCurrent.GetComponent<Light> ().range = 0.3f;
						}
					}
				}
			}


		} else {
			if (Input.GetMouseButtonDown (1) || Input.GetKeyDown(KeyCode.Space)) {
				objCurrent.GetComponent<CameraChange> ().stopCamera ();
			}
		}
			
	}

}
