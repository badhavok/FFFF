using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float speed = 70f;

	public int damagePhysical = 5;
	public int damageMagical = 0;

	public int damageEarth = 0;
	public int damageLightening = 0;
	public int damageWater = 0;
	public int damageFire = 0;
	public int damageIce = 0;
	public int damageWind = 0;


	[Header ("AoE/Explosion")]
	public float explosionRadius = 0f;
	public GameObject impactEffect;

	[Header ("Use Poison")]
	public float poisonChance = 0f;
	public int poisonStrength = 0;
	public float poisonTime = 0f;

	[Header ("Use Fear")]
	public float fearChance, fearTime = 0f;

	[Header ("Use Gravity")]
	public float gravityChance = 0f;

	[Header ("Use Chicken")]
	public float chickenChance = 0f;

	[Header ("Use Doom")]
	public float doomChance, doomTime = 0f;

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
			if(gravityChance > 0)
			{
				var rand = Random.Range(1, 100);

				if (rand < gravityChance)
				{
					//Debug.Log("Gravity damage is: " + e.maxHealth / 8 + " phys + " + e.maxHealth / 8 + " mag");
					e.TakePenDamage(e.maxHealth / 8, e.maxHealth / 8);
				}
				return;
			}
			e.TakeDamage(damagePhysical, damageMagical);

			if(chickenChance > 0)
			{
				var rand = Random.Range(1,100);
				//Debug.Log("I'm going to turn you into a chicken!");
				if (rand < chickenChance)
				{
					e.isChicken = true;
				}
			}
			if(doomChance > 0)
			{
				var rand = Random.Range(1, 100);

				if (rand < doomChance)
				{
					e.Doom(doomTime);
				}
			}
			if (fearChance > 0)
			{
				var rand = Random.Range(1,100);

				if (rand < fearChance)
				{
					e.Fear(fearTime);
				}
			}
			if (poisonChance > 0)
			{
				var rand = Random.Range(1,100);

				if (rand < poisonChance)
				{
					e.Poison(poisonStrength, poisonTime);
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
