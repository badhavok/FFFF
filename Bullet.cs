using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float speed = 70f;

	public int damagePhysical = 50;
	public int damageMagical = 0;
	public int damageFire = 0;
	public int damageWater = 0;
	public int damageLightening = 0;
	public int damageEarth = 0;

	[Header ("AoE/Explosion")]
	public float explosionRadius = 0f;
	public GameObject impactEffect;

	[Header ("Use Poison")]
	public float poisonChance = 0f;
	public int poisonStrength = 0;
	public float poisonTime = 0f;

	public float fearChance, fearTime = 0f;

	public float gravityChance = 0f;

	public float chickenChance = 0f;

	public float doomChance, doomTime = 0f;

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
		GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(effectIns, 5f);

		if (explosionRadius > 0f)
		{
			Explode();
		} else
		{
			Damage(target);
		}

		Destroy(gameObject);
	}

	void Explode ()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.transform);
			}
		}
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
					Debug.Log("Gravity damage is: " + e.maxHealth / 8 + " phys + " + e.maxHealth / 8 + " mag");
					e.TakeDamage(e.maxHealth / 8, e.maxHealth / 8);
				}
				return;
			}
			e.TakeDamage(damagePhysical, damageMagical);

			if(chickenChance > 0)
			{
				var rand = Random.Range(1,100);

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
