﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class updatePlayerStats : MonoBehaviour
{
	private float bonusMeleeAttackPower;
	private float bonusRangedAttackPower;
	private float bonusLifeSpan;
	private float bonusRangedAttackSpeed;
	private float bonusHealth;
	private float bonusDefence;
	float timer = 0;
	WhiteController whitecell;
	Slider healthSlider=null;
	Slider defenseSlider=null;
	Slider attackSlider=null;
	Slider lifespanSlider=null;
	Text playerNameText=null;

	// Use this for initialization
	void Start ()
	{
		healthSlider = GameObject.Find ("PlayerHealthSlider").GetComponent<Slider> ();
		attackSlider = GameObject.Find ("PlayerAttackSlider").GetComponent<Slider> ();
		defenseSlider = GameObject.Find ("PlayerDefenseSlider").GetComponent<Slider> ();
		lifespanSlider = GameObject.Find ("PlayerLifespanSlider").GetComponent<Slider> ();
		playerNameText = GameObject.Find ("PlayerNameText").GetComponent<Text> ();
	}

	public void SetUpPlayer (WhiteController whitecell)
	{
		Start ();
		if (playerNameText != null)
			playerNameText.text = whitecell.getNickname ();
		this.whitecell = whitecell;
		this.whitecell.updatePowerStats (bonusMeleeAttackPower);
		this.whitecell.updateRangedAttackStats (bonusRangedAttackPower);
		this.whitecell.GetComponentInChildren<Shooter> ().damagePerShot += (int)bonusRangedAttackPower;
		this.whitecell.GetComponentInChildren<Shooter> ().timeBetweenBullets += (int)bonusRangedAttackSpeed;
	}

	public void updateStats ()
	{
		if (whitecell == null)
			return;
		//try { // The cell can die meanwhile
			if (healthSlider != null)
				healthSlider.value = whitecell.health () * 100f;
			if (defenseSlider != null)
				defenseSlider.value = whitecell.defense () * 100f;
			if (attackSlider != null)
				attackSlider.value = whitecell.power () * 100f;
			if (lifespanSlider != null)
				lifespanSlider.value = whitecell.time_left_to_live () / whitecell.lifespan_in_seconds;
			//Debug.Log ("Time left to live " + whitecell.time_left_to_live ());
			if (whitecell.time_left_to_live () < 2) {
				Debug.Log (whitecell.getNickname () + " dies of old age");
				whitecell = null;
			}
		//} catch {}
	}

	// Update is called once per frame
	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
		if (timer > 2) { // update player stats every 2 seconds
			updateStats ();
			timer = 0;
		}
	}
}
