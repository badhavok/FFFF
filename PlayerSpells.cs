using UnityEngine;
using System.Collections;

public class PlayerSpells : MonoBehaviour {

	public PlayerMenu playerMenu;
	public Transform spawnPoint;

	public bool castEMP, castBarrage /*a laser shower*/, castMeteor, castFear, castChicken, castGravity, castBasicSummonSpell, castAdvancedSummonSpell = false;
	public float range, counterEMP, stopTime, counterBarrage, counterMeteor, counterFear, counterChicken, counterGravity, counterSummonSpell = 0;
	[HideInInspector] public int castCount, summonAmount;
	[HideInInspector]
	public float dmgMulti, empCount, barrageCount, meteorCount, dotDmg, dotTime, fearCount, fearTime, chickenCount, gravityCount, summonSpellCount, gravityActive = 0;
	[HideInInspector] public string enemyTag = "Enemy";
	[HideInInspector] private Transform target;
	[HideInInspector] private Enemy targetEnemy;
	[HideInInspector] private Vector3 myPos, myPosi;

	public GameObject[] basicSummoningPool;
	public GameObject[] advancedSummoningPool;
	public int basicSummonAmount, advancedSummonAmount;

	void Start ()
	{
		empCount = counterEMP;
		barrageCount = counterBarrage;
		meteorCount = counterMeteor;
		fearCount = counterFear;
		chickenCount = counterChicken;
		gravityCount = counterGravity;
		summonSpellCount = counterSummonSpell;
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	void Update()
	{
			if (Input.GetMouseButtonDown(0))
			{
				//Debug.Log("Spell test");
				if (castEMP)
				{
					Debug.Log("Casting EMP on mousedown");
					CastEMP();
				}
				if (castBarrage)
				{
					CastBarrage();
				}
				if (castMeteor)
				{
					CastMeteor();
				}
				if (castFear)
				{
					CastFear();
				}
				if (castChicken)
				{
					CastChicken();
				}
				if (castGravity)
				{
					CastGravity();
				}
				if (castBasicSummonSpell)
				{
					Debug.Log("I should be spawning - coroutine");
					CastBasicSummonSpell();
				}
				if(castAdvancedSummonSpell)
				{
					CastAdvancedSummonSpell();
				}
			}
	}
	public void SelectCastEMP () //dps with stun
	{
		castEMP = true;
		playerMenu.HideSpell();
	}
	public void SelectCastBarrage() //high dps
	{
		castBarrage = true;
		playerMenu.HideSpell();
	}
	public void SelectCastMeteor() //dps with DoT
	{
		castMeteor = true;
		playerMenu.HideSpell();
	}
	public void SelectCastFear() // runs backwards
	{
		castFear = true;
		playerMenu.HideSpell();
	}
	public void SelectCastChicken() //turns into chickens (no def)
	{
		castChicken = true;
		playerMenu.HideSpell();
	}
	public void SelectCastGravity() //high dps (25% HP) but ~25% chance to happen
	{
		castGravity = true;
		playerMenu.HideSpell();
	}
	public void SelectBasicSummonSpell() //summon basic mobs
	{
		castBasicSummonSpell = true;
		playerMenu.HideSpell();
	}
	public void SelectAdvancedSummonSpell() //summon from a pool of mobs
	{
		castAdvancedSummonSpell = true;
		playerMenu.HideSpell();
	}
	public void CastEMP () //dps with stun - 5% of HP
	{
		dmgMulti = 20f;
		castCount = 1;
		range = 5f;
		stopTime = empCount;
		AoE(range);
		Debug.Log("I'm casting EMP");
		castEMP = false;
	}
	public void CastBarrage() //high dps - .5% x 50
	{
		dmgMulti = 200f;
		castCount = 50;
		range = 10f;
		AoE(range);
		Debug.Log("Casting Barrage");
		castBarrage = false;
	}
	public void CastMeteor() //dps with DoT - 10% + 10s*.75%
	{
		dmgMulti = 100f;
		dotDmg = .45f;
		dotTime = 10f;
		castCount = 5;
		range = 10f;
		AoE(range);
		Debug.Log("Casting Meteor");
		castMeteor = false;
	}
	public void CastFear() // runs backwards - 5% HP
	{
		dmgMulti = 20f;
		castCount = 1;
		fearTime = 10f;
		range = 10f;
		AoE(range);
		Debug.Log("Casting Fear");
		castFear = false;
	}
	public void CastChicken() //turns into chickens (no def)
	{
		dmgMulti = 20f;
		castCount = 1;
		range = 15f;
		AoE(range);
		Debug.Log("Casting Chicken");
		castChicken = false;
	}
	public void CastGravity() //high dps (50% HP) but ~40% chance to happen
	{
		dmgMulti = 2f;
		castCount = 1;
		range = 15f;
		AoE(range);
		Debug.Log("Casting Gravity + active ");
  	castGravity = false;
	}
	public void CastBasicSummonSpell() //summon basic mobs
	{
		summonAmount = 10;
		StartCoroutine(BasicSummonSpell());
		Debug.Log("I'm supposed to spawn now Basic");
		castBasicSummonSpell = false;
	}
	public void CastAdvancedSummonSpell() //summon from a pool of mobs
	{
		summonAmount = 5;
		StartCoroutine(AdvancedSummonSpell());
		Debug.Log("I'm supposed to spawn now Advanced");
		castAdvancedSummonSpell = false;
	}
	void AoE (float AoERange)
	{
		Debug.Log("AoE Cast test");
		range = AoERange;

		if (Input.touchCount > 0)
		 {
			Debug.Log("input test");
			Touch u = Input.GetTouch(0);
			{
				 if(u.phase == TouchPhase.Began)
				 {
					Debug.Log("Touch phase... ");

					RaycastHit touchHit;
					Ray ray = Camera.main.ScreenPointToRay(u.position);
					if (Physics.Raycast(ray, out touchHit, 100))
					{
							myPos = touchHit.point;
					}
				 	Debug.Log("Hallo... anyone there, touch? + pos = " + myPos + " + range = " + range);
					Collider[] colliders = Physics.OverlapSphere(myPos, range);
					foreach (Collider collider in colliders)
					{
						//Debug.Log("Found enemy touch");
						if (collider.tag == "Enemy")
						{
							AoEDamage(collider.transform);
						}
					}
				}
			}
		}

		else
		{
			RaycastHit mouseHit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out mouseHit, 100))
			{
					myPosi = mouseHit.point;
			}
			//Debug.Log("Hallo... anyone there, mouse? + pos = " + myPosi + " + range = " + range);
			Collider[] colliders = Physics.OverlapSphere(myPosi, range);
			foreach (Collider collider in colliders)
			{
				//Debug.Log("Found enemy mouse");
				if (collider.tag == "Enemy")
					{
						AoEDamage(collider.transform);
					}
			}
		}
		//Vector3 dir = firePoint.position - target.position;
	}
	void AoEDamage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();

		if(castGravity)
		{
			var rand = Random.Range(1, 100);
			//Debug.Log("Gravity on!");
			if (rand <= 40)
			{
			e.GravityDMG(2);
			//Debug.Log("I'm hit");
			}
			return;
		}
		//Debug.Log("I'm in AoEDamage");
		if(castEMP)
		{
			e.Stop(stopTime);
		}
		if(castMeteor)
		{
			e.Poison(dotDmg, dotTime);
		}
		if(castFear)
		{
			e.Fear(fearTime);
		}
		if(castChicken)
		{
			e.Chicken();
		}

		for (int i = 0; i < castCount; i++)
		{
			Debug.Log("I have been hit + " + i + " many times...");
			e.GravityDMG(dmgMulti);
		}
	}
	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
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
	public IEnumerator BasicSummonSpell()
	{
		//Debug.Log("I'm in the summonloop with index: " + summoningIndex);
		for (int i = 0; i < basicSummoningPool.Length; i++)
			{
				//Debug.Log("Summon amount = " + basicSummoningPool.Length + "...");
				for (int j = 0; j < summonAmount; j++)
				{
					Debug.Log("I'm trying to spawn a " + basicSummoningPool[i] + " ...");
					GameObject enemy = Instantiate(basicSummoningPool[i], spawnPoint.position, spawnPoint.rotation);
					++WaveSpawner.EnemiesAlive;
					yield return new WaitForSeconds(1.0f);
				}
			}
	}
	public IEnumerator AdvancedSummonSpell()
	{
		//Debug.Log("I'm in the summonloop with index: " + summoningIndex);
		for (int i = 0; i < advancedSummoningPool.Length; i++)
			{
				//Debug.Log("Summon amount = " + basicSummoningPool.Length + "...");
				for (int j = 0; j < summonAmount; j++)
				{
					Debug.Log("I'm trying to spawn a " + advancedSummoningPool[i] + " ...");
					GameObject enemy = Instantiate(advancedSummoningPool[i], spawnPoint.position, spawnPoint.rotation);
					++WaveSpawner.EnemiesAlive;
					yield return new WaitForSeconds(1.0f);
				}
			}
	}
}
