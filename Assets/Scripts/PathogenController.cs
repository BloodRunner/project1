using UnityEngine;
using System.Collections;

public class PathogenController : CellController {
	static Stats myStats=new Stats();
	public void Awake ()
	{
		myStats.health=1f;
		myStats.power=1f;
		myStats.defense=1f;
		myStats.speed=1f;
	}

	public override float health () {
		return ((stats_health/100.0f) * bodyStats.health * myStats.health);
	}
	public override float defense () {
		return ((stats_defense/100.0f) * bodyStats.defense * myStats.defense);
	}
	public override float power () {
		return ((stats_power/100.0f) * bodyStats.power * myStats.power);
	}
	public override float speed () {
		return ((stats_speed/100.0f) * bodyStats.speed * myStats.speed);
	}
}
