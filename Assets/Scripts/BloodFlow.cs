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
	private string bind;
	private bool isBound;
	private IEnumerator coroutine1;
	private IEnumerator coroutine2;
	private IEnumerator coroutine3;


	void Start(){
		coroutine1 = standardPatrol ();
		coroutine2 = boundPatrol ();
		coroutine3 = missionPatrol ();
	}

	public IEnumerator standardPatrol(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			if (agent.remainingDistance < 0.8f) {
				dest = bfctrl.GetNext (dest, myMission);
				agent.destination = GameObject.Find (dest).transform.position;
			}
		}
	}

	public IEnumerator boundPatrol(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			if (agent.remainingDistance < 0.8f) {
				if(dest == bind){
					agent.destination = GameObject.Find (dest).transform.position;
					yield return new WaitForSeconds (0.3f);
				} else{
					dest = bfctrl.GetNext (dest, myMission);
					agent.destination = GameObject.Find (dest).transform.position;
				}

			}
		}
	}

	public IEnumerator missionPatrol(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			if (agent.remainingDistance < 0.8f) {
				for(int i = 0; i < GameObject.Find(dest).GetComponent<NextWaypoint>().missions.Length; i++){
					if(GameObject.Find(dest).GetComponent<NextWaypoint>().missions[i] == myTempMission){
						myMission = myTempMission;
						dest = bfctrl.GetNext (dest, myMission);
						agent.destination = GameObject.Find (dest).transform.position;
						StartCoroutine (coroutine1);
						StopCoroutine (coroutine3);
					}
				}
				dest = bfctrl.GetNext (dest, myMission);
				agent.destination = GameObject.Find (dest).transform.position;


			}
		}
	}

	public void startMission(string mission){
		myTempMission = mission;
		StopAllCoroutines();
		StartCoroutine (coroutine3);

	}

	public string getMyMission(){
		return myMission;
	}

	public string getMyDest(){
		return dest;
	}

	/*void Start(){
		isBound = false;
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
		//if(search.Length <= 0){
		//	transform.position = new Vector3(GameObject.Find ("Heart (E/F)").transform.position.x,0.1f,GameObject.Find ("Heart (E/F)").transform.position.z);
		//}
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
		//transform.position = new Vector3(GameObject.Find (waypoint.name).transform.position.x,0.1f,GameObject.Find (waypoint.name).transform.position.z); 
		//print (transform.position.ToString());
		myMission = waypoint.GetComponent<NextWaypoint> ().missions [random];
		dest = bfctrl.GetNext (waypoint.name, myMission);
	}

	public IEnumerator patrol(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			if (agent.remainingDistance < 0.8f) {
				if (isBound && dest == bind) {
					agent.destination = GameObject.Find (dest).transform.position;
				} else {
					dest = bfctrl.GetNext (dest, myMission);
					agent.destination = GameObject.Find (dest).transform.position;
				}
			}
		}
	}*/

	/*public IEnumerator playerChoice(){
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

	public void bindTo(string location){
		if (isBound == false) {
			isBound = true;
			bind = location;
			this.gameObject.GetComponent<Light> ().color = Color.green;
		}else {
			isBound = false;
			this.gameObject.GetComponent<Light> ().color = Color.white;
			StartCoroutine (patrol());
		}
	}

	public void startPlayer(){
		StartCoroutine (coroutine2);
	}

	public void stopPlayer(){
		//print (isBound.ToString ());
		bool wasIBound = isBound;
		string dest3 = this.GetComponent<BloodFlow> ().getMyDest ();
		Vector3 dest2 = this.GetComponent<BloodFlow> ().getMyDestV ();
		if(this.name == "KillerT"){
			wasIBound = this.GetComponent<BloodFlow> ().amIBound ();
		}
		StopCoroutine (coroutine2);
		isBound = false;
		if(wasIBound){
			bindTo (dest);
			this.agent.destination = dest2;
			dest = dest3;
		}
		//print (isBound.ToString ());
	}

	public string getMyMission(){
		return myMission;
	}

	public bool amIBound(){
		return isBound;
	}

	public void setMyBound(bool setToThis){
		isBound = setToThis;
	}

	public string getMyDest(){
		return dest;
	}

	public Vector3 getMyDestV(){
		return agent.destination;
	}

	public void setMyMission(string setTo){
		myMission = setTo;
	}

	public void destOveride(Vector3 destOverides){
		agent.destination = destOverides;
	}*/


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
