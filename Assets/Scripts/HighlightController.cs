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
	private bool started;
	private RaycastHit hit;
	//int groundMask;  

	// Use this for initialization
	void Start () {
		//topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		//coroutine = mouseHovering ();
		obj = GameObject.Find ("White");
		objOld  = GameObject.Find ("White");
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
				Collider[] search = Physics.OverlapSphere (end, 3f);
				range = 3;
				objOld = obj;
				if (search.Length > 0) {
					for (int i = 0; i < search.Length; i++) {
						obj = search [i].gameObject;
						if (obj.name == "White" || obj.name == "KillerT") {
							if (Vector3.Distance (end, search [i].transform.position) < range) {
								//print ("Ping");
								range = Vector3.Distance (end, search [i].transform.position);
							}
						}
					}
				}
				if (objOld.name == "White" || objOld.name == "KillerT") {
					if (objOld != obj || Vector3.Distance (end, objOld.transform.position) > 3f) {
						objOld.GetComponent<Light> ().range = 0.3f;
						objOld.GetComponent<Light> ().enabled = false;
						//print ("rob");
					}
				}
				if (obj.name == "White" || obj.name == "KillerT") {
					if (Vector3.Distance (end, obj.transform.position) <= 3f) {
						obj.GetComponent<Light> ().range = 3f;
						obj.GetComponent<Light> ().enabled = true;
						//print ("sob");
					}
				}
			}
		}
	}


	void OnMouseDown(){
		if(Input.GetMouseButtonDown(0)){
			Camera cx = Camera.current;
			Camera[] cxs = Camera.allCameras;
			if (cx == null) {
				cx = cxs [0];
			}
			Ray ray = cx.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Vector3 adjusted = ray.GetPoint (hit.distance - 0.1f);
				Vector3 end = new Vector3 (adjusted.x, 0.1f, adjusted.z);
				if(Vector3.Distance (end, obj.transform.position) < range){
					obj.GetComponent<CameraChange> ().startFollow();
				}
			}
		}
	}

	// Update is called once per frame
	/*void Update () {
		if (GameObject.Find ("topCamera").GetComponent<Camera>().enabled == true) {
			mouse = new Vector3 (Input.mousePosition.x,Input.mousePosition.y,20.3f );
			worldPoint = topCamera.ScreenToWorldPoint (mouse);
			worldPoint = new Vector3 (worldPoint.x, 0.0f, worldPoint.z);
			//print (worldPoint.ToString ());
			Collider[] search = Physics.OverlapSphere (worldPoint, 3f);
			//print (search.Length);
			range = 3;
			objOld = obj;
			if (search.Length > 0) {
				for (int i = 0; i < search.Length; i++) {
					obj = search [i].gameObject;
					if (obj.name == "White" || obj.name == "KillerT") {
						if (Vector3.Distance (worldPoint, search [i].transform.position) < range) {
							//print ("Ping");
							range = Vector3.Distance (worldPoint, search [i].transform.position);
						}
					}
				}
			}
			//print ("dob");
			if (objOld.name == "White" || objOld.name == "KillerT") {
				if (objOld != obj || Vector3.Distance (worldPoint, objOld.transform.position) > 3f) {
					objOld.GetComponent<Light> ().range = 0.3f;
					objOld.GetComponent<Light> ().enabled = false;
					//print ("rob");
				}
			}
			if (obj.name == "White" || obj.name == "KillerT") {
				if (Vector3.Distance (worldPoint, obj.transform.position) <= 3f) {
					obj.GetComponent<Light> ().range = 3f;
					obj.GetComponent<Light> ().enabled = true;
					//print ("sob");
				}
			}
		} else {

		}
		//print (started.ToString());
		//print (GameObject.Find ("topCamera").GetComponent<Camera> ().enabled.ToString ());
	}*/

	/*public IEnumerator mouseHovering(){
		while (true) {
			started = true;
			mouse = new Vector3 (Input.mousePosition.x,Input.mousePosition.y,20.3f );
			worldPoint = topCamera.ScreenToWorldPoint (mouse);
			worldPoint = new Vector3 (worldPoint.x, 0.0f, worldPoint.z);
			print (worldPoint.ToString ());
			Collider[] search = Physics.OverlapSphere (worldPoint, 3f);
			print (search.Length);
			range = 3;
			objOld = obj;
			if (search.Length > 0) {
				for (int i = 0; i < search.Length; i++) {
					obj = search [i].gameObject;
					if (obj.name == "White" || obj.name == "KillerT") {
						if (Vector3.Distance (worldPoint, search [i].transform.position) < range) {
							print ("Ping");
							range = Vector3.Distance (worldPoint, search [i].transform.position);
						}
					}
				}
			}
			print ("dob");
			if (objOld.name == "White" || objOld.name == "KillerT") {
				if (objOld != obj || Vector3.Distance (worldPoint, objOld.transform.position) > 3f) {
					objOld.GetComponent<Light> ().range = 0.3f;
					objOld.GetComponent<Light> ().enabled = false;
					print ("rob");
				}
			}
			if (obj.name == "White" || obj.name == "KillerT") {
				if (Vector3.Distance (worldPoint, obj.transform.position) <= 3f) {
					obj.GetComponent<Light> ().range = 3f;
					obj.GetComponent<Light> ().enabled = true;
					print ("sob");
				}
			}
			yield return new WaitForSeconds (0.02f);
		}
	}*/
}
