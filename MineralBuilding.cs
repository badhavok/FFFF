using UnityEngine;
using System.Collections;

public class MineralBuilding : MonoBehaviour {

	public float mineralBonus = 0;
	public float mineralCounter = 15;
	public float mineralCountdown;

	void Start ()
	{
		mineralCountdown = mineralCounter;
	}
	void Update ()
	{
		if (mineralCountdown <= 0)
		{
			PlayerStats.Money += mineralBonus;
			Debug.Log("I've paid " + mineralBonus);
			mineralCountdown += mineralCounter;
		}
		else
		{
			mineralCountdown -= Time.deltaTime;
		}
	}
}
