using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject ui;
	public GameObject options;

	public string menuSceneName = "MainMenu";
	
    public SceneFader sceneFader;
	public Text currentWave;

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
		{
			Toggle();
		}
		currentWave.text = WaveSpawner.CurrentWave.ToString();;
	}

	public void Toggle ()
	{
		ui.SetActive(!ui.activeSelf);

		if (ui.activeSelf)
		{
			Time.timeScale = 0f;
		} else
		{
			Time.timeScale = 1f;
		}
		if(options)
		{
			options.SetActive(false);
		}
	}
	public void OptionsMenu()
	{
		ui.SetActive(!ui.activeSelf);
		options.SetActive(true);
	}
	public void Retry ()
	{
		Toggle();
		sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

	public void Menu ()
	{
		Toggle();
		sceneFader.FadeTo(menuSceneName);
	}
	public void Quit ()
	{
		Application.Quit();
	}

}
