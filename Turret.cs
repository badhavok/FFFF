using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private Transform target;
	private Enemy targetEnemy;
	
	[Header("General")]

	public float range = 15f;

	[Header("Use Bullets")]
	
	public bool useBullets = false;
	public GameObject bulletPrefab;
	public float fireRate = 1f;
	private float fireCountdown = 0f;

	[Header("Use Laser")]
	public bool useLaser = false;
	public int physicalDamageOverTime = 0;
	public int magicDamageOverTime = 30;
	public float slowAmount = .5f;
	public bool overHeat = false;
	public float minHeat = 0f;
	public float maxHeat = 2f;

	public LineRenderer lineRenderer;
	public ParticleSystem impactEffect;
	public Light impactLight;
	
	[Header("Use AoE")]
	public bool useAoe = false;
	
	public int physicalDamageAoe = 1;
	public int magicDamageAoe = 1;
	public float countdownAoe = 1f;
	public float stunTime = 2f;
	public float stunChance = 20f;
	
	public ParticleSystem aoeImpactEffect;
	public Light aoeImpactLight;

	[Header("Unity Setup Fields")]

	public string enemyTag = "Enemy";

	public Transform partToRotate;
	public float turnSpeed = 10f;

	public Transform firePoint;

	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
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

	// Update is called once per frame
	void Update () {
		if (target == null)
		{
			if (useLaser)
			{
				if (minHeat < 0)
				{
					minHeat = 0;
				}
				
				minHeat -= Time.deltaTime;
				countdownAoe -= Time.deltaTime;
				fireCountdown -= Time.deltaTime;
				
				if (lineRenderer.enabled)
				{
					lineRenderer.enabled = false;
					impactEffect.Stop();
					impactLight.enabled = false;
				}
			}

			return;
		}

		LockOnTarget();

		if (useLaser)
			{
				if (overHeat)
				{
					minHeat -= Time.deltaTime;
					if (minHeat <= 0f)
						overHeat = false;
				}
				else
					Laser();
			}
			
		else if (useAoe)
		{
			if (countdownAoe <= 0f)
			{
				AoE();
				countdownAoe = 1f / fireRate;
			}
			countdownAoe -= Time.deltaTime;
		}
		else
		{
			if (fireCountdown <= 0f)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}
			fireCountdown -= Time.deltaTime;
		}
	}

	void LockOnTarget ()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void Laser ()
	{
		targetEnemy.TakeDamage(physicalDamageOverTime, magicDamageOverTime * Time.deltaTime);
		targetEnemy.Slow(slowAmount, 0.1f);
		
		if (!lineRenderer.enabled)
		{
			lineRenderer.enabled = true;
			impactEffect.Play();
			impactLight.enabled = true;
		}

		lineRenderer.SetPosition(0, firePoint.position);
		lineRenderer.SetPosition(1, target.position);

		Vector3 dir = firePoint.position - target.position;

		impactEffect.transform.position = target.position + dir.normalized;

		impactEffect.transform.rotation = Quaternion.LookRotation(dir);
		
		minHeat += Time.deltaTime;
			if (minHeat > maxHeat)
			{
				overHeat = true;
				if (lineRenderer.enabled)
					{
						lineRenderer.enabled = false;
						impactEffect.Stop();
						impactLight.enabled = false;
					}
			}
	}
	
	void AoE ()
	{
	
		Collider[] colliders = Physics.OverlapSphere(transform.position, range);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				AoEDamage(collider.transform);
			}
		}
			
		//Vector3 dir = firePoint.position - target.position;
		
	}
	
	void AoEDamage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();
		//EnemyStats stats = enemy.GetComponent<EnemyStats>();

		if (e != null)
		{
			if (e.isFlying)
			{
				Debug.Log("Woah.. what was that");
				return;
			}
			else
			{
				e.TakeDamage(physicalDamageAoe, magicDamageAoe);
			
				if (stunChance > 0)
				{
					var rand = Random.Range(1, 100);
					
					if (rand < stunChance)
					{
						e.Stop(stunTime);
					}
						
				}
			}
		}
	}
	
	void Shoot ()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if (bullet != null)
			bullet.Seek(target);
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
