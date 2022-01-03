using UnityEngine;
using System.Collections;

public class LiquidBuilding : MonoBehaviour {

	public float liquidBonus = 0;
	public float liquidCounter = 15;
	public float liquidCountdown;

	void Start ()
	{
		liquidCountdown = liquidCounter;
	}
	void Update ()
	{
		if (liquidCountdown <= 0)
		{
			PlayerStats.Liquid += liquidBonus;
			Debug.Log("I've paid " + liquidBonus);
			liquidCountdown += liquidCounter;
		}
		else
		{
			liquidCountdown -= Time.deltaTime;
		}
	}
}
