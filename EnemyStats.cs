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
	public static float Points;
	public float startPoints = 100;

	public int startBluntDef = 0;
	public int startSlashDef = 0;
	public int startPierceDef = 0;
	public int startMagDef = 0;

	public int startFireDef = 0; // Weak to Water, strong against Ice
	public int startIceDef = 0; // Weak to Fire, strong against Wind
	public int startWaterDef = 0; // Weak to Lightning, strong against Fire
	public int startLighteningDef = 0; // Weak to Earth, strong against Water
	public int startEarthDef = 0; // Weak to Wind, strong against Lightning
	public int startWindDef = 0 ; // Weak to Ice, strong against Earth
	public int startLightDef = 0; // Opposite of Dark
	public int startDarkDef = 0; // Opposite of Light
	public bool fireEnemy, iceEnemy, waterEnemy, lighteningEnemy, earthEnemy, windEnemy, lightEnemy, darkEnemy;
	public int fireResist = 0; // Weak to Water, strong against Ice
	public int iceResist = 0; // Weak to Fire, strong against Wind
	public int waterResist = 0; // Weak to Lightning, strong against Fire
	public int lighteningResist = 0; // Weak to Earth, strong against Water
	public int earthResist = 0; // Weak to Wind, strong against Lightning
	public int windResist = 0 ; // Weak to Ice, strong against Earth
	public int lightResist = 0; // Opposite of Dark
	public int darkResist = 0; // Opposite of Light

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
		Points = startPoints;
		hoverCount = hoverTimeBoost;
		dropCount = dropTime;
	}

}
