using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class is used for enemies that have spells, only needs to be assigned if the enemy needs it
[RequireComponent(typeof(Enemy))]
public class EnemySpells : MonoBehaviour {

	private Enemy enemy;
	private Turret tower;

	private Transform target;
	private Enemy targetEnemy;
	private Turret targetTower;
	public float range = 0;
	public string enemyTag = "Enemy";
	public string towerTag = "Tower";

	//List<EnemySpells> floatList = new List<EnemySpells>();
	public string[] customSpellList;
	private string castingThisSpell;
	public bool customSpell, multiSpell;
	public float customCastCountdown;
	public float customCastTime;
	private int spellToCast;
	public bool casting;
	public float isCasting = 0;

	//Is the enemy casting on itself
	[Header("Self casting")]
	public bool castSelf = false;
	[Header("AoE casting")]
	public bool aoECast = false;
	[Header("Spell list")]

	//This makes the enemy invisible to detection
	public bool buffHide = false;
	public float countdownHide = 0;
[HideInInspector] public float hideCount = 0;

	//This heals an enemy
	public bool buffHealing = false;
	public float bonusHealth = 0;
	public float countdownHealth = 0;
	private float healCount = 0;

	//This makes an enemy immune to damage/debuffs - but the towers will still shoot at it
	public bool buffImmune = false;
	public float countdownImmune = 0;
[HideInInspector]	public float immuneCount = 0;

	//This makes the enemy move faster
	public bool buffSpeed = false;
	public float bonusSpeed, countdownSpeed, speedCount = 0;
	public bool buffDefences = false;
	public int buffDefSlash, buffDefBlunt, buffDefPierce, buffDefMag;
[HideInInspector] public float buffDefCount;
	public float countdownBuffDef = 0;
	
	public bool debuffTowerSpeed = false;
	public float debuffSpeed, countdownDebuffSpeed, debuffSpeedCount = 0;
	public bool attackTowerHP = false;
	public int attackTowerDmg = 0;
	public float countdownAttackTower, attackTowerHPCount = 0;
	//This makes an enemy duplicate itself (weaker than summon as it detects current HP as well)
	public bool duplicate = false;

	//This makes the enemy immune to ground damage - only "air" towers can target it
	public bool buffLevitate = false;
	public float countdownLevitate = 0;
[HideInInspector] public float levitateCount = 0;

	//This allows the enemy to summon from a list you choose from`
	public bool buffSummoner = false;
	public GameObject[] summoningPool;
	public float summonAmount = 0;
	public float countdownSummon = 0;
	public float summonCount = 0;

	//This drains HP from an enemy within range
	public bool vampire = false;
	public float countdownVampire = 0;
	public float vampireDamage = 0;
	public float vampireCount = 0;
	public LineRenderer lineRenderer;
	public ParticleSystem impactEffect;
	public Light impactLight;
	public Transform firePoint;
	private Animator anim;
	AnimatorClipInfo[] m_CurrentClipInfo;
    float m_CurrentClipLength;
	public ParticleSystem[] castList;
	//public bool includeChildren = true;

