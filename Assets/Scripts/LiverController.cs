using UnityEngine;
using System.Collections;

public class LiverController : OrganController {
	// Heart damage reduces body cell reproduction
	public override void damageBody(){
		if (!bodystate) {
			Debug.LogError (name + " BodyState is missing");
		} else {
			bodystate.updateRedReprodStats (stats_health);
			bodystate.updateWhiteReprodStats (stats_health);
			// TODO: pathogen speed
		}
		Debug.Log (name +" damageBody " + bodystate.showStats ());
	}
}
