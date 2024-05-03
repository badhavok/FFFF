using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurretBlueprint {

	// Cost to build
	public GameObject prefab;
	public int cost;
	// Cost for the first upgrade
	public GameObject prefabUpgrade;
	public int upgradeCost;
	// Cost for the second upgrade
	public GameObject prefabUpgradeTwo;
	public int upgradeCostTwo;
	/* // Cost to chose DPS
	public GameObject prefabDps;
	public int upgradeCostDps;
	public int upgradeSpecialDps;

	// Cost to upgrade DPS
	public GameObject prefabDpsOne;
	public int upgradeCostDpsOne;
	public int upgradeSpecialDpsOne;
	public GameObject prefabDpsTwo;
	public int upgradeCostDpsTwo;
	public int upgradeSpecialDpsTwo;

	// Cost to chose SUP
	public GameObject prefabSup;
	public int upgradeCostSup;
	public int upgradeSpecialSup;
 */
	// Cost to upgrade SUP
	public GameObject prefabSupOne;
	public int upgradeCostSupOne;
	public int upgradeSpecialSupOne;
	public GameObject prefabSupTwo;
	public int upgradeCostSupTwo;
	public int upgradeSpecialSupTwo;

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
	/* public int GetUpgradeValueDps ()
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
	} */
}
