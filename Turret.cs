using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Turret : MonoBehaviour {

	public AudioSource audioSource;
	public AudioClip[] audioClipArray;

	public Targeting targeting;
	public TurretDots dots;
	public TurretBuffs buffs;
	public Transform target;
	public Enemy targetEnemy;
	public Turret targetTurret;
	public Turret targetedTurret;
	public Base targetBase;

	//Most of the below settings are self explanitory

	[Header("Unity Setup Fields")]

	private string enemyTag = "Enemy";
	private string baseTag = "Base";
	private string turretTag = "Turret";

	public Transform partToRotate;
	public float turnSpeed = 10f;

	public Transform firePoint;
	
	[Header("Upgrades from scene > rng/dmg/mag")]
	//rangeBonus, damageBonus, magDamageBonus (ready for fireBonus etc);
	public int[] nodeBonuses = { 0, 0, 0 };
	public bool isUpgradedByNode = false;
	public GameObject buffsUI;
	public GameObject rangeUI;
	public GameObject buffAtkSpdUI;
	public GameObject debuffAtkSpdUI;

	[Header("General")]
	public float startHealthPoints = 4;
	public float healthPoints;
	public GameObject healthUI;
	public Text healthText;
	public bool immune;
	public bool buffingHide;

	public float range = 15f;
	public bool nearestEnemy, furthestEnemy, closestToStart, closestToEnd, healBase;
	private int startLives;
	[Header("Animations")]
	AnimatorClipInfo[] m_CurrentClipInfo;
    float m_CurrentClipLength;
	public ParticleSystem[] castList;
	
	[Header("Healing")]
	public bool canHeal;
	public float healRate;
	public float startHealRate;
	public GameObject healPrefab;

	[Header("Use Bullets")]

	public bool useBullets = false;
	public GameObject bulletPrefab;
	public float fireRate; 
	public float startFireRate;
	public float fireCountdown = 0f;
	private bool doShoot, canFire = false;

	// [Header("Use Laser")]
	// public bool useLaser = false;
	// public int physicalDamageOverTime = 0;
	// public int magicDamageOverTime = 0;
	// public float slowAmount = .5f;
	// public bool overHeat = false;
	// public float minHeat = 0f;
	// public float maxHeat = 2f;

	// public LineRenderer lineRenderer;
	// public ParticleSystem impactEffect;
	// public Light impactLight;

	[Header("Use AoE")]
	public bool useAoe = false;
	public float AoERate = 3f;
	private float countdownAoe = .5f;

	//public int physicalDamageAoe = 1;
	public int magicDamageAoe = 1;

	public int bluntDamageAoE = 0;
	public int slashingDamageAoE = 0;
	public int piercingDamageAoE = 0;

	// public int damageEarthAoE = 0;
	// public int damageLighteningAoE = 0;
	// public int damageWaterAoE = 0;
	// public int damageFireAoE = 0;
	// public int damageIceAoE = 0;
	// public int damageWindAoE = 0;

	public ParticleSystem aoeImpactEffect;
	//AoE (testing #52) = #27 Expose of Darkness OR #44 Purify water OR #52 Lightening field #33 Demoic sphere OR #43Darkdraw OR
	public Light aoeImpactLight;

	[Header("Added Effects")]
	//Note that these are also added on bullets and shouldn't be duplicated - changed to be tower buffs and leaving debuffs to bullet class
	// public float stunTime = 0f;
	// public float stunChance = 0f;
	// public float silenceTime = 0f;
	// public float silenceChance = 0f;
	// public float virusChance = 0f;
	// public float virusTime = 0f;
	// public float virusDamage = 0f;
	// public float virusRange = 0f;

	private float pathCompare, pathRemain = 10000;

	private Animator anim;
	public float animCooldown = 0f;
	private Camera cam;

	public bool debuffSpeedTower, buffSpeedTower;
	private bool debuffSpeedTrigger, buffSpeedTrigger;
	public float debuffSpeed, countdownDebuffSpeed, buffSpeed, countdownBuffSpeed;
	private float healBonus;
	private string buffHide = "BuffHide";
	
	void Start () {
		
		this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		healthPoints = startHealthPoints;

		cam = CameraController.PlayerCam;
		anim = gameObject.GetComponentInChildren<Animator>();
		targeting = gameObject.GetComponent<Targeting>();
		dots = gameObject.GetComponent<TurretDots>();
		buffs = gameObject.GetComponent<TurretBuffs>();

		startFireRate = fireRate;
		startHealRate = healRate;
		audioSource.clip = audioClipArray[0];
		if(isUpgradedByNode)
		{
			//buffsUI.SetActive(true);
			if(nodeBonuses[0] > 0)
			{
				rangeUI.SetActive(true);
				range = range + nodeBonuses[0];
			}
		}
	}
	void Update () {
		
		buffsUI.transform.LookAt(cam.transform);
		if(buffingHide)
		{
			buffs.BuffEffect(buffHide, 6, 0);
		}
		if (healthPoints <= 0)
		{
			healthText.text = "My time here is done..";
			anim.SetBool("Death", true);
			anim.SetBool("Idle", false);
			m_CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
			m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
			//Debug.Log("The clip length is - " + m_CurrentClipLength + " .");
			//isCasting = m_CurrentClipLength * 3;
			//enemy.Casting(m_CurrentClipLength * 3);
			foreach (ParticleSystem casting in castList)
				{
				if(!casting.isPlaying)
						{
						casting.Play();
						}
				}

			Destroy(gameObject, 1.5f);
			return;
		}
		else if (healthPoints > 0 & healthPoints < startHealthPoints)
		{
			healthUI.SetActive(true);
			healthText.text = healthPoints.ToString() + " / " + startHealthPoints + " health";
		}
		if(healthPoints == startHealthPoints)
		{
			healthUI.SetActive(false);
		}
		if (!isUpgradedByNode)
		{

		}
		else
		{

		}
		if(fireRate > startFireRate || healRate > startHealRate)
		{
			buffAtkSpdUI.SetActive(true);
		}
		else if(fireRate < startFireRate || healRate < startHealRate)
		{
			debuffAtkSpdUI.SetActive(true);
		}
		else
		{
			debuffAtkSpdUI.SetActive(false);
			buffAtkSpdUI.SetActive(false);
		}
		if(nearestEnemy)
		{
			targeting.TargetEnemy(range);
		}
		if(furthestEnemy)
		{
            targeting.FurthestTarget(range);
		}
        if (closestToStart)
        {
            targeting.ClosestToStartTarget(range);
        }
        if (closestToEnd)
		{
			targeting.ClosestToEndTarget(range);
			// ClosestToEndTarget();
		}
		if (healBase)
		{
			targeting.TargetFriendly(range);
		}
		if(target)
		{
			LockOnTarget();

			if (useAoe)
			{
				if (countdownAoe <= 0f)
				{
					AoE();
					countdownAoe += AoERate;
				}
				countdownAoe -= Time.deltaTime;
				return;
			}
			else
			{
				if (fireCountdown <= 0f)
				{
					anim.SetBool("Attack1", true);
					anim.SetBool("Idle", false);
					animCooldown = 0.5f;
					doShoot = true;

					fireCountdown = 100f / fireRate;
					if(healBase)
					{
						fireCountdown = 100f / healRate;
						Debug.Log("Heal reset");
					}
				}
				if (animCooldown >= 0f)
				{
					anim.SetBool("Attack1",false);
					anim.SetBool("Idle", true);
					
					if (doShoot == true)
					{
						if(healBase)
						{
							Heal();
							Debug.Log("I've shot and healed");
						}
						else
						{
							Shoot();					
						}
						doShoot = false;
					}
					animCooldown -= Time.deltaTime;
				}
			}
		}
		else
		{
			return;
		}

		fireCountdown -= Time.deltaTime;
	}
	//Buttons for the UI
	public void ClosestToEnd()
	{
		nearestEnemy = false;
		furthestEnemy = false;
		closestToStart = false;
		healBase = false;
		closestToEnd = true;
	}
    public void ClosestToStart()
	{
		nearestEnemy = false;
		furthestEnemy = false;
		closestToStart = true;
		healBase = false;
		closestToEnd = false;
	}
	public void NearestToTurret()
	{
		nearestEnemy = true;
		furthestEnemy = false;
		closestToStart = false;
		healBase = false;
		closestToEnd = false;
	}
	public void FarthestFromTurret()
	{
		nearestEnemy = false;
		furthestEnemy = true;
		closestToStart = false;
		healBase = false;
		closestToEnd = false;
	}
	public void HealTheBase()
	{
		nearestEnemy = false;
		furthestEnemy = false;
		closestToStart = false;
		healBase = true;
		closestToEnd = false;
	}

	// {
	// 	//Debug.Log("I'm trying to target");
	// 	GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
	// 	float closestToEnd = Mathf.Infinity;
	// 	GameObject closestToEndEnemy = null;
	// 	foreach (GameObject enemy in enemies)
	// 	{
	// 		float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

	// 		Enemy pathRemaining = enemy.GetComponent<Enemy>();
	// 		pathRemain = pathRemaining.remainingPathDist;

	// 		if (distanceToEnemy <= range)
	// 		{
	// 			if (pathRemain <= pathCompare)
	// 			{
	// 				//Debug.Log("I'm in the path loop.  Remain = " + pathRemain + ".  Compare = " + pathCompare);
	// 				pathCompare = pathRemain;
	// 				closestToEnd = distanceToEnemy;
	// 				closestToEndEnemy = enemy;
	// 			}
	// 		}
	// 	}
	// 	if (closestToEndEnemy != null && closestToEnd <= range)
	// 	{
	// 		Debug.Log("I'm targetting");
	// 		target = closestToEndEnemy.transform;
	// 		targetEnemy = closestToEndEnemy.GetComponent<Enemy>();
	// 	}
	// 	else
	// 	{
	// 		//Debug.Log("Clearing out");
	// 		target = null;
	// 		pathCompare = 10000;
	// 		pathRemain = 10000;
	// 	}
	// 	enemies = null;
	// }
	// //Target the base to heal it and recover "lives"
	void HealingBase ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(baseTag);
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
			LockOnTarget();
		} else
		{
			target = null;
		}
		enemies = null;
	}
	void HealingTurret ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(turretTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

			targetedTurret = enemy.GetComponent<Turret>();

			if (distanceToEnemy < shortestDistance && targetedTurret.healthPoints < targetedTurret.startHealthPoints)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
			targetedTurret = null;
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy.GetComponent<Enemy>();
			targetedTurret = nearestEnemy.GetComponent<Turret>();
			LockOnTarget();
		} else
		{
			nearestEnemy = null;
			//turgetedTurret = null;
			target = null;
		}
		enemies = null;
	}
	public void BuffSpeed (float bSpd, float bCountdown)
	{
		//Debug.Log("I'm in the speed loop");
		buffSpeedTower = true;
		buffSpeedTrigger = true;
		buffSpeed = bSpd;
		countdownBuffSpeed = bCountdown;
	}
	void LockOnTarget ()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	// void Laser ()
	// {
	// 	targetEnemy.TakePenDamage(physicalDamageOverTime * Time.deltaTime, magicDamageOverTime * Time.deltaTime);
	// 	//Debug.Log("I'm taking " + physicalDamageOverTime * Time.deltaTime + " physical damage & " + magicDamageOverTime * Time.deltaTime + " magic damage.");
	// 	targetEnemy.Slow(slowAmount, 0.1f);

	// 	if (!lineRenderer.enabled)
	// 	{
	// 		lineRenderer.enabled = true;
	// 		impactEffect.Play();
	// 		impactLight.enabled = true;
	// 	}

	// 	lineRenderer.SetPosition(0, firePoint.position);
	// 	lineRenderer.SetPosition(1, target.position);

	// 	Vector3 dir = firePoint.position - target.position;

	// 	impactEffect.transform.position = target.position + dir.normalized;

	// 	impactEffect.transform.rotation = Quaternion.LookRotation(dir);

	// 	minHeat += Time.deltaTime;
	// 		if (minHeat > maxHeat)
	// 		{
	// 			overHeat = true;
	// 			if (lineRenderer.enabled)
	// 				{
	// 					lineRenderer.enabled = false;
	// 					impactEffect.Stop();
	// 					impactLight.enabled = false;
	// 				}
	// 		}
	// }
	void AoE ()
	{
		//Debug.Log("I'm shooting an emeny!");
		aoeImpactEffect.Play();
		Collider[] colliders = Physics.OverlapSphere(transform.position, range);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				// AoEDamage(collider.transform);
			}
		}
		//Vector3 dir = firePoint.position - target.position;
		colliders = null;
	}
	// void AoEDamage (Transform enemy)
	// {
	// 	Enemy e = enemy.GetComponent<Enemy>();
	// 	//EnemyStats stats = enemy.GetComponent<EnemyStats>();

	// 	if (e != null)
	// 	{
	// 		if (e.isFlying)
	// 		{
	// 			//Debug.Log("Woah.. is someone there?");
	// 			return;
	// 		}
	// 		else
	// 		{
	// 			e.TakeDamage(bluntDamageAoE, slashingDamageAoE, piercingDamageAoE, magicDamageAoe);
	// 			if(virusChance > 0)
	// 			{
	// 				var rand = Random.Range(1, 100);

	// 				if (rand < virusChance)
	// 				{
	// 					e.Virus(virusTime, virusRange, virusDamage);
	// 				}
	// 			}
	// 			if (silenceChance > 0)
	// 			{
	// 				var rand = Random.Range(1, 100);

	// 				if (rand < silenceChance)
	// 				{
	// 					e.Silence(silenceTime);
	// 				}
	// 			}
	// 			if (stunChance > 0)
	// 			{
	// 				var rand = Random.Range(1, 100);

	// 				if (rand <= stunChance)
	// 				{
	// 					e.Stop(stunTime);
	// 					//Debug.Log("I'm being stunned");
	// 				}
	// 			}
	// 		}
	// 	}
	// }
	void Shoot ()
	{
		//Debug.Log("FireAwayyyy");
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if (bullet != null)
			bullet.Seek(target);
			audioSource.clip = audioClipArray[0];
			audioSource.Play();	
	}
	void Heal ()
	{
		//Debug.Log("FireAwayyyy");
		GameObject bulletGO = (GameObject)Instantiate(healPrefab, firePoint.position, firePoint.rotation);

		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if (bullet != null)
			bullet.Seek(target);
		
	}
	public void Healing(float healSpell)
	{
		healBonus = healSpell;

		if (healthPoints == startHealthPoints)
		{
			// Debug.Log("Thanks but don't need it, healing was " + healBonus);
		}
		else
		{
			healthPoints += healBonus;
			if (healthPoints > startHealthPoints)
			{
				//If overhealed - adjust HP back to MaxHP
				healthPoints = startHealthPoints;
			}
		}
		healBonus = 0;
	}
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
