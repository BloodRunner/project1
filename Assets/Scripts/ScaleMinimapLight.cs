using UnityEngine;
using System.Collections;

public class ScaleMinimapLight : MonoBehaviour {

	public Light organ;
	public Color alive;
	public Color dead;
	public GameObject state;
	public string nameOrgan;

	// Use this for initialization
	void Start () {
		organ = GetComponent<Light> ();
		state = GameObject.Find (nameOrgan);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(state.GetComponent<OrganController> ().showStats());
		organ.color = Color.Lerp(dead,alive,state.GetComponent<OrganController>().get_stats_health());
	}
}
