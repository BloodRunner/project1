using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

	
public class GameController : MonoBehaviour {
	public float pressure;
	public RedController Red;
	public WhiteController White;
	public WhiteController killerT;
	public PathogenController[] infections=new PathogenController[10];
	public string[] instructions = new string[10];
	public int instructionWait = 5;
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
	public Text characterCount;
	public Button restartButton;
	public GUIText messageText;
	public GUIText restartText;
	public GUIText gameoverText;
	public Camera topCamera;
	public Camera followCamera;
	protected int numInfections=0;
	//MessageBoard msgbd = new MessageBoard ();
	OrganController[] all_organs;
	float timer; // timer to limit the UI tally update


	// Use this for initialization
	void Start () {
		//msgbd = new MessageBoard ();
		pressure = 1;
		score = 0;
		UpdateScore (0);
		gameOver = false;
		restart=false;
		if (restartText)
			restartText.text = "";
		if (gameoverText)
			gameoverText.text = "";
		if (messageText)
			messageText.text = "";
		healthText.text = "Score";
		restartButton.enabled = false;
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera>();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera>();
		followCamera.enabled = false;
		topCamera.enabled = true;
		if (bodystate == null) {
			Debug.Log ("Game Controller missing body state");
		}
		all_organs = GameObject.FindObjectsOfType (typeof(OrganController)) as OrganController[];
		ThymusController tc = GameObject.FindObjectOfType (typeof(ThymusController)) as ThymusController;
		setUpDefence(redCount,whiteCount,tc.transform.position); // defenders spawned from thymus
		StartCoroutine( SpawnWaves());
	}

	public float getOrgansScores() {
		float total = 0;
		foreach (OrganController organ in all_organs) {
			total += organ.get_stats_health ();
		}
		return total;
	}

	public void tallyCharacters() {
		string total = "Ally Count:";
		string lastN = null;
		CellController[] cells = GameObject.FindObjectsOfType (typeof(WhiteController)) as WhiteController[];
		total += "\nWhite Blood Cell:" + cells.Length;
		cells = GameObject.FindObjectsOfType (typeof(RedController)) as RedController[];
		total += "\nRed Blood Cell:" + cells.Length;
		cells = GameObject.FindObjectsOfType (typeof(PathogenController)) as PathogenController[];
		total += "\nEnemy Count:\nPathogens:" + cells.Length;
		characterCount.text= total;
		//Debug.Log (total);
	}

	public bool bodyIsAlive() {
		int alive = 0;
		foreach (OrganController organ in all_organs) {
			if (organ.get_stats_health () > 0) {
				//Debug.Log (organ.name + " " + organ.get_stats_health ());
				alive = alive | organ.mask;
			}
		}
		if (alive == 127)
			return true;
		else
			return false;
	}

	public void checkGameOver(){
		if (!bodyIsAlive ())
			GameOver ();
	}

	public bool isGameOver() {
		return gameOver;
	}

	public int numInfection() {
		GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Infection") as GameObject[];
		Debug.Log(objectsWithTag.Length);
		return objectsWithTag.Length;
	}

	// Coroutine
	IEnumerator SpawnInstructions() {
		foreach (string line in instructions) {
			if (gameOver)
				break;
			instructionText.text = line;
			yield return new WaitForSeconds (instructionWait);
		}
		instructionText.text = "";
	}

	// Coroutine
	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds (startWait);
		CellController cell;
		int level = 0;
		while(true) {
			PathogenController cc = infections [level];
			Debug.Log ("Spawnwave level="+level+" "+ cc.name +" "+ infectionCount);
			showMessage ("Level "+ (level+1) +": " + infectionCount + cc.name + " are coming out from "+ infectedOrgan.name , 5);
			for (int i = 0; i < infectionCount; i++) {
				// Instantiate at infection point in organs!
				cell = Instantiate (cc, infectedOrgan.transform.position, Quaternion.identity) as PathogenController;
				cell.bodystate = this.bodystate;
				cell.gameController = this;
				//Debug.Log ("Sending out " + cell.name);
				yield return new WaitForSeconds (spawnWait);
			}
			// Organ is now a spawner

			foreach (OrganController organ in all_organs) {
				if (organ.get_stats_health () <= 0) {
					showMessage (infectionCount + cc.name + " are coming out from undead "+ organ.name +". Send some white blood cells to clear the infection", 5);

					cell = Instantiate (cc, organ.transform.position, Quaternion.identity) as PathogenController;;
					cell.bodystate = this.bodystate;
					cell.gameController = this;
					//Debug.Log ("Sending out " + cell.name+ " from " + organ.name);
				}
			}
			//Debug.Log ("gameover=" + gameOver);
			if (gameOver) {
				
				restart = true;
				try {
					Button restartButton = GameObject.Find ("RestartButton").GetComponent<Button>() as Button;
					restartButton.enabled = true;
				} catch {
					if (restartText)
						restartText.text = "Press 'R' for Restart";
				}
				break;
			}
			if (level < 5) {// increase in difficulty
				level++;
			} else {
				infectionCount += 5;
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	public void setUpDefence(int redCount, int whiteCount, Vector3 location){
		Quaternion spawnRotation = Quaternion.identity;
		CellController cell;
		for (int i = 0; i < redCount; i++) {
			cell = Instantiate (Red, location, spawnRotation) as RedController;
			cell.bodystate = this.bodystate;
			cell.gameController = this;
		}
		for (int i = 0; i < whiteCount; i++) {
			cell = Instantiate (White, location, spawnRotation) as WhiteController;
			cell.bodystate = this.bodystate;
			cell.gameController = this;
		}
		string msg="Sending out " + redCount + " reds and " + whiteCount + " white cells";
		showMessage (msg, 5);
	}

	public void spawnKillerT(int count) {
	Quaternion spawnRotation = Quaternion.identity;
	CellController cell;
	for (int i = 0; i < count; i++) {
			cell = Instantiate (killerT, infectedOrgan.transform.position, spawnRotation) as WhiteController;
		cell.bodystate = this.bodystate;
		cell.gameController = this;
	}
		Debug.Log ("Spawning " + count + " Killer T cells");
	}

	public void RestartGame() {
		SceneManager.LoadSceneAsync (0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadSceneAsync (0);
			}
		}
		timer += Time.deltaTime;
		if (score > 0) {
			if (killerT != null && killerT.points > 0) {
				int count = (int) (score / killerT.points);
				count -= killTcount;
				spawnKillerT (count);
				killTcount = count;
				//msgbd.printMessages ();
			}
		}
		if (timer >= 2) {//  - count chars every 2 seconds
			//characterCount.text = "Ally Count:\nRed Blood Cells:\nWhite Blood Cells:\nEnemy Count:\nBacteria:\nVirus:\nPrion:\nParasite:\nZika";
			tallyCharacters ();
		}
	}


	public void UpdateScore(float scorept) {
		score += scorept;
		if (healthText)
			healthText.text = "Score:" + score;
	}
	public void GameOver() {
		gameOver = true;
		if (gameoverText)
			gameoverText.text = "Game Over";
		//Instantiate(restartButton,Canvas
	}

	IEnumerator timedMessage(string message, int seconds) {
		messageText.text = message;
		yield return new WaitForSeconds (seconds);
		messageText.text = "";
	}

	public void showMessage(string message, int seconds) {	
		//msgbd.addMessage (message);	
		if (gameoverText)
			StartCoroutine( timedMessage (message, seconds));
	}

	public void pauseGame(){
		if (Time.timeScale == 0)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
	}

	public void quitGame(){
		Application.Quit ();
	}


	public void showRestartButton(){
		restartButton.enabled = true;
	}

}
