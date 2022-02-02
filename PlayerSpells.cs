using UnityEngine;
using System.Collections;

public class PlayerSpells : MonoBehaviour {

	public bool castEMP, castBarrage /*a laser shower*/, castMeteor, castFear, castChicken, castGravity, castSummonSpell = false;
	public float range, counterEMP, counterBarrage, counterMeteor, counterFear, counterChicken, counterGravity, counterSummonSpell = 0;
	[HideInInspector]
	public float damagePhysical, damageMagical, empCount, barrageCount, meteorCount, fearCount, chickenCount, gravityCount, summonSpellCount = 0;

	void Start ()
	{
		empCount = counterEMP;
		barrageCount = counterBarrage;
		meteorCount = counterMeteor;
		fearCount = counterFear;
		chickenCount = counterChicken;
		gravityCount = counterGravity;
		summonSpellCount = counterSummonSpell;
	}
	void OnMouseDown()
	{
		Debug.Log("Spell test");
		if (castEMP)
		{
			CastEMP();
		}
		if (castBarrage)
		{
			CastBarrage();
		}
		if (castMeteor)
		{
			CastMeteor();
		}
		if (castFear)
		{
			CastFear();
		}
		if (castChicken)
		{
			CastChicken();
		}
		if (castGravity)
		{
			CastGravity();
		}
		if (castSummonSpell)
		{
			CastSummonSpell();
		}
	}
	public void SelectCastEMP () //dps with stun
	{
		castEMP = true;
		Debug.Log("EMP Selected");
	}
	public void SelectCastBarrage() //high dps
	{

	}
	public void SelectCastMeteor() //dps with DoT
	{

	}
	public void SelectCastFear() // runs backwards
	{

	}
	public void SelectCastChicken() //turns into chickens (no def)
	{

	}
	public void SelectCastGravity() //high dps (25% HP) but ~25% chance to happen
	{

	}
	public void SelectCastSummonSpell() //summon basic mobs
	{

	}
	public void SelectCastSummonSpellAdvanced() //summon from a pool of mobs
	{

	}
	public void CastEMP () //dps with stun
	{
		damagePhysical = 10;
		damageMagical = 30;
		range = 20f;
		AoE(range);
		Debug.Log("I'm casting EMP, range = " + range);
		castEMP = false;
	}
	public void CastBarrage() //high dps
	{

	}
	public void CastMeteor() //dps with DoT
	{

	}
	public void CastFear() // runs backwards
	{

	}
	public void CastChicken() //turns into chickens (no def)
	{

	}
	public void CastGravity() //high dps (25% HP) but ~25% chance to happen
	{

	}
	public void CastSummonSpell() //summon basic mobs
	{

	}
	public void CastSummonSpellAdvanced() //summon from a pool of mobs
	{

	}
	void AoE (float AoERange)
	{
		Debug.Log("AoE Cast test");
		range = AoERange;
		if (Input.touchCount > 0)
		 {
			Touch t = Input.GetTouch(0);
			{
				 if(t.phase == TouchPhase.Began)
				 {
					Vector3 pos = t.position;
					if (GetComponent<Collider>().gameObject.CompareTag("Enemy"))
					 {
							Collider[] colliders = Physics.OverlapSphere(pos, range);
							foreach (Collider collider in colliders)
							{
								if (collider.tag == "Enemy")
								{
									AoEDamage(collider.transform);
								}
							}
						}
					}
				}
			}
		//Vector3 dir = firePoint.position - target.position;
	}
	void AoEDamage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();

		e.TakeDamage(damagePhysical, damageMagical);

		if(castEMP)
		{
			e.Stop(20f);
		}

		damagePhysical = 0;
		damageMagical = 0;
	}
}
