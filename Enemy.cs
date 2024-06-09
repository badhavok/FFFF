using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This is the main enemy class and should be assigned to EVERYTHING that is going to be attacked
//It contains the code required to give the enemy HP and get attacked etc..
public class Enemy : MonoBehaviour {
	[Header("Must be set")]
	public EnemyStats enemyStats;
	public EnemyMovement enemyMovement;
	[Header("Casting enemy?")]
	public EnemySpells enemySpells;

	private Transform target;
	private Enemy targetEnemy;

	[Header("Unity Stuff")]
	public GameObject canvas;
	public Image healthBar;
	public string enemyTag = "Enemy";
	public float startSpeed = 10f;

	[Header("Is it special?")]
	public bool isSpider = false;
	public bool isWorm = false;
	public bool isFlying = false;
	public bool isChicken = false;
	public bool isBoss, isMiniBoss, isRandom = false;

//Most of the variables are straight forward
//Public variables are set by the attack/buff
//Private variables are used to run the counter
	public float speed;
[HideInInspector]	public float maxHealth, baseHealth;
	public float updatedHealth;
[HideInInspector] public int bluntDef, slashDef, pierceDef, magDef;
[HideInInspector] public int startBluntDef, startSlashDef, startPierceDef, startMagDef;
[HideInInspector] public bool buffSlashDef, buffPierceDef, buffBluntDef, buffMagDef = false;
[HideInInspector] public float countdownSlashBuffDef, countdownPierceBuffDef, countdownBluntBuffDef, countdownMagBuffDef;
[HideInInspector] public int fireDef, iceDef, waterDef, lighteningDef, earthDef, windDef, lightDef, darkDef;
[HideInInspector] public int startFireDef, startIceDef, startWaterDef, startLighteningDef, startEarthDef, startWindDef, startLightDef, startDarkDef;
[HideInInspector] public float imFlying, countdownChicken, chk;
//This is to set the path of any summons, to where the enemy that spawned them, is (E.G not the very start of the path)
[HideInInspector] public bool fromDropship = false;

	public bool speedEnemy;
[HideInInspector] public float countdownSpeed, bonusSpeed;

[HideInInspector] public float countdownSlow, slowSpeed;
[HideInInspector]	public bool slowEnemy;

[HideInInspector] public float countdownStop, countdownCasting;
	public bool stopEnemy, castingEnemy = false;

[HideInInspector]	public float countdownFear;
[HideInInspector] public bool fearEnemy = false;

	public float countdownPoison, poisonInterval, physicalStrengthPoison, magicalStrengthPoison;
[HideInInspector]	public bool poisonEnemy;

[HideInInspector] public float countdownImmune, imm;
[HideInInspector]	public bool immune = false;

	public float countdownSilence, sil;
	public bool silence = false;

[HideInInspector] public float countdownDoom, doo;
[HideInInspector]	public bool doom = false;
[HideInInspector]	public float virusR, virusT, spreadTime, spreadRange, damHP;
[HideInInspector]	public bool virus = false;
[HideInInspector] public float damVam = 0;

	//Might change this to an animation or particle system?
	public GameObject deathEffect;
	//Variable to show a doom counter above the enemy
	public Text doomCountdownText;

	private Animator anim;

[HideInInspector] public float slashingDamage, piercingDamage, bluntDamage, magDamage;

	public float remainingPathDist;
	public static int path;

	[Header("Am I golden?")]
	public bool goldenEnemy = false;
	private float goldBonus = 0;
	private float pointsBonus = 0;
	public bool noDrop = false;
	public bool isDead = false;
	private bool touchedScreen;
	private Camera cam;
	
	void Start ()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		cam = CameraController.PlayerCam;
		speed = startSpeed;
		bluntDef = enemyStats.startBluntDef;
		slashDef = enemyStats.startSlashDef;
		pierceDef = enemyStats.startPierceDef;
		magDef = enemyStats.startMagDef;
		fireDef = enemyStats.startFireDef;
		iceDef = enemyStats.startIceDef;
		waterDef = enemyStats.startWaterDef;
		lighteningDef = enemyStats.startLighteningDef;
		earthDef = enemyStats.startEarthDef;
		windDef = enemyStats.startWindDef;
		lightDef = enemyStats.startLightDef;
		darkDef = enemyStats.startDarkDef;

		if(isBoss || isMiniBoss)
		{
			pointsBonus = enemyStats.startPoints;
		}
		//Calculation to adjust hp based on the level
		//[base + (base / 2) + (level * 10) + (wave * 10)]
		//Calculate the HP of a boss differently to an enemy
		if(isBoss)
		{
			updatedHealth = enemyStats.startHealth;
			maxHealth = updatedHealth;
			//Debug.Log("Boss, I have " + updatedHealth + " HP");
		}
		else
		//If it's not a boss it 'must' be an enemy
		{
			baseHealth = enemyStats.startHealth;
			updatedHealth = baseHealth + (baseHealth / 2) + (BuildManager.Level * 10) + (BuildManager.Wave * 10);
			maxHealth = updatedHealth;

			//physDef = enemyStats.startPhysDef;
			
			//Debug.Log("I have " + updatedHealth + " HP");
		}
	}
	void Update()
	{
		if (isDead)
		{
			this.gameObject.tag = "Fallen";
			enabled = false;
			return;
		}
		remainingPathDist = enemyMovement.remainingPathDistance;
		//Debug.Log("My path dist is : " + remainingPathDistance);

		if(pointsBonus > 0)
		{
			pointsBonus -= speed * Time.deltaTime;
		}
		//Code to hide HP bar until the enemy has been attacked for the first time
		if (updatedHealth == maxHealth)
		{
			canvas.SetActive(false);
		}
		else
		{
			//Debug.Log("I'm not at max HP");
			canvas.SetActive(true);
			canvas.transform.LookAt(cam.transform);
		}
		
		//Code to set the effects on the "current" enemy
		if(virus)
		{
			TargetEnemy(virusR);
			targetEnemy.Virus(spreadTime, spreadRange, damHP);
			if(targetEnemy != null)
			{
				//Debug.Log("I've spread!");
			}
			virusT -= Time.deltaTime;
			if (virusT < 0)
			{
				virus = false;
			}
		}
		//Area to detect if the enemy is golden/a boss and take hits when the player is tapping on the screen
		if (goldenEnemy || isMiniBoss || isBoss)
		{
			//If the enemy is golden, but not a boss, increase the amount of gold it will 'drop' when it is killed
			if(goldenEnemy)
			{
				goldBonus += Time.deltaTime;
			}
			if(Input.touchCount > 0)
			{
				Touch finger = Input.GetTouch(0);
				
				if(finger.phase == TouchPhase.Began)
				{
					touchedScreen = true;
				}
				else
				{
					touchedScreen = false;
				}
			}

			if (touchedScreen || Input.GetMouseButtonDown(0) || Input.GetKey("k"))
			{				
				if (GetComponent<Collider>().gameObject.CompareTag("Enemy"))
				{
					//Debug.Log("I am golden, I am offended by your touch");
					//Sets how much damage is done - might update this to be in the inspector
					updatedHealth -= 2;
					healthBar.fillAmount = updatedHealth / maxHealth;
					if (updatedHealth <= 0 && !isDead)
					{
						Die();
					}
				}
			}
		}
		if (poisonEnemy)
		{
			if (poisonInterval > 0)
			{
				//Debug.Log("In poison loop - interval = " + poisonInterval);
				if (countdownPoison >= 1.0f)
				{
					poisonInterval--;
					TakeDamage(0f, 0f, 0f, magicalStrengthPoison);
					//Debug.Log("I'm taking damage: " + magicalStrengthPoison + " for " + poisonInterval + ".");
					countdownPoison = 0;
				}
				countdownPoison += Time.deltaTime;
			}
			else
			{
					poisonEnemy = false;
					//Debug.Log("Poison end");
			}
		}
		if (castingEnemy)
		{
			speed = 0;
			countdownCasting -= Time.deltaTime;
			Debug.Log("Stopped");
			if (countdownCasting <= 0)
			{
				castingEnemy = false;
				speed = startSpeed;
				Debug.Log("I'm free");
			}
		}
		if(buffSlashDef)
		{
			countdownSlashBuffDef -= Time.deltaTime;
			if (countdownSlashBuffDef <= 0)
			{
				buffSlashDef = false;
				slashDef = enemyStats.startSlashDef;
			}
		}
		else
		{
			slashDef = enemyStats.startSlashDef;
		}
		if(buffBluntDef)
		{
			countdownBluntBuffDef -= Time.deltaTime;
			if (countdownBluntBuffDef <= 0)
			{
				buffBluntDef = false;
				bluntDef = enemyStats.startBluntDef;
			}
		}
		else
		{
			bluntDef = enemyStats.startBluntDef;
		}
		if(buffPierceDef)
		{
			countdownPierceBuffDef -= Time.deltaTime;
			if (countdownPierceBuffDef <= 0)
			{
				buffPierceDef = false;
				pierceDef = enemyStats.startPierceDef;
			}
		}
		else
		{
			pierceDef = enemyStats.startPierceDef;
		}
		if(buffMagDef)
		{
			countdownMagBuffDef -= Time.deltaTime;
			if (countdownMagBuffDef <= 0)
			{
				buffMagDef = false;
				magDef = enemyStats.startMagDef;
			}
		}
		else
		{
			magDef = enemyStats.startMagDef;
		}
		//Any code below this will not run if the enemy is a boss; this is to ignore any buffs/debuffs that would make the boss OP and no need to run through the loops
		if(isBoss)
		{
			silence = false;
			return;
		}
		//The code under hear will be buff/debuffs that ONLY affect normal enemies - NOT BOSSES
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
			//canvas.SetActive(true);
			if(countdownDoom <= 0)
			{
				Die();
			}
		}
		//Needs to be specified so when "Immune" is cast on an enemy with doom, it will remove the debuff and hide the counter
		else if (!doom)
		{
			countdownDoom = 20000;
      		//canvas.SetActive(false);
		}
		if (immune)
		{
			countdownImmune -= Time.deltaTime;
			if (countdownImmune <= 0)
			{
				immune = false;
			}
		}
		if (speedEnemy & !castingEnemy)
		{
			//Debug.Log("Speed enemy loop + " + bonusSpeed + " .");
			speed = startSpeed + bonusSpeed;
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
		else if (stopEnemy)
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
		else if (!speedEnemy && !castingEnemy)
		{
			speed = startSpeed;
		}
		else
		{

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
		if (silence)
		{
			countdownSilence -= Time.deltaTime;

			if (countdownSilence <=0)
			{
				silence = false;
			}
		}
		//Hovercraft is used for the temporary flying units
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
				//Debug.Log("Agaaain agaaainnnnn");
				enemyStats.isHovercraft = false;
			}
		}
		//This allows units to "duplicate" without the need to add the enemy spell code
		if (enemyStats.isDropship)
		{
			if (enemyStats.dropCount <=0)
			{
				//Debug.Log("I'm dropping ship... err... yeah");
				StartCoroutine(Spawn());
				enemyStats.dropCount += enemyStats.dropTime;
			}
			else
			{
				enemyStats.dropCount -= Time.deltaTime;
			}
		}
	}
	//Function for healing/vampire?
	public void Healing (float healSpell)
	{
		float healthToHeal = maxHealth * healSpell - maxHealth;
		Debug.Log("I'm healing " + healthToHeal + " hp");
		
		//If Max HP, move on
		if (updatedHealth == maxHealth)
		{
			Debug.Log("Thanks but don't need it, healing was " + healSpell);
		}
		else
		{
			updatedHealth += healthToHeal;
			if (updatedHealth > maxHealth)
			{
				//If overhealed - adjust HP back to MaxHP
				updatedHealth = maxHealth;
			}
			Debug.Log("Health is now " + updatedHealth);
			healthBar.fillAmount = updatedHealth / maxHealth;
			// Debug.Log("I'm being healed by " + healSpell);
		}
		healthToHeal = 0;
	}
	//Following buff the various def stats
	public void BuffSlashDef (int buffingSlashDef, float buffingSlashDefCountdown)
	{
		buffSlashDef = true;
		slashDef = slashDef + buffingSlashDef;
		countdownSlashBuffDef = buffingSlashDefCountdown;
	}
	public void BuffBluntDef (int buffingBluntDef, float buffingBluntDefCountdown)
	{
		buffBluntDef = true;
		bluntDef = bluntDef + buffingBluntDef;
		countdownBluntBuffDef = buffingBluntDefCountdown;
	}
	public void BuffPierceDef (int buffingPierceDef, float buffingPierceDefCountdown)
	{
		buffPierceDef = true;
		pierceDef = pierceDef + buffingPierceDef;
		countdownPierceBuffDef = buffingPierceDefCountdown;
	}
	public void BuffMagDef (int buffingMagDef, float buffingMagDefCountdown)
	{
		buffMagDef = true;
		magDef = magDef + buffingMagDef;
		countdownMagBuffDef = buffingMagDefCountdown;
	}
	//Calculate "Gravity" attacks differently since they are a ratio of max HP (Might adjust to current HP if too OP)
	public void GravityDMG (float gravity)
	{
		updatedHealth = maxHealth / gravity;
		healthBar.fillAmount = updatedHealth / maxHealth;

		//Calculate if this has killed the enemy - might adjust this to 1HP (would it make a difference?)
		if (updatedHealth <= 0)
		{
			Die();
		}
	}
	//Function used by "specialist" things to ignore all forms of defence - reserved for Breath attacks
	public void IgnoreImmuneDMG (float bluntDamage, float piercingDamage, float slashingDamage, float magDamage)
	{
		float amount = bluntDamage + piercingDamage + slashingDamage + magDamage;

		updatedHealth -= amount;
		healthBar.fillAmount = updatedHealth / maxHealth;

		if (updatedHealth <= 0)
		{
			Die();
		}
		amount = 0;
	}
	//Function for any "special" towers that may have ignore armour/defenses but not ignore immune spells
	public void TakePenDamage (float bluntDamage, float piercingDamage, float slashingDamage, float magDamage)
	{
		if (immune)
		{
			//Debug.Log("I'm immune!");
			return;
		}
		// Debug.Log("The damage " + this.gameObject + " taking is - " + bluntDamage + " blunt; " + piercingDamage + " pierce; " + slashingDamage + " slashing; " + magDamage + " magic;");
		float amount = bluntDamage + piercingDamage + slashingDamage + magDamage;

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
	//Function to calculate standard damage and deduct any defense the unit may have, including immunity
	public void TakeDamage (float bluntDamage, float piercingDamage, float slashingDamage, float magDamage)
	{
		if (immune)
		{
			Debug.Log("I'm immune!");
			return;
		}
		if(bluntDamage > 0)
		{
		bluntDamage = bluntDamage - bluntDef;
		}
		if(piercingDamage > 0)
		{
		piercingDamage = piercingDamage - pierceDef;
		}
		if(slashingDamage > 0)
		{
		slashingDamage = slashingDamage - slashDef;
		}
		if(magDamage > 0)
		{
		magDamage = magDamage - magDef;
		}
		if(bluntDamage < 0)
		{
		bluntDamage = 0;
		}
		if(piercingDamage < 0)
		{
		piercingDamage = 0;
		}
		if(slashingDamage < 0)
		{
		slashingDamage = 0;
		}
		if(magDamage < 0)
		{
		magDamage = 0;
		}
		//Debug.Log("The damage " + this.gameObject + " taking is - " + bluntDamage + " blunt; " + piercingDamage + " pierce; " + slashingDamage + " slashing; " + magDamage + " magic;");
		float amount = bluntDamage + piercingDamage + slashingDamage + magDamage;

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
	// Calculate elemental damage
	public void ElementalDamage (float fireDamage, float iceDamage, float waterDamage, float lighteningDamage, float earthDamage, float windDamage, float lightDamage, float darkDamage)
	{
		if(iceDamage > 0)
		{
			iceDamage = iceDamage - iceDef;
			
			if(enemyStats.fireEnemy)
			{
				iceDamage = iceDamage / enemyStats.fireResist;
			}
			else if(enemyStats.iceEnemy)
			{
				iceDamage = iceDamage / enemyStats.iceResist;
			}
			if(enemyStats.windEnemy)
			{
				iceDamage = iceDamage * enemyStats.windResist;
			}
			Mathf.RoundToInt(iceDamage);
		}
		if(fireDamage > 0)
		{
			fireDamage = fireDamage - fireDef;
			
			if(enemyStats.waterEnemy)
			{
				fireDamage = fireDamage / enemyStats.waterResist;
			}
			else if(enemyStats.fireEnemy)
			{
				fireDamage = fireDamage / enemyStats.fireResist;
			}
			if(enemyStats.iceEnemy)
			{
				fireDamage = fireDamage * enemyStats.iceResist;
			}
			Mathf.RoundToInt(fireDamage);
		}
		if(waterDamage > 0)
		{
			waterDamage = waterDamage - waterDef;
			
			if(enemyStats.lighteningEnemy)
			{
				waterDamage = waterDamage / enemyStats.lighteningResist;
			}
			else if(enemyStats.waterEnemy)
			{
				waterDamage = waterDamage / enemyStats.waterResist;
			}
			if(enemyStats.fireEnemy)
			{
				waterDamage = waterDamage * enemyStats.fireResist;
			}
			Mathf.RoundToInt(waterDamage);
		}
		if(lighteningDamage > 0)
		{
			lighteningDamage = lighteningDamage - lighteningDef;
			
			if(enemyStats.earthEnemy)
			{
				lighteningDamage = lighteningDamage / enemyStats.earthResist;
			}
			else if(enemyStats.lighteningEnemy)
			{
				lighteningDamage = lighteningDamage / enemyStats.lighteningResist;
			}
			if(enemyStats.waterEnemy)
			{
				lighteningDamage = lighteningDamage * enemyStats.waterResist;
			}
			Mathf.RoundToInt(lighteningDamage);
		}
		if(earthDamage > 0)
		{
			earthDamage = earthDamage - earthDef;
			
			if(enemyStats.windEnemy)
			{
				earthDamage = earthDamage / enemyStats.windResist;
			}
			else if(enemyStats.earthEnemy)
			{
				earthDamage = earthDamage / enemyStats.earthResist;
			}
			if(enemyStats.lighteningEnemy)
			{
				earthDamage = earthDamage * enemyStats.lighteningResist;
			}
			Mathf.RoundToInt(earthDamage);
		}
		if(windDamage > 0)
		{
			windDamage = windDamage - windDef;
			
			if(enemyStats.iceEnemy)
			{
				windDamage = windDamage / enemyStats.iceResist;
			}
			else if(enemyStats.windEnemy)
			{
				windDamage = windDamage / enemyStats.windResist;
			}
			if(enemyStats.earthEnemy)
			{
				windDamage = windDamage * enemyStats.earthResist;
			}
			Mathf.RoundToInt(windDamage);
		}
		if(lightDamage > 0)
		{
			lightDamage = lightDamage - lightDef;

			if(enemyStats.darkEnemy)
			{
				lightDamage = lightDamage * enemyStats.darkResist;
			}
			if(enemyStats.lightEnemy)
			{
				lightDamage = lightDamage / enemyStats.lightResist;
			}
			Mathf.RoundToInt(lightDamage);
		}
		if(darkDamage > 0)
		{
			darkDamage = darkDamage - darkDef;

			if(enemyStats.lightEnemy)
			{
				darkDamage = darkDamage * enemyStats.lightResist;
			}
			if(enemyStats.darkEnemy)
			{
				darkDamage = darkDamage / enemyStats.darkResist;
			}
			Mathf.RoundToInt(darkDamage);
		}
		if(fireDamage < 0)
		{
		fireDamage = 0;
		}
		if(iceDamage < 0)
		{
		iceDamage = 0;
		}
		if(waterDamage < 0)
		{
		waterDamage = 0;
		}
		if(lighteningDamage < 0)
		{
		lighteningDamage = 0;
		}
		if(earthDamage < 0)
		{
		earthDamage = 0;
		}
		if(windDamage < 0)
		{
		windDamage = 0;
		}
		if(lightDamage < 0)
		{
		lightDamage = 0;
		}
		if(darkDamage < 0)
		{
		darkDamage = 0;
		}
		Debug.Log("The damage " + this.gameObject + " is taking is - " + fireDamage + " fire - " + iceDamage + " ice - " + waterDamage + " water - " + lighteningDamage + " lightening - " + earthDamage + " earth -" + windDamage + " wind - " + lightDamage + " light - " + darkDamage + " dark ");
		float elementalDamage = fireDamage + iceDamage + waterDamage + lighteningDamage + earthDamage + windDamage + lightDamage + darkDamage;

		//Debug.Log("I'm taking damage " + amount);
		if(elementalDamage <= 0)
		{
			return;
		}
		else
		{
			TakePenDamage(0, 0, 0, elementalDamage);
			elementalDamage = 0;
		}
	}
	//Function to drain HP from nearby enemies
	public void Vampire(float damageVam)
	{
		damVam = damageVam;
		updatedHealth -= damVam;
		healthBar.fillAmount = updatedHealth / maxHealth;
		//Debug.Log("I have taken " + damVam + " damage!");
		if (updatedHealth <= 0)
		{
			Die();
		}
	}
	//Virus is a DoT that can be spread to near/passing enemies, dealt by specialist towers and spells
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
	//Doom is a fatal attack once the counter has ended; can be undone(healed) by enemies with the "immune" spell
	public void Doom(float doo)
	{
		doom = true;
		countdownDoom = doo;
	}
	//Turns the enemies into chickens; these are all set with low HP and can't be undone
	public void Chicken()
	{
		isChicken = true;
		silence = true;
		immune = false;
	}
	//Stops enemies from casting spells
	public void Silence(float sil)
	{
		countdownSilence = sil;
		silence = true;
	}
	//Makes enemies immune and heals them from all debuffs
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
	//Slows the enemy (default debuff of Laser towers)
	public void Slow (float slo, float slt)
	{
		slowEnemy = true;
		slowSpeed = slo;
		countdownSlow = slt;
	}
	//Stops enemies from moving
	public void Stop (float stp)
	{
		stopEnemy = true;
		countdownStop = stp;
		Silence(stp);
	}
	//Does things like stop walking when casting spells etc...
	public void Casting (float cst)
	{
		castingEnemy = true;
		countdownCasting = cst;
	}
	//Scares enemies into running backwards (towards the start point)
	public void Fear (float fea)
	{
		fearEnemy = true;
		countdownFear = fea;
	}
	//DoT effect that can't spread
	public void Poison (float psnS, float psnT)
	{
		poisonEnemy = true;
		magicalStrengthPoison = psnS;
		countdownPoison = 1;
		poisonInterval = psnT;
	}
	//Buff to speed up enemies
	public void Speed (float spdB, float spdD)
	{
		//Debug.Log("I'm in the speed loop");
		speedEnemy = true;
		bonusSpeed = spdB;
		countdownSpeed = spdD;
	}
	//Buff to make enemies fly - to avoid damage from ground towers
	public void Flying (float flyT)
	{
		enemyStats.isHovercraft = true;
		imFlying = flyT;
	}
	//I "should" split this out to it's own class but this function is used to "duplicate" the unit without the enemyspell class
	public IEnumerator Spawn()
	{
		for (int i = 0; i < enemyStats.dropAmount; i++)
				{
					//Debug.Log("Spawn now! " + enemyStats.dropEnemy + " is chosen");
					// Added 0.1 to stop the enemy from moving before it has completed the spawns
					Stop(enemyStats.dropSpeed + 0.1f);
					GameObject enemy = Instantiate(enemyStats.dropEnemy, transform.position, Quaternion.identity);
					enemy.GetComponent<Enemy>().fromDropship = true;
					enemy.GetComponent<EnemyMovement>().wavepointIndex = enemyMovement.wavepointIndex;
					enemy.GetComponent<EnemyMovement>().remainingPathDistance = enemyMovement.remainingPathDistance;
					++WaveSpawner.EnemiesAlive;
					yield return new WaitForSeconds(enemyStats.dropSpeed);
				}
	}
	//Used for bosses as they generally won't need the enemyspell class but they will want to summon enemies that aren't a boss/itself
	public IEnumerator SummonNow()
	{
		// Adding a wait so that the casting animation can play
		yield return new WaitForSeconds(1.5f);
		// This line generates a number and then summons the enemy relating to that number from the pool assigned in the inspector
		int summoningIndex = Random.Range(0, enemySpells.summoningPool.Length);
		//Debug.Log("I'm in the summonloop with index: " + summoningIndex);
		for (int i = 0; i < enemySpells.summonAmount; i++)
			{
				GameObject enemy = Instantiate(enemySpells.summoningPool[summoningIndex], transform.position, Quaternion.identity);
				enemy.GetComponent<Enemy>().fromDropship = true;
				enemy.GetComponent<EnemyMovement>().wavepointIndex = enemyMovement.wavepointIndex;
				if(enemyMovement.endPath)
				{
					enemy.GetComponent<EnemyMovement>().endPath = enemyMovement.endPath;				
				}
				++WaveSpawner.EnemiesAlive;
				yield return new WaitForSeconds(1.0f);
			}
	}
	//Used by enemies with buffs/vampire(debuffs?)
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
	//If enemy dies
	public void Die ()
	{
		if(isDead)
		{
			return;
		}

		anim.SetBool("Death", true);
		anim.SetBool("Move", false);
		this.gameObject.tag = "Fallen";
		speed = 0f;

		if(!noDrop)
		{
			float roundedResult = Mathf.Round(goldBonus / 2) * 2;
			roundedResult += EnemyStats.Worth;
			PlayerStats.Money += roundedResult;

			float roundedResult2 = Mathf.Round(pointsBonus / 2) * 2;
			roundedResult2 += EnemyStats.Points;
			PlayerStats.Points += roundedResult2;
		}
		//Debug.Log("Gold bonus: " + goldBonus + " & Enemy worth: " + roundedResult);

		//GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		//Destroy(effect, 5f);

		if (isBoss)
		{
			//Debug.Log("Bosses alive = " + WaveSpawner.BossAlive);
			--WaveSpawner.BossAlive;
		} 
		if (isMiniBoss)
		{
			//Debug.Log("MiniB alive = " + WaveSpawner.BossAlive);
			--WaveSpawner.BossAlive;
		}
		else
		{
			//Debug.Log("Enemies alive = " + WaveSpawner.EnemiesAlive);
			if(WaveSpawner.EnemiesAlive < 0)
			{
				WaveSpawner.EnemiesAlive = 0;
			}else
			{
				--WaveSpawner.EnemiesAlive;
			}
		}
		isDead = true;
		Destroy(gameObject, 3);
	}

}
