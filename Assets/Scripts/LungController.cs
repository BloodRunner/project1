using UnityEngine;
using System.Collections;

public class LungController : OrganController {

	// Lung damage reduces oxygenation power
	public override void damageBody(){
		if (!bodystate) {
			Debug.LogError (name+" BodyState is missing");
		}
		else
			bodystate.updateRedPowerStats(stats_health);
		Debug.Log (name +" damageBody " + bodystate.showStats ());
	}
}
