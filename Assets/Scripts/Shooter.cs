using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityStandardAssets;

public class Shooter : MonoBehaviour {
	private RaycastHit hit;
	private Ray ray;
	public int damagePerShot = 20;                  // The damage inflicted by each bullet.
	public float timeBetweenBullets = 0.15f;        // The time between each shot.
	public float range = 100f;                      // The distance the gun can fire.

	float timer=0;                                    // A timer to determine when to fire.
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
		shootableMask = LayerMask.GetMask("Infection", "Walls", "Organ");
		groundMask = LayerMask.GetMask("Ground");
		// Set up the references.
		gunParticles = GetComponent<ParticleSystem> ();
		gunAudio = GetComponent<AudioSource> ();

		gunLine = GetComponent<LineRenderer> ();
		if (gunLine == null) {
			gameObject.AddComponent<LineRenderer> ();
			gunLine = GetComponent<LineRenderer> ();
		}
		gunLine.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
		gunLine.SetWidth (0.1f, 0.1f);
		gunLine.SetColors (Color.red, Color.yellow);
		gunLine.enabled = false;
	}

	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;
		// If the Fire1 button is being press and it's time to fire...
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets)
		{
			Camera cx = Camera.current;
			Camera[] cxs = Camera.allCameras;
			if (cx == null) {
				cx = cxs [0]; // Why is current null half the time
				//Debug.Log("Camera Current is null!");
			}
			Ray ray= cx.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			//DrawLine (transform.position, ray.GetPoint(30), Color.red, 0.1f);
			if (Physics.Raycast (ray,out hit,groundMask)) {
				//Debug.DrawLine(ray,hit);
				Vector3 adjusted = ray.GetPoint( hit.distance - 0.1f ); // ground is at 0.1f
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
	}

	void Shoot (Vector3 dir)
	{
		// Reset the timer.
		timer = 0f;

		// Play the gun shot audioclip.
		gunAudio.Play ();
		//Quaternion targetRotation = Quaternion.LookRotation(dir - transform.position);
		//transform.rotation = targetRotation;

		//gunParticles.transform.LookAt (dir);
		transform.rotation = Quaternion.LookRotation (dir- transform.position);
		if (gunParticles)
			gunParticles.Play ();
		// Enable the line renderer and set it's first position to be the end of the gun.
		gunLine.enabled = true;
		// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
		shootRay.origin = transform.position;
		shootRay.direction = dir - transform.position; //transform.forward;
		gunLine.SetPosition (0, shootRay.origin);
		// Perform the raycast against gameobjects on the shootable layer and if it hits something...
		shootRay.direction = new Vector3 (shootRay.direction.x, 0f, shootRay.direction.z);
		//Debug.Log ("shoot direction " + shootRay );
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			if (shootHit.collider.tag.Equals("Infection")) {
			 //  Debug.Log ("shot enemy " + shootHit.collider.name );
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
			//Debug.Log ("shot nothinng to " + shootRay.origin + shootRay.direction * range );
		}
	}

	// Old!
	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
	{
		//Debug.Log ("start line=" + start + " to " + end);
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.startColor = color;
		lr.endColor=color;
		lr.startWidth=0.1f;
		lr.endWidth= 0.1f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}


}