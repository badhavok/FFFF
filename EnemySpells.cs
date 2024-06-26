using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class is used for enemies that have spells, only needs to be assigned if the enemy needs it
[RequireComponent(typeof(Enemy))]
public class EnemySpells : MonoBehaviour {

	private Enemy enemy;
	private EnemyBuffs b;
	private Targeting targeting;
	private Turret turret;

	public Transform target;
	public Enemy targetEnemy;
    public List<Enemy> enemyList = new List<Enemy>();
	public Turret targetTurret;
	public List<Turret> turretList = new List<Turret>();
	public float range = 0;
	public string enemyTag = "Enemy";
	public string turretTag = "Turret";

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

	//This makes the enemy move faster
	public float bonusSpeed = 0;
	public float bonusSpeedTime = 0;
	private string buffImmune = "BuffImmune";
	private string healing = "Healing";
	private string buffHide = "BuffHide";
	private string speedBuff = "SpeedBuff";
	private string buffSlash = "BuffSlash";
	private string buffBlunt = "BuffBlunt";
	private string buffPierce = "BuffPierce";
	private string buffMag = "BuffMag";
	private string debuffTurretSpeed = "DebuffTurretSpeed";
	private string debuffTurretHealSpeed = "DebuffTurretHealSpeed";
	private string debuffSilence = "DebuffSilence";
	private string attackTurretHP = "AttackTurretHP";

	public float immuneTime = 0;
	public float healingTics, bonusHeal;
	public float hideTime = 0;
	public float buffDefSlash, buffDefBlunt, buffDefPierce, buffDefMag;
	public float buffDefTime = 0;
	
	public float debuffTurretSpeedTime, debuffTurretSPD = 0f;
	public float debuffTurretHealSpeedTime, debuffTurretHealSPD = 0f;
	public float debuffSilenceTime, debuffSilenceSPD = 0f;
	public float attackTurretHPTics, attackTurretHPDMG = 0;
	public GameObject attackTurretHPEffect;
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
		b = GetComponent<EnemyBuffs>();
		targeting = GetComponent<Targeting>();

