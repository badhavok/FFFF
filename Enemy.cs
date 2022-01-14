using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {

	public EnemyStats enemyStats;
	public EnemyMovement enemyMovement;
	public EnemySpells enemySpells;

	private Transform target;
	private Enemy targetEnemy;
	public string enemyTag = "Enemy";

	public float startSpeed = 10f;

	public float speed;
	public float maxHealth, baseHealth, updatedHealth;
	public int physDef, magDef;
	public bool isSpider = false;
	public bool isFlying = false;
	public bool isBoss = false;
	public bool isChicken = false;
[HideInInspector] public float imFlying, countdownChicken, chk;
[HideInInspector] public bool fromDropship = false;
	private bool speedEnemy;
	private float countdownSpeed, bonusSpeed;

	private float countdownSlow, slowSpeed;
	private bool slowEnemy;

	private float countdownStop;
	private bool stopEnemy = false;

	private float countdownFear;
[HideInInspector] public bool fearEnemy = false;

	private float countdownPoison,physicalStrengthPoison, magicalStrengthPoison;
	private bool poisonEnemy;

[HideInInspector] public float countdownImmune, imm;
	private bool immune = false;

[HideInInspector] public float countdownSilence, sil;
	public bool silence = false;

[HideInInspector] public float countdownDoom, doo;
	public bool doom = false;
	public float virusR, virusT, spreadTime, spreadRange, damHP;
	public bool virus = false;
[HideInInspector] public float damVam = 0;

	public GameObject deathEffect;

	[Header("Unity Stuff")]
	public GameObject canvas;
	public Image healthBar;

	public Text doomCountdownText;

[HideInInspector] public float physAmount, physDamage, magAmount, magDamage;
	public bool goldenEnemy = false;
	private float goldBonus = 0;

	private bool isDead = false;

	void Start ()
	{
		speed = startSpeed;
		//[base + (base / 2) + (level * 10) + (wave * 10)]
		if(isBoss)
		{
			maxHealth = updatedHealth;
			updatedHealth = enemyStats.startHealth;
			Debug.Log("Boss, I have " + updatedHealth + " HP");
		}
		else
		{
			baseHealth = enemyStats.startHealth;
			updatedHealth = baseHealth + (baseHealth / 2) + (BuildManager.Level * 10) + (BuildManager.Wave * 10);
			maxHealth = updatedHealth;

			physDef = enemyStats.startPhysDef;
			magDef = enemyStats.startMagDef;
			Debug.Log("I have " + updatedHealth + " HP");
		}
	}

	public void Healing (float healSpell)
	{
		if (updatedHealth == maxHealth)
		{
			Debug.Log("Thanks but don't need it");
		}
		else
		{
			updatedHealth += healSpell;
			if (updatedHealth > maxHealth)
			{
				updatedHealth = maxHealth;
			}
			healthBar.fillAmount = updatedHealth / maxHealth;
			Debug.Log("I'm being healed by " + healSpell);
		}
	}

	public void TakeDamage (float physAmount, float magAmount)
	{
		if (immune)
		{
			Debug.Log("I'm immune!");
			return;
		}
		physDamage = physAmount - physDef;
		magDamage = magAmount - magDef;

		float amount = physDamage + magDamage;

		//Debug.Log("I'm taking damage " + amount);
		if(amount <= 0)
		{
		}
		else
		{
			updatedHealth -= amount;
			healthBar.fillAmount = updatedHealth / maxHealth;
		}

		//Debug.Log("Am I healing? " + updatedHealth);

		if (updatedHealth <= 0)
		{
			Die();
		}
		amount = 0;
	}
	public void Vampire(float damageVam)
	{
		damVam = damageVam;
		updatedHealth -= damVam;
		healthBar.fillAmount = updatedHealth / maxHealth;
		Debug.Log("I have taken " + damVam + " damage!");
		if (updatedHealth <= 0)
		{
			Die();
		}
	}
	public void Virus(float virT, float virR, float damageHP)
	{
		virus = true;
		virusT = virT;
		virusR = virR;
		damHP = damageHP;
		spreadTime = virT;
		spreadRange = virR;
		maxHealth -= damHP;
		updatedHealth -= damHP;
	}
	public void Doom(float doo)
	{
		doom = true;
		countdownDoom = doo;
	}
	public void Chicken()
	{
		isChicken = true;
		silence = true;
		immune = false;
	}
	public void Silence(float sil)
	{
		countdownSilence = sil;
		silence = true;
	}
	public void Immune (float imm)
	{
		countdownImmune = imm;
		immune = true;
		doom = false;
		slowEnemy = false;
		stopEnemy = false;
		fearEnemy = false;
		poisonEnemy = false;
	}
	public void Slow (float slo, float slt)
	{
		slowEnemy = true;
		slowSpeed = slo;
		countdownSlow = slt;
	}
	public void Stop (float stp)
	{
		stopEnemy = true;
		countdownStop = stp;
	}
	public void Fear (float fea)
	{
		fearEnemy = true;
		countdownFear = fea;
	}
	public void Poison (int psnS, float psnT)
	{
		poisonEnemy = true;
		magicalStrengthPoison = psnS;
		countdownPoison = psnT;
	}

	public void Speed (float spdB, float spdD)
	{
		speedEnemy = true;
		bonusSpeed = spdB;
		countdownSpeed = spdD;
	}
	public void Flying (float flyT)
	{
		enemyStats.isHovercraft = true;
		imFlying = flyT;
	}
	void Update ()
	{
		if (updatedHealth == maxHealth)
		{
			canvas.SetActive(false);
		}
		else
		{
			canvas.SetActive(true);
		}
		if(virus)
		{
			TargetEnemy(virusR);
			targetEnemy.Virus(spreadTime, spreadRange, damHP);
			if(targetEnemy != null)
			{
				Debug.Log("I've spread!");
			}
			virusT -= Time.deltaTime;
			if (virusT < 0)
			{
				virus = false;
			}
		}
		if(isChicken)
		{
			Die();
			GameObject enemy = Instantiate(BuildManager.ChickenEnemy, transform.position, Quaternion.identity);
			enemy.GetComponent<Enemy>().fromDropship = true;
			enemy.GetComponent<EnemyMovement>().wavepointIndex = enemyMovement.wavepointIndex;
			++WaveSpawner.EnemiesAlive;
		}
		if (doom)
		{
			Debug.Log("I'm dooomeeddddd");
			countdownDoom -= Time.deltaTime;
			countdownDoom = Mathf.Clamp(countdownDoom, 0f, Mathf.Infinity);
			doomCountdownText.text = string.Format("{0:00.00}", countdownDoom);
			canvas.SetActive(true);
			if(countdownDoom <= 0)
			{
				Die();
			}
		}
		else if (!doom)
		{
			countdownDoom = 0;
		}
		if (immune)
		{
			countdownImmune -= Time.deltaTime;
			if (countdownImmune <= 0)
			{
				immune = false;
			}
		}
		if (goldenEnemy || isBoss)
		{
			goldBonus += Time.deltaTime;
			if (Input.touchCount > 0)
			 {
				Touch t = Input.GetTouch(0);
				{
					 if(t.phase == TouchPhase.Began)
					 {
						Vector3 pos = t.position;
						if (GetComponent<Collider>().gameObject.CompareTag("Enemy"))
						 {
							Debug.Log("I am touched");
							updatedHealth -= 2;
							healthBar.fillAmount = updatedHealth / maxHealth;
							if (updatedHealth <= 0 && !isDead)
							{
								Die();
							}
						 }
					 }
				 }
			 }
		}
		if (speedEnemy)
		{
			speed = startSpeed * (1f + bonusSpeed);
			countdownSpeed -= Time.deltaTime;
			if (countdownSpeed <= 0)
			{
				speedEnemy = false;
				speed = startSpeed;
				//Debug.Log("Weeeee... again please!");
			}
		}
		if (slowEnemy)
		{
			speed = startSpeed * (1f - slowSpeed);
			//Debug.Log("I'm slower by... " + speed + "... damn");
			countdownSlow -= Time.deltaTime;
			if (countdownSlow <= 0)
			{
				slowEnemy = false;
				speed = startSpeed;
				//Debug.Log("Yeah, " + speed + " baby.... Weeeeee");
			}
		}
		else
		{
			speed = startSpeed;
		}
		if (stopEnemy)
		{
			speed = startSpeed * (1f - 1f);
			countdownStop  -= Time.deltaTime;
			//Debug.Log("Stopped");
			if (countdownStop <= 0)
			{
				stopEnemy = false;
				speed = startSpeed;
				//Debug.Log("I'm free");
			}
		}
		else
		{
			speed = startSpeed;
		}
		if (fearEnemy)
		{
			countdownFear -= Time.deltaTime;
			//Debug.Log("I have been feared");
			if (countdownFear <= 0)
			{
				fearEnemy = false;
			}
		}
		if (poisonEnemy)
		{
			countdownPoison -= Time.deltaTime;
			TakeDamage(physicalStrengthPoison, magicalStrengthPoison);
			//Debug.Log("I'm taking damage: " + strengthPoison + " for " + countdownPoison + ".");
			if (countdownPoison <= 0)
			{
				poisonEnemy = false;
				//Debug.Log("Poison end");
			}
		}
		if (enemyStats.isHovercraft)
		{
			if (enemyStats.hoverCount <= 0)
			{
				isFlying = true;
				enemyStats.hoverCount += 10;
			}
			else if (enemyStats.hoverCount < 5 )
			{
				enemyStats.hoverCount -= Time.deltaTime;
				isFlying = false;
			}
			else
			{
				enemyStats.hoverCount -= Time.deltaTime;
			}
			if (imFlying > 0)
			{
				isFlying = true;
				imFlying -= Time.deltaTime;
			}
			else if (imFlying <= 0)
			{
				isFlying = false;
				Debug.Log("Agaaain agaaainnnnn");
				enemyStats.isHovercraft = false;
			}
		}
		if (enemyStats.isDropship)
		{
			if (enemyStats.dropCount <=0)
			{
				StartCoroutine(Spawn());
				enemyStats.dropCount += enemyStats.dropTime;
			}
			else
			{
				enemyStats.dropCount -= Time.deltaTime;
			}
		}
	}
	private IEnumerator Spawn()
	{
		for (int i = 0; i < enemyStats.dropAmount; i++)
				{
					GameObject enemy = Instantiate(enemyStats.dropEnemy, transform.position, Quaternion.identity);
					enemy.GetComponent<Enemy>().fromDropship = true;
					enemy.GetComponent<EnemyMovement>().wavepointIndex = enemyMovement.wavepointIndex;
					++WaveSpawner.EnemiesAlive;
					yield return new WaitForSeconds(1.0f);
				}
	}
	public IEnumerator SummonNow()
	{
		int summoningIndex = Random.Range(0, enemySpells.summoningPool.Length);
		Debug.Log("I'm in the summonloop with index: " + summoningIndex);
		for (int i = 0; i < enemySpells.summonAmount; i++)
			{
				GameObject enemy = Instantiate(enemySpells.summoningPool[summoningIndex], transform.position, Quaternion.identity);
				enemy.GetComponent<Enemy>().fromDropship = true;
				enemy.GetComponent<EnemyMovement>().wavepointIndex = enemyMovement.wavepointIndex;
				++WaveSpawner.EnemiesAlive;
				yield return new WaitForSeconds(1.0f);
			}
	}
	void TargetEnemy(float range)
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
	void Die ()
	{
		isDead = true;
		float roundedResult = Mathf.Round(goldBonus / 2) * 2;
		roundedResult += EnemyStats.Worth;
		PlayerStats.Money += roundedResult;
		//Debug.Log("Gold bonus: " + goldBonus + " & Enemy worth: " + roundedResult);

		GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);

		--WaveSpawner.EnemiesAlive;

		Destroy(gameObject);
	}

}
