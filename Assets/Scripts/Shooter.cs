using UnityEngine;
using System.Collections;
using UnityStandardAssets;

public class Shooter : MonoBehaviour {
	ParticleSystem weapon;
	private RaycastHit hit;
	private Ray ray;
	public int damagePerShot = 20;                  // The damage inflicted by each bullet.
	public float timeBetweenBullets = 0.15f;        // The time between each shot.
	public float range = 100f;                      // The distance the gun can fire.

	float timer;                                    // A timer to determine when to fire.
	Ray shootRay;                                   // A ray from the gun end 
	RaycastHit shootHit;                            // A raycast hit to get the closest hit
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	int groundMask;  
	ParticleSystem gunParticles;                    // Reference to the particle system.
	LineRenderer gunLine;                           // Reference to the line renderer.
	AudioSource gunAudio;                           // Reference to the audio source.
	float effectsDisplayTime = 0.2f;  

	void Awake ()
	{
		// Create a layer mask for the Shootable layer.
		shootableMask = LayerMask.GetMask ("Infection") | LayerMask.GetMask ("Walls");
		groundMask = LayerMask.GetMask ("Ground") | LayerMask.GetMask ("Walls");

		// Set up the references.
		gunParticles = GetComponent<ParticleSystem> ();
		gunAudio = GetComponent<AudioSource> ();

		gameObject.AddComponent<LineRenderer>();
		gunLine = GetComponent<LineRenderer>();
		gunLine.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		gunLine.SetWidth(0.1f, 0.1f);
		gunLine.SetColors(Color.red, Color.yellow);
		gunLine.enabled = false;
	}

	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

		//if (Physics.Raycast (ray, out hit, 5)) {
			/*
				Vector3 targetPoint = ray.GetPoint(hit);
			//	Vector3 distance = Vector3.Distance(hit.transform.position, transform.position); // Within shooting range?
				Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
				transform.rotation = targetRotation;
				Debug.Log ("Weapon hits " + hit.collider.tag);
				Vector3 bulletHolePosition = hit.point + hit.normal * 0.01f;
*/
			//Quaternion bulletHoleRotation = Quaternion.FromToRotation (-Vector3.forward, hit.normal);
			//GameObject hole = (GameObject)GameObject.Instantiate (bulletHolePrefab, bulletHolePosition, bulletHoleRotation);

		//}
		// If the Fire1 button is being press and it's time to fire...
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets)
		{
			Camera cx = Camera.current;
			Camera[] cxs = Camera.allCameras;
			if (cx == null) {
				cx = cxs [0]; // Why is current null half the time
				Debug.Log("Camera Current is null!");
			}
			Ray ray= cx.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			//DrawLine (transform.position, ray.GetPoint(30), Color.red, 0.1f);
			if (Physics.Raycast (ray,out hit,groundMask)) {
				//Debug.DrawLine(ray,hit);
				Vector3 adjusted = ray.GetPoint( hit.distance - 0.1f );
				Vector3 end = new Vector3 (adjusted.x, 0.1f, adjusted.z);
				//Debug.Log("Hit the ground at "+ hit.point+ " dir "+ dir);
				Shoot (end);
				//DrawLine (transform.position, hit.point, Color.yellow, 0.2f);
			}

		}

		// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
		if(timer >= timeBetweenBullets * effectsDisplayTime)
		{
			// ... disable the effects.
			DisableEffects ();
		}
	}

	public void DisableEffects ()
	{
		// Disable the line renderer and the light.
		gunLine.enabled = false;
		//gunLight.enabled = false;
	}

	void Shoot (Vector3 dir)
	{
		// Reset the timer.
		timer = 0f;

		// Play the gun shot audioclip.
		//gunAudio.Play ();
		//Quaternion targetRotation = Quaternion.LookRotation(dir - transform.position);
		//transform.rotation = targetRotation;

		//gunParticles.transform.LookAt (dir);
		transform.rotation = Quaternion.LookRotation (dir- transform.position);
		gunParticles.Play ();
		//	weapon.Play ();
		// Enable the line renderer and set it's first position to be the end of the gun.
		gunLine.enabled = true;
		// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
		shootRay.origin = transform.position;
		shootRay.direction = dir- transform.position; //transform.forward;
		gunLine.SetPosition (0, shootRay.origin);
		// Perform the raycast against gameobjects on the shootable layer and if it hits something...
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			if (shootHit.collider.tag.Equals("Infection")) {
				Debug.Log ("shot enemy " + shootHit.collider.name );
			// Try and find the cell script on the gameobject hit.
				CellController enemy = shootHit.collider.GetComponent <CellController> ();
				if(enemy != null)
					enemy.TakeDamage (damagePerShot, shootHit.point);
			}
			// Set the second position of the line renderer to the point the raycast hit.
			gunLine.SetPosition (1, shootHit.point);
		}
		// If the raycast didn't hit anything on the shootable layer...
		else
		{
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}

	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
	{
		//Debug.Log ("start line=" + start + " to " + end);
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.SetColors(color, color);
		lr.SetWidth(0.1f, 0.1f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}


}