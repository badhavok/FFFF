using UnityEngine;
using System.Collections;

public class PlayerSpells : MonoBehaviour {

	public bool castEMP, castBarrage /*a laser shower*/, castMeteor, castFear, castChicken, castGravity, castSummonSpell = false;
	public float counterEMP, counterBarrage, counterMeteor, counterFear, counterChicken, counterGravity, counterSummonSpell = 0;
	[HideInInspector]
	public float empCount, barrageCount, meteorCount, fearCount, chickenCount, gravityCount, summonSpellCount = 0;

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
	void Update()
	{
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
		Debug.Log("EMP Selected");
		castEMP = true;
	}
	void SelectCastBarrage() //high dps
	{

	}
	void SelectCastMeteor() //dps with DoT
	{

	}
	void SelectCastFear() // runs backwards
	{

	}
	void SelectCastChicken() //turns into chickens (no def)
	{

	}
	void SelectCastGravity() //high dps (25% HP) but ~25% chance to happen
	{

	}
	void SelectCastSummonSpell() //summon basic mobs
	{

	}
	void SelectCastSummonSpellAdvanced() //summon from a pool of mobs
	{

	}
	void CastEMP () //dps with stun
	{
		Debug.Log("I'm casting EMP");
	}
	void CastBarrage() //high dps
	{

	}
	void CastMeteor() //dps with DoT
	{

	}
	void CastFear() // runs backwards
	{

	}
	void CastChicken() //turns into chickens (no def)
	{

	}
	void CastGravity() //high dps (25% HP) but ~25% chance to happen
	{

	}
	void CastSummonSpell() //summon basic mobs
	{

	}
	void CastSummonSpellAdvanced() //summon from a pool of mobs
	{

	}
}
