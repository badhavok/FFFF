using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public string levelToLoad = "SaveSelect";

	public SceneFader sceneFader;
//	public SaveSerial saveData;

	public void Play ()
	{
//		saveData.LoadGame();
		sceneFader.FadeTo(levelToLoad);
	}

	public void Quit ()
	{
		Debug.Log("Exciting...");
		Application.Quit();
	}
}
