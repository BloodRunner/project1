using UnityEngine;
using System.Collections;

public class NextWaypoint : MonoBehaviour {

	public string target;
	public string target2;
	public string toHeart1;
	public string toHeart2;
	public string toLLung;
	public string toRLung;
	public string toStomach;
	public string toLiver;
	public string toBrain;
	public string toLKidney;
	public string toRKidney;
	public string toThymus;

	public string requestTarget(string dest){
		if(dest == "thymus"){
			return toThymus;
		}else if(dest == "heart1"){
			return toHeart1;
		}else if(dest == "heart2"){
			return toHeart2;
		}else if(dest == "lung1"){
			return toLLung;
		}else if(dest == "lung2"){
			return toRLung;
		}else if(dest == "stomach"){
			return toStomach;
		}else if(dest == "liver"){
			return toLiver;
		}else if(dest == "brain"){
			return toBrain;
		}else if(dest == "kidney2"){
			return toLKidney;
		}else if(dest == "kidney1"){
			return toRKidney;
		}else{
			return target;
		}
	}
}
