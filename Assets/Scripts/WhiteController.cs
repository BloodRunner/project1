using UnityEngine;
using System.Collections;

public class WhiteController : CellController {

	public override float health () {
		return ((stats_health/100.0f) * bodyStats.health * bodystate.whiteHealth());
	}
	public override float defense () {
		return ((stats_defense/100.0f) * bodyStats.defense * bodystate.whiteDefense());
	}
	public override float power () {
		return ((stats_power/100.0f) * bodyStats.power * bodystate.whitePower());
	}
	public override float speed () {
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
}
