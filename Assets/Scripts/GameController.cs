using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public GameObject infections;
	public Vector3 spawnValues;
	public int infectionCount;
	public int spawnWait;
	public int startWait,waveWait;
	private float score=100; // starts at 100%, dies at 0
	public GUIText healthText;
	private bool gameOver;
	private bool restart=false;
	public GUIText restartText;
	public GUIText gameoverText;

	// Coroutine
	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds (startWait);
		while(true) {
			for (int i = 0; i < infectionCount; i++) {
				// Instantiate at infection point in organs!
				Vector3 position = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (infections, position, spawnRotation);
				// Instantiate returns a Transform so need to add "as GameObject" after call
				// GameObject=... as GameObject
				yield return new WaitForSeconds (spawnWait);
			}
			//Debug.Log ("gameover=" + gameOver);
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
		UpdateScore ();
		gameOver = false;
		restart=false;
		restartText.text = "";
		gameoverText.text = "";
		//StartCoroutine( SpawnWaves ());
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
			}}
	}
		
	void setScore(float scorept) {
		score = scorept;
	}
	void UpdateScore() {
		healthText.text = "Score:" + score;
	}
}
