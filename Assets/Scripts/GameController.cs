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
	public string[] levelEditor;
	//private string[] listOfOrgans = new string[] {"Heart (E/F)","Left Lung (C)","Stomach (G)","Liver (H)","Right Lung (D)","Brain (A)","Left Kidney (I)","Thymus (B)","Right Kidney (J)"};
	string[] pathogens;
	public string[] instructions = new string[10];
	public int instructionWait = 5;
	public BodyState bodystate;
	// Whole Body Status
	//public Vector3 spawnValues;
	public GameObject infectedOrgan;
	private int infectionCount;
	public int redCount, whiteCount;
	public int spawnWait;
	public int startWait, waveWait;
	private int killTcount = 0;
	// A new one every N point in Score
	private float score = 0;
	private bool gameOver;
	private bool restart = false;
	public Text characterCount;
	public Text instructionText;
	public Text scoreText;
	public Text messageText;
	Button restartButton;
	public Text gameoverText;
	public Camera topCamera;
	public Camera followCamera;
	protected int numInfections = 0;
	//MessageBoard msgbd = new MessageBoard ();
	OrganController[] all_organs;
	OrganController thymus;
	float timer;
	// timer to limit the UI tally update
	float difficultyLevel = 1;
	updatePlayerStats playerStats;
	bool WaitForNextLevel;
	int level=0;
	GameObject playerpanel;
	CanvasGroup playerpanel_cg;
	AudioSource readyToGoSound, gameOverSound, victorySound;
	private GameObject[] boxes;


	// Use this for initialization
	void Start ()
	{
		boxes = GameObject.FindGameObjectsWithTag ("directional");
		for(int i = 0; i < boxes.Length; i++){
			boxes [i].gameObject.SetActive (false);
		}
		//msgbd = new MessageBoard ();
		showRestartButton (0);
		deadpenguin (false);
		score = 0;
		UpdateScore (0);
		gameOver = false;
		restart = false;
		if (gameoverText)
			gameoverText.text = "";
		if (messageText)
			messageText.text = "";
		//healthText.text = "";
		topCamera = GameObject.Find ("topCamera").GetComponent<Camera> ();
		followCamera = GameObject.Find ("followCamera").GetComponent<Camera> ();
		followCamera.enabled = false;
		topCamera.enabled = true;
		playerpanel = GameObject.Find ("PlayerPanel");
		playerpanel_cg = playerpanel.GetComponent<CanvasGroup> ();

		AudioSource [] audiosources = gameObject.GetComponentsInChildren<AudioSource> ();
		for (int i =0; i < audiosources.Length; i++) {
			if (audiosources[i].name.Equals("GameOverSound"))
				gameOverSound = audiosources[i];
			else if (audiosources[i].name.Equals("readyToGoSound"))
				readyToGoSound = audiosources[i];
			else if (audiosources[i].name.Equals("VictorySound"))
				victorySound = audiosources[i];		
		}
		if (bodystate == null) {
			Debug.Log ("Game Controller missing body state");
		}
		pathogens = new string[infections.Length];
		for (int i = 0; i < infections.Length; i++) {
			pathogens [i] = infections [i].name;
		}
		all_organs = GameObject.FindObjectsOfType (typeof(OrganController)) as OrganController[];
		ThymusController tc = GameObject.FindObjectOfType (typeof(ThymusController)) as ThymusController;
		//StartCoroutine (SpawnInstructions ());
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

	public int tallyCharacters ()
	{
		int[] count = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };// Bad coding practice!
		string total = "Ally Count:";
		int white = 0, t = 0;
		CellController[] cells = GameObject.FindObjectsOfType (typeof(WhiteController)) as WhiteController[];
		for (int i = 0; i < cells.Length; i++) {
			if (cells [i].name.Equals ("White")) {
				white++;
			} else {
				t++;
			}
		}
		total += "\nKiller T Cell:" + t;
		total += "\nWhite Blood Cell:" + white;
		cells = GameObject.FindObjectsOfType (typeof(RedController)) as RedController[];
		total += "\nRed Blood Cell:" + cells.Length;
		cells = GameObject.FindObjectsOfType (typeof(PathogenController)) as PathogenController[];
		total += "\nEnemy Count:";
		numInfections = cells.Length; 
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
		return numInfections;
	}

	public bool bodyIsAlive ()
	{
		int alive = 0;
		foreach (OrganController organ in all_organs) {
			if (organ.get_stats_health () > 0) {
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
		if (!bodyIsAlive ()) {
			deadpenguin (true);
			GameOver ();
		}
	}

	public bool isGameOver ()
	{
		return gameOver;
	}


	public int numInfection ()
	{
		CellController[] cells = GameObject.FindObjectsOfType (typeof(PathogenController)) as PathogenController[];
		//Debug.Log (" Number of infection cells " + cells.Length);
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


	IEnumerator spawnLevel (int level)
	{
		CellController cell;
		char[] delim = { ',' };
		bool foundOrgan = false;
		string[] words;
		string organs = "";
		PathogenController cc = infections [0];
		words = levelEditor [level].Split (delim);

		bool randomed = false;
		for (int i = 0; i < words.Length; i++) {
			for (int x = 0; x < all_organs.Length; x++) {
				if (words [i] == all_organs [x].name) {
					organs += " " + all_organs [x].name;
					organs += ",";
				} else if (!randomed && words [i].Equals ("Random")) {
					organs += " random organs,";
					randomed = true;
				}
			}
		}
		organs.TrimEnd (',');
		showMessage ("Level " + (level+1) + ":" + "Pathogens are coming out of" + organs, 25);
		//Debug.Log ("!!!Level " + (level + 1) + ":" + "Pathogens are coming out of" + organs);
		for (int i = 0; i < words.Length; i++) {
			for (int x = 0; x < all_organs.Length; x++) {
				if (words [i] == all_organs [x].name) {
					foundOrgan = true;
					infectedOrgan = GameObject.Find (all_organs [x].name);
					Debug.Log (infectedOrgan.name);
					break;
				}
			}
			if (foundOrgan == true) {
				foundOrgan = false;
				continue;
			} else if (words [i] == "Random") {
				int rand = Random.Range (0, all_organs.Length);
				infectedOrgan = GameObject.Find (all_organs [rand].name);
				foundOrgan = false;
				continue;
			} else {
				foundOrgan = false;
				char[] inf = words [i].ToCharArray (0, 1);
				int infT = (int)inf [0];
				//print (infT.ToString());
				if (infT > 71) {
					Debug.Log ("Spawn Level out of range " + words [i]+ " " + (infT - 'A'));
					print (infT.ToString ());
				}

				cc = infections [infT - 65];
				int infNum = int.Parse (words [i].Substring (1));
				infectionCount = infNum * (int)(4 * difficultyLevel + 1);
				for (int z = 0; z < infectionCount; z++) {
					// Instantiate at infection point in organs!
					cell = Instantiate (cc, infectedOrgan.transform.position, Quaternion.identity) as PathogenController;
					cell.bodystate = this.bodystate;
					cell.gameController = this;
					//Debug.Log ("Sending out " + cell.name);
					yield return new WaitForSeconds (spawnWait);
				}
			}
		}
	}

	// Coroutine
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		CellController cell;
		//int level = 0;
		/*const*/
		int numlevels = levelEditor.Length;
		PathogenController cc = infections [0];
		while (!gameOver && level < numlevels) {
			//showNextLevelButton (1);
			StartCoroutine (spawnLevel (level));
			cc = infections [level % infections.Length];
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
			// Wait for player to kill all pathogens or die
			if (level < numlevels) {// increase in difficulty
				//yield return new WaitForSeconds (waveWait);
				while (numInfection () > 0) {					
					if (gameOver) {
						//restart = true;
						//showRestartButton (1);
						break;
					}
					yield return new WaitForSeconds (3);
				}
				level++;
				if (!gameOver) {
					if (level < numlevels) {
						showMessage ("You have lived through level " + level + ". More pathogens are coming", 1);			
						showNextLevelButton (1);

						yield return new WaitWhile (() => WaitForNextLevel);								
					} else {
						Victory (); // All pathogens killed and no more levels
					}
				}
			}
		}
		/*
		if (gameOver) {
			restart = true;
			showRestartButton (1);
		} */
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
			cell = Instantiate (killerT, GameObject.Find ("Organ_Thymus").transform.position, spawnRotation) as WhiteController;
			cell.bodystate = this.bodystate;
			cell.gameController = this;
		}

	}

	public void RestartGame ()
	{
		SceneManager.LoadSceneAsync (1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadSceneAsync (1);
			}
		}
		timer += Time.deltaTime;

		if (timer >= 5) {//  - do this every 5 seconds
			if (score > 0) {
				if (killerT != null && killerT.points > 0) {
					int count = (int)(score / killerT.points);
					if (count > killTcount)
						Debug.Log ("Spawning " + count + "-" + killTcount + " Killer T cells");
					spawnKillerT (count - killTcount);
					killTcount = count;

				}
			}
			tallyCharacters ();
		}
	}


	public void UpdateScore (float scorept)
	{
		score += scorept;
		if (scoreText != null)
			scoreText.text = "Score:" + score + " Level:"+ level;
		
	}

	public void GameOver ()
	{
		gameOver = true;
		if (gameOverSound)
			gameOverSound.Play ();
		if (gameoverText)
			gameoverText.text = "Game Over";
		showRestartButton (1);
	}

	public void Victory ()
	{
		gameOver = true;
		if (victorySound)
			victorySound.Play ();
		if (gameoverText)
			gameoverText.text = "You have won! All pathogens are eliminated!";
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

	public void showInstructions ()
	{
		GameObject panel = GameObject.Find ("InstructionsScrollView");
		CanvasGroup cg = panel.GetComponent<CanvasGroup> ();
		bool on_off = cg.interactable; 
		if (!on_off) { // Toggle interactable
			cg.interactable = true;
			cg.alpha = 1;
			Time.timeScale = 0;
		} else {
			cg.interactable = false;
			cg.alpha = 0;
			Time.timeScale = 1;
		}
	}


	public void showPlayerStats (WhiteController whitecell)
	{
		//Debug.Log ("show player stats");
		if (playerpanel_cg == null) {
			playerpanel = GameObject.Find ("PlayerPanel");
			playerpanel_cg = playerpanel.GetComponent<CanvasGroup> (); 
		}
		playerpanel_cg.alpha = 1;
		if (!playerStats)
			playerStats = gameObject.GetComponent (typeof(updatePlayerStats)) as updatePlayerStats;
		if (!playerStats)
			playerStats = gameObject.AddComponent (typeof(updatePlayerStats)) as updatePlayerStats;
		playerStats.enabled = true;
		playerStats.SetUpPlayer (whitecell);
	}

	public void removePlayerStats ()
	{
		if (playerpanel_cg == null) {
			playerpanel = GameObject.Find ("PlayerPanel");
			playerpanel_cg = playerpanel.GetComponent<CanvasGroup> (); 
		}
		playerpanel_cg.alpha = 0;
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
		Button restartButton = GameObject.Find ("RestartButton").GetComponent<Button> () as Button;
		CanvasGroup cg = restartButton.GetComponent<CanvasGroup> ();
		//Debug.Log ("Restart button pressed");
		if (on_off == 1) {
			restartButton.enabled = true;
			cg.interactable = true;
			cg.alpha = 1;
		} else {
			cg.interactable = false;
			cg.alpha = 0;
		}
	}

	public void nextLevel ()
	{
		//Debug.Log ("Next level button pressed");
		if (readyToGoSound)
			readyToGoSound.Play ();
		WaitForNextLevel = false;
		showNextLevelButton (0);
		//StartCoroutine (spawnLevel (level));
	}

	public void showNextLevelButton (int on_off)
	{
		//Debug.Log ("show next level button");
		Button nlButton = GameObject.Find ("NextLevelButton").GetComponent<Button> () as Button;
		Button pauseButton = GameObject.Find ("pauseButton").GetComponent<Button> () as Button;

		CanvasGroup cg = nlButton.GetComponent<CanvasGroup> ();
		if (on_off == 1) {
			WaitForNextLevel = true;
			cg.interactable = true;
			cg.alpha = 1;
			nlButton.enabled = true;
			pauseButton.enabled = false;
			Time.timeScale = 0;
		} else {
			cg.interactable = false;
			cg.alpha = 0;
			pauseButton.enabled = true;
			Time.timeScale = 1;
		}


	}

	void deadpenguin(bool on_off) {
		SpriteRenderer xeye = GameObject.Find("xeye").GetComponent<SpriteRenderer>();
		SpriteRenderer no_cross = GameObject.Find("no_cross").GetComponent<SpriteRenderer>();
		xeye.enabled = on_off;
		no_cross.enabled   = on_off;
	}
}