	void Start()
	{
		enemy = GetComponent<Enemy>();
		anim = gameObject.GetComponentInChildren<Animator>();
		//casting = gameObject.GetComponentsInChildren<ParticleSystem>();
		//Sets the variables used by the counters
		hideCount = countdownHide;
		healCount = countdownHealth;
		speedCount = countdownSpeed;
		summonCount = countdownSummon;
		levitateCount = countdownLevitate;
		immuneCount = countdownImmune;
		vampireCount = countdownVampire;
		buffDefCount = countdownBuffDef;
		customCastTime = customCastCountdown;
		attackTowerHPCount = countdownAttackTower;
	}
	void Update()
	{
		if(isCasting > 0)
		{
			//Debug.Log("I think I'm stuck here");
			isCasting -= Time.deltaTime;
		}
		else if (isCasting == 0)
		{
		
		}
		else
		{
			//Debug.Log("No, I'm actually stuck here");
			anim.SetBool("Cast", false);
			anim.SetBool("Move", true);
			isCasting = 0;
		}
		//Detects if the enemy can cast and avoids the loop if it can't
		if (enemy.silence)
		{
			//Debug.Log("I can't cast!");
		}
		else
		{
			if(casting)
			{
				anim.SetBool("Cast", true);
				anim.SetBool("Move", false);
				m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
				m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
				//Debug.Log("The clip length is - " + m_CurrentClipLength + " .");
				isCasting = m_CurrentClipLength * 3;
				enemy.Casting(m_CurrentClipLength * 3);
				casting = false;
				
				foreach (ParticleSystem casting in castList)
				{
				if(!casting.isPlaying)
						{
						casting.Play();
						}
				}
			}
			
			if(vampire)
			{
				BuffVampire();
			}
			if (buffImmune)
			{
				BuffImmune();
			}
			if (buffDefences)
			{
				BuffDefences();
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
			if (debuffTowerSpeed)
			{
				DebuffTowerSpeed();
			}
			if (attackTowerHP)
			{
				AttackTowerHP();
			}
			if(customSpell)
			{
				
				if(customCastTime < 0)
				{
					if(multiSpell)
					{
						MultiSpell();
					}
					else
					{
						RandomSpell();
					}
				}
				else if (customCastTime >= 0 & customCastTime < customCastCountdown -1)
				{
					castingThisSpell = "empty";
					CastingSpell(castingThisSpell);
					customCastTime -= Time.deltaTime;
				}
				else
				{
					customCastTime -= Time.deltaTime;
				}
			}
		}
	}
	//Setting the variable names in the inspector will give the mob a list of spells which are chosen at random
	void RandomSpell()
	{
			spellToCast = Random.Range(0, customSpellList.Length);
			castingThisSpell = customSpellList[spellToCast];
			CastingSpell(castingThisSpell);
			//Debug.Log("Spell number " + spellToCast + " is the spell " + castingThisSpell + " ... or " + customSpellList[spellToCast] + " ...");
			customCastTime = customCastCountdown;
	}
	void MultiSpell()
	{
		for(int i = 0; i < customSpellList.Length; i++)
		
			{
				castingThisSpell = customSpellList[i];
				Debug.Log("Spell number " + i + " is the spell " + castingThisSpell + " ... or " + customSpellList[i] + " ...");
				CastingSpell(castingThisSpell);
			}
		
		customCastTime = customCastCountdown;
	}
	void CastingSpell(string castingThisSpell)
	{
		switch(castingThisSpell)
				{
					default:
						buffSpeed = false;
						buffDefences = false;
						buffHealing = false;
						buffSummoner = false;
						buffImmune = false;
						debuffTowerSpeed = false;
						attackTowerHP = false;

						hideCount = countdownHide;
						countdownHealth =0;
						healCount = countdownHealth;
						countdownSpeed = 0;
						speedCount = countdownSpeed;
						countdownSummon =0;
						summonCount = countdownSummon;
						levitateCount = countdownLevitate;
						countdownImmune =0;
						immuneCount = countdownImmune;
						vampireCount = countdownVampire;
						countdownBuffDef = 0;
						buffDefCount = countdownBuffDef;
						countdownDebuffSpeed = 0;
						debuffSpeedCount = countdownDebuffSpeed;
						countdownAttackTower = 0;
						attackTowerHPCount = countdownAttackTower;
						
						break;

					case "buffDefences" : 
						
						buffDefences = true;
						countdownBuffDef = 5;

						break;

					case "buffHealing" :

						buffHealing = true;
						countdownHealth = 5;

						break; 

					case "buffSpeed" :

						buffSpeed = true;
						countdownSpeed = 5;

						break; 
					case "buffSummoner" :

						buffSummoner = true;
						countdownSummon = 5;

						break;
					case "buffImmune" :

						buffImmune = true;
						countdownImmune = 5;

						break;
					case "debuffTowerSpeed" :

						debuffTowerSpeed = true;
						countdownDebuffSpeed = 5;

						break;
					case "attackTowerHP" :

						attackTowerHP = true;
						countdownAttackTower =5;

						break;
				}
	}
	//Steals HP from nearby enemies and adds it to [own] HP
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
	//Boosts speed of target(s)
	void BuffSpeed()
	{
		//Debug.Log("I'm in the speed void");
		if (speedCount < 0)
		{
			if(castSelf)
			{
				casting = true;
				//Debug.Log("I'm casting now");
				enemy.Speed(bonusSpeed, countdownSpeed);
				//casting = true;
			}
			else if (aoECast)
			{
				AoE(0);
			}
			else
			{
				//Debug.Log("Targeting");
				TargetEnemy();
				if(targetEnemy)
				{
					// Debug.Log("Enemy targeted - " + targetEnemy + " .");
					targetEnemy.Speed(bonusSpeed, countdownSpeed);
				}
			}
			//Debug.Log("Weeeeee.... don't say bye then!");
			speedCount += (countdownSpeed);
		}
		else
		{
			speedCount -= Time.deltaTime;
		}
	}
	//Heal target(s)
	void BuffHealing()
	{
		// Debug.Log("I'm in the healing void");
		if (healCount <= 0)
		{
			if(castSelf)
			{
				casting = true;
				enemy.Healing(bonusHealth);
				// Debug.Log("Oh wow, look at me... I'm healing me!");
			}
			else if (aoECast)
			{
				AoE(1);
			}
			else
			{
				TargetEnemy();
				if (targetEnemy)
				{
				targetEnemy.Healing(bonusHealth);
				// Debug.Log("Oh wow, look at me... I'm healing you!");
				}
			}
			healCount += (countdownHealth);
			//Debug.Log("Heal count is above 0");
		}
		else
		{
			//Debug.Log("I'm counting down");
			healCount -= Time.deltaTime;
		}
	}
	//Buff target(s)
	void BuffDefences()
	{
		// Debug.Log("I'm in the defences void");
		if (buffDefCount <= 0)
		{
			if(castSelf)
			{
				casting = true;
				enemy.BuffSlashDef(buffDefSlash, countdownBuffDef);
				enemy.BuffBluntDef(buffDefBlunt, countdownBuffDef);
				enemy.BuffPierceDef(buffDefPierce, countdownBuffDef);
				enemy.BuffMagDef(buffDefMag, countdownBuffDef);
				// Debug.Log("Oh wow, look at me... I'm raising my defence!");
			}
			else if (aoECast)
			{
				AoE(2);
			}
			else
			{
				TargetEnemy();
				if (targetEnemy)
				{
				targetEnemy.BuffSlashDef(buffDefSlash, countdownBuffDef);
				targetEnemy.BuffBluntDef(buffDefBlunt, countdownBuffDef);
				targetEnemy.BuffPierceDef(buffDefPierce, countdownBuffDef);
				targetEnemy.BuffMagDef(buffDefMag, countdownBuffDef);
				// Debug.Log("Oh wow, look at me... I'm raising your defence!");
				}
			}
			buffDefCount += (countdownBuffDef);
			//Debug.Log("Heal count is above 0");
		}
		else
		{
			//Debug.Log("I'm counting down");
			buffDefCount -= Time.deltaTime;
		}
	}
	//Give target(s) immunity to debuffs and damage
	void BuffImmune()
	{
		if (immuneCount <= 0)
		{
			if(castSelf)
			{
				enemy.Immune(countdownImmune);
				casting = true;
			}
			else if (aoECast)
			{
				AoE(3);
			}
			else
			{
			TargetEnemy();
			targetEnemy.Immune(countdownImmune);
			}
			//Debug.Log("Tis but a scratch!");
			immuneCount += (countdownImmune);
		}
		else
		{
			immuneCount -= Time.deltaTime;
		}
	}
	//Gives target(s) "levitate" to avoid ground attack (yet to be implemented)
	void BuffLevitate()
	{
		if (levitateCount <= 0)
		{
			if(castSelf)
			{
				casting = true;
				enemy.Flying(countdownLevitate);
				//Debug.Log("I'm flying!!");
			}
			else
			{
				TargetEnemy();
				targetEnemy.Flying(countdownLevitate);
				//Debug.Log("Woah, what's going on?");
			}
			//[change opaqueness]
			levitateCount += (countdownLevitate * 1.5f);
		}
		else
		{
			levitateCount -= Time.deltaTime;
		}
	}
	//Summons the prefab(s) set in the inspector
	void BuffSummoner()
	{
		if (summonCount <= 0)
		{
			casting = true;
			StartCoroutine(enemy.SummonNow());
			// Debug.Log("I summon youuu..");
			summonCount += (countdownSummon * 2);
		}
		// else if (summonCount <= (countdownSummon / 2))
		// {
		// 	summonCount -= Time.deltaTime;
		// 	Debug.Log("Countdown is... " + summonCount + " .");
		// }
		else
		{
			summonCount -= Time.deltaTime;
		}
	}
	//Makes an enemy invisible
	void BuffHide()
	{
		if (hideCount < 0)
		{
			casting = true;
			enemy.gameObject.tag = "Fallen";
			//Debug.Log("Shhh nothing to see hear");
			//[change opaqueness]
			hideCount += (countdownHide * 2.0f);
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
		casting = true;
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
	void AoE (int spellNumber)
	{
		//Debug.Log("I'm shooting an emeny!");
		casting = true;
		Collider[] colliders = Physics.OverlapSphere(transform.position, range);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				AoECast(collider.transform, spellNumber);
			}
		}
		//Vector3 dir = firePoint.position - target.position;
		colliders = null;
	}
	void AoECast (Transform enemy, int castingSpell)
	{
		Enemy e = enemy.GetComponent<Enemy>();
		//Debug.Log("Enemy targeted - " + e + " .");
		switch(castingSpell)
		{
			default :
				break;
			case 0:
				e.Speed(bonusSpeed, countdownSpeed);
				break;
			case 1:
				e.Healing(bonusHealth);
				break;
			case 2:
				e.BuffSlashDef(buffDefSlash, countdownBuffDef);
				e.BuffBluntDef(buffDefBlunt, countdownBuffDef);
				e.BuffPierceDef(buffDefPierce, countdownBuffDef);
				e.BuffMagDef(buffDefMag, countdownBuffDef);
				break;
			case 3:
				e.Immune(countdownImmune);
				break;
		}
	}
	//Debuffs tower(s) attack speed
	void DebuffTowerSpeed()
	{
		if (debuffSpeedCount < 0)
		{
			
			if (aoECast)
			{
				TowerAoE(0);
			}
			else
			{
				// Debug.Log("Targeting");
				TargetTower();

				if(targetTower)
				{
					//Debug.Log("Tower targeted - " + targetTower + " .");
					targetTower.DebuffSpeed(debuffSpeed, countdownDebuffSpeed);
				}
				
			}
			debuffSpeedCount += (countdownDebuffSpeed);
		}
		else
		{
			debuffSpeedCount -= Time.deltaTime;
		}
	}
	void AttackTowerHP()
	{
		if (attackTowerHPCount < 0)
		{
			
			if (aoECast)
			{
				TowerAoE(1);
			}
			else
			{
				// Debug.Log("Targeting");
				TargetTower();

				if(targetTower)
				{
					//Debug.Log("Tower targeted - " + targetTower + " .");
					targetTower.AttackTowerHP(attackTowerDmg);
				}
				
			}
			attackTowerHPCount += (countdownAttackTower);
		}
		else
		{
			attackTowerHPCount -= Time.deltaTime;
		}
	}
	void TargetTower()
	{
		//Debug.Log("Targeting Tower");
		casting = true;
		GameObject[] towers = GameObject.FindGameObjectsWithTag(towerTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestTower = null;
		foreach (GameObject tower in towers)
		{
			if (tower == gameObject) continue;

			float distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
			if (distanceToTower < shortestDistance)
			{
				shortestDistance = distanceToTower;
				nearestTower = tower;
			}
		}
		if (nearestTower != null && shortestDistance <= range)
		{
			target = nearestTower.transform;
			targetTower = nearestTower.GetComponent<Turret>();
		} else
		{
			target = null;
		}
	}
	void TowerAoE (int spellNumber)
	{
		//Debug.Log("I'm shooting an emeny!");
		casting = true;
		Collider[] colliders = Physics.OverlapSphere(transform.position, range);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Tower")
			{
				TowerAoECast(collider.transform, spellNumber);
			}
		}
		//Vector3 dir = firePoint.position - target.position;
		colliders = null;
	}
	void TowerAoECast (Transform tower, int castingSpell)
	{
		Turret t = tower.GetComponent<Turret>();
		//Debug.Log("Enemy targeted - " + e + " .");
		switch(castingSpell)
		{
			default :
				break;
			case 0:
				t.DebuffSpeed(debuffSpeed, countdownDebuffSpeed);
				break;
			case 1:
				t.AttackTowerHP(attackTowerDmg);
				break;
			case 2:
				
				break;
			case 3:
				
				break;
		}
		
	}
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
