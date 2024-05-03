using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static bool GameIsOver;
	public bool success;
	public int diamondWin;
	public GameObject gameOverUI;
	public GameObject completeLevelUI;
	void Start ()
	{
		GameIsOver = false;
	}
	// Update is called once per frame
	void Update () {
		if (GameIsOver)
			return;

		if (PlayerStats.Lives <= 0)
		{
			EndGame();
		}
	}
	void EndGame ()
	{
		GameIsOver = true;
		gameOverUI.SetActive(true);
	}
	public void WinLevel ()
	{
//		player.diamondCurrency = 50;
//		player.completedLevels = 2;
//		player.SaveSave();
		GameIsOver = true;
		completeLevelUI.SetActive(true);
	}

}
