using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public static float Money, Gas, Liquid, Plasma, Mineral;
	public float startMoney = 400;

	public static int Lives;
	public int startLives = 20;

	public static int Rounds;

	void Start ()
	{
		Money = startMoney;
		Lives = startLives;

		Rounds = 0;
	}

}
