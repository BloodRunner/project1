using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {

	private string dest;
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
	private string missionLocation;


	void Start () {
		detectionRange = 10f;
		bfctrl = GameObject.Find ("GameController").GetComponent<BloodFlowController> ();
		agent = GetComponent<NavMeshAgent> ();
		whereAmI ();
		agent.destination = GameObject.Find (dest).transform.position;
		agent.autoBraking = false;
		coroutine = patrol ();
		coroutine2 = playerChoice();
		coroutine3 = missionPatrol ();
		StartCoroutine (coroutine);
	}

	public IEnumerator patrol(){
		while (true) {
			yield return new WaitForSeconds (0.1f);
			if (agent.remainingDistance < 0.5f) {
				dest = bfctrl.GetNext (dest);
				agent.destination = GameObject.Find (dest).transform.position;
			}
		}
	}

	public IEnumerator missionPatrol(){
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
	}

	public IEnumerator playerChoice(){
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
	}

	public void setSecretMission(string secret){
		missionLocation = secret;
		StopCoroutine (coroutine);
		StartCoroutine (coroutine3);
	}

	public void whereAmI(){
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
	}

	/*public void startMission(string mission){
		missionLocation = mission;
		StopCoroutine (coroutine);
		StartCoroutine (coroutine3);
	}*/
		

	/*public Vector3[] points;
	private int destPoint;
	public Vector3 dest;
	private NavMeshAgent agent;
	private IEnumerator coroutine;
	private Hashtable patrolls;
	private float delay;
	private float random;
	private float chance1;
	private float chance2;
	private float chance3;
	private float chance4;
	private float chance5;
	private float chance6;

	void Start () {
		
		delay = 0;
		chance1 = 25;
		chance2 = 50;
		chance3 = 66.7f;
		chance4 = 83.4f;
		chance5 = 100f;
		destPoint = 0;
		//whereAmI ();
		//setPath();
		agent = GetComponent<NavMeshAgent> ();
		agent.autoBraking = false;
		coroutine = patrol ();
		StartCoroutine (coroutine);
	}



	void BloodFlowing(){
		if (points.Length == 0)
			return;
		else if (destPoint == points.Length - 1) {
			agent.destination = points [destPoint];
			//print (agent.destination.ToString());
			destPoint = 0;

		} else {
			agent.destination = points [destPoint];
			//print (agent.destination.ToString());
			destPoint = (destPoint + 1);
		}

	}

	public IEnumerator patrol(){
		while (true) {
			if (agent.remainingDistance < 0.5f) {
				BloodFlowing ();
				yield return new WaitForSeconds (0.1f);
			} else {
				yield return new WaitForSeconds (0.1f);
			}
		}
	}

	public float getDelay(){
		return delay;
	}

	public void setDelay(float setTo){
		delay = setTo;
	}

	public void setAPatrollPoint(Vector3 thePoint, int place){
		points [place] = thePoint;
	}

	//inserts a patroll point at the designated part of the list and pushes everything at and following that point back.
	public void insertPatrollPoint(Vector3 thePoint, int place){
		Vector3[] newpoints = new Vector3[points.Length + 1];
		for (int i = 0; i < newpoints.Length; i++) {
			if (i == place) {
				newpoints [place] = thePoint;
			} else if (i >= place) {
				newpoints [i] = points [i-1];
			} else {
				newpoints [i] = points [i];
			}
		}
		points = newpoints;
	}

	public void removePoint(int place){
		Vector3[] newpoints = new Vector3[points.Length - 1];
		for (int i = 0; i < points.Length -1; i++){
			if (i >= place) {
				newpoints [i - 1] = points [i];
			} else {
				newpoints [i] = points [i];
			}
		}
		points = newpoints;
	}

	public void whereAmI(){
		Collider[] search = Physics.OverlapSphere(this.GetComponent<Transform>().position,1f);
		for(int i = 0; i < search.Length;i++){
			if(search[i].CompareTag("1")){
				destPoint = 0;
				//print (1);
			} else if(search[i].CompareTag("2")){
				destPoint = 1;
				//print (2);
			} else if(search[i].CompareTag("3")){
				destPoint = 2;
				//print (3);
			}else if(search[i].CompareTag("4")){
				destPoint = 3;
				//print (4);
			}
		}
	}

	public void setPath(){
		//if(path == null){
			random = Random.Range (0,100);
			if(random <= chance1){
				path1 (0);
			}
			else if(random <= chance2){
				path2 (0);
			}
			else if(random <= chance3){
				path3 (0);
			}
			else if(random <= chance4){
				path4 (0);
			}
			else if(random <= chance5){
				path5 (0);
			}
		//}
	}

	public void initTable(){
		patrolls = new Hashtable ();
		patrolls.Add ("Heart1", new Vector3 (9.7f,0f,-2f));
		patrolls.Add ("Lung1", new Vector3 (-0.85f,0f,-13.2f));
		patrolls.Add ("Lung2", new Vector3 (-1.11f,0f,0.91f));
		patrolls.Add ("Heart2", new Vector3 (9.49f,0f,-0.67f));
		patrolls.Add ("Inter1", new Vector3 (5.36f,0f,8.55f));
		patrolls.Add ("Inter2", new Vector3 (-5.29f,0f,8.33f));
		patrolls.Add ("Inter3", new Vector3 (-8.98f,0f,8.44f));
		patrolls.Add ("Brain", new Vector3 (-10.19f,0f,-7.13f));
		patrolls.Add ("Inter5", new Vector3 (-8.62f,0f,-19.17f));
		patrolls.Add ("Thymus", new Vector3 (-5.29f,0f,-5.09f));
		patrolls.Add ("Inter6", new Vector3 (-5.15f,0f,-19.03f));
	}


	public void path1(int place){
		points = new Vector3[5];
		points [0] = new Vector3(9.7f,0f,-2f); 
		points [1] = new Vector3(-0.85f,0f,-13.2f); 
		points [2] = new Vector3(-1.11f,0f,0.91f) ;
		points [3] = new Vector3(9.49f,0f,-0.67f) ;
		points [4] = new Vector3(5.36f,0f,8.55f) ;
		points [5] = new Vector3(5.36f,0f,8.55f) ;
	}
	public void path2(int place){
		points = new Vector3[5];
		points [0] = new Vector3(9.7f,0f,-2f); 
		points [1] = new Vector3(-0.85f,0f,-13.2f); 
		points [2] = new Vector3(-1.11f,0f,0.91f) ;
		points [3] = new Vector3(9.49f,0f,-0.67f) ;
		points [4] = new Vector3(5.36f,0f,8.55f) ;
	}
	public void path3(int place){
		points = new Vector3[5];
		points [0] = new Vector3(9.7f,0f,-2f); 
		points [1] = new Vector3(-0.85f,0f,-13.2f); 
		points [2] = new Vector3(-1.11f,0f,0.91f) ;
		points [3] = new Vector3(9.49f,0f,-0.67f) ;
		points [4] = new Vector3(5.36f,0f,8.55f) ;
	}
	public void path4(int place){
		points = new Vector3[5];
		points [0] = new Vector3(9.7f,0f,-2f); 
		points [1] = new Vector3(-0.85f,0f,-13.2f); 
		points [2] = new Vector3(-1.11f,0f,0.91f) ;
		points [3] = new Vector3(9.49f,0f,-0.67f) ;
		points [4] = new Vector3(5.36f,0f,8.55f) ;
	}
	public void path5(int place){
		points = new Vector3[5];
		points [0] = new Vector3(9.7f,0f,-2f); 
		points [1] = new Vector3(-0.85f,0f,-13.2f); 
		points [2] = new Vector3(-1.11f,0f,0.91f) ;
		points [3] = new Vector3(9.49f,0f,-0.67f) ;
		points [4] = new Vector3(5.36f,0f,8.55f) ;
	}

	public Vector3 target1;
	public Vector3 target2;
	public Vector3 target3;
	public float chance1;
	public float chance2;
	public float chance3;
	private float random;
	public BoxCollider interaction;


	//the most not elegant way of dealing with this.
	void OnTriggerStay(Collider collision) {
		
		if (collision.CompareTag ("Host") || collision.CompareTag ("Infection")) {
			random = Random.Range (0,100);
			if(random <= chance1){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target1);
			}
			else if(random <= chance2){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target2);
			}
			else if(random <= chance3){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target3);
			}
		}
	}
	*/

}
