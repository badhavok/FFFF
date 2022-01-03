using UnityEngine;
using System.Collections;

public class GasBuilding : MonoBehaviour {

	public float gasBonus = 0;
	public float gasCounter = 15;
	public float gasCountdown;

	void Start ()
	{
		gasCountdown = gasCounter;
	}
	void Update ()
	{
		if (gasCountdown <= 0)
		{
			PlayerStats.Gas += gasBonus;
			Debug.Log("I've paid " + gasBonus);
			gasCountdown += gasCounter;
		}
		else
		{
			gasCountdown -= Time.deltaTime;
		}
	}
}
