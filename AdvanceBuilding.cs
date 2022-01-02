using UnityEngine;
using System.Collections;

public class AdvanceBuilding : MonoBehaviour {

	public static int BuildLevel;
	public int buildingLevel = 0;
	public static bool IsExists = false;

	void Start ()
	{
		BuildLevel = buildingLevel;
		IsExists = true;
	}
}
