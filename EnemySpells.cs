using UnityEngine;
using System.Collections;

//This class is used for enemies that have spells, only needs to be assigned if the enemy needs it
[RequireComponent(typeof(Enemy))]
public class EnemySpells : MonoBehaviour {

	private Enemy enemy;

	private Transform target;
	private Enemy targetEnemy;
	public string enemyTag = "Enemy";

	public float range = 0;
	public bool casting;
	public float isCasting = 0;

	//Is the enemy casting on itself
	[Header("Self casting")]
	public bool castSelf = false;

	//This makes the enemy invisible to detection
	public bool buffHide = false;
	public float countdownHide = 0;
[HideInInspector] public float hideCount = 0;

	//This heals an enemy
	public bool buffHealing = false;
	public float bonusHealth = 0;
	public float countdownHealth = 0;
	public float healCount = 0;

	//This makes an enemy immune to damage/debuffs - but the towers will still shoot at it
	public bool buffImmune = false;
	public float countdownImmune = 0;
[HideInInspector]	public float immuneCount = 0;

	//This makes the enemy move faster
	public bool buffSpeed = false;
	public float bonusSpeed = 0;
	public float countdownSpeed = 0;
	public float speedCount = 0;

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
			casting = true;
			}
			else
			{
			TargetEnemy();
			targetEnemy.Immune(countdownImmune);
			}
			//Debug.Log("Tis but a scratch!");
			immuneCount += (countdownImmune * 1.5f);
		}
		else
		{
			immuneCount -= Time.deltaTime;
		}
	}
	void BuffHealing()
	{
		//Debug.Log("I'm in the healing void");
		if (healCount <= 0)
		{
			if(castSelf)
			{
				casting = true;
				enemy.Healing(bonusHealth);
				//Debug.Log("Oh wow, look at me... I'm healing me!");
			}
			else
			{
				TargetEnemy();
				targetEnemy.Healing(bonusHealth);
				//Debug.Log("Oh wow, look at me... I'm healing you!");
			}
			healCount += (countdownHealth * 1.5f);
			//Debug.Log("Heal count is above 0");
		}
		else
		{
			//Debug.Log("I'm counting down");
			healCount -= Time.deltaTime;
		}
	}
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
	void BuffSummoner()
	{
		if (summonCount <= 0)
		{
			casting = true;
			StartCoroutine(enemy.SummonNow());
			//Debug.Log("I summon youuu..");
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

	void BuffSpeed()
	{
		if (speedCount < 0)
		{
			if(castSelf)
			{
				casting = true;
				//Debug.Log("I'm casting now");
				enemy.Speed(bonusSpeed, countdownSpeed);
				//casting = true;
			}
			else
			{
				//Debug.Log("Targeting");
				TargetEnemy();
				if(targetEnemy)
				{
					//Debug.Log("Enemy targeted - " + targetEnemy + " .");
					targetEnemy.Speed(bonusSpeed, countdownSpeed);
				}
			}
			//Debug.Log("Weeeeee.... don't say bye then!");
			speedCount += (countdownSpeed * 1.5f);
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
			casting = true;
			enemy.gameObject.tag = "Fallen";
			//Debug.Log("Shhh nothing to see hear");
			//[change opaqueness]
			hideCount += (countdownHide * 1.5f);
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


}
