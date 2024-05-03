using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This is the class for the player menu; including shop/spells
public class PlayerMenu : MonoBehaviour {

  public GameObject spellUI;
  public GameObject spellList;
  public Button[] spellButtons;
  public bool showSpell = false;
  //Required to detect if the player has built the spell tower and then enables the UI on the screen
  public void Update()
	{
		if(SpellBuilding.SpellLevel > 0)
		{
			spellUI.SetActive(true);
		}
		else
		{
			spellUI.SetActive(false);
		}
	}
  //Function to hide the spell menu once the player is finished with it
  public void HideSpell()
  {
    Debug.Log("clicked");
    if(showSpell)
    {
      spellList.SetActive(true);
      showSpell = false;
    }
    else
    {
      spellList.SetActive(false);
      showSpell = true;
    }
  }
  //Functions used by the player to speed up/slow down the game
  //INFO: Speed = 0 means the game will still 'play' but no towers will build since the in-game time has stopped completely.  This means a player can freely build base towers, but restricts upgrades - I may want/need to change this
  public void SpeedUpTwo ()
  {
    Time.timeScale = 2f;
  }
  public void SpeedUpThree()
  {
    Time.timeScale = 3f;
  }
  public void SpeedUpOne ()
  {
    Time.timeScale = 1f;
  }
  public void SpeedDown ()
  {
    Time.timeScale = 0.5f;
  }
  //Not a literal pause; since the "player pause" will introduce the menu allowing a restart/quit
  public void Pause()
  {
    Time.timeScale = 0.000001f;
  }
}
