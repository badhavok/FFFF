using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GasUI : MonoBehaviour {

	public Text gasText;

	// Update is called once per frame
	void Update () {
		gasText.text = "$" + PlayerStats.Gas.ToString();
	}
}
