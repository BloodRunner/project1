using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public float pressure;
	public RedController Red;
	public WhiteController White;
	public WhiteController killerT;
	public GameObject infections;
	public BodyState bodystate; // Whole Body Status
	//public Vector3 spawnValues;
	public GameObject infectedOrgan;
	public int infectionCount;
	public int redCount, whiteCount;
	public int spawnWait;
	public int startWait,waveWait;
	private int killTcount = 0;  // A new one every N point in Score
	private float score=100; // starts at 100%, dies at 0
	public GUIText healthText;
	private bool gameOver;
	private bool restart=false;
	public GUIText restartText;
	public GUIText gameoverText;
	public Camera topCamera;
	public Camera followCamera;
	protected int numInfections=0;
	OrganController[] all_organs;

	// Use this for initialization
	void Start () {
		pressure = 1;
		score = 0;
		UpdateScore (0);
		gameOver = false;
		restart=false;
		if (restartText)
			restartText.text = "";
		if (gameoverText)
			gameoverText.text = "";
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera>();
		followCamera.enabled = false;
		topCamera.enabled = true;
		if (bodystate == null) {
			Debug.Log ("Game Controller misisng body state");
		}

		setUpDefence(redCount,whiteCount);
		StartCoroutine( SpawnWaves ());
		all_organs = GameObject.FindObjectsOfType (typeof(OrganController)) as OrganController[];

	}

	public float getOrgansScores() {
		float total = 0;
		//	GameObject obj = GameObject.Find(name);
		//OrganController[] organs = GameObject.FindObjectsOfType (typeof(OrganController)) as OrganController[];
		foreach (OrganController organ in all_organs) {
			total += organ.get_stats_health ();
		}
		return total;
	}

	public int numInfection() {
		GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Infection") as GameObject[];
		Debug.Log(objectsWithTag.Length);
		return objectsWithTag.Length;
	}
	// Coroutine
	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds (startWait);
		while(true) {
			for (int i = 0; i < infectionCount; i++) {
				// Instantiate at infection point in organs!
				//Vector3 position = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (infections, infectedOrgan.transform.position, spawnRotation);
				// Instantiate returns a Transform so need to add "as GameObject" after call
				// GameObject=... as GameObject
				yield return new WaitForSeconds (spawnWait);
			}
			// Organ is now a spawner
			foreach (OrganController organ in all_organs) {
				if (organ.get_stats_health () >= 0)
					Instantiate (infections, organ.transform.position, Quaternion.identity);
			}
			//Debug.Log ("gameover=" + gameOver);
			if (gameOver) {
				if (restartText)
					restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	public void setUpDefence(int redCount, int whiteCount){
		Quaternion spawnRotation = Quaternion.identity;
		CellController cell;
		for (int i = 0; i < redCount; i++) {
			cell = Instantiate (Red, infectedOrgan.transform.position, spawnRotation) as RedController;
			cell.bodystate = this.bodystate;
			cell.gameController = this;
		}
		for (int i = 0; i < whiteCount; i++) {
			cell = Instantiate (White, infectedOrgan.transform.position, spawnRotation) as WhiteController;
			cell.bodystate = this.bodystate;
			cell.gameController = this;
		}
		Debug.Log ("Sending out " + redCount + " reds and " + whiteCount + " white cells");
	}

	public void spawnKillerT(int count) {
	Quaternion spawnRotation = Quaternion.identity;
	CellController cell;
	for (int i = 0; i < count; i++) {
			cell = Instantiate (killerT, infectedOrgan.transform.position, spawnRotation) as WhiteController;
		cell.bodystate = this.bodystate;
		cell.gameController = this;
	}
	}
	public void GameOver() {
		gameOver = true;
		if (gameoverText)
			gameoverText.text = "Game Over";
	}

	IEnumerator timedMessage(string message, int seconds) {
		gameoverText.text = message;
		yield return new WaitForSeconds (seconds);
		gameoverText.text = "";
	}

	public void showMessage(string message, int seconds) {		
		if (gameoverText)
		  StartCoroutine( timedMessage (message, seconds));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadSceneAsync (0);
			}
		}
		if (score > 0) {
			if (killerT != null && killerT.points > 0) {
				int count = (int) (score / killerT.points);
				count -= killTcount;
				spawnKillerT (count);
				killTcount = count;
			}
		}
	}

	void setScore(float scorept) {
		score = scorept;
	}
	public void UpdateScore(float scorept) {
		score += scorept;
		if (healthText)
			healthText.text = "Score:" + score;
	}
}
