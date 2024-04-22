using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This is the main enemy class and should be assigned to EVERYTHING that is going to be attacked
//It contains the code required to give the enemy HP and get attacked etc..
public class Base : MonoBehaviour {

    public int health, healthBar;

    public GameObject fullHealth;
	public GameObject ninetyHealth;
    public GameObject eightyHealth;
    public GameObject seventyHealth;
    public GameObject sixtytyHealth;
    public GameObject fiftyHealth;
    public GameObject fourtyHealth;
    public GameObject thirtyHealth;
    public GameObject twentyHealth;
    public GameObject tenHealth;
    public GameObject lastHealth;

    void Start ()
    {
        health = PlayerStats.Lives; 
        healthBar = health;
    }
    
    void Update()
    {
        health = PlayerStats.Lives;
        DisplayDamage(healthBar);
    }

    void DisplayDamage(int healthBar)
    {
        switch (healthBar)
            {
                default:
                fullHealth.SetActive(true);
        		// ninetyHealth.SetActive(false);
                // eightyHealth.SetActive(false);
        		// seventyHealth.SetActive(false);
                // sixtytyHealth.SetActive(false);
        		// fiftyHealth.SetActive(false);
                // fourtyHealth.SetActive(false);
        		// thirtyHealth.SetActive(false);
                // twentyHealth.SetActive(false);
        		// tenHealth.SetActive(false);
                // lastHealth.SetActive(false);
                Debug.Log("Full");
                return;

                case var expression when (health < healthBar && health >= healthBar * 90 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(true);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("90");
                return;
                
                case var expression when (health <= healthBar * 90 / 100 && health >= healthBar * 80 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(true);
        		//seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("80");
                return;
                
                case var expression when (health < healthBar * 80 / 100 && health >= healthBar * 70 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(true);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("70");
                return;

                case var expression when (health < healthBar * 70 / 100 && health >= healthBar * 60 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(true);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("60");
                return;

                case var expression when (health < healthBar * 60 / 100 && health >= healthBar * 50 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(true);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("50");
                return;

                case var expression when (health < healthBar * 50 / 100 && health >= healthBar * 40 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(true);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("40");
                return;

                case var expression when (health < healthBar * 40 / 100 && health >= healthBar * 30 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(true);
                // twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("30");
                return;
                
                case var expression when (health < healthBar * 30 / 100 && health >= healthBar * 20 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(true);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(false);
                Debug.Log("20");
                return;
                
                case var expression when (health < healthBar * 20 / 100 && health >= healthBar * 10 / 100):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(true);
                //lastHealth.SetActive(false);
                Debug.Log("10");
                return;

                case var expression when (health < healthBar * 10 / 100 && health >= 0):
                fullHealth.SetActive(false);
        		ninetyHealth.SetActive(false);
                eightyHealth.SetActive(false);
        		seventyHealth.SetActive(false);
                sixtytyHealth.SetActive(false);
        		fiftyHealth.SetActive(false);
                fourtyHealth.SetActive(false);
        		thirtyHealth.SetActive(false);
                twentyHealth.SetActive(false);
        		tenHealth.SetActive(false);
                lastHealth.SetActive(true);
                Debug.Log("almost gone");
                return;

                case var expression when (health < 0):
                Debug.Log("over");
                return;

            }
    }
}