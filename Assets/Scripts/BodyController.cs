using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* This allows Unity UI to see class and show it
 * */
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}
// Base stats for cells,
[System.Serializable]
public class Stats
{
	// base level multiplied by ( 0 - 100% of base level )
	public float health, speed, defense, reprodRate;
	// regen==reprod)
	public float power;
	// attack if white cell or infection, oxygenation if red cell
	public float delay;
	// Pause before it can follow a command
	public int level;
}
	
public class BodyController : MonoBehaviour
{
	public string myname;
	public GameController gameController;
	public BodyState bodystate; // Hold the global state of all the cells
	public Stats bodyStats;

	protected Hashtable inContact = new Hashtable ();
	protected float stats_health = 100.0f;
	protected float stats_speed = 100.0f;
	protected float stats_defense = 100.0f;
	protected float stats_reprodRate = 100.0f;
	protected float stats_power = 100.0f;
	protected float stats_delay = 0f;
	protected int stats_level = 1;
	public Slider healthSlider;
	public Slider powerSlider;
	public Slider defenseSlider;
	protected float healthColorTime;
	protected Renderer myRenderer;
	protected Material myMaterial;

	public void awake() {
		healthColorTime = Time.time;
		myRenderer = this.GetComponent< Renderer >();
		if ( myRenderer==null)
			myRenderer = this.GetComponentInChildren< Renderer >();
		if (gameController==null)
			gameController = GameObject.FindObjectOfType (typeof(GameController)) as GameController;
		if (bodystate == null)
			bodystate = GameObject.FindObjectOfType (typeof(BodyState)) as BodyState;
	}

	public void setBodyState(BodyState bs) {
		if (bs)
			bodystate=bs;
	}

	public virtual float health () {
		return ((stats_health/100.0f) * bodyStats.health);
	}

	public virtual float defense () {
		return ((stats_defense/100.0f) * bodyStats.defense);
	}

	public virtual float power () {
		return ((stats_power / 100.0f) * bodyStats.power);
	}
	// Lower number is faster - reproduce every N seconds
	public virtual float reprodRate () {
		if (bodyStats.reprodRate < 2) {
			Debug.LogError (name +" !!!ReprodRate (<2) is messed up " + bodyStats.reprodRate);
			bodyStats.reprodRate = 2;
		}
		return (bodyStats.reprodRate);
	}
		
	// Drift speed
	public virtual float speed () { 
		return (stats_speed/100f) * bodyStats.speed;
	}

	public string showStats(){
		return name+" H:"+ health()+" D:"+ defense();
	}

	public float get_stats_power(){ return stats_power;
	}
	public float get_stats_health(){ return stats_health;
	}
	public float get_stats_defense(){ return stats_defense;
	}
	public float get_bodystats_reprod(){ return bodyStats.reprodRate;
	}
	// Default - will be overridden by subclass
	public virtual void deathHandler (){
		//DestroyObject (gameObject);
	}
	public virtual void movehealthSlider (){
		if (healthSlider != null) {
			//Debug.Log(name+"slider ="+ stats_health);
			healthSlider.value = stats_health;
		}
	}
	public virtual void updateHealthStats(float point) {
		stats_health += point;
		if (stats_health > 100f) // health goes up by oxygen power
			stats_health = 100f;	
		
		if (stats_health <= 0) {
			stats_health = 0;
			deathHandler (); // each subclass does something different
		}
		else
			instantMuteColors(stats_health / 100f);
		
		movehealthSlider ();
		/*
		float freq = 2f;
		if (10 < stats_health && stats_health < 95 && healthColorTime < Time.time) {
			//Debug.Log ("mute health = "+ stats_health);
			muteColors (stats_health / 100f, freq);
			healthColorTime = Time.time + freq; // Do this once every freq seconds
		}
*/

	}
	public virtual void updatePowerStats(float point) {
		stats_power += point;
		if (stats_power > 100f) // health goes up by oxygen power
			stats_power = 100f;
		if (stats_power <= 0) {stats_power = 0;}

	}
	public virtual void updateDefenseStats(float point) {
		stats_defense += point;
		if (stats_defense > 100f) // health goes up by oxygen power
			stats_defense = 100f;
		if (stats_defense <= 0) {stats_defense = 0; }
	}

	// lerp doesn't seem to look better
	// From 0 to 1 : 1 is healthy, 0 is dead
	// Healthy = shiny , metallic and saturated colors, Sick = dull 
	public  void muteColors(float intensity, float freq) {
		//Debug.Log (name + " muteColors " + intensity);
		if (!myRenderer) {
			myRenderer = this.GetComponentInChildren< MeshRenderer > ();
			Debug.Log (name + " has no renderer ");
		}
		Material mat = myRenderer.material;
		Color color = mat.color;
		Color faded = mat.color * intensity ; // new Color (color.r, color.g, color.b, intensity);
		Debug.Log (name + " changes from "+ color + " to "+ faded);
		//material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
		//MaterialProperty mainTex = FindProperty("_MainTex");
		StartCoroutine(UpdateMaterial(mat,intensity,color,faded,5f,freq));
	}

	protected IEnumerator UpdateMaterial(Material material, float intensity, 
		Color color, Color faded,
		float steps, float duration)
	{
		float progress = 0; 
		float increment = steps/duration; //The amount of change to apply.
		while (progress < 1) {
			float smoothness, shininess;
			smoothness = intensity * .5f;
			shininess = intensity * .5f;
			material.shader = Shader.Find("Specular");

			material.SetFloat("_Smoothness", smoothness);
			material.SetFloat("_Shininess", shininess);
		// material.SetColor("_EmissionColor", color);
			material.SetFloat("_Glossiness", shininess);

			DynamicGI.SetEmissive (myRenderer, Color.Lerp(color, faded, progress));
			DynamicGI.UpdateMaterials(myRenderer);
			DynamicGI.UpdateEnvironment();
			progress += increment;
			yield return new WaitForSeconds(steps);
		}
	}


	public void instantMuteColors(float intensity) {
		if (intensity > .95)
			return;
		if (!myRenderer) {
			myRenderer = this.GetComponentInChildren< MeshRenderer > ();
			//Debug.LogError (name + " has no renderer ");
		}
		if (!myMaterial) {
			myMaterial = myRenderer.material;
			myMaterial.shader = Shader.Find ("Specular");
		}
		float shininess;
		//	shininess= material.GetFloat("_Smoothness") * .5f;	
		shininess= intensity * 0.3f;
	
		Color faded = myMaterial.color * intensity;
		//Debug.Log (name + " changes from "+ myMaterial.color + " to "+ faded);
		myMaterial.SetFloat("_Shininess", shininess);
		myMaterial.SetFloat("_Glossiness", shininess);
		myMaterial.SetFloat("_Smoothness", shininess);
		DynamicGI.SetEmissive (myRenderer, faded);
		DynamicGI.UpdateMaterials(myRenderer);
		DynamicGI.UpdateEnvironment();
	}
}

