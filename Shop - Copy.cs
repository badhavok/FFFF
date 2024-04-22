using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopCopy : MonoBehaviour {

	public GameObject menuUI;
	public GameObject turretUI;
	public GameObject buildingUI;
	public GameObject resourceUI;
	public GameObject tooltipUI;

	public Text tooltipDisplay;
	private Vector3 myPosi;
	public Vector3 positionOffset;

	[HideInInspector]
	public bool menuOpen, shopOpen, buildingOpen = false;

	BuildManager buildManager;

	//Sets which towers are going to be used in the game
	public TurretBlueprint standardTurret, missileLauncher, laserBeamer, aoeTurret;
	//Sets which buildings are going to be used in the game
	public BuildingBlueprint goldBuilding, advanceTower, gasBuilding, liquidBuilding, plasmaBuilding, mineralBuilding, spellTower;

	void Start ()
	{
		buildManager = BuildManager.instance;
		menuUI.SetActive(true);
	}
	//Section to control which menu is showing to the player
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

	//Section to set the button functions
		//Select [x] tower/building`
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

	//Section to show the tooltip on mouseover

	public void TooltipSelectStandardTurret ()
	{
		//Set a Gameobject in Unity and activate/deactivate accordingly here
		Debug.Log("Standard Turret Tooltip");
		tooltipDisplay.text = "Standard Turret Tooltip";
		//tooltipUI.SetActive(true);
	}
	public void TooltipSelectMissileLauncher()
	{
		Debug.Log("Missile Launcher Tooltip");
	}
	public void TooltipSelectLaserBeamer()
	{
		Debug.Log("Laser Beamer Tooltip");
	}
	public void TooltipSelectAoETurret()
	{
		Debug.Log("AoE Turret Tooltip");
	}
	public void TooltipSelectGoldGenerator()
	{
		Debug.Log("Gold Generator Tooltip");
	}
	public void TooltipSelectGasGenerator()
	{
		Debug.Log("Gas Generator Tooltip");
	}
	public void TooltipSelectLiquidGenerator()
	{
		Debug.Log("Liquid Generator Tooltip");
	}
	public void TooltipSelectPlasmaGenerator()
	{
		Debug.Log("Plasma Generator Tooltip");
	}
	public void TooltipSelectMineralGenerator()
	{
		Debug.Log("Mineral Generator Tooltip");
	}
	public void TooltipSelectAdvanceTower()
	{
		Debug.Log("Advance Tower Tooltip");
	}
	public void TooltipSelectSpellTower()
	{
		Debug.Log("Spell Tower Tooltip");
	}
	public void OnMouseExit()
	{
		tooltipUI.SetActive(false);
	}
	//Section to detect where the player is clicking
	public Vector3 GetBuildPosition ()
	{
		RaycastHit mouseHit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out mouseHit, 100))
		{
				myPosi = mouseHit.point;
		}
		return myPosi + positionOffset;
	}
}
