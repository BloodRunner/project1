using UnityEngine;
using System.Collections;

public class Zapper : MonoBehaviour {
	public float damagePerZap;
	void Start(){
	}

	void Update(){
	}

	void OnTriggerEnter(Collider other) {
		//Debug.Log(name+"-"+tag+" zapped "+ other.name+"="+other.tag);
		if (other.tag.Equals("Infection")){
			Debug.Log(name+"-"+tag+" zapped "+ other.name+"="+other.tag);
			CellController enemy = other.GetComponent <CellController> ();
			if(enemy != null)
				enemy.TakeDamage (damagePerZap, transform.position);
		}
	}

}
