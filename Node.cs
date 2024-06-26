using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public Color hoverColor;
	public Color notEnoughMoneyColor;
	public Color usedColor;
	public Vector3 positionOffset;

	//[HideInInspector]
	public GameObject turret, building;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
	public BuildingBlueprint buildingBlueprint;
	[HideInInspector]
	public bool isBase = true;

	//listing all the bonus upgrades given to turrets
	[Header("Bonuses give:  RNG, DMG, MAG")]
	public bool hasBonus = false;
	public bool nodeUsed, enableButton = false;
	public int[] nodeBonuses = { 0, 0, 0 };

	[HideInInspector]
	//listing all the turret upgrades
	public bool isFirstUpgrade, isSecondUpgrade, isDPS, isDPSOne, isDPSTwo, isSUP, isSUPOne, isSUPTwo, advanceOne, advanceTwo, advanceThree = false;
	[HideInInspector]
	//listing all the building upgrades
	public bool isBuilding, isBUpgrade, isBUpgrade2, canBuildB = false;

	private Renderer rend;
	private Color startColor;

	BuildManager buildManager;

	void Start ()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;

		buildManager = BuildManager.instance;
    }

	public Vector3 GetBuildPosition()
	{
		return transform.position + positionOffset;
	}

	void OnMouseDown()
	{
		//Debug.Log("mousedown");
		if (EventSystem.current.IsPointerOverGameObject())
			return;
		//Debug.Log("mousedown #2");
		if (turret != null || building != null)
		{
			//Debug.Log("Selecting node");
			Time.timeScale = 0f;
			buildManager.SelectNode(this);
			enableButton = turret.GetComponent<Turret>().canHeal;
			return;
		}
		//Debug.Log("Checking canbuild");
		if (buildManager.costOfTower <= 1 || buildManager.nothingSelected)
			return;

		BuildThis();

		if (hasBonus)
		{
			turret.GetComponent<Turret>().isUpgradedByNode = true;
			turret.GetComponent<Turret>().nodeBonuses = nodeBonuses;
		}

		buildManager.canBuildB = false;
		buildManager.canBuildT = false;
		buildManager.turretSelected = false;
		buildManager.SelectTurretToBuild(null);
		buildManager.nothingSelected = true;
	}
	//Final function to build what the player has selected
	void BuildThis ()
	{
		//Debug.Log("Build this");
		if (buildManager.canBuildB)
		{
			BuildBuilding(buildManager.GetBuildingToBuild());
			//Debug.Log("I'm building a building");
			buildManager.buildingToBuild = null;
		}
		if (buildManager.canBuildT)
		{
			BuildTurret(buildManager.GetTurretToBuild());
			//Debug.Log("I'm building a turret");
			buildManager.turretToBuild = null;
		}
	}

	//Due to multiple bool checks & menus used in the game, I have split out each build/upgrade depending on what the current upgrade level is

	void BuildBuilding (BuildingBlueprint blueprintB)
	{
		//Debug.Log("In building loop");
		if (PlayerStats.Money < blueprintB.costB)
	  {
	    //Debug.Log("Not enough money to build that!");
	    return;
	  }

	  PlayerStats.Money -= blueprintB.costB;

	  GameObject _building = (GameObject)Instantiate(blueprintB.prefabB, GetBuildPosition(), Quaternion.identity);
	  building = _building;

	  buildingBlueprint = blueprintB;

	  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
	  Destroy(effect, 5f);

		isBuilding = true;
	  // Debug.Log("Turret build!");
	}

	public void UpgradeBuilding ()
	{
	  if (PlayerStats.Money < buildingBlueprint.upgradeBCost || PlayerStats.Money < buildingBlueprint.upgradeBCostTwo)
	  {
	    //Debug.Log("Not enough money to upgrade that!");
	    return;
	  }
	  if (!isBUpgrade)
	  {
	  PlayerStats.Money -= buildingBlueprint.upgradeBCost;

	  //Get rid of the old turret
	  Destroy(building);

	  //Build a new one
	  GameObject _building = (GameObject)Instantiate(buildingBlueprint.prefabBUpgrade, GetBuildPosition(), Quaternion.identity);
	  building = _building;

	  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
	  Destroy(effect, 5f);

	  isBUpgrade = true;
	  }
	  else if (isBUpgrade)
	  {
	    PlayerStats.Money -= buildingBlueprint.upgradeBCostTwo;

	    //Get rid of the old turret
	    Destroy(building);

	    //Build a new one
	    GameObject _building = (GameObject)Instantiate(buildingBlueprint.prefabBUpgradeTwo, GetBuildPosition(), Quaternion.identity);
	    building = _building;

	    GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
	    Destroy(effect, 5f);
	    isBUpgrade2 = true;
	  }
	}
	void BuildTurret (TurretBlueprint blueprint)
	{
		//Debug.Log("In build loop");
		
		if (PlayerStats.Money < blueprint.cost)
		{
			Debug.Log("Not enough money to build that!");
			return;
		}

		nodeUsed = true;

	    PlayerStats.Money -= blueprint.cost;

	  	GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
	  	turret = _turret;

	  	turretBlueprint = blueprint;

	  	GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
	  	Destroy(effect, 5f);

	  // Debug.Log("Turret build!");
	}

	public void UpgradeTurret ()
	{
	  if (PlayerStats.Money < turretBlueprint.upgradeCost)
	  {
	   // Debug.Log("Not enough money to upgrade that!");
	    return;
	  }
	  if (!isFirstUpgrade)
	  {
	  PlayerStats.Money -= turretBlueprint.upgradeCost;

	  //Get rid of the old turret
	  Destroy(turret);

	  //Build a new one
	  GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabUpgrade, GetBuildPosition(), Quaternion.identity);
	  turret = _turret;

	  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
	  Destroy(effect, 5f);

	  isFirstUpgrade = true;
	  }
	}
	public void UpgradeTurretTwo()
	{
		if (PlayerStats.Money < turretBlueprint.upgradeCostTwo)
		{
			//Debug.Log("Not enough money to upgrade that!");
			return;
		}
		if (isFirstUpgrade && AdvanceBuilding.BuildLevel > 0)
	  {
	    PlayerStats.Money -= turretBlueprint.upgradeCostTwo;

	    //Get rid of the old turret
	    Destroy(turret);

	    //Build a new one
	    GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabUpgradeTwo, GetBuildPosition(), Quaternion.identity);
	    turret = _turret;

	    GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
	    Destroy(effect, 5f);
	    isSecondUpgrade = true;
	  }
		else
		{
			Debug.Log("Can't advance");
		}
	}
	/* public void UpgradeToDPS ()
	{
		if (AdvanceBuilding.BuildLevel > 1)
		{
			PlayerStats.Money -= turretBlueprint.upgradeCostDps;

		  //Get rid of the old turret
		  Destroy(turret);

		  //Build a new one
		  GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabDps, GetBuildPosition(), Quaternion.identity);
		  turret = _turret;

		  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		  Destroy(effect, 5f);

		  isDPS = true;
		}
		else
		{
			Debug.Log("Can't advance DPS");
		}
	}
	public void UpgradeToSup ()
	{
		if (AdvanceBuilding.BuildLevel > 1)
		{
			PlayerStats.Money -= turretBlueprint.upgradeCostSup;

		  //Get rid of the old turret
		  Destroy(turret);

		  //Build a new one
		  GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabSup, GetBuildPosition(), Quaternion.identity);
		  turret = _turret;

		  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		  Destroy(effect, 5f);

		  isSUP = true;
		}
		else
		{
			Debug.Log("Can't advance SUP");
		}
	}
	public void UpgradeTurretDpsOne ()
	{
		if (AdvanceBuilding.BuildLevel > 2)
		{
			PlayerStats.Money -= turretBlueprint.upgradeCostDpsOne;

		  //Get rid of the old turret
		  Destroy(turret);

		  //Build a new one
		  GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabDpsOne, GetBuildPosition(), Quaternion.identity);
		  turret = _turret;

		  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		  Destroy(effect, 5f);

		  isDPSOne = true;
		  isDPSTwo = true;
		}
		else
		{
			Debug.Log("Can't advance DPS 1");
		}
	}
	public void UpgradeTurretDpsTwo ()
	{
		if (AdvanceBuilding.BuildLevel > 2)
		{
			PlayerStats.Money -= turretBlueprint.upgradeCostDpsTwo;

		  //Get rid of the old turret
		  Destroy(turret);

		    //Build a new one
		  GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabDpsTwo, GetBuildPosition(), Quaternion.identity);
		  turret = _turret;

		  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		  Destroy(effect, 5f);

		  isDPSTwo = true;
		  isDPSOne = true;
		}
	}
	public void UpgradeTurretSupOne ()
	{
		if (AdvanceBuilding.BuildLevel > 2)
		{
			PlayerStats.Money -= turretBlueprint.upgradeCostSupOne;

		  //Get rid of the old turret
		  Destroy(turret);

		  //Build a new one
		  GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabSupOne, GetBuildPosition(), Quaternion.identity);
		  turret = _turret;

		  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		  Destroy(effect, 5f);

		  isSUPOne = true;
		  isSUPTwo = true;
		}
		else
		{
			Debug.Log("Can't upgrade to SUP 1");
		}
	}
	public void UpgradeTurretSupTwo ()
	{
		if (AdvanceBuilding.BuildLevel > 2)
		{
			PlayerStats.Money -= turretBlueprint.upgradeCostSupTwo;

		  //Get rid of the old turret
		  Destroy(turret);

		    //Build a new one
		  GameObject _turret = (GameObject)Instantiate(turretBlueprint.prefabSupTwo, GetBuildPosition(), Quaternion.identity);
		  turret = _turret;

		  GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		  Destroy(effect, 5f);

		  isSUPTwo = true;
		  isSUPOne = true;
		}
	} */
	//Gets costs for the current building level when player wants to sell it
	public void SellBuilding ()
	{
		if(isBUpgrade2)
		{
			PlayerStats.Money += buildingBlueprint.GetUpgradeBValueTwo();
		}
		else if(isBUpgrade)
		{
			PlayerStats.Money += buildingBlueprint.GetUpgradeBValue();
		}
		else if(isBuilding)
		{
			PlayerStats.Money += buildingBlueprint.GetBSellAmount();
		}
		GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
	  Destroy(effect, 5f);

	  Destroy(building);
	  buildingBlueprint = null;
		isBuilding = false;
		isBUpgrade = false;
		isBUpgrade2 = false;

		Destroy(turret);
		turretBlueprint = null;
		isFirstUpgrade = false;
		isSecondUpgrade = false;
		isDPS  = false;
		isDPSOne  = false;
		isDPSTwo  = false;
		isSUP  = false;
		isSUPOne  = false;
		isSUPTwo  = false;
	}
	//Gets turret costs for when the player decides to sell it
	public void SellTurret ()
	{
	  /* if(isSUPTwo)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValueSupTwo();
	  }
	  else if(isSUPOne)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValueSupOne();
	  }
	  else if(isSUP)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValueSup();
	  }
	  else if(isDPSTwo)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValueDpsTwo();
	  }
	  else if(isDPSOne)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValueDpsOne();
	  }
	  else if(isDPS)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValueDps();
	  } */  //else
	  if(isSecondUpgrade)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValueTwo();
	  }
	  else if(isFirstUpgrade)
	  {
	    PlayerStats.Money += turretBlueprint.GetUpgradeValue();
	  }
	  else
	  {
	  PlayerStats.Money += turretBlueprint.GetSellAmount();
	  }

	  GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
	  Destroy(effect, 5f);

	  Destroy(turret);
	  turretBlueprint = null;
	  nodeUsed = false;
	  isFirstUpgrade = false;
	  isSecondUpgrade = false;
	  isDPS  = false;
	  isDPSOne  = false;
	  isDPSTwo  = false;
	  isSUP  = false;
	  isSUPOne  = false;
	  isSUPTwo  = false;

		Destroy(building);
	  buildingBlueprint = null;
		isBuilding = false;
		isBUpgrade = false;
		isBUpgrade2 = false;
	}
	public void CloseEnd()
	{
			turret.GetComponent<Turret>().ClosestToEnd();
	}
	public void CloseStart()
	{
			turret.GetComponent<Turret>().ClosestToStart();
	}
	public void CloseTurret()
	{
			turret.GetComponent<Turret>().NearestToTurret();
	}
	public void FarTurret()
	{
			turret.GetComponent<Turret>().FarthestFromTurret();
	}
	public void BaseHeal()
	{
			turret.GetComponent<Turret>().HealTheBase();
	}
	void OnMouseEnter ()
	{
		//Sets the colour of the node according to if the player can afford to build anything
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		// if (!buildManager.canBuildT)
		// 	return;
		if(nodeUsed)
		{
			rend.material.color = usedColor;
			return;
		}
		if (buildManager.HasMoney)
		{
			rend.material.color = hoverColor;
		} else
		{
			rend.material.color = notEnoughMoneyColor;
		}
	}

	void OnMouseExit ()
	{
		//Sets the colour back when the mouse has exited the node
		rend.material.color = startColor;
  }

}
