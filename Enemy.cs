using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {

	public EnemyStats enemyStats;
	public EnemyMovement enemyMovement;
	public EnemySpells enemySpells;

	public float startSpeed = 10f;

	public float speed;
	private float maxHealth;
	public bool isSpider = false;
	public bool isFlying = false;
	public bool isBoss = false;
[HideInInspector] public float imFlying;	
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

	public GameObject deathEffect;

	[Header("Unity Stuff")]
	public Image healthBar;

[HideInInspector] public float physAmount, physDamage, magAmount, magDamage;
	public bool goldenEnemy = false;
	private float goldBonus = 0;

	private bool isDead = false;

	void Start ()
	{
		speed = startSpeed;
		maxHealth = enemyStats.startHealth;
	}

	public void Healing (float healSpell)
	{
		if (enemyStats.startHealth == maxHealth)
		{
			Debug.Log("Thanks but don't need it");
		}
		else
		{
			enemyStats.startHealth += healSpell;
			if (enemyStats.startHealth > maxHealth)
			{
				enemyStats.startHealth = maxHealth;
			}
			healthBar.fillAmount = enemyStats.startHealth / maxHealth;
			Debug.Log("I'm being healed by " + healSpell);
		}
	}

	public void TakeDamage (float physAmount, float magAmount)
	{
		if (immune)
		{
			//Debug.Log("I'm immune!");
			return;
		}
		physDamage = physAmount - enemyStats.physDef;
		magDamage = magAmount - enemyStats.magDef;

		float amount = physDamage + magDamage;

		//Debug.Log("I'm taking damage " + amount);
		if(amount <= 0)
		{
		}
		else
		{
			enemyStats.startHealth -= amount;
			healthBar.fillAmount = enemyStats.startHealth / maxHealth;
		}

		//Debug.Log("Am I healing? " + enemyStats.startHealth);

		if (enemyStats.startHealth <= 0 && !isDead)
		{
			Die();
		}
		amount = 0;
	}

	public void Immune (float imm)
	{
		countdownImmune = imm;
		immune = true;
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
		if (immune)
		{
			countdownImmune -= Time.deltaTime;
			if (countdownImmune <= 0)
			{
				immune = false;
			}
		}
		if (goldenEnemy)
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
							enemyStats.startHealth -= 2;
							healthBar.fillAmount = enemyStats.startHealth / maxHealth;
							if (enemyStats.startHealth <= 0 && !isDead)
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
				Debug.Log("Sorry... again please!");
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
		GameObject enemy = Instantiate(enemySpells.summoningPool[summoningIndex], transform.position, Quaternion.identity);
		enemy.GetComponent<Enemy>().fromDropship = true;
		enemy.GetComponent<EnemyMovement>().wavepointIndex = enemyMovement.wavepointIndex;
		++WaveSpawner.EnemiesAlive;
		yield return new WaitForSeconds(1.0f);
	}

	void Die ()
	{
		isDead = true;
		float roundedResult = Mathf.Round(goldBonus / 2) * 2;
		roundedResult += EnemyStats.Worth;
		PlayerStats.Money += roundedResult;
		Debug.Log("Gold bonus: " + goldBonus + " & Enemy worth: " + roundedResult);

		GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);

		--WaveSpawner.EnemiesAlive;

		Destroy(gameObject);
	}

}
