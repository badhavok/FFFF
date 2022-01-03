using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public static float Money, Gas, Liquid, Plasma, Mineral;
	public float startMoney = 400;
	public float startGas = 0;
	public float startLiquid = 0;
	public float startPlasma = 0;
	public float startMineral = 0;

	public static int Lives;
	public int startLives = 20;

	public static int Rounds;

	void Start ()
	{
		Money = startMoney;
		Lives = startLives;
		Gas = startGas;
		Liquid = startLiquid;
		Plasma = startPlasma;
		Mineral = startMineral;

		Rounds = 0;
	}

}
