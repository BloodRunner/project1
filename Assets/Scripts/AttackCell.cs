using UnityEngine;
using System.Collections;

public class AttackCell : MonoBehaviour {

	private IEnumerator coroutine;
	public float range;
	public SphereCollider agro;
	public GameObject target;
	public bool hasTarget;
	private NavMeshAgent redCell;
	// Use this for initialization
	void Start () {
		hasTarget = false;
		target = null;
		redCell = GetComponent<NavMeshAgent> ();
		coroutine = CHARRGGGEEEE ();
	}


	private IEnumerator CHARRGGGEEEE(){
		if (target == null) {
			StopCoroutine (coroutine);
		}
		redCell.destination = target.transform.position;
		yield return null;
	}

	void OnTriggerEnter(Collider cell){
		if(target==null){
			if(cell.CompareTag("cell")){
				target = cell.gameObject;
				StartCoroutine (coroutine);
			}
		}
	}


}
