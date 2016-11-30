using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

	
public class GameController : MonoBehaviour
{
	public RedController Red;
	public WhiteController White;
	public WhiteController killerT;
	public PathogenController[] infections = new PathogenController[10];
	string[] pathogens;
	public string[] instructions = new string[10];
	public int instructionWait = 5;
	public BodyState bodystate;
	// Whole Body Status
	//public Vector3 spawnValues;
	public GameObject infectedOrgan;
	public int infectionCount;
	public int redCount, whiteCount;
	public int spawnWait;
	public int startWait, waveWait;
	private int killTcount = 0;
	// A new one every N point in Score
	private float score = 100;
	// starts at 100%, dies at 0
	public GUIText healthText;
	private bool gameOver;
	private bool restart = false;
	public Text characterCount;
	public Text instructionText;
	public Text scoreText;
	public Button restartButton;
	public GUIText messageText;
	//public GUIText restartText;
	public GUIText gameoverText;
	public Camera topCamera;
	public Camera followCamera;
	protected int numInfections = 0;
	//MessageBoard msgbd = new MessageBoard ();
	OrganController[] all_organs;
	float timer;
	// timer to limit the UI tally update
	bool winnable = false;
	float difficultyLevel = 1;
	updatePlayerStats playerStats;


	// Use this for initialization
	void Start ()
	{
		//msgbd = new MessageBoard ();
		showRestartButton (0);
		score = 0;
		UpdateScore (0);
		gameOver = false;
		restart = false;
		//if (restartText)
		//	restartText.text = "";
		if (gameoverText)
			gameoverText.text = "";
		if (messageText)
			messageText.text = "";
		//healthText.text = "";
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera> ();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera> ();
		followCamera.enabled = false;
		topCamera.enabled = true;
		if (bodystate == null) {
			Debug.Log ("Game Controller missing body state");
		}
		pathogens = new string[infections.Length];
		for (int i = 0; i < infections.Length; i++) {
			pathogens [i] = infections [i].name;
		}
		all_organs = GameObject.FindObjectsOfType (typeof(OrganController)) as OrganController[];
		ThymusController tc = GameObject.FindObjectOfType (typeof(ThymusController)) as ThymusController;
		StartCoroutine (SpawnInstructions ());
		setUpDefence (redCount, whiteCount, tc.transform.position); // defenders spawned from thymus
		StartCoroutine (SpawnWaves ());
	}

	public float getOrgansScores ()
	{
		float total = 0;
		foreach (OrganController organ in all_organs) {
			total += organ.get_stats_health ();
		}
		return total;
	}

	public void tallyCharacters ()
	{
		//string[] pathogens = { "Virus", "Bacteria", "Parasite", "Prion", "Zika" }; 
		int[] count = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };// Bad coding practice!
		string total = "Ally Count:";

