using UnityEngine;
using System.Collections;

public class PlayerMenu : MonoBehaviour {

  public GameObject spellUI;
  public GameObject spellList;

  public bool showSpell = false;

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
  public void Pause()
  {
    Time.timeScale = 0f;
  }
}
