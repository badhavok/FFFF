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
		ChickenEnemy = chickenEnemy;
	}

	public static GameObject ChickenEnemy;
	public GameObject chickenEnemy;

	public GameObject buildEffect;
	public GameObject sellEffect;

	public TurretBlueprint turretToBuild;
	public BuildingBlueprint buildingToBuild;
	public PlayerSpells selectedSpell;

	[HideInInspector]
	public bool turretSelected, canBuildB, canBuildT, buildingSelected = false;
	private Node selectedNode;

	//public bool CanBuildT { get { return turretToBuild != null; } }
	//public bool CanBuildB { get { return buildingToBuild != null; } }
	public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

	void Update()
	{
		Wave = WaveSpawner.CurrentWave;
		Level = currentLevel;
		if(AdvanceBuilding.IsExists)
		{
			upgradeLevel = AdvanceBuilding.BuildLevel;
		}
		else
		{
			upgradeLevel = 0;
		}
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
		if (selectedNode == node)
		{
			DeselectNode();
			return;
		}

		selectedNode = node;
		turretToBuild = null;
		buildingToBuild = null;
		nodeUI.SetTarget(node);
	}
	public void DeselectNode()
	{
		selectedNode = null;
		nodeUI.Hide();
	}
	public void SelectSpell (PlayerSpells spell)
	{
		Debug.Log("Selecting spell");
		selectedSpell = spell;
	}
	public void SelectTurretToBuild (TurretBlueprint turret)
	{
		Debug.Log("Select turret in BuildManager");
		canBuildT = true;
		canBuildB = false;
		turretToBuild = turret;
		DeselectNode();
	}
	public void SelectBuildingToBuild (BuildingBlueprint building)
	{
		canBuildB = true;
		canBuildT = false;
		buildingToBuild = building;
		DeselectNode();
	}
	public TurretBlueprint GetTurretToBuild ()
	{
		return turretToBuild;
	}
	public BuildingBlueprint GetBuildingToBuild ()
	{
		return buildingToBuild;
	}
}
