using UnityEngine;
using System.Collections;

public class ThymusController : OrganController
{
	// Thymus damage reduces speed
	public override void damageBody(){
		if (!bodystate) {
			Debug.LogError (name + " BodyState is missing");
		} else {
			bodystate.updateWhitePowerStats (stats_health);
			bodystate.updateWhiteReprodStats (stats_health);
		}
		Debug.Log (name +" damageBody " + bodystate.showStats ());
	}
}

