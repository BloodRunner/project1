using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directional : MonoBehaviour {
	//0 = right 1 = left
	public string[] mission = new string[2];


	public string getMission(int x){
		return mission[x];
	}

}
