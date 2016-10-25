using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public GameObject hazards;
	public Vector3 spawnValues;
	public int pressure;
	public int hazardCount;
	public int spawnWait;
	public int startWait,waveWait;
	private int score;
	public GUIText scoretext;
	private bool gameOver;
	private bool restart=false;
	public GUIText restartText;
	public GUIText gameoverText;

	// Coroutine
	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds (startWait);
		while(true) {
			for (int i = 0; i < hazardCount; i++) {
				Vector3 position = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazards, position, spawnRotation);
				// Instantiate returns a Transform so need to add "as GameObject" after call
				// GameObject=... as GameObject
				yield return new WaitForSeconds (spawnWait);
			}
			Debug.Log ("gameover=" + gameOver);
			if (gameOver) {
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
	// Use this for initialization
	void Start () {
		score = 0;
		UpDateScore ();
		gameOver = false;
		restart=false;
		restartText.text = "";
		gameoverText.text = "";
		StartCoroutine( SpawnWaves ());
	}

	public void GameOver() {
		gameOver = true;
		gameoverText.text = "Game Over";
	}
	
	// Update is called once per frame
	void Update () {
		if (restart) {
			if (Input.GetKeyDown(KeyCode.R)) {
				SceneManager.LoadSceneAsync(0);
				//Application.LoadLevel(Application.loadedLevel);
			}}
	}

	public void addScore(int scoreValue){
		score += scoreValue;
		UpDateScore ();
	}

	void UpDateScore() {
		scoretext.text = "Score:" + score;
	}
}
