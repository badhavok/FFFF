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
	void CastEMP ()
	{

	}
	void CastBarrage()
	{

	}
	void CastMeteor()
	{
		
	}
	void CastFear()
	{

	}
	void CastChicken()
	{

	}
	void CastGravity()
	{

	}
	void CastSummonSpell()
	{

	}
}
