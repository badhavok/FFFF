using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlasmaUI : MonoBehaviour {

	public Text plasmaText;

	// Update is called once per frame
	void Update () {
		plasmaText.text = "$" + PlayerStats.Plasma.ToString();
	}
}
