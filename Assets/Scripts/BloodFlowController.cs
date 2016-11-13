using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodFlowController : MonoBehaviour {

	public Hashtable flowPoints;
	private GameObject[] waypoints;
	private string wayp;
	private string storage;
	private string[] targets;
	private float random;
	// Use this for initialization
	void Awake () {
		waypoints = GameObject.FindGameObjectsWithTag ("waypoints");
		flowPoints = new Hashtable();
		initializeWaypoints ();
	}
	
	private void initializeWaypoints(){
		for(int i = 0; i < waypoints.Length; i++){
			targets = new string[2];
			wayp = waypoints [i].name;
			targets [0] = waypoints [i].GetComponent<NextWaypoint> ().target;
			targets [1] = waypoints [i].GetComponent<NextWaypoint> ().target2;	
			//print (targets [0].ToString ());
			//print (wayp.ToString ());
			flowPoints.Add (wayp, targets);

		}
	}

	public string GetNext(string dest){
		random = Random.Range (1, 100);
		if (random <= 50) {
			targets = (string[])flowPoints [dest]; 
			//print (targets.ToString());
			return (targets [0]);
		} else {
			targets = (string[])flowPoints [dest];
			//print (targets.ToString());
			return (targets [1]);
		} 
	}
}
