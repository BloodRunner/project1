using UnityEngine;
using System.Collections;

// Tag = Host
// Name = organ name
public class HeartController : OrganController {
	// Heart damage reduces speed
	public override void damageBody(){
		if (!bodystate) {
			Debug.LogError (name + " BodyState is missing");
		} else {
			bodystate.updateRedSpeedStats (stats_health);
			bodystate.updateWhiteSpeedStats (stats_health);
			// TODO: pathogen speed
		}
		Debug.Log (name +" damageBody " + bodystate.showStats ());
	}

}
