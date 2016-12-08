using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WhiteController : CellController {
	GameObject explosion;
	GameObject shooter;

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
			gameController.showMessage (nickname + " killed in combat ", 3);
		} else
			Debug.Log (name + " dies - gameController empty");

		if (tag.Equals ("Hostcell") && this.gameObject.GetComponent<PlayerMovement> ().enabled == true) {
			this.gameObject.GetComponent<CameraChange> ().stopCamera ();
		}
		DestroyObject (gameObject,2);
	}


	public override void updateHealthStats(float point){
		base.updateHealthStats (point);

	}

	public override void updatePowerStats(float point){
		base.updatePowerStats (point);

	}

	public override void updateDefenseStats(float point){
		base.updateDefenseStats (point);
	}

	/*public override void updateRangedAttackSpeedStats(float point){
		base.updateRangedAttackSpeedStats (point);

	}

	public override void updateRangedAttackStats(float point){
		base.updateRangedAttackStats (point);

	}*/

	/*
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
*/
	public void addStats(){
		Debug.Log ("Adding stats to " + nickname + "." + tag);
		gameController.showPlayerStats (this);
	}

	public void removeStats(){
		gameController.removePlayerStats (this);
	}

	public string getNickname(){
		return nickname;
	}

	public void buffMeleeAttack(float val){
		this.bodyStats.power += val;
	}
	public void buffRangedAttack(float val){
		this.GetComponentInChildren<Shooter> ().damagePerShot += (int)val;
	}
	public void buffRangedAttackSpeed(float val){
		if((this.GetComponentInChildren<Shooter> ().timeBetweenBullets - val)> 0 ){
			this.GetComponentInChildren<Shooter> ().timeBetweenBullets -= val;
		}
	}
	public void buffHealth(float val){
		this.bodyStats.health += val;
	}
	public void buffDefence(float val){
		this.bodyStats.defense += val;
	}
	public void buffLifeSpan(float val){
		this.lifespan_in_seconds += val;
	}
}
