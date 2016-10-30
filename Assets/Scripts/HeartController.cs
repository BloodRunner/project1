﻿using UnityEngine;
using System.Collections;

// Tag = Host
// Name = organ name
public class HeartController : OrganController {
	// Heart damage reduces speed
	public override void damageBody(){
		slowDownAllCells(myorganStats.health);
	}
		
}