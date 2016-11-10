using UnityEngine;
using System.Collections;

public class StomachController : OrganController
{
	// Stomach damage reduces defense
	public override void damageBody(){
		if (!bodystate) {
			Debug.LogError (name + " BodyState is missing");
		} else {
			//bodystate.updateRedHealthStats (stats_health);
			//bodystate.updateWhiteHealthStats (stats_health);
			bodystate.updateRedDefenseStats (stats_health);
			bodystate.updateWhiteDefenseStats (stats_health);
		}
		Debug.Log (name +" damageBody " + bodystate.showStats ());
	}
}

