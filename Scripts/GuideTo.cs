using UnityEngine;
using System.Collections;

public class GuideTo : MonoBehaviour {

	private GameObject[] missions;
	public string myMission;
	private bool badProgrammingBool;

	void Awake(){
		missions = GameObject.FindGameObjectsWithTag ("mission");
		badProgrammingBool = false;
	}


	public void startMission(){
		if (badProgrammingBool == false) {
			badProgrammingBool = true;
			//this.GetComponent<Light> ().enabled = true;
			print ("mission");
			for (int i = 0; i < missions.Length; i++) {
				missions [i].GetComponent<Director> ().startMission (myMission);
			}
		} else {
			badProgrammingBool = false;
			//this.GetComponent<Light> ().enabled = false;
			for (int i = 0; i < missions.Length; i++) {
				missions [i].GetComponent<Director> ().endMission ();
			}
		}
	}
	/*void OnMouseDown(){
		if (badProgrammingBool == false) {
			badProgrammingBool = true;
			this.GetComponent<Light> ().enabled = true;
			print ("mission");
			for (int i = 0; i < missions.Length; i++) {
				missions [i].GetComponent<Director> ().startMission (myMission);
			}
		} else {
			badProgrammingBool = false;
			this.GetComponent<Light> ().enabled = false;
			for (int i = 0; i < missions.Length; i++) {
				missions [i].GetComponent<Director> ().endMission ();
			}
		}
	}*/
}
