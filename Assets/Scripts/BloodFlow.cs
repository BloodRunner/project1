﻿using UnityEngine;
using System.Collections;

public class BloodFlow : MonoBehaviour {

	public Vector3[] points;
	private int destPoint;
	public Vector3 dest;
	private NavMeshAgent agent;
	private IEnumerator coroutine;
	private float delay;
	private float random;
	private float chance1;
	private float chance2;
	private float chance3;
	private float chance4;
	private float chance5;
	private float chance6;

	void Start () {
		
		delay = 0;
		chance1 = 25;
		chance2 = 50;
		chance3 = 66.7f;
		chance4 = 83.4f;
		chance5 = 100f;
		destPoint = 0;
		//whereAmI ();
		setPath();
		agent = GetComponent<NavMeshAgent> ();
		agent.autoBraking = false;
		coroutine = patrol ();
		StartCoroutine (coroutine);
	}



	void BloodFlowing(){
		if (points.Length == 0)
			return;
		else if (destPoint == points.Length - 1) {
			agent.destination = points [destPoint];
			//print (agent.destination.ToString());
			destPoint = 0;

		} else {
			agent.destination = points [destPoint];
			//print (agent.destination.ToString());
			destPoint = (destPoint + 1);
		}

	}

	public IEnumerator patrol(){
		while (true) {
			if (agent.remainingDistance < 0.5f) {
				BloodFlowing ();
				yield return new WaitForSeconds (0.1f);
			} else {
				yield return new WaitForSeconds (0.1f);
			}
		}
	}

	public float getDelay(){
		return delay;
	}

	public void setDelay(float setTo){
		delay = setTo;
	}

	public void setAPatrollPoint(Vector3 thePoint, int place){
		points [place] = thePoint;
	}

	//inserts a patroll point at the designated part of the list and pushes everything at and following that point back.
	public void insertPatrollPoint(Vector3 thePoint, int place){
		Vector3[] newpoints = new Vector3[points.Length + 1];
		for (int i = 0; i < newpoints.Length; i++) {
			if (i == place) {
				newpoints [place] = thePoint;
			} else if (i >= place) {
				newpoints [i] = points [i-1];
			} else {
				newpoints [i] = points [i];
			}
		}
		points = newpoints;
	}

	public void removePoint(int place){
		Vector3[] newpoints = new Vector3[points.Length - 1];
		for (int i = 0; i < points.Length -1; i++){
			if (i >= place) {
				newpoints [i - 1] = points [i];
			} else {
				newpoints [i] = points [i];
			}
		}
		points = newpoints;
	}

	public void whereAmI(){
		Collider[] search = Physics.OverlapSphere(this.GetComponent<Transform>().position,1f);
		for(int i = 0; i < search.Length;i++){
			if(search[i].CompareTag("1")){
				destPoint = 0;
				//print (1);
			} else if(search[i].CompareTag("2")){
				destPoint = 1;
				//print (2);
			} else if(search[i].CompareTag("3")){
				destPoint = 2;
				//print (3);
			}else if(search[i].CompareTag("4")){
				destPoint = 3;
				//print (4);
			}
		}
	}

	public void setPath(){
		//if(path == null){
			random = Random.Range (0,100);
			if(random <= chance1){
				path1 (0);
			}
			else if(random <= chance2){
				path1 (0);
			}
			else if(random <= chance3){
				path1 (0);
			}
			else if(random <= chance4){
				path1 (0);
			}
			else if(random <= chance5){
				path1 (0);
			}
		//}
	}


	public void path1(int place){
		points = new Vector3[4];
		points [0] = new Vector3(10.01f,0f,-1.57f); 
		points [1] = new Vector3(-0.76f,0f,-12.57f); 
		points [2] = new Vector3(-0.88f,0f,0.74f) ;
		points [3] = new Vector3(-6.04f,0f,-5.03f) ;
	}
	public void path2(int place){
		points = new Vector3[4];
		points [0] = new Vector3(10.01f,0f,-1.57f); 
		points [1] = new Vector3(-0.76f,0f,-12.57f); 
		points [2] = new Vector3(-0.88f,0f,0.74f) ;
		points [3] = new Vector3(-9.19f,0f,-7.11f) ;
	}
	public void path3(int place){
		points = new Vector3[4];
		points [0] = new Vector3(10.01f,0f,-1.57f); 
		points [1] = new Vector3(-0.76f,0f,-12.57f); 
		points [2] = new Vector3(-0.88f,0f,0.74f) ;
		points [3] = new Vector3(-15.63f,0f,-19.86f) ;
	}
	public void path4(int place){
		points = new Vector3[5];
		points [0] = new Vector3(10.01f,0f,-1.57f); 
		points [1] = new Vector3(-0.76f,0f,-12.57f); 
		points [2] = new Vector3(-0.88f,0f,0.74f) ;
		points [3] = new Vector3(-15.43f,0f,9.04f) ;
		points [4] = new Vector3(-15.63f,0f,-19.86f);
	}
	public void path5(int place){
		points = new Vector3[5];
		points [0] = new Vector3(10.01f,0f,-1.57f); 
		points [1] = new Vector3(-0.76f,0f,-12.57f); 
		points [2] = new Vector3(-0.88f,0f,0.74f) ;
		points [3] = new Vector3(20.38f,0f,4.14f) ;
		points [4] = new Vector3(20.63f,0f,-9.15f);
	}


	/*
	public Vector3 target1;
	public Vector3 target2;
	public Vector3 target3;
	public float chance1;
	public float chance2;
	public float chance3;
	private float random;
	public BoxCollider interaction;


	//the most not elegant way of dealing with this.
	void OnTriggerStay(Collider collision) {
		
		if (collision.CompareTag ("Host") || collision.CompareTag ("Infection")) {
			random = Random.Range (0,100);
			if(random <= chance1){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target1);
			}
			else if(random <= chance2){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target2);
			}
			else if(random <= chance3){
				collision.GetComponent<Collider> ().GetComponent<NavMeshAgent> ().SetDestination (target3);
			}
		}
	}
	*/

}
