using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public static float Money;
	public float startMoney = 300;
	public static int Lives;
	public int startLives = 20;
	public static float Points;
	public float startPoints;

	public static int Rounds;

	void Awake ()
	{
		Money = startMoney;
		Lives = startLives;
		Points = startPoints;

		Rounds = 0;
	}

}
