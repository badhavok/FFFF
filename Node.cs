using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

	public Color hoverColor;
	public Color notEnoughMoneyColor;
  public Vector3 positionOffset;

	[HideInInspector]
	public GameObject turret;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
	public bool isBase = true;
	[HideInInspector]
	public bool isFirstUpgrade, isSecondUpgrade, isDPS, isDPSOne, isDPSTwo, isSUP, isSUPOne, isSUPTwo  = false;

	private Renderer rend;
	private Color startColor;

	BuildManager buildManager;

	void Start ()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;

		buildManager = BuildManager.instance;
    }

	public Vector3 GetBuildPosition ()
	{
		return transform.position + positionOffset;
	}

	void OnMouseDown ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (turret != null)
		{
			buildManager.SelectNode(this);
			return;
		}

		if (!buildManager.CanBuild)
			return;

		BuildTurret(buildManager.GetTurretToBuild());
	}

	void BuildTurret (TurretBlueprint blueprint)
	{
		if (PlayerStats.Money < blueprint.cost)
		{
			Debug.Log("Not enough money to build that!");
			return;
		}

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
		if (PlayerStats.Money < turretBlueprint.upgradeCost || PlayerStats.Money < turretBlueprint.upgradeCostTwo || PlayerStats.Money < turretBlueprint.upgradeCostDps || PlayerStats.Money < turretBlueprint.upgradeCostSup)
		{
			Debug.Log("Not enough money to upgrade that!");
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
		else if (isFirstUpgrade)
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
	}
	public void UpgradeToDPS ()
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
	public void UpgradeToSup ()
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
	public void UpgradeTurretDpsOne ()
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
	public void UpgradeTurretDpsTwo ()
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
	public void UpgradeTurretSupOne ()
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
	public void UpgradeTurretSupTwo ()
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
	public void SellTurret ()
	{
		if(isSUPTwo)
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
		}
		else if(isSecondUpgrade)
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
		isFirstUpgrade = false;
		isSecondUpgrade = false;
		isDPS  = false;
		isDPSOne  = false;
		isDPSTwo  = false;
		isSUP  = false;
		isSUPOne  = false;
		isSUPTwo  = false;
	}

	void OnMouseEnter ()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (!buildManager.CanBuild)
			return;

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
		rend.material.color = startColor;
    }

}
