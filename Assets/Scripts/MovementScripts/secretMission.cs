using UnityEngine;
using System.Collections;

public class secretMission : MonoBehaviour {


	public string[] destList;
	private GameObject missionCell;
	private GameObject[] cells;
	private GameObject[] killerTCells;
	public string me;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)){
			findNearestCell();
		}

	}

	public void findNearestCell(){
		cells = GameObject.FindGameObjectsWithTag("Host");
		for(int i = 0; i < destList.Length; i ++){
			for (int c = 0; c < cells.Length; c++) {
				if(cells[c].name == "White" ||cells[c].name == "KillerT"){
					if(cells[c].GetComponent<BloodFlow>().getDest() == destList[i]){
						cells [c].GetComponent<BloodFlow> ().setSecretMission (me);
					}
				}
			}
		}
	}
		
}
