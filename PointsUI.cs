using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointsUI : MonoBehaviour {

	public Text pointsText;

	// Update is called once per frame
	void Update () {
		pointsText.text = PlayerStats.Points.ToString() + " points";
	}
}
