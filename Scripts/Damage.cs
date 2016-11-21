using UnityEngine;
using System.Collections;
using System;

	public class Damage
	{
		private int id;
		private float damagePerSecond;
		private float nextDamage;

		public Damage ( float dms, float nextTime)
		{
			damagePerSecond = dms;
			nextDamage = nextTime;
		}
		public bool nextAttack (float now){
			if (now > nextDamage) {
				nextDamage = now + 1; // in a second
				return true;
			}
			return false;
		}
		public float damage(){
			return damagePerSecond;
		}
	}


