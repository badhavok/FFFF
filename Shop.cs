using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

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
	public TurretBlueprint unit1,unit2,unit3,unit4,unit5,unit6;

	//Sets which buildings are going to be used in the game
	//public BuildingBlueprint goldBuilding, advanceTower, gasBuilding, liquidBuilding, plasmaBuilding, mineralBuilding, spellTower;

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
		buildManager.nothingSelected = true;
		//buildManager.SelectBuildingToBuild(null);
	}

	//Section to set the button functions
		//Select [x] tower/building`
	public void Unit1 ()
	{
		//Debug.Log("Standard Turret Selected");
		buildManager.SelectTurretToBuild(unit1);
		buildManager.nothingSelected = false;
	}
	public void Unit2()
	{
		//Debug.Log("Missile Launcher Selected");
		buildManager.SelectTurretToBuild(unit2);
		buildManager.nothingSelected = false;
	}
	public void Unit3()
	{
		//Debug.Log("Laser Beamer Selected");
		buildManager.SelectTurretToBuild(unit3);
		buildManager.nothingSelected = false;
	}
	public void Unit4()
	{
		//Debug.Log("AoE Turret Selected");
		buildManager.SelectTurretToBuild(unit4);
		buildManager.nothingSelected = false;
	}
	public void Unit5()
	{
		//Debug.Log("Gold Generator Selected");
		buildManager.SelectTurretToBuild(unit5);
		buildManager.nothingSelected = false;
	}
	public void Unit6()
	{
		//Debug.Log("Gas Generator Selected");
		buildManager.SelectTurretToBuild(unit6);
		buildManager.nothingSelected = false;
	}
	/* public void SelectAdvanceTower()
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
	} */

	//Section to show the tooltip on mouseover

	public void TooltipUnit1 ()
	{
		//Set a Gameobject in Unity and activate/deactivate accordingly here
		//Debug.Log("Standard Turret Tooltip");
		tooltipDisplay.text = "Standard Turret Tooltip";
		//tooltipUI.SetActive(true);
	}
	public void TooltipUnit2()
	{
		//Debug.Log("Missile Launcher Tooltip");
	}
	public void TooltipUnit3()
	{
		//Debug.Log("Laser Beamer Tooltip");
	}
	public void TooltipUnit4()
	{
		//Debug.Log("AoE Turret Tooltip");
	}
	public void TooltipUnit5()
	{
		//Debug.Log("Gold Generator Tooltip");
	}
	public void TooltipUnit6()
	{
		//Debug.Log("Gas Generator Tooltip");
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
