using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodFlowController : MonoBehaviour {

	public Hashtable flowPoints;
	private GameObject[] waypoints;
	public string[][] names = new string[][]{
		new string[]{"mission1", "heart1","lung1","lung2", "heart2", "inter1","inter2","inter3","brain","inter5","hometrip"},
		new string[]{"mission2", "heart1","lung1","lung2", "heart2", "inter1","inter2","beforethymus","thymus","afterthymus","inter4","hometrip"},
		new string[]{"mission3", "heart1","lung1","lung2", "heart2", "inter1","stomach","liver","hometrip"},
		new string[]{"mission4", "heart1","lung1","lung2", "heart2", "inter1","stomach","kidney1","kidney2","hometrip"}
	};
	private Vector3[][] targetWaypoints;
	private string wayp;
	private string storage;
	private string[] targets;
	private float random;
	// Use this for initialization
	void Awake () {
		waypoints = GameObject.FindGameObjectsWithTag ("waypoints");
		flowPoints = new Hashtable();
		//initializeWaypoints ();
	}

	void Start(){
		
	}
	public string GetNext(string dest, string mission){
		for(int i = 0; i < names.Length; i++){
			if (mission == names [i] [0]) {
				for(int x = 1; x < names[i].Length; x++){
					if(dest == names[i][x]){
						if (x == names [i].Length - 1) {
							return names [i] [1];
						} else {
							return names [i] [x+1];
						}
					}
				}
				break;
			}
		}
		return names [0] [1];
	}

	/*private void initializeWaypoints(){
		targetWaypoints = new Vector3[names.Length][];
		for (int z = 0; z < names.Length; z++) {
			targetWaypoints[z] = new Vector3[names[z].Length -1];
		}
		for (int i = 0; i < names.Length; i++) {
			for(int x = 0; names[i].Length -1; x++){
				targetWaypoints [i] [x] = new Vector3 (GameObject.Find (names [i] [x + 1]).transform.position);
			}
		}
	}*/


	/*private void initializeWaypoints(){
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
		targets = (string[])flowPoints [dest]; 
		if (random <= 50) {
			//print (targets.ToString());
			return (targets [0]);
		} else {
			//print (targets.ToString());
			return (targets [1]);
		} 
	}*/
}
