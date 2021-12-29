using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemySpells : MonoBehaviour {

	private Enemy enemy;

	private Transform target;
	private Enemy targetEnemy;
	public string enemyTag = "Enemy";
	
	public float range = 0;
	
	public bool buffHide = false;
	public float countdownHide = 0;
[HideInInspector] public float hideCount = 0;

	public bool buffHealing = false;
	public float bonusHealth = 0;
	public float countdownHealth = 0;
[HideInInspector] public float healCount = 0;
	
	public bool buffImmune = false;
	public float countdownImmune = 0;
[HideInInspector] public float immuneCount = 0;
	
	public bool buffSpeed = false;
	public float bonusSpeed = 0;
	public float countdownSpeed = 0;
[HideInInspector] public float speedCount = 0;

	public bool duplicate = false;
	
	public bool buffLevitate = false;
	public float countdownLevitate = 0;
[HideInInspector] public float levitateCount = 0;
	
	public bool buffSummoner = false;
	public GameObject[] summoningPool;
	public float countdownSummon = 0;
[HideInInspector] public float summonCount = 0;
	
	public bool vampire = false;

	void Start()
	{
		enemy = GetComponent<Enemy>();
		hideCount = countdownHide;
		healCount = countdownHealth;
		speedCount = countdownSpeed;
		summonCount = countdownSummon;
		levitateCount = countdownLevitate;
		immuneCount = countdownImmune;
	}

	void Update()
	{
		if (buffImmune)
		{
			BuffImmune();
		}
		if (buffHealing)
		{
			BuffHealing();
		}
		if (buffHide)
		{
			HideMe();
		}
		if (buffSpeed)
		{
			BuffSpeed();
		}
		if (buffSummoner)
		{
			BuffSummoner();
		}
		if (buffLevitate)
		{
			BuffLevitate();
		}
	}
	void BuffImmune()
	{
		if (immuneCount <= 0)
		{
			TargetEnemy();
			targetEnemy.Immune(countdownImmune);
			//Debug.Log("Tis but a scratch!");
			immuneCount += (countdownImmune * 2);
		}
		else
		{
			immuneCount -= Time.deltaTime;
		}
	}
	void BuffHealing()
	{
		if (healCount <= 0)
		{
			TargetEnemy();
			targetEnemy.Healing(bonusHealth);
			Debug.Log("Oh wow, look at me... I'm healing you!");
			healCount += (countdownHealth * 2);
		}
		else
		{
			healCount -= Time.deltaTime;
		}
	}
	void BuffLevitate()
	{
		if (levitateCount <= 0)
		{
			TargetEnemy();
			targetEnemy.Flying(countdownLevitate);
			Debug.Log("Woah, what's going on?");
			//[change opaqueness]
			levitateCount += (countdownLevitate * 2);
		}
		else
		{
			levitateCount -= Time.deltaTime;
		}
	}
	void BuffSummoner()
	{
		if (summonCount < 0)
		{
			StartCoroutine(enemy.SummonNow());
			Debug.Log("I summon youuu..");
			summonCount += (countdownSummon * 2);
		}
		else if (summonCount <= (countdownSummon / 2))
		{
			summonCount -= Time.deltaTime;
			Debug.Log("What... I need to wait?");
		}
		else
		{
			summonCount -= Time.deltaTime;
		}
	}
	
	void BuffSpeed()
	{
		if (speedCount < 0)
		{
			TargetEnemy();
			targetEnemy.Speed(bonusSpeed, countdownSpeed);
			//Debug.Log("Weeeeee.... don't say bye then!");
			speedCount += (countdownSpeed * 2);
		}
		else
		{
			speedCount -= Time.deltaTime;
		}
	}
	
	void HideMe()
	{
		if (hideCount < 0)
		{
			enemy.gameObject.tag = "Fallen";
			//Debug.Log("Shhh nothing to see hear");
			//[change opaqueness]
			hideCount += (countdownHide * 2);
		}
		else if (hideCount <= 5)
		{
			enemy.gameObject.tag = "Enemy";
			hideCount -= Time.deltaTime;
			//Debug.Log("Here we go again");
		}
		else
		{
			hideCount -= Time.deltaTime;
		}
	}
	
	void TargetEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			if (enemy == gameObject) continue;
			
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy.GetComponent<Enemy>();
		} else
		{
			target = null;
		}
	}
	
}