using UnityEngine;
using System.Collections;

/* This cell is spawned when we have enough points from killed pathogens */
public class KillerTcellController : WhiteController
{	
	//public int pointcost; // number of points cost for each Killer T cell
	public ParticleSystem weapon;
	private RaycastHit hit;
	private Ray mouseray, ray;
	private RaycastHit mousehit;
	// Lightning ball
	public override void special ()
	{
		if (Input.GetButtonDown ("Fire1")) { 
			Debug.Log ("TKiller T shooting");
			if (Physics.Raycast (mouseray, out mousehit)) {
				if (weapon != null && weapon.IsAlive ()) {
					mouseray = Camera.main.ScreenPointToRay (Input.mousePosition); // Construct a ray from the current mouse coordinates
					var direction = (mousehit.point - transform.position); 
					ray = new Ray (transform.position, direction); // ray firing from cell's location towards the mouse click point in world coords
					Debug.DrawRay (ray.origin, ray.direction, Color.yellow);

					weapon.transform.position = transform.position;
					weapon.transform.rotation = Quaternion.LookRotation (direction);
					weapon.Play ();
				}
			}
		}
	}

}