		CellController[] cells = GameObject.FindObjectsOfType (typeof(WhiteController)) as WhiteController[];
		total += "\nWhite Blood Cell:" + cells.Length;
		cells = GameObject.FindObjectsOfType (typeof(RedController)) as RedController[];
		total += "\nRed Blood Cell:" + cells.Length;
		cells = GameObject.FindObjectsOfType (typeof(PathogenController)) as PathogenController[];
		total += "\nEnemy Count:";
		for (int i = 0; i < cells.Length; i++) {
			for (int j = 0; j < pathogens.Length; j++) {
				if (cells [i].name.Equals (pathogens [j])) {
					count [j]++;
					break;
				}
			}
		}
		for (int j = 0; j < pathogens.Length; j++) {
			if (count [j] > 0)
				total += "\n" + pathogens [j] + ": " + count [j];
		}
		characterCount.text = total;
		//Debug.Log (total);
	}

	public bool bodyIsAlive ()
	{
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

	public void checkGameOver ()
	{
		if (!bodyIsAlive ())
			GameOver ();
	}

	public bool isGameOver ()
	{
		return gameOver;
	}


	public int numInfection ()
	{
		CellController[] cells = GameObject.FindObjectsOfType (typeof(PathogenController)) as PathogenController[];
		Debug.Log (" Number of infection cells " + cells.Length);
		return cells.Length;
	}

	// Coroutine
	IEnumerator SpawnInstructions ()
	{
		foreach (string line in instructions) {
			if (gameOver)
				break;
			instructionText.text = line;
			yield return new WaitForSeconds (instructionWait);
		}
		instructionText.text = "";
	}

	public bool checkForVictory ()
	{
		//All pathogens are dead
		if (winnable) {
			if (!gameOver && numInfection () == 0) {
				gameOver = true;
				if (gameoverText)
					gameoverText.text = "You have won! All pathogens are eliminated!";
				return true;
			}
		}
		return false;
	}

	// Coroutine
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		CellController cell;
		int level = 0;
		const int numlevels = 5;
		while (true) {
			if (level >= numlevels)
				break;
			PathogenController cc = infections [level];
			Debug.Log ("Spawnwave level=" + level + " " + cc.name + " " + infectionCount);
			showMessage ("Level " + (level + 1) + ": " + infectionCount + " " + cc.name + " are coming out from " + infectedOrgan.name, 5);
			//stopgap killerT cell spawning
			if (level > 1) {
				spawnKillerT (1);
			}

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
					showMessage (infectionCount + " " + cc.name + " are coming out from undead " + organ.name + ". Send some white blood cells to clear the infection", 5);

					cell = Instantiate (cc, organ.transform.position, Quaternion.identity) as PathogenController;

					cell.bodystate = this.bodystate;
					cell.gameController = this;
					//Debug.Log ("Sending out " + cell.name+ " from " + organ.name);
				}
			}
			//Debug.Log ("gameover=" + gameOver);
			if (gameOver) {
				restart = true;
				try {
					showRestartButton (1);
				} catch {
					/*
					if (restartText)
						restartText.text = "Press 'R' for Restart";
				*/
				}
				break;
			} else if (level < numlevels) {// increase in difficulty
				level++;
				yield return new WaitForSeconds (waveWait);
			} else {
				winnable = true;
				break;
			}
		}
		winnable = true;
		Debug.Log ("Winnable now " + level);
	}

	public void setUpDefence (int redCount, int whiteCount, Vector3 location)
	{
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
		string msg = "Sending out " + redCount + " reds and " + whiteCount + " white cells";
		showMessage (msg, 5);
	}

	public void spawnKillerT (int count)
	{
		Quaternion spawnRotation = Quaternion.identity;
		CellController cell;
		//Stopgap measure
		for (int i = 0; i < count; i++) {
			cell = Instantiate (killerT, GameObject.Find ("thymus").transform.position, spawnRotation) as WhiteController;
			cell.bodystate = this.bodystate;
			cell.gameController = this;
		}

	}

	public void RestartGame ()
	{
		Debug.Log ("Restart Game ");
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

		if (timer >= 2) {//  - do this every 2 seconds
			if (score > 0) {
				if (killerT != null && killerT.points > 0) {
					int count = (int)(score / killerT.points);
					if (count > killTcount)
						Debug.Log ("Spawning " + count + "-" + killTcount + " Killer T cells");
					spawnKillerT (count - killTcount);
					killTcount = count;

				}
			}
			//characterCount.text = "Ally Count:\nRed Blood Cells:\nWhite Blood Cells:\nEnemy Count:\nBacteria:\nVirus:\nPrion:\nParasite:\nZika";
			tallyCharacters ();
			checkForVictory ();
		}
	}


	public void UpdateScore (float scorept)
	{
		score += scorept;
		if (scoreText != null)
			scoreText.text = "Score:" + score;
	}

	public void GameOver ()
	{
		gameOver = true;
		if (gameoverText)
			gameoverText.text = "Game Over";
		showRestartButton (1);
	}

	IEnumerator timedMessage (string message, int seconds)
	{
		messageText.text = message;
		yield return new WaitForSeconds (seconds);
		messageText.text = "";
	}

	public void showMessage (string message, int seconds)
	{	
		//msgbd.addMessage (message);	
		if (messageText)
			StartCoroutine (timedMessage (message, seconds));
	}

	public void showSettings ()
	{
		Debug.Log ("show settings");
		//pauseGame ();
		//	Button settingsButton = GameObject.Find ("settingsButton").GetComponent<Button> () as Button;
		//settingsButton.GetComponent<Text>.text = "Save Settings";
		GameObject panel = GameObject.Find ("settingsPanel");
		CanvasGroup cg = panel.GetComponent<CanvasGroup> ();
		bool on_off = cg.interactable; 
		if (!on_off) { // Toggle interactable
			cg.interactable = true;
			cg.alpha = 1;
		} else {
			cg.interactable = false;
			cg.alpha = 0;
		}
		//settingsButton.GetComponent<Text>.text = "Save Settings";
	}

	public void showPlayerStats (WhiteController whitecell)
	{
		Debug.Log ("show player stats");
		GameObject panel = GameObject.Find ("PlayerPanel");
		CanvasGroup cg = panel.GetComponent<CanvasGroup> (); 
		cg.alpha = 1;

		playerStats = gameObject.GetComponent (typeof(updatePlayerStats)) as updatePlayerStats;
		if (!playerStats)
			playerStats = gameObject.AddComponent (typeof(updatePlayerStats)) as updatePlayerStats;
		playerStats.enabled = true;
		playerStats.SetUpPlayer (whitecell);
	}

	public void removePlayerStats ()
	{
		GameObject panel = GameObject.Find ("PlayerPanel");
		CanvasGroup cg = panel.GetComponent<CanvasGroup> ();
		cg.alpha = 0;
		/*if (playerStats) {
			playerStats.enabled = false;
		} */
	}

	public void pauseGame ()
	{
		if (Time.timeScale == 0) {
			Time.timeScale = 1;
			AudioListener.pause = false;
		} else {
			Time.timeScale = 0;
			AudioListener.pause = true;
		}
	}

	public void ChangeAudioVolume (float volume)
	{
		AudioListener.volume = volume;
	}

	public void ChangeDifficultyLevel (float level)
	{
		difficultyLevel = level;
	}

	public void quitGame ()
	{
		Application.Quit ();
	}

	public void showRestartButton (int on_off)
	{
		Debug.Log ("show restart Button");
		Button restartButton = GameObject.Find ("RestartButton").GetComponent<Button> () as Button;
		CanvasGroup cg = restartButton.GetComponent<CanvasGroup> ();
		if (on_off == 1) {
			cg.interactable = true;
			cg.alpha = 1;
		} else {
			cg.interactable = false;
			cg.alpha = 0;
		}
	}

}
