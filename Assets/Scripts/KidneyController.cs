using UnityEngine;
using System.Collections;

public class KidneyController : OrganController {
	public OrganController otherKidney;

		
	// Constricted movement - what does it mean?
	public override void damageBody(){
		//Debug.Log (name +" damageBody " + bodystate.showStats ());
		if (otherKidney == null || !ReferenceEquals (otherKidney, null)) {
			bodystate.updateRedSpeedStats (stats_speed); // Only one kidney
		} else {
			bodystate.updateRedPowerStats( otherKidney.get_stats_power() + stats_power); // 2 kidneys
		}

	}
}
