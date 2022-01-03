using UnityEngine;
using System.Collections;

public class PlasmaBuilding : MonoBehaviour {

	public float plasmaBonus = 0;
	public float plasmaCounter = 15;
	public float plasmaCountdown;

	void Start ()
	{
		plasmaCountdown = plasmaCounter;
	}
	void Update ()
	{
		if (plasmaCountdown <= 0)
		{
			PlayerStats.Plasma += plasmaBonus;
			Debug.Log("I've paid " + plasmaBonus);
			plasmaCountdown += plasmaCounter;
		}
		else
		{
			plasmaCountdown -= Time.deltaTime;
		}
	}
}
