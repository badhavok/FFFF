using UnityEngine;

public class Shop : MonoBehaviour {

	public GameObject menuUI;
	public GameObject turretUI;
	public GameObject buildingUI;
	public GameObject resourceUI;

	[HideInInspector]
	public bool menuOpen, shopOpen, buildingOpen = false;

	BuildManager buildManager;

	public TurretBlueprint standardTurret, missileLauncher, laserBeamer, aoeTurret;
	public BuildingBlueprint goldBuilding, advanceTower, gasBuilding, liquidBuilding, plasmaBuilding, mineralBuilding, spellTower;

	void Start ()
	{
		buildManager = BuildManager.instance;
		menuUI.SetActive(true);
	}
	public void OpenTurretMenu()
	{
		menuUI.SetActive(false);
		turretUI.SetActive(true);
		buildingUI.SetActive(false);
		resourceUI.SetActive(false);
	}
	public void OpenBuildingMenu()
	{
		menuUI.SetActive(false);
		turretUI.SetActive(false);
		buildingUI.SetActive(true);
		resourceUI.SetActive(false);
	}
	public void OpenResourceMenu()
	{
		menuUI.SetActive(false);
		turretUI.SetActive(false);
		buildingUI.SetActive(false);
		resourceUI.SetActive(true);
	}
	public void BackToMain()
	{
		menuUI.SetActive(true);
		turretUI.SetActive(false);
		buildingUI.SetActive(false);
		resourceUI.SetActive(false);
		buildManager.SelectTurretToBuild(null);
		buildManager.SelectBuildingToBuild(null);
	}
	public void SelectStandardTurret ()
	{
		Debug.Log("Standard Turret Selected");
		buildManager.SelectTurretToBuild(standardTurret);
	}
	public void SelectMissileLauncher()
	{
		Debug.Log("Missile Launcher Selected");
		buildManager.SelectTurretToBuild(missileLauncher);
	}
	public void SelectLaserBeamer()
	{
		Debug.Log("Laser Beamer Selected");
		buildManager.SelectTurretToBuild(laserBeamer);
	}
	public void SelectAoETurret()
	{
		Debug.Log("AoE Turret Selected");
		buildManager.SelectTurretToBuild(aoeTurret);
	}
	public void SelectGoldGenerator()
	{
		Debug.Log("Gold Generator Selected");
		buildManager.SelectBuildingToBuild(goldBuilding);
	}
	public void SelectGasGenerator()
	{
		Debug.Log("Gas Generator Selected");
		buildManager.SelectBuildingToBuild(gasBuilding);
	}
	public void SelectLiquidGenerator()
	{
		Debug.Log("Liquid Generator Selected");
		buildManager.SelectBuildingToBuild(liquidBuilding);
	}
	public void SelectPlasmaGenerator()
	{
		Debug.Log("Plasma Generator Selected");
		buildManager.SelectBuildingToBuild(plasmaBuilding);
	}
	public void SelectMineralGenerator()
	{
		Debug.Log("Mineral Generator Selected");
		buildManager.SelectBuildingToBuild(mineralBuilding);
	}
	public void SelectAdvanceTower()
	{
		if (buildManager.upgradeLevel > 0)
		{
			Debug.Log("Advance Tower exists already");
			buildManager.SelectBuildingToBuild(null);
		}
		else
		{
			Debug.Log("Advance Tower Selected");
			buildManager.SelectBuildingToBuild(advanceTower);
		}
	}
	public void SelectSpellTower()
	{
		if(buildManager.spellCount > 0)
		{
			Debug.Log("Spell Tower exists already");
			buildManager.SelectBuildingToBuild(null);
		}
		else
		{
			Debug.Log("Spell Tower Selected");
			buildManager.SelectBuildingToBuild(spellTower);
		}
	}
}
