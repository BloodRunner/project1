using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {

	public string mission1;
	public string mission2;
	public string mission3;
	public string mission4;
	private string missions;
	private bool onAMission;

	void Start(){
		onAMission = false;
	}

	void OnTriggerEnter(Collider collision){
		if(onAMission || collision.GetComponent<BloodFlow>()){
			if(collision.gameObject.name == "White" || collision.gameObject.name == "KillerT"){
				if(missions == "mission1"){
					collision.GetComponent<NavMeshAgent>().destination = GameObject.Find (mission1).transform.position;
				} else if(missions == "mission2"){
					collision.GetComponent<NavMeshAgent>().destination = GameObject.Find (mission2).transform.position;
				} else if(missions == "mission3"){
					collision.GetComponent<NavMeshAgent>().destination = GameObject.Find (mission3).transform.position;
				}else if(missions == "mission4"){
					collision.GetComponent<NavMeshAgent>().destination = GameObject.Find (mission4).transform.position;
				}
			}
		}
	}

	public void startMission(string mission){
		onAMission = true;
		missions = mission;
	}

	public void endMission(){
		onAMission = false;
	}
		
}
