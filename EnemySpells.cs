using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemySpells : MonoBehaviour {

	private Enemy enemy;

	private Transform target;
	private Enemy targetEnemy;
	public string enemyTag = "Enemy";

	public float range = 0;

	public bool castSelf = false;

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
	public float summonAmount = 0;
	public float countdownSummon = 0;
[HideInInspector] public float summonCount = 0;

	public bool vampire = false;
	public float countdownVampire = 0;
	public float vampireDamage = 0;
	public float vampireCount = 0;
	public LineRenderer lineRenderer;
	public ParticleSystem impactEffect;
	public Light impactLight;
	public Transform firePoint;

	void Start()
	{
		enemy = GetComponent<Enemy>();
		hideCount = countdownHide;
		healCount = countdownHealth;
		speedCount = countdownSpeed;
		summonCount = countdownSummon;
		levitateCount = countdownLevitate;
		immuneCount = countdownImmune;
		vampireCount = countdownVampire;
	}

	void Update()
	{
		if (enemy.silence)
		{
			Debug.Log("I can't cast!");
		}
		else
		{
			if(vampire)
			{
				BuffVampire();
			}
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
				BuffHide();
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
	}
	void BuffVampire()
	{
		if (vampireCount <= 0)
		{
			lineRenderer.enabled = false;
			impactEffect.Stop();
			impactLight.enabled = false;
			vampireCount += (countdownVampire * 2);
		}
		else if (vampireCount <= (countdownVampire / 2))
		{
			vampireCount -= Time.deltaTime;
			Debug.Log("I vant to suck your blood!");
			TargetEnemy();
			targetEnemy.Vampire(vampireDamage);
			enemy.Healing(vampireDamage);

			lineRenderer.SetPosition(0, firePoint.position);
			lineRenderer.SetPosition(1, target.position);

			if(targetEnemy != null)
			{
				lineRenderer.enabled = true;
				impactEffect.Play();
				impactLight.enabled = true;
			}
			else
			{
				lineRenderer.enabled = false;
				impactEffect.Stop();
				impactLight.enabled = false;
				Debug.Log("Not attacking");
				vampireCount -= Time.deltaTime;
			}

			Vector3 dir = firePoint.position - target.position;

			impactEffect.transform.position = target.position + dir.normalized;

			impactEffect.transform.rotation = Quaternion.LookRotation(dir);
		}
		else
		{
			lineRenderer.enabled = false;
			impactEffect.Stop();
			impactLight.enabled = false;
			Debug.Log("Oh noes, no more blood!");
			vampireCount -= Time.deltaTime;
		}
	}
	void BuffImmune()
	{
		if (immuneCount <= 0)
		{
			if(castSelf)
			{
			enemy.Immune(countdownImmune);
			}
			else
			{
			TargetEnemy();
			targetEnemy.Immune(countdownImmune);
			}
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
			if(castSelf)
			{
				enemy.Healing(bonusHealth);
			}
			else
			{
				TargetEnemy();
				targetEnemy.Healing(bonusHealth);
				Debug.Log("Oh wow, look at me... I'm healing you!");
			}
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
			if(castSelf)
			{
				enemy.Flying(countdownLevitate);
				Debug.Log("I'm flying!!");
			}
			else
			{
				TargetEnemy();
				targetEnemy.Flying(countdownLevitate);
				Debug.Log("Woah, what's going on?");
			}
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
			//Debug.Log("I summon youuu..");
			summonCount += (countdownSummon * 2);
		}
		else if (summonCount <= (countdownSummon / 2))
		{
			summonCount -= Time.deltaTime;
			//Debug.Log("What... I need to wait?");
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
			if(castSelf)
			{
				enemy.Speed(bonusSpeed, countdownSpeed);
			}
			else
			{
				TargetEnemy();
				targetEnemy.Speed(bonusSpeed, countdownSpeed);
			}
			//Debug.Log("Weeeeee.... don't say bye then!");
			speedCount += (countdownSpeed * 2);
		}
		else
		{
			speedCount -= Time.deltaTime;
		}
	}

	void BuffHide()
	{
		if (hideCount < 0)
		{
			enemy.gameObject.tag = "Fallen";
			//Debug.Log("Shhh nothing to see hear");
			//[change opaqueness]
			hideCount += (countdownHide * 2);
		}
		else if (hideCount <= (countdownHide / 2))
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
