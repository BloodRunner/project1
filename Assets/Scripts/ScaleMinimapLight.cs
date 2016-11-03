using UnityEngine;
using System.Collections;

public class ScaleMinimapLight : MonoBehaviour {

	public Light organ;
	public Color alive;
	public Color dead;

	// Use this for initialization
	void Start () {
		organ = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		//organ.color = Color.Lerp(alive,dead,the healthscale);
	}
}
