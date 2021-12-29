using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fallen : MonoBehaviour {

void OnMouseDown ()
	{
		if (this.gameObject.tag == "Fallen")
		{
			this.gameObject.tag = "Enemy";
			Debug.Log("AN ENEMY!");
			return;
		}
		else if (this.gameObject.tag == "Enemy")
		{
			this.gameObject.tag = "Fallen";
			Debug.Log("Where did it go?!");
			return;
		}
	}
	
}