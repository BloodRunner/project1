using UnityEngine;
using System.Collections;

public class LungController : OrganController {
	public LungController otherLung;
	// Lung damage reduces oxygenation power
	public override void damageBody(){
		Debug.Log (name +" damageBody " + bodystate.showStats ());
		if (otherLung == null || !ReferenceEquals (otherLung, null)) {
			bodystate.updateRedPowerStats (stats_health); // Only one lung
		} else {
			bodystate.updateRedPowerStats( otherLung.get_stats_power() + stats_health); // 2 kidneys
		}

	}
}
