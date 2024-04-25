using UnityEngine;

public class BuildManager : MonoBehaviour {

	public int upgradeLevel, spellCount, currentWave, currentLevel;
	public static int Wave, Level;

	public NodeUI nodeUI;
	public PlayerMenu playerMenu;

	public static BuildManager instance;

	void Awake ()
	{
		if (instance != null)
		{
			Debug.LogError("More than one BuildManager in scene!");
			return;
		}
		instance = this;
		//Used as the global variable to set Chicken textures for player spell
		ChickenEnemy = chickenEnemy;
	}

	public static GameObject ChickenEnemy;
	public GameObject chickenEnemy;

	public GameObject buildEffect;
	public GameObject sellEffect;

	public TurretBlueprint turretToBuild;
	public BuildingBlueprint buildingToBuild;
	public int costOfTower;
	public PlayerSpells selectedSpell;

	[HideInInspector]
	public bool turretSelected, nothingSelected, canBuildB, canBuildT, buildingSelected = false;
	private Node selectedNode;
	private bool nodeMenu;

	// Bools to detect whether the player is able to build
	//public bool CanBuildT { get { return turretToBuild != null; } }
	//public bool CanBuildB { get { return buildingToBuild != null; } }

	//This is set to make debugging easier
	public bool HasMoney = false;

	void Update()
	{
		//Setting the variables for the player UI
		Wave = WaveSpawner.CurrentWave;
		Level = currentLevel;

		if (PlayerStats.Money < costOfTower)
		{
			HasMoney = false;
		}
		else
		{
			HasMoney = true;
			canBuildT = true;
		}

        //Detecting if the Advanced building is built and allows the towers to be upgraded
        if (AdvanceBuilding.IsExists)
		{
			upgradeLevel = AdvanceBuilding.BuildLevel;
		}
		else
		{
			upgradeLevel = 0;
		}
		//Detecting if the Spell building is built and allows PlayerSpells to be used
		if(SpellBuilding.SpellLevel > 0)
		{
			spellCount = SpellBuilding.SpellLevel;
		}
		else
		{
			spellCount = 0;
		}
	}

	public void SelectNode (Node node)
	{
		//Selecting the node which the player is going to build on
		if (selectedNode == node)
		{
			nodeMenu = false;
			DeselectNode();
			return;
		}

		selectedNode = node;
		turretToBuild = null;
		turretSelected = false;
		buildingToBuild = null;
		nodeUI.SetTarget(node);
		nodeMenu = true;
	}

	public void DeselectNode()
	{
		//Deselects node once something is built or player clicks something else
		selectedNode = null;
		nodeUI.Hide();
		nothingSelected = true;
		if(!nodeMenu)
		{
			Time.timeScale = 1f;
		}
	}
/*	public void SelectSpell ()
	{
		Debug.Log("Selecting spell");
		selectedSpell = spell;
	}*/
	public void SelectTurretToBuild (TurretBlueprint turret)
	{
		Debug.Log("Selected turret in BuildManager");
		canBuildT = true;
		canBuildB = false;
		turretToBuild = turret;
		if(turretToBuild != null)
		{
			costOfTower = turretToBuild.cost;
		}
		//DeselectNode();
	}
	public void SelectBuildingToBuild (BuildingBlueprint building)
	{
		Debug.Log("Selected Building in BuildManager");
		canBuildB = true;
		canBuildT = false;
		buildingToBuild = building;
		DeselectNode();
	}

	public TurretBlueprint GetTurretToBuild ()
	{
		//Gets the turret from the shop UI
		return turretToBuild;
	}
	public BuildingBlueprint GetBuildingToBuild ()
	{
		//Gets the building from the shop UI
		return buildingToBuild;
	}
}
