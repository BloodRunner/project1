using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This allows Unity UI to see class and show it
 * */

public class RedController :  CellController{
    
    
    public override float health () {
		if (!bodystate)
			return ((stats_health / 100.0f) * bodyStats.health);
		return ((stats_health/100.0f) * bodyStats.health * bodystate.redHealth());
    }
    public override float defense () {
		if (!bodystate)
			return ((stats_defense / 100.0f) * bodyStats.defense);
		return ((stats_defense/100.0f) * bodyStats.defense * bodystate.redDefense());
    }
    public override float power () {
		return ((stats_power/100.0f) * bodyStats.power * bodystate.redPower());
    }
    public override float speed () {
		if (!bodystate) {
			//Debug.Log ("BodyState is Missing");
			return ((stats_speed / 100.0f) * bodyStats.speed);
		}
		return ((stats_speed/100.0f) * bodyStats.speed * bodystate.redSpeed());
    }
	// Lower number is faster
	public override float reprodRate () {
		if (bodyStats.reprodRate < 1) {
			//Debug.LogError (name +" !!!ReprodRate (<10) is messed up " + bodyStats.reprodRate);
			bodyStats.reprodRate = 1;
		}
		if (!bodystate) {
			//Debug.Log (name +" BodyState is Missing");
			return (bodyStats.reprodRate);
		}
		return (bodyStats.reprodRate * bodystate.redReprodRate());
	}
}
