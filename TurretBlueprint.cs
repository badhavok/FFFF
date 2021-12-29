using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurretBlueprint {

	public GameObject prefab;
	public int cost;

	public GameObject prefabUpgrade;
	public int upgradeCost;

	public GameObject prefabUpgradeTwo;
	public int upgradeCostTwo;

	public GameObject prefabDps;
	public int upgradeCostDps;
	public GameObject prefabDpsOne;
	public int upgradeCostDpsOne;
	public GameObject prefabDpsTwo;
	public int upgradeCostDpsTwo;

	public GameObject prefabSup;
	public int upgradeCostSup;
	public GameObject prefabSupOne;
	public int upgradeCostSupOne;
	public GameObject prefabSupTwo;
	public int upgradeCostSupTwo;

	public int GetSellAmount ()
	{
		return cost / 2;
	}
  public int GetUpgradeValue ()
	{
		return upgradeCost / 2;
	}
	public int GetUpgradeValueTwo ()
	{
		return upgradeCostTwo / 2;
	}
	public int GetUpgradeValueDps ()
	{
		return upgradeCostDps / 2;
	}
	public int GetUpgradeValueDpsOne ()
	{
		return upgradeCostDpsOne / 2;
	}
	public int GetUpgradeValueDpsTwo ()
	{
		return upgradeCostDpsTwo / 2;
	}
	public int GetUpgradeValueSup ()
	{
		return upgradeCostSup / 2;
	}
	public int GetUpgradeValueSupOne ()
	{
		return upgradeCostSupOne / 2;
	}
	public int GetUpgradeValueSupTwo ()
	{
		return upgradeCostSupTwo / 2;
	}
}
