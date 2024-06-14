using UnityEngine;

public class Bullet : MonoBehaviour {

	public Transform target;

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
	public float debuffDefChance = 0f;
	public float countdownDebuffDef = 0f;
	public int debuffDefSlash, debuffDefPierce, debuffDefBlunt, debuffDefMag = 0;
	private string DeBuffSlashDef = "DeBuffSlash";
	private string DeBuffBluntDef = "DeBuffBlunt";
	private string DeBuffPierceDef = "DeBuffPierce";
	private string DeBuffMagDef = "DeBuffMag";
	[Header ("Use Poison")]
	private string poison = "Poison";
	public float poisonChance = 0f;
	public int poisonStrength = 0;
	public float poisonTime = 0f;
	[Header ("Use Slow")]
	private string slow = "Slow";
	public float slowChance = 0f;
	public float slowSpeed = 0f;
	public float slowTime = 0f;
	[Header ("Use Stop")]
	private string stop = "Stop";
	public float stopChance = 0f;
	public float stopTime = 0f;
	[Header ("Use Silence")]
	private string silence = "Silence";
	public float silenceChance =0f;
	public float silenceTime = 0f;

	[Header ("Use Fear")]
	private string fear = "Fear";
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
		// Debug.Log("Target is " + target);
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

		if (dir.magnitude <= distanceThisFrame && target != null)
		{
			HitTarget();
			// Debug.Log("Just ran HitTarget");
			target = null;
			// Debug.Log("I'm nulling target - " + target);
			return;
		}
		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);
	}

	void HitTarget ()
	{
		// Debug.Log("Hit Target");
		if (explosionRadius > 0f)
		{
			Explode();
		} 
		else
		{
			Damage(target);
		}
		if(impactEffect)
		{
			ImpactEffect();
		}
		target = null;
		Destroy(gameObject);
		// Debug.Log("Destroy bullet object- " + gameObject);
	}
	void ImpactEffect()
	{
		GameObject effectIns = Instantiate(impactEffect, target.gameObject.transform);//, transform.rotation);
		Destroy(effectIns, 1f);
			
	}
	void Explode ()
	{
		Collider[] colliders = Physics.OverlapSphere(target.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.transform);
				// Debug.Log($"{gameObject.name} is dealing explosion damage to {GetComponent<Collider>().gameObject.name}, which is {Vector3.Distance(GetComponent<Collider>().transform.position, target.position)} units from target position {target.position}, explosion Radius is {explosionRadius}");
			}
		}
		colliders = null;
	}

	void Damage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();
		EnemyDots d = enemy.GetComponent<EnemyDots>();
		Debug.Log("Damage target - " + e);
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
			if(debuffDefChance > 0 && !e.buffSlashDef)
			{
				var rand = Random.Range(1,100);
				//Debug.Log("I'm in the defence chance");
				if (rand < debuffDefChance)
				{
					d.DotEffect(DeBuffSlashDef, countdownDebuffDef, debuffDefSlash);
					d.DotEffect(DeBuffBluntDef, countdownDebuffDef, debuffDefBlunt);
					d.DotEffect(DeBuffPierceDef, countdownDebuffDef, debuffDefPierce);
					d.DotEffect(DeBuffMagDef, countdownDebuffDef, debuffDefMag);
				}
			}

			e.TakeDamage(damageBlunt, damagePiercing, damageSlashing, damageMagical);

			//Silence debuff
			if(silenceChance > 0)
			{
				var rand = Random.Range(1,100);
				//Debug.Log("I'm going to silence you!");
				if (rand < silenceChance)
				{
					d.DotEffect(silence, silenceTime, 0f);
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
						d.DotEffect(slow, slowTime, slowSpeed);
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
						d.DotEffect(stop, stopTime, 0f);
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
						d.DotEffect(fear, fearTime, 0);
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
				var rand = Random.Range(1,100);

				if (rand < poisonChance)
				{
					// Debug.Log("MUAHAHAA... Poison!  Cast on > " + e);
					d.DotEffect(poison, poisonTime, poisonStrength);
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
