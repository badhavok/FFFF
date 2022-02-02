using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

	public GameObject ui;
	public GameObject uiBuildings;
	public GameObject advanceUI;
	public GameObject advanceUIDPS;
	public GameObject advanceUISUP;

	public Text upgradeCost;
	public Button upgradeButton;

	public bool imDPS, imSUP, imUpgraded, upgradeLevelOne, upgradedLevelTwo, upgradedLevelThree = false;

	public Text sellAmount;

	private Node target;

	BuildManager buildManager;

	public void SetTarget (Node _target)
	{
		target = _target;
		transform.position = target.GetBuildPosition();
		Debug.Log("through loop");
		if(target.isBuilding && !target.isBUpgrade)
		{
			upgradeCost.text = "$" + target.buildingBlueprint.upgradeBCost;
			upgradeButton.interactable = true;
			uiBuildings.SetActive(true);
			Debug.Log("Building");
		}
		if(target.isBUpgrade && !target.isBUpgrade2)
		{
			upgradeCost.text = "$" + target.buildingBlueprint.upgradeBCost;
			upgradeButton.interactable = true;
			uiBuildings.SetActive(true);
			Debug.Log("Building");
		}
		else if (target.isBUpgrade2)
		{
			upgradeCost.text = "DONE";
			upgradeButton.interactable = false;
			sellAmount.text = "$" + target.buildingBlueprint.GetUpgradeBValueTwo();
			ui.SetActive(true);
			Debug.Log("building end");
		}
		else if (target.isBase && !target.isFirstUpgrade)
		{
			upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
			upgradeButton.interactable = true;
			ui.SetActive(true);
			Debug.Log("1");
		}
		else if (target.isFirstUpgrade && !target.isSecondUpgrade)
		{
			upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
			upgradeButton.interactable = true;
			ui.SetActive(true);
			Debug.Log("2");
		}
		else if (target.isSecondUpgrade && !target.isDPS && !target.isSUP)
		{
				Debug.Log("Ready to advance");
				upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
				upgradeButton.interactable = true;
				advanceUI.SetActive(true);
				Debug.Log("3");
		}
		else if (target.isDPS && !target.isDPSOne)
		{
				upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
				upgradeButton.interactable = true;
				advanceUIDPS.SetActive(true);
				Debug.Log("4");
		}
		else if (target.isSUP && !target.isSUPOne)
		{
				upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
				upgradeButton.interactable = true;
				advanceUISUP.SetActive(true);
				Debug.Log("5");
		}
		else
		{
			upgradeCost.text = "DONE";
			upgradeButton.interactable = false;
			sellAmount.text = "$" + target.turretBlueprint.GetUpgradeValueSupTwo();
			ui.SetActive(true);
			Debug.Log("end");
		}
	}
	public void UpgradeBuilding ()
	{
			target.UpgradeBuilding();
			BuildManager.instance.DeselectNode();
	}
	public void Upgrade ()
	{
			target.UpgradeTurret();
			BuildManager.instance.DeselectNode();
	}
	public void UpgradeToDPS ()
	{
			target.UpgradeToDPS();
			BuildManager.instance.DeselectNode();
	}
	public void UpgradeToSup ()
	{
			target.UpgradeToSup();
			BuildManager.instance.DeselectNode();
	}
	public void UpgradeTurretDpsOne ()
	{
			target.UpgradeTurretDpsOne();
			BuildManager.instance.DeselectNode();
	}
	public void UpgradeTurretDpsTwo ()
	{
			target.UpgradeTurretDpsTwo();
			BuildManager.instance.DeselectNode();
	}
	public void UpgradeTurretSupOne ()
	{
			target.UpgradeTurretSupOne();
			BuildManager.instance.DeselectNode();
	}
	public void UpgradeTurretSupTwo ()
	{
			target.UpgradeTurretSupTwo();
			BuildManager.instance.DeselectNode();
	}
	public void Sell ()
	{
		target.SellTurret();
		BuildManager.instance.DeselectNode();
	}
	public void SellBuilding ()
	{
		target.SellBuilding();
		BuildManager.instance.DeselectNode();
	}
	public void Hide ()
	{
		advanceUIDPS.SetActive(false);
		advanceUISUP.SetActive(false);
		advanceUI.SetActive(false);
		uiBuildings.SetActive(false);
		ui.SetActive(false);
	}
}
