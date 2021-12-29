using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour {

	public Text livesText;
	public Text wavesText;

	// Update is called once per frame
	void Update () {

		livesText.text = PlayerStats.Lives.ToString() + " LIVES";
	}
}
