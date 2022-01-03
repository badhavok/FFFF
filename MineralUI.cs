using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MineralUI : MonoBehaviour {

	public Text mineralText;

	// Update is called once per frame
	void Update () {
		mineralText.text = "$" + PlayerStats.Mineral.ToString();
	}
}
