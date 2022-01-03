using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LiquidUI : MonoBehaviour {

	public Text liquidText;

	// Update is called once per frame
	void Update () {
		liquidText.text = "$" + PlayerStats.Liquid.ToString();
	}
}
