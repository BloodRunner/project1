using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {
	private float lastDist;
	public BloodFlowController bfctrl;
	private UnityEngine.AI.NavMeshAgent agent;
	private float detectionRange = 4f;
	private GameObject waypoint;
	private string myMission;
	private string myTempMission;
	private string dest;
	private string bind;
	private bool onMission;
	private bool isPlayer;
	private IEnumerator coroutine1;
	private IEnumerator coroutine2;
	private IEnumerator coroutine3;
	private IEnumerator coroutine4;

	private WhiteController me;
	private bool turn;
	public float speed = 2.0f;
	private GameObject[] directionals;
	private Directional[] dirMission;
	private string[] dirWayp;

	private string[] waypoints;
	public float moveSpeed;


	void Awake (){
		bfctrl = GameObject.Find ("GameController").GetComponent<BloodFlowController> ();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		detectionRange = 10f;
		isPlayer = false;
		directionals = GameObject.FindGameObjectsWithTag("directional");
		dirWayp = new string[directionals.Length];
		dirMission = new Directional[directionals.Length];
		for (int i = 0; i < directionals.Length; i++) {
			dirMission [i] = directionals [i].GetComponent<Directional> ();
			dirWayp [i] = directionals [i].name;
		}
		me = this.GetComponent<WhiteController> ();
		whereAmI ();
	}


	void Start(){
		onMission = false;
		coroutine1 = standardPatrol ();
		coroutine2 = boundPatrol ();
		coroutine3 = missionPatrol ();
		coroutine4 = playerPatrol ();
		startPatrol ();
	}

	private void whereAmI(){
		GameObject[] gm = GameObject.FindGameObjectsWithTag ("waypoints");
		//Collider[] search = Physics.OverlapSphere(this.gameObject.GetComponent<Transform>().position,detectionRange);
		//print (search.Length);
		//if(search.Length <= 0){
		//	transform.position = new Vector3(GameObject.Find ("Heart (E/F)").transform.position.x,0.1f,GameObject.Find ("Heart (E/F)").transform.position.z);
		//}
		lastDist = detectionRange;
		for(int i = 0; i < gm.Length;i++){
			if(gm[i].CompareTag("waypoints")){
				if(Vector3.Distance(this.gameObject.transform.position, gm[i].transform.position)<= lastDist){
					//print (search[i].name);
					lastDist = Vector3.Distance (this.transform.position, gm[i].transform.position);
					waypoint = gm[i].gameObject;
				}
			}
		}
		int random = Random.Range (0, waypoint.GetComponent<NextWaypoint> ().missions.Length);
		//transform.position = new Vector3(GameObject.Find (waypoint.name).transform.position.x,0.1f,GameObject.Find (waypoint.name).transform.position.z); 
		//print (transform.position.ToString());
		myMission = waypoint.GetComponent<NextWaypoint> ().missions [random];
		dest = bfctrl.GetNext (waypoint.name, myMission);
	}

	public IEnumerator standardPatrol(){
		while (true) {
			yield return new WaitForSeconds (0.2f);
			if (agent.remainingDistance < 1f) {
				dest = bfctrl.GetNext (dest, myMission);
				agent.destination = GameObject.Find (dest).transform.position;
			}
		}
	}

	public IEnumerator boundPatrol(){
		while (true) {
			yield return new WaitForSeconds (0.2f);
			if (agent.remainingDistance < 1f) {
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

	public IEnumerator playerPatrol(){
		while (true) {
			yield return new WaitForSeconds (0.2f);
			if (agent.remainingDistance < 1f) {
				for(int i = 0; i < dirWayp.Length; i++){
					if(dirMission[i].nameOfWayp == dest){
						if (turn) {
							myMission = dirMission [i].getMission (0);
						} else {
							myMission = dirMission [i].getMission (1);
						}
					}
				}
				dest = bfctrl.GetNext (dest, myMission);
				agent.destination = GameObject.Find (dest).transform.position;

			}
		}
	}

	void Update(){
		if(isPlayer){
			agent.speed = me.bodyStats.speed + Input.GetAxis ("Vertical");
			if (Input.GetAxis ("Horizontal") != 0) {
				playerDirectionals (Input.GetAxis ("Horizontal"));
			}
		}
	}

	public void playerDirectionals(float dir){

		if (dir > 0) {
			turn = true;
			for(int i = 0; i < directionals.Length; i ++){
				if (directionals [i].name == "right") {
					directionals [i].SetActive (true);
				} else {
					directionals [i].SetActive (false);
				}
			}
		}else {
			turn = false;
			for(int i = 0; i < directionals.Length; i ++){
				if (directionals [i].name == "left") {
					directionals [i].SetActive (true);
				} else {
					directionals [i].SetActive (false);
				}
			}
		}
	}

	public IEnumerator missionPatrol(){
		onMission = true;
		while (true) {
			yield return new WaitForSeconds (0.2f);
			if (agent.remainingDistance < 1f) {
				NextWaypoint missionList = GameObject.Find (dest).GetComponent<NextWaypoint> ();
				for(int i = 0; i < missionList.missions.Length; i++){
					if(missionList.missions[i] == myTempMission){
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

	public void startDefend(string mission, string binding){
		myTempMission = mission;
		bind = binding;
		StopAllCoroutines();
		StartCoroutine (coroutine2);
	}

	public void startPlayer(){
		isPlayer = true;
		StopAllCoroutines();
		StartCoroutine (coroutine4);
	}

	//use this to stop player
	public void startPatrol(){
		isPlayer = false;
		StopAllCoroutines();
		StartCoroutine (coroutine1);
	}

	public string getMyMission(){
		return myMission;
	}

	public string getMyDest(){
		return dest;
	}

	public bool onAMission(){
		return onMission;
	}

	/*void Start(){
		isBound = false;
		detectionRange = 10;

		bfctrl = GameObject.Find ("GameController").GetComponent<BloodFlowController> ();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
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
