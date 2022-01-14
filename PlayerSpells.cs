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
		Debug.Log("I'm casting EMP");
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
}
