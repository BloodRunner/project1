using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
	public float freq = 1.0f;   
	float timer=0;
	SpriteRenderer sr;
	Color white = new Color(255,255,255);
	Color black = new Color(0,0,0);
	void Awake () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= freq){
			timer = 0f;
			sr.color = white;
		}

		if(timer >= freq * .5)
		{
			// off
			sr.color = black;
		}
	}
}
