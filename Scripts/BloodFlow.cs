using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {
	private float lastDist;
	public BloodFlowController bfctrl;
	private NavMeshAgent agent;
	private float detectionRange;
	private GameObject waypoint;
	private string myMission;
	private string myTempMission;
	private string dest;
	private IEnumerator coroutine;
	private IEnumerator coroutine2;
	private IEnumerator coroutine3;

	void Start(){
		detectionRange = 10;
		bfctrl = GameObject.Find ("GameController").GetComponent<BloodFlowController> ();
		agent = GetComponent<NavMeshAgent> ();
		coroutine2 = playerChoice();
		whereAmI ();
		coroutine = patrol ();
		StartCoroutine (coroutine);
	}

	private void whereAmI(){
		Collider[] search = Physics.OverlapSphere(this.GetComponent<Transform>().position,detectionRange);
		lastDist = detectionRange;
		for(int i = 0; i < search.Length;i++){
			if(search[i].CompareTag("waypoints")){
				if(Vector3.Distance(this.transform.position, search[i].transform.position)<= lastDist){
					lastDist = Vector3.Distance (this.transform.position, search [i].transform.position);
					waypoint = search [i].gameObject;
				}
			}
		}
		int random = Random.Range (0, waypoint.GetComponent<NextWaypoint> ().missions.Length);
		myMission = waypoint.GetComponent<NextWaypoint> ().missions [random];
		dest = bfctrl.GetNext (waypoint.name, myMission);
	}

	public IEnumerator patrol(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			if (agent.remainingDistance < 0.8f) {
				dest = bfctrl.GetNext (dest,myMission);
				agent.destination = GameObject.Find (dest).transform.position;
			}
		}
	}

	public IEnumerator playerChoice(){
		while (true) {
			Collider[] here = Physics.OverlapSphere(this.GetComponent<Transform>().position,0.2f);
			for(int i = 0; i < here.Length;i++){
				if(here[i].CompareTag("direction")){
					dest = here [i].GetComponent<BloodFlowChoice> ().target;
					myMission = here [i].GetComponent<BloodFlowChoice> ().mission;
					dest = bfctrl.GetNext (dest,myMission);
					agent.destination = GameObject.Find (dest).transform.position;
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	public void startPlayer(){
		StartCoroutine (coroutine2);
	}

	public void stopPlayer(){
		StopCoroutine (coroutine2);
	}



	/*private string dest;
	private string store; 
	public BloodFlowController bfctrl;
	private NavMeshAgent agent;
	private IEnumerator coroutine;
	private IEnumerator coroutine2;
	private IEnumerator coroutine3;
	private float random;
	private float detectionRange;
	private float lastDist;
	private bool onMission;
	private string missionLocation; */


	/*void Start () {
		detectionRange = 10f;
		bfctrl = GameObject.Find ("GameController").GetComponent<BloodFlowController> ();
		agent = GetComponent<NavMeshAgent> ();
		whereAmI ();
		agent.destination = GameObject.Find (dest).transform.position;
		agent.autoBraking = true;
		coroutine = patrol ();
		coroutine2 = playerChoice();
		//coroutine3 = missionPatrol ();
		StartCoroutine (coroutine);
	}

	public IEnumerator patrol(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			if (agent.remainingDistance < 0.8f) {
				dest = bfctrl.GetNext (dest);
				agent.destination = GameObject.Find (dest).transform.position;
			}
		}
	}*/

	/*public IEnumerator missionPatrol(){
		while (true) {
			if (dest == missionLocation) {
				dest = bfctrl.GetNext (dest);
				agent.destination = GameObject.Find (dest).transform.position;
				StartCoroutine (coroutine);
				StopCoroutine (coroutine3);
			} 
			if (agent.remainingDistance < 0.5f) {
				dest = GameObject.Find (dest).GetComponent<NextWaypoint> ().requestTarget (missionLocation);
				agent.destination = GameObject.Find (dest).transform.position;
			}
			yield return new WaitForSeconds (0.1f);
		}
	}*/

	/*public IEnumerator playerChoice(){
		while (true) {
			Collider[] here = Physics.OverlapSphere(this.GetComponent<Transform>().position,0.2f);
			for(int i = 0; i < here.Length;i++){
				if(here[i].CompareTag("direction")){
					dest = here [i].GetComponent<BloodFlowChoice> ().target;
					agent.destination = GameObject.Find (dest).transform.position;
				}
			}
			yield return new WaitForSeconds (0.1f);
		}
	}

	public void startPlayer(){
		StartCoroutine (coroutine2);
	}

	public void stopPlayer(){
		StopCoroutine (coroutine2);
	}

	public string getDest(){
		return dest;
	}*/

	/*public void setSecretMission(string secret){
		missionLocation = secret;
		StopCoroutine (coroutine);
		StartCoroutine (coroutine3);
	}*/

	/*public void whereAmI(){
		Collider[] search = Physics.OverlapSphere(this.GetComponent<Transform>().position,detectionRange);
		lastDist = detectionRange;
		for(int i = 0; i < search.Length;i++){
			if(search[i].CompareTag("waypoints")){
				if(Vector3.Distance(this.transform.position, search[i].transform.position)<= lastDist){
					lastDist = Vector3.Distance (this.transform.position, search [i].transform.position);
					dest = search [i].name.ToString();
					dest = bfctrl.GetNext(dest);
				}
			}
		}
	}*/
}
