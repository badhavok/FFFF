using UnityEngine;

public class Shop : MonoBehaviour {

	public GameObject menuUI;
	public GameObject turretUI;
	public GameObject buildingUI;
	[HideInInspector]
	public bool menuOpen, shopOpen, buildingOpen = false;

	BuildManager buildManager;

	public TurretBlueprint standardTurret;
	public TurretBlueprint missileLauncher;
	public TurretBlueprint laserBeamer;
	public TurretBlueprint aoeTurret;
	public BuildingBlueprint goldBuilding;
	public BuildingBlueprint advanceTower;

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
	}
	public void OpenBuildingMenu()
	{
		menuUI.SetActive(false);
		turretUI.SetActive(false);
		buildingUI.SetActive(true);
	}
	public void BackToMain()
	{
		menuUI.SetActive(true);
		turretUI.SetActive(false);
		buildingUI.SetActive(false);
		buildManager.SelectTurretToBuild(null);
		buildManager.SelectBuildingToBuild(null);;
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
	public void SelectAdvanceTower()
	{
		Debug.Log("Advance Tower Selected");
		buildManager.SelectBuildingToBuild(advanceTower);
	}
}
