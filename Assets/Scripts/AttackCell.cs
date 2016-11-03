using UnityEngine;
using System.Collections;

public class AttackCell : MonoBehaviour {

	private IEnumerator coroutine;
	public float range;
	public SphereCollider agro;
	public GameObject target;
	public bool hasTarget;
	private NavMeshAgent me;
	private Vector3 dest;
	// Use this for initialization
	void Start () {
		hasTarget = false;
		target = null;
		me = GetComponent<NavMeshAgent> ();
		coroutine = CHARRGGGEEEE ();
	}


	private IEnumerator CHARRGGGEEEE(){
		if (target == null) {
			me.SetDestination (dest);
			StopCoroutine (coroutine);
		}
		me.destination = target.transform.position;
		yield return null;
	}

	void OnTriggerEnter(Collider cell){
		if(target==null){
			if(cell.name == "Red"){
				dest = me.destination;
				target = cell.gameObject;
				StartCoroutine (coroutine);
			}
		}
	}


}
