using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuildingBlueprint {

	public GameObject prefabB;
	public int costB;

	public GameObject prefabBUpgrade;
	public int upgradeBCost;

	public GameObject prefabBUpgradeTwo;
	public int upgradeBCostTwo;

	public int GetBSellAmount ()
	{
		return costB / 2;
	}
  public int GetUpgradeBValue ()
	{
		return upgradeBCost / 2;
	}
	public int GetUpgradeBValueTwo ()
	{
		return upgradeBCostTwo / 2;
	}
}
