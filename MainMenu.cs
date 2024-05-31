using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject optionsMenu;
	public GameObject mainMenu;
	
	public string levelToLoad = "SaveSelect";

	public SceneFader sceneFader;
//	public SaveSerial saveData;

	public void Play ()
	{
//		saveData.LoadGame();
		sceneFader.FadeTo(levelToLoad);
	}
	
	public void OptionsMenu()
	{
		optionsMenu.SetActive(true);
		mainMenu.SetActive(false);
	}

	public void Quit ()
	{
		Application.Quit();
	}
}
