using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	public SceneFader fader;
	public int currency;
	public int displayCurrency;
	public int completedLevels;
	public int i;
	public Text displayCurrencyText;
	public Button[] levelButtons;
	//Used in the menu to detect what levels the player has unlocked and allows them to be clickable in the menu
	void Update ()
	{
		int levelReached = PlayerPrefs.GetInt("levelReached", 1);
		// completedLevels = Player.CompletedLevels;

		if(completedLevels > levelReached)
		{
			levelReached = completedLevels;
			i = 0;
			Debug.Log("I'm updating the levels completed! & level reached " + levelReached);
		}

		for (i = 0; i < levelButtons.Length; i++)
		{
			Debug.Log("Inside level loop");
			if (i + 1 > levelReached)
				{
					levelButtons[i].interactable = false;
					Debug.Log("I = " + i);
				}
				else
				{
					levelButtons[i].interactable = true;
					Debug.Log("I = " + i);
				}
		}

//		currency = Player.DiamondCurrency;
//		displayCurrency = currency;
//		displayCurrencyText.text = "D: " + displayCurrency.ToString();
	}
	//Function to load the level that was selected
	public void Select (string levelName)
	{
		fader.FadeTo(levelName);
	}

}