		anim = gameObject.GetComponentInChildren<Animator>();
		//casting = gameObject.GetComponentsInChildren<ParticleSystem>();
		customCastTime = customCastCountdown;
	}
	void Update()
	{
		if(enemy.isDead)
		{
			enabled = false;
		}
		if(isCasting > 0)
		{
			//Debug.Log("I think I'm stuck here");
			isCasting -= Time.deltaTime;
		}
		else
		{
			//Debug.Log("No, I'm actually stuck here");
			anim.SetBool("Cast", false);
			anim.SetBool("Move", true);
			// isCasting = 0;
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
				isCasting = m_CurrentClipLength + 1f;
				enemy.Casting(m_CurrentClipLength + 1f);
				casting = false;
				
				foreach (ParticleSystem casting in castList)
				{
				if(!casting.isPlaying)
						{
						casting.Play();
						}
				}
			}
			if(customSpell)
			{
				targetEnemy = null;
				if(customCastTime < 0)
				{
					casting = true;
					if(multiSpell)
					{
						MultiSpell();
					}
					else
					{
						RandomSpell();
					}
				}
				else if (customCastTime >= 0 & customCastTime < customCastCountdown - 0.01f)
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
				// Debug.Log("Spell number " + i + " is the spell " + castingThisSpell + " ... or " + customSpellList[i] + " ...");
				CastingSpell(castingThisSpell);
			}
		
		customCastTime = customCastCountdown;
	}
	void CastingSpell(string castingThisSpell)
	{
		switch(castingThisSpell)
				{
					default:
						break;
					case "buffImmune" :
						BuffImmune();
						break;
					case "buffDefences" : 
						BuffDefences();
						break;
					case "healing" :
						Healing();
						break; 
					case "buffHide" :
						BuffHide();
						break;
					case "buffSpeed" :
						BuffSpeed();
						break;
					case "buffSummoner" :
						buffSummoner = true;
						countdownSummon = 5;
						break;
					case "debuffTurretSpeed" :
						DebuffTurretSpeed();
						break;
					case "debuffTurretHealSpeed" :
						DebuffTurretHealSpeed();
						break;
					case "debuffSilence" :
						Debug.Log("Enemy spells silence");
						DebuffSilence();
						break;
					case "attackTurretHP" :
						AttackTurretHP();
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
			targeting.TargetEnemy(range);
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
		if (aoECast)
		{
			targeting.TargetAoEEnemy(range);
			foreach (Enemy targetEnemy in enemyList)
			{
				CastImmune(targetEnemy);
			}
			// Debug.Log("Casting AoE Immunity");
			enemyList.Clear();
			return;
		}
		if(castSelf)
		{
			targetEnemy = enemy;
		}
		else
		{
			targeting.TargetEnemy(range);
		}
		if(targetEnemy)
		{
			CastImmune(targetEnemy);
		}
	}
	void CastImmune(Enemy targetEnemy)
	{
		targetEnemy.buffs.BuffEffect(buffImmune, immuneTime, 0);
	}
	//Heal target(s)
	void Healing()
	{
		//Debug.Log("I'm in the speed void");
		if (aoECast)
		{
			targeting.TargetAoEEnemy(range);
			foreach (Enemy targetEnemy in enemyList)
			{
				CastHealing(targetEnemy);
			}
			// Debug.Log("Casting AoE Heal");
			enemyList.Clear();
			return;
		}
		if(castSelf)
		{
			targetEnemy = enemy;
		}
		else
		{
			targeting.TargetEnemy(range);
		}
		if(targetEnemy)
		{
			BuffingSpeed(targetEnemy);
		}
	}
	void CastHealing(Enemy targetEnemy)
	{
		targetEnemy.buffs.BuffEffect(healing, healingTics, bonusHeal);
	}
	void BuffHide()
	{
		if (aoECast)
		{
			targeting.TargetAoEEnemy(range);
			foreach (Enemy targetEnemy in enemyList)
			{
				CastHide(targetEnemy);
			}
			// Debug.Log("Casting AoE Hide");
			enemyList.Clear();
			return;
		}
		if(castSelf)
		{
			targetEnemy = enemy;
		}
		else
		{
			targeting.TargetEnemy(range);
		}
		if(targetEnemy)
		{
			CastHide(targetEnemy);
		}
	}
	void CastHide(Enemy targetEnemy)
	{
		targetEnemy.buffs.BuffEffect(buffHide, hideTime, 0);
	}
	// Buff enemy speed
	void BuffSpeed()
	{
		//Debug.Log("I'm in the speed void");
		if (aoECast)
		{
			targeting.TargetAoEEnemy(range);
			foreach (Enemy targetEnemy in enemyList)
			{
				BuffingSpeed(targetEnemy);
			}
			// Debug.Log("Casting AoE Speed");
			enemyList.Clear();
			return;
		}
		if(castSelf)
		{
			targetEnemy = enemy;
		}
		else
		{
			targeting.TargetEnemy(range);
		}
		if(targetEnemy)
		{
			BuffingSpeed(targetEnemy);
		}
	}
	void BuffingSpeed(Enemy targetEnemy)
	{
		targetEnemy.buffs.BuffEffect(speedBuff, bonusSpeedTime, bonusSpeed);
		// Debug.Log("TargetEnemy = " + targetEnemy + ".  Speed = " + targetEnemy.speed);
	}
	//Buff target(s)
	void BuffDefences()
	{
		if (aoECast)
		{
			targeting.TargetAoEEnemy(range);
			foreach (Enemy targetEnemy in enemyList)
			{
				BuffingDefences(targetEnemy);
			}
			// Debug.Log("Casting AoE Defence");
			enemyList.Clear();
			return;
		}
		if(castSelf)
		{
			targetEnemy = enemy;
		}
		else
		{
			targeting.TargetEnemy(range);
		}
		if(targetEnemy)
		{
			BuffingDefences(targetEnemy);
		}
		//Debug.Log("Heal count is above 0");
	}
	void BuffingDefences(Enemy targetEnemy)
	{
		targetEnemy.buffs.BuffEffect(buffSlash, buffDefTime, buffDefSlash);
		targetEnemy.buffs.BuffEffect(buffBlunt, buffDefTime, buffDefBlunt);
		targetEnemy.buffs.BuffEffect(buffPierce, buffDefTime, buffDefPierce);
		targetEnemy.buffs.BuffEffect(buffMag, buffDefTime, buffDefMag);
		// Debug.Log("Target is - " + targetEnemy + " - countdown = " + buffDefTime + " - def bonus = " + buffDefSlash + ".  New def = " + targetEnemy.slashDef);
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
				targeting.TargetEnemy(range);
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
	//Debuffs tower(s) attack speed
	void DebuffTurretSpeed()
	{
		if (aoECast)
		{
			// Debug.Log("Turret AoE Speed");
			targeting.TargetAoETurret(range);
			foreach (Turret targetTurret in turretList)
			{
				DebuffTurretSPD(targetTurret);
			}
			turretList.Clear();
			return;
		}
		else
		{
			targeting.TargetTurret(range);
			if(targetTurret)
			{
				DebuffTurretSPD(targetTurret);
			}	
		}
	}
	void DebuffTurretSPD(Turret targetTurret)
	{
		// Debug.Log("Turret debuff speed = " + targetTurret);
		targetTurret.dots.DotEffect(debuffTurretSpeed, debuffTurretSpeedTime, debuffTurretSPD);
	}
	void DebuffTurretHealSpeed()
	{
		if (aoECast)
		{
			// Debug.Log("Turret AoE Heal Speed");
			targeting.TargetAoETurret(range);
			foreach (Turret targetTurret in turretList)
			{
				DebuffTurretHealSPD(targetTurret);
			}
			turretList.Clear();
			return;
		}
		else
		{
			targeting.TargetTurret(range);
			if(targetTurret)
			{
				DebuffTurretHealSPD(targetTurret);
			}	
		}
	}
	void DebuffTurretHealSPD(Turret targetTurret)
	{
		// Debug.Log("Turret debuff speed = " + targetTurret);
		targetTurret.dots.DotEffect(debuffTurretHealSpeed, debuffTurretHealSpeedTime, debuffTurretHealSPD);
	}
	void DebuffSilence()
	{
		if (aoECast)
		{
			// Debug.Log("Turret AoE Heal Speed");
			targeting.TargetAoETurret(range);
			foreach (Turret targetTurret in turretList)
			{
				CastDebuffSilence(targetTurret);
			}
			turretList.Clear();
			return;
		}
		else
		{
			targeting.TargetTurret(range);
			if(targetTurret)
			{
				CastDebuffSilence(targetTurret);
			}	
		}
	}
	void CastDebuffSilence(Turret targetTurret)
	{
		targetTurret.dots.DotEffect(debuffSilence, debuffSilenceTime, debuffSilenceSPD);
	}
	void AttackTurretHP()
	{
		// Debug.Log("Turret AoE Damage");
		if (aoECast)
		{
			targeting.TargetAoETurret(range);
			foreach (Turret targetTurret in turretList)
			{
				AttackingTurretHP(targetTurret);
			}
			turretList.Clear();
			return;
		}
		else
		{
			targeting.TargetTurret(range);
			if(targetTurret)
			{
				AttackingTurretHP(targetTurret);
			}	
		}
	}
	void AttackingTurretHP(Turret targetTurret)
	{
		targetTurret.dots.DotEffect(attackTurretHP, attackTurretHPTics, attackTurretHPDMG);
		GameObject effectIns = Instantiate(attackTurretHPEffect, targetTurret.transform.position, transform.rotation);
		Destroy(effectIns, 1.5f);	
		// Debug.Log("Turret attacking = " + targetTurret);
	}
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}