using UnityEngine;
using System.Collections;

//This is used to seperate some of the more specific stats used, to reduce the size of the 'main' enemy class
//Might need to move more stats over to this script, or just add these back to the normal script
[RequireComponent(typeof(Enemy))]
public class EnemyStats : MonoBehaviour {

	private Enemy enemy;

	//public static float Health;
	public float startHealth = 10;

	public static float Worth;
	public float startWorth = 10;

	public int startPhysDef = 0;
	public int startMagDef = 0;

	public int fireDef = 0; // DoT
	public int iceDef = 0; // Slow
	public int lighteningDef = 0; // Stun
	public int earthDef = 0; // Debuffs

	public bool isHovercraft = false;
	public float hoverTimeBoost = 0;
[HideInInspector] public float hoverCount = 0;

	public bool isDropship = false;
	public int dropAmount = 0;
	public float dropTime, dropSpeed = 0;
[HideInInspector] public float dropCount = 0;
	public GameObject dropEnemy;

	public bool undeadEnemy = false;
	public bool chickenEnemy = false;

	void Start()
	{
		enemy = GetComponent<Enemy>();
		Worth = startWorth;
		hoverCount = hoverTimeBoost;
		dropCount = dropTime;
	}

}
