using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDots : MonoBehaviour
{
    private float timer, damage;
    private string spell;
    private Turret turret;
    void Start()
    {
        turret = GetComponent<Turret>();
    }

    public void DotEffect(string spell, float timer, float damage)
    {
        switch (spell)
        {
            case "AttackTurretHP":
                Debug.Log("Attacking turret spell");
                StartCoroutine(AttackTurretHP(timer, damage));
            return;
            case "DebuffTurretSpeed":
                StartCoroutine(DebuffTurretSpeed(timer, damage));
            return;
            // case "Silence":
            //     StartCoroutine(Silence(timer));
            // return;
            // case "Slow":
            //     StartCoroutine(Slow(timer, damage));    
            // return;
            // case "Stop":
            //     StartCoroutine(Stop(timer));
            // return;

        }
    }
    public IEnumerator AttackTurretHP(float aTimer, float damage)
    {
        while(aTimer > 0)
        {
            for(int i = 0; i < aTimer; i++)
            {
                aTimer--;
            }
            turret.healthPoints -= damage;
            Debug.Log("Damaged");
            yield return new WaitForSeconds(1f); 
        }
    }
    public IEnumerator DebuffTurretSpeed(float dTimer, float dAmount)
    {
        while(dTimer > 0)
        {
            turret.debuffAtkSpdUI.SetActive(true);
            for(int i = 0; i < dTimer; i++)
            {
                dTimer--;
            }
            turret.fireRate = turret.fireRate / dAmount;
            Debug.Log("I'm slowed - " + turret);
            yield return new WaitForSeconds(1f); 
        }
		turret.fireRate = turret.startFireRate;
		turret.debuffAtkSpdUI.SetActive(false);
    }
    // public IEnumerator Silence(float sTimer)
    // {
    //     // Debug.Log("sTimer is - " + sTimer);
    //     while(sTimer > 0)
    //     {
    //         for(int i = 0; i < sTimer; i++)
    //         {
    //             sTimer--;
    //         }
    //         turret.silence = true;
    //         // Debug.Log("Silenced");
    //         yield return new WaitForSeconds(1f); 
    //     }
    //     turret.silence = false;
    // }
    // public IEnumerator Slow(float sTimer, float sAmount)
    // {
    //     // Debug.Log("sTimer is - " + sTimer);
    //     while(sTimer > 0)
    //     {
    //         if(turret.stopturret)
    //         {
    //             yield break;
    //         }
    //         if(turret.speedturret)
    //         {
    //             turret.speed = turret.startSpeed;
    //             turret.speedturret = false;
    //             yield break;
    //         }
    //         turret.slowturret = true;
    //         for(int i = 0; i < sTimer; i++)
    //         {
    //             sTimer--;
    //         }
    //         if(turret.speed > (turret.startSpeed * (1f - sAmount)))
    //         {
    //             turret.speed = turret.startSpeed * (1f - sAmount);
    //         }
    //         // Debug.Log("Slowed");
    //         yield return new WaitForSeconds(1f); 
    //     }
    //     turret.slowturret = false;
    //     turret.speed = turret.startSpeed;
    // }
    // public IEnumerator Stop(float sTimer)
    // {
    //     // Debug.Log("sTimer is - " + sTimer);
    //     Silence(sTimer);
    //     while(sTimer > 0)
    //     {
    //         if(turret.speedturret)
    //         {
    //             turret.speed = turret.startSpeed;
    //             turret.speedturret = false;
    //             yield break;
    //         }
    //         turret.stopturret = true;
    //         for(int i = 0; i < sTimer; i++)
    //         {
    //             sTimer--;
    //         }
    //         if(turret.speed > 0)
    //         {
    //             turret.speed = 0f    ;
    //         }
    //         // Debug.Log("Stopped");
    //         yield return new WaitForSeconds(1f); 
    //     }
    //     turret.stopturret = false;
    //     turret.speed = turret.startSpeed;
    // }

}
