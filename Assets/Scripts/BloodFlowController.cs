using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodFlowController : MonoBehaviour {

	public Hashtable flowPoints;
	//private GameObject[] waypoints;
	public string[][] names = new string[][]{
		new string[]{"mission1", "1","2","3", "4","5","6","7","8","9","10","11","12","13","l1","l2","l3","ll1","ll2","ll3","ll4","ll5","ll6","ll7","ll8","ll9","ll10","ll11","ll12","ll13","ll14","ll15","ll16","ll17","ll18","bret1","bret2","bret3","ret1","ret2","ret3"},
		new string[]{"mission2", "1","2","3", "4","5","6","7","8","9","10","11","12","13","l1","l2","l3","lr1","lr2","lr3","lr4","lr5","bret1","bret2","bret3","ret1","ret2","ret3"},
		new string[]{"mission3", "1","2","3", "4","5","6","7","8","9","10","11","12","13","r1","r2","r3","r4","r5","r6","rl1","rl2","rl3","rl4","rl5","rl6","rl7","tret1","tret2","tret3","tret4","tret5","tret6","ret1","ret2","ret3"},
		new string[]{"mission4", "1","2","3", "4","5","6","7","8","9","10","11","12","13","r1","r2","r3","r4","r5","r6","rr1","rr2","rr3","rr4","rr5","rr6","rr7","rr8","rr9","rr10","rr11","rr12","rr13","rr14","rr15","rr16","rr17","tret1","tret2","tret3","tret4","tret5","tret6","ret1","ret2","ret3"}
	};
	private Vector3[][] targetWaypoints;
	private string wayp;
	private string storage;
	private string[] targets;
	private float random;
	// Use this for initialization
	void Awake () {
		//waypoints = GameObject.FindGameObjectsWithTag ("waypoints");
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

	public void makeMission(int[] order,string mission){
		BloodFlow thisCell = findNearest ("White", order,true).GetComponent<BloodFlow>();
		if (thisCell == null) {
			return;
		}
		thisCell.startMission (mission);
	}

	public GameObject findNearest(string obj, int[] order, bool single){
		GameObject[] cells = GameObject.FindGameObjectsWithTag ("Host");
		for(int i = 1; i < order.Length; i++){
			for (int x = names[order[i]-1].Length-1; x > 0; x--) {
				for (int z = 0; z < cells.Length; z++) {
					if(cells[z].name == obj){
						if (single == true) {
							if (cells [z].GetComponent<BloodFlow>().onAMission () == false) {
								return cells [z];
							}
						} else {
							return cells [z];
						}
					}
				}
			}
		}
		return null;
	}

	public void defendMission(string organWaypoint, string mission, int[] order){
		BloodFlow thisCell = findNearest ("KillerT", order,true).GetComponent<BloodFlow>();
		if (thisCell == null) {
			return;
		}
		thisCell.startDefend (mission, organWaypoint);
	}

	/*public string boundToNext(string dest, string mission, string bind){
		for(int i = 0; i < names.Length; i++){
			if (mission == names [i] [0]) {
				for(int x = 1; x < names[i].Length; x++){
					if (dest == bind) {
						return bind;
					}
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

	public void makeMission(int[] order){
		GameObject[] cells = GameObject.FindGameObjectsWithTag ("Host");
		for (int i = 1; i < order.Length; i++) {
			for (int x = names[order[i]-1].Length-1; x > 0; x--) {
				for (int z = 0; z < cells.Length; z++) {
					if(cells[z].name == "White"){
						if (cells [z].GetComponent<BloodFlow> ().getMyMission () == names [order[i]-1][0]) {
							if(cells[z].GetComponent<BloodFlow> ().getMyDest() == names [order[i]-1][x]){
								cells [z].GetComponent<BloodFlow> ().startMission (names [order[0]-1][0]);
								cells [z].GetComponent<PingCell> ().PingIt ();
								print ("Pinged!!!!");
								return;
							}
						}
					}
				}
			}
		}
	}*/

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
