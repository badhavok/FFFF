using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float speed = 70f;

	//public int damagePhysical = 5;
	public int damageMagical = 0;
	public int damageBlunt = 0;
	public int damageSlashing = 0;
	public int damagePiercing = 0;
	public int damageEarth = 0;
	public int damageLightening = 0;
	public int damageWater = 0;
	public int damageFire = 0;
	public int damageIce = 0;
	public int damageWind = 0;
	public int damageLight = 0;
	public int damageDark = 0;

	[Header ("AoE/Explosion")]
	public float explosionRadius = 0f;
	public GameObject impactEffect;
	[Header ("Use Defence Down")]
	public float defenceChance = 0f;
	public float countdownDebuffDef = 0f;
	public int debuffDefSlash, debuffDefPierce, debuffDefBlunt, debuffDefMag = 0;

	[Header ("Use Poison")]
	public float poisonChance = 0f;
	public int poisonStrength = 0;
	public float poisonTime = 0f;
	[Header ("Use Slow")]
	public float slowChance = 0f;
	public float slowSpeed = 0f;
	public float slowTime = 0f;
	[Header ("Use Stop")]
	public float stopChance = 0f;
	public float stopTime = 0f;
	[Header ("Use Silence")]
	public float silenceChance =0f;
	public float silenceTime = 0f;

	[Header ("Use Fear")]
	public float fearChance = 0f;
	public float fearTime = 0f;
	[Header ("Use Virus")]
	public float virusChance = 0f;
	public float virusTime = 0f;
	public float virusRange = 0f;
	public float virusDamage = 0f;

	[Header ("Use Gravity")]
	public float gravityChance = 0f;

	[Header ("Use Chicken")]
	public float chickenChance = 0f;

	[Header ("Use Doom")]
	public float doomChance = 0f;
	public float doomTime = 0f;

	void Start()
	{

	}
	//Moves towards enemy
	public void Seek (Transform _target)
	{
		target = _target;
	}

	// Update is called once per frame
	void Update () {

		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}
		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);
	}

	void HitTarget ()
	{
		//GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		//Destroy(effectIns, 5f);

		if (explosionRadius > 0f)
		{
			Explode();
		} 
		else
		{
			Damage(target);
		}
		target = null;
		Destroy(gameObject);
	}

	void Explode ()
	{
		Collider[] colliders = Physics.OverlapSphere(target.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.transform);
				//Debug.Log($"{gameObject.name} is dealing explosion damage to {collider.gameObject.name}, which is {Vector3.Distance(collider.transform.position, target.position)} units from target position {target.position}, explosion Radius is {explosionRadius}");
			}
		}
		colliders = null;
	}

	void Damage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();

		if (e != null)
		{
			// if(gravityChance > 0)
			// {
			// 	var rand = Random.Range(1, 100);

			// 	if (rand < gravityChance)
			// 	{
			// 		//Debug.Log("Gravity damage is: " + e.maxHealth / 8 + " phys + " + e.maxHealth / 8 + " mag");
			// 		e.TakePenDamage(e.maxHealth / 8, e.maxHealth / 8);
			// 	}
			// 	return;
			// }
			
			//Defence debuff
			if(defenceChance > 0 && !e.buffSlashDef)
			{
				var rand = Random.Range(1,100);
				Debug.Log("I'm in the defence chance");
				if (rand < defenceChance)
				{
					e.BuffSlashDef(debuffDefSlash, countdownDebuffDef);
					e.BuffBluntDef(debuffDefBlunt, countdownDebuffDef);
					e.BuffPierceDef(debuffDefPierce, countdownDebuffDef);
					e.BuffMagDef(debuffDefMag, countdownDebuffDef);
				}
			}

			e.TakeDamage(damageBlunt, damagePiercing, damageSlashing, damageMagical);

			//Silence debuff
			if(silenceChance > 0)
			{
				if(!e.silence)
				{
					var rand = Random.Range(1,100);
					//Debug.Log("I'm going to silence you!");
					if (rand < silenceChance)
					{
						e.Silence(silenceTime);
					}
				}
			}
			//Slow debuff
			if(slowChance > 0)
			{
				if(!e.slowEnemy)
				{
					var rand = Random.Range(1,100);
					//Debug.Log("I'm going to slow you!");
					if (rand < slowChance)
					{
						e.Slow(slowSpeed, slowTime);
					}
				}
			}
			//Stop debuff
			if(stopChance > 0)
			{
				if(!e.stopEnemy)
				{
					var rand = Random.Range(1,100);
					//Debug.Log("I'm going to stop you!");
					if (rand < stopChance)
					{
						e.Stop(stopTime);
					}
				}
			}
			//Turns them into a chicken
			if(chickenChance > 0)
			{
				if(!e.isChicken)
				{
					var rand = Random.Range(1,100);
					//Debug.Log("I'm going to turn you into a chicken!");
					if (rand < chickenChance)
					{
						e.isChicken = true;
					}
				}
			}
			//Doom debuff
			if(doomChance > 0)
			{
				if(!e.doom)
				{
					var rand = Random.Range(1, 100);

					if (rand < doomChance)
					{
						e.Doom(doomTime);
					}
				}
			}
			//Fear debuff
			if (fearChance > 0)
			{
				if(!e.fearEnemy)
				{
					var rand = Random.Range(1,100);

					if (rand < fearChance)
					{
						e.Fear(fearTime);
					}
				}
			}
			//Virus DoT
			if(virusChance > 0)
			{
				if(!e.virus)
				{
					var rand = Random.Range(1,100);
					
					if (rand < virusChance)
					{
						e.Virus(virusTime, virusRange, virusDamage);
					}
				}
			}
			//Poison DoT
			if (poisonChance > 0)
			{
				if(!e.poisonEnemy)
				{
					var rand = Random.Range(1,100);

					if (rand < poisonChance)
					{
						//Debug.Log("MUAHAHAA... Poison!");
						e.Poison(poisonStrength, poisonTime);
					}
				}
				else
				{
					//Debug.Log("Already poisoned");
				}
			}
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
