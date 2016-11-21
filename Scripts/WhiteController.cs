﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WhiteController : CellController {
	GameObject explosion;
	GameObject shooter;
	public Text playerNameText;
	bool is_player = false;

	void Start() {
		setNickname (Namer.getName ());
	}

	// health is based on state at time of spawn
	public void setNickname(string name) {
		nickname= name;
	}
	public override float health () {
		return ((stats_health/100.0f) * bodyStats.health );
	}
	public override float defense () {
		if (bodystate == null) {
			return ((stats_defense / 100.0f) * bodyStats.defense);
		}
		return ((stats_defense/100.0f) * bodyStats.defense * bodystate.whiteDefense());
	}
	public override float power () {
		if (bodystate == null)
			return((stats_power / 100.0f) * bodyStats.power);
		return ((stats_power/100.0f) * bodyStats.power * bodystate.whiteCellPower());
	}
	public override float speed () {
		if (bodystate == null)
			return ((stats_speed / 100.0f) * bodyStats.speed);
		return ((stats_speed/100.0f) * bodyStats.speed * bodystate.whiteSpeed());
	}
	// Lower number is faster, doesn't get damaged by enemies directly
	public override float reprodRate () {
		if (bodyStats.reprodRate < 1) {
			Debug.LogError (name +" !!!ReprodRate (<1) is messed up " +bodyStats.reprodRate);
			bodyStats.reprodRate = 1;
		}
		if (!bodystate) {
			Debug.Log (name +" BodyState is Missing");
			return (bodyStats.reprodRate);
		}
		return (bodyStats.reprodRate * bodystate.whiteReprodRate());
	}

	public override void deathHandler (){
		//Debug.Log (name + " dies ");
		if (explosion!=null && name.Equals ("WhiteT")) {
			GameObject gobj = Instantiate (explosion, transform.position, Quaternion.identity) as GameObject;;
			DestroyObject (gobj, 2);
		}
		if (gameController != null) {
			gameController.showMessage (name + " dies ", 3);
		} else
			Debug.Log (name + " dies - gameController empty");
		DestroyObject (gameObject,2);
	}


	public override void updateHealthStats(float point){
		base.updateHealthStats (point);
		if (is_player && healthSlider!=null)
			healthSlider.value = stats_health * bodystate.whiteHealth();
	}

	public override void updateDefenseStats(float point){
		base.updateDefenseStats (point);
		if (is_player && defenseSlider!=null)
			defenseSlider.value = stats_defense * bodystate.whiteDefense();
	}

	public void addShooter() {
		Debug.Log ("Adding shooter to " + name + "." + tag);
		//GameObject shooterObj = Resources.Load<GameObject>("Shooter");
		Shooter shooter = gameObject.AddComponent( typeof(Shooter) ) as Shooter;
		shooter.transform.SetParent(gameObject.transform, false);
	}

	public void removeShooter() {
		if (shooter != null) {
			DestroyObject (shooter);
			shooter = null;
		}
	}

	public void addStats(){
		Debug.Log ("Adding stats to " + nickname + "." + tag);
		is_player = true;
		if (playerNameText != null)
			playerNameText.text = nickname;
	}

	public void removeStats(){
		is_player = false;
	}
}