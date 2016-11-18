using UnityEngine;
using System.Collections;

public class PathogenController : CellController {

	public override float health () {
		return ((stats_health/100.0f) * bodyStats.health );
	}
	public override float defense () {
		return ((stats_defense/100.0f) * bodyStats.defense );
	}
	public override float power () {
		return ((stats_power/100.0f) * bodyStats.power );
	}
	public override float speed () {
		return ((stats_speed/100.0f)  * bodyStats.speed);
	}

}
