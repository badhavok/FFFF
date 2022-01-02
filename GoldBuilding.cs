using UnityEngine;
using System.Collections;

public class GoldBuilding : MonoBehaviour {

	public float goldBonus = 0;
	public float goldCounter = 15;
	public float goldCountdown;

	void Start ()
	{
		goldCountdown = goldCounter;
	}
	void Update ()
	{
		if (goldCountdown <= 0)
		{
			PlayerStats.Money += goldBonus;
			Debug.Log("I've paid " + goldBonus);
			goldCountdown += goldCounter;
		}
		else
		{
			goldCountdown -= Time.deltaTime;
		}
	}
}
