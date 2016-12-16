using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* This allows Unity UI to see class and show it
 * */

public class RedController :  CellController{  
	// health is based on state at time of spawn
    public override float health () {
		if (bodystate==null)
			return ((stats_health / 100.0f) * bodyStats.health);
		return ((stats_health/100.0f) * bodyStats.health * bodystate.redHealth());
    }
    public override float defense () {
		if (bodystate==null)
			return ((stats_defense / 100.0f) * bodyStats.defense);
		return ((stats_defense/100.0f) * bodyStats.defense * bodystate.redDefense());
    }
    public override float power () {
		if (bodystate == null)
			return ((stats_power / 100.0f) * bodyStats.power);
		return ((stats_power/100.0f) * bodyStats.power * bodystate.redPower());
    }
    public override float speed () {
		if (bodystate==null) {
			//Debug.Log ("BodyState is Missing");
			return ((stats_speed / 100.0f) * bodyStats.speed);
		}
		return ( .5f + (stats_speed/100.0f) * bodyStats.speed * bodystate.redSpeed());
    }
	// Lower number is faster
	public override float reprodRate () {
		if (bodyStats.reprodRate < 1) {
			Debug.LogError (name +" !!!ReprodRate (<1) is messed up " + bodyStats.reprodRate);
			bodyStats.reprodRate = 1;
		}
		if (!bodystate) // Why is this null when it is set in unity UI
			bodystate = GetComponent<BodyState> ();
		if (!bodystate) {
			Debug.Log (name +" BodyState is Missing");
			return (bodyStats.reprodRate);
		}
		return (bodyStats.reprodRate * bodystate.redReprodRate());
	}
}
