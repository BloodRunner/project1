using UnityEngine;
using System.Collections;

public class WhiteController : CellController {
	
	// health is based on state at time of spawn
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
		return ((stats_power/100.0f) * bodyStats.power * bodystate.whitePower());
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
}
