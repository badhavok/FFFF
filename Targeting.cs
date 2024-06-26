using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    private string enemyTag = "Enemy";
	private string baseTag = "Lives";
	private string turretTag = "Turret";

	private EnemySpells e;
	private Bullet b;
	private Turret t;

    private Transform target;
	private Enemy targetEnemy;
	private Base targetBase;
	private Turret targetedTurret;

	private float pathCompare, pathRemain;
	void Start()
	{
		e = gameObject.GetComponent<EnemySpells>();
		b = gameObject.GetComponent<Bullet>();
		t = gameObject.GetComponent<Turret>();
	}

    public void TargetEnemy(float range)
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
		}
		else
		{
			target = null;
		}
		GiveBackEnemy(target);
		enemies = null;
	}
	public void FurthestTarget(float range)
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float furthestDistance = Mathf.Infinity;
		GameObject furthestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy <= range)
			{
				furthestDistance = distanceToEnemy;
				furthestEnemy = enemy;
			}
		}

		if (furthestEnemy != null && furthestDistance <= range)
		{
			target = furthestEnemy.transform;
		}
		else
		{
			target = null;
		}
		GiveBackEnemy(target);
		enemies = null;
	}
	public void ClosestToStartTarget(float range)
	{

		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float closestToEnd = Mathf.Infinity;
		GameObject closestToEndEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

			if (distanceToEnemy <= range)
			{
				closestToEnd = distanceToEnemy;
				closestToEndEnemy = enemy;
			}
		}

		if (closestToEndEnemy != null && closestToEnd <= range)
		{
			target = closestToEndEnemy.transform;
		}
		else
		{
			target = null;
		}
		GiveBackEnemy(target);
		enemies = null;
	}
	public void ClosestToEndTarget(float range)
	{
		//Debug.Log("I'm trying to target");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float closestToEnd = Mathf.Infinity;
		GameObject closestToEndEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

			Enemy pathRemaining = enemy.GetComponent<Enemy>();
			pathRemain = pathRemaining.remainingPathDist;

			if (distanceToEnemy <= range)
			{
				if (pathRemain <= pathCompare)
				{
					//Debug.Log("I'm in the path loop.  Remain = " + pathRemain + ".  Compare = " + pathCompare);
					pathCompare = pathRemain;
					closestToEnd = distanceToEnemy;
					closestToEndEnemy = enemy;
				}
			}
		}
		if (closestToEndEnemy != null && closestToEnd <= range)
		{
			//Debug.Log("I'm targetting");
			target = closestToEndEnemy.transform;
			GiveBackEnemy(target);
		}
		else
		{
			//Debug.Log("Clearing out");
			target = null;
			pathCompare = 10000;
			pathRemain = 10000;
		}
		enemies = null;
	}
	public void TargetFriendly(float range)
	{
		Debug.Log("Who to target?");
		if(PlayerStats.Lives < PlayerStats.StartLives)
		{
			Debug.Log("Playerstats > " + PlayerStats.Lives + " : max = " + PlayerStats.StartLives );
			TargetBase(range);
			if(!target)
			{
				TargetTurret(range);
				Debug.Log("No base in range");
			}
		}
		else
		{
			Debug.Log("Base at full health");
			TargetTurret(range);
		}
	}
	public void TargetBase(float range)
	{
		GameObject[] homeBases = GameObject.FindGameObjectsWithTag(baseTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestBase = null;
		foreach (GameObject home in homeBases)
		{
			float distanceToBase = Vector3.Distance(transform.position, home.transform.position);
			if (distanceToBase < shortestDistance)
			{
				shortestDistance = distanceToBase;
				nearestBase = home;
			}
		}

		if (nearestBase != null && shortestDistance <= range)
		{
			target = nearestBase.transform;
		}
		else
		{
			// Debug.Log("Base not detected");
			TargetTurret(range);
		}
		GiveBackBase(target);
		homeBases = null;
	}
	public void TargetTurret(float range)
	{
		GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestTurret = null;
		foreach (GameObject turret in turrets)
		{
			float distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);
			targetedTurret = turret.GetComponent<Turret>();

			if (distanceToTurret < shortestDistance && targetedTurret.healthPoints < targetedTurret.startHealthPoints)
			{
				shortestDistance = distanceToTurret;
				nearestTurret = turret;
			}
			targetedTurret = null;
		}

		if (nearestTurret != null && shortestDistance <= range)
		{
			target = nearestTurret.transform;
		}
		else
		{
			target = null;
		}
		GiveBackTurret(target);
		// Debug.Log("I'm targeting - " + target);
		turrets = null;
	}
	public void TargetAoEEnemy(float range)
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		foreach (GameObject enemy in enemies)
		{
			if (enemy == gameObject) continue;

			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < range)
			{
				Enemy targetEnemy = enemy.GetComponent<Enemy>();
				e.enemyList.Add(targetEnemy);
				// Debug.Log("I'm targeting - " + targetEnemy);
			}
		}
	}
	public void TargetAoETurret(float range)
	{
		GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
		foreach (GameObject turret in turrets)
		{
			if (turret == gameObject) continue;

			float distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);
			if (distanceToTurret < range)
			{
				Turret targetTurret = turret.GetComponent<Turret>();
				e.turretList.Add(targetTurret);
				// Debug.Log("I'm targeting - " + targetTurret);
			}
		}
	}
	void GiveBackEnemy(Transform target)
	{
		if(target != null)
		{
			if(e)
			{
				e.target = target;
				e.targetEnemy = target.GetComponent<Enemy>();
			}
			if(b)
			{
				// b.target = target;
				// b.targetEnemy = target.GetComponent<Enemy>();
			}
			if(t)
			{
				t.target = target;
				t.targetEnemy = target.GetComponent<Enemy>();
			}
		}
		else
		{
			
		}
	}
	void GiveBackBase(Transform target)
	{
		Debug.Log("Giving back the base");
		if(target != null)
		{
			if(e)
			{
				e.target = target;
				// e.targetBase = target.GetComponent<Base>();
			}
			if(b)
			{
				// b.target = target;
				// b.targetBase = target.GetComponent<Base>();
			}
			if(t)
			{
				t.target = target;	
				t.targetBase = target.GetComponent<Base>();
				Debug.Log("Target is > " + target + " & Sending > " + t.targetBase );
			}
		}
		else
		{
			
		}
	}
	void GiveBackTurret(Transform target)
	{
		if(target != null)
		{
			if(e)
			{
				e.target = target;
				e.targetTurret = target.GetComponent<Turret>();
			}
			if(b)
			{
				// b.target = target;
				// b.targetTurret = target.GetComponent<Turret>();
			}
			if(t)
			{
				t.target = target;
				t.targetTurret = target.GetComponent<Turret>();
			}
		}
		else
		{

		}
	}
}