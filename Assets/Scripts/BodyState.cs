using UnityEngine;
using System.Collections;

[System.Serializable]	
public class BodyState: MonoBehaviour
{
	private float redStats_health = 1f;
	private float redStats_speed = 1f;
	private float redStats_defense = 1f;
	private float redStats_reprodRate = 1f;
	private float redStats_power = 1f;
	private float whiteStats_health = 1f;
	private float whiteStats_speed = 1f;
	private float whiteStats_defense = 1f;
	private float whiteStats_reprodRate = 1f;
	private float whiteStats_power = 1f;
	private float pathogenStats_speed = 1f;

	// Update cell stats for the whole body
	public void updateRedHealthStats (float percent)
	{
		redStats_health = percent/100f;
	}
	public void updateRedDefenseStats (float percent)
	{
		redStats_defense = percent/100f;
	}
	public void updateRedPowerStats (float percent)
	{
		redStats_power = percent/100f;
	}
	public void updateRedSpeedStats (float percent)
	{
		redStats_speed = percent/100f;
	}
	public void updateRedReprodStats (float percent)
	{
		if (percent == 0)
			percent = .1f;
		redStats_reprodRate = 100f/percent; // lower is better
	}
	public void updateWhiteHealthStats (float percent)
	{
		whiteStats_health = percent/100f;
	}
	public void updateWhiteDefenseStats (float percent)
	{
		whiteStats_defense = percent/100f;
	}
	public void updateWhitePowerStats (float percent)
	{
		whiteStats_power = percent/100f;
	}
	public void updateWhiteSpeedStats (float percent)
	{
		whiteStats_speed = percent/100f;
	}
	public void updateWhiteReprodStats (float percent)
	{
		if (percent == 0)
			percent = .1f;
		whiteStats_reprodRate = 100f/percent;
	}
	public void updatepathogenSpeedStats (float percent)
	{
		pathogenStats_speed = percent/100f;
	}
	public float redHealth() {
		return redStats_health;
	}
	public float redDefense() {
		return redStats_defense;
	}
	public float redPower() {
		return redStats_power;
	}
	public float redSpeed() {
		return redStats_speed;
	}
	public float redReprodRate() {
		return redStats_reprodRate;
	}
	public float whiteHealth() {
		return whiteStats_health;
	}
	public float whitePower() {
		return whiteStats_power;
	}
	public float whiteSpeed() {
		return whiteStats_speed;
	}
	public float whiteDefense() {
		return whiteStats_defense;
	}
	public float whiteReprodRate() {
		return whiteStats_reprodRate;
	}
	public string showStats(){
		return (name + " white: H:" + whiteHealth () + " D:" + whiteDefense () + " Sp:" + whiteSpeed () +
		"Red: H:" + redHealth () + " D:" + redDefense () + " Sp:" + redSpeed ());
	}
}
	
