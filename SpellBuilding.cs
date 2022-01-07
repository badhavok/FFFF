using UnityEngine;
using System.Collections;

public class SpellBuilding : MonoBehaviour {

	public static int SpellLevel;
	public int spellLevel = 0;
	public static bool SpellExists = false;

	void Update()
	{
		SpellLevel = spellLevel;
		SpellExists = true;
	}
	void OnDestroy()
	{
			Debug.Log("I'm null");
			SpellExists = false;
			SpellLevel = 0;
	}
}
