using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDots : MonoBehaviour
{
    // Everything that effects HP runs on 'tic' (currently set at 3s)
    
    private float timer, damage;
    private float tic = 3f;
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
                StartCoroutine(AttackTurretHP(timer, damage));
            return;
            case "DebuffTurretSpeed":
                StartCoroutine(DebuffTurretSpeed(timer, damage));
            return;
            case "DebuffTurretHealSpeed":
                StartCoroutine(DebuffTurretHealSpeed(timer, damage));
            return;
            case "DebuffSilence":
                StartCoroutine(DebuffSilence(timer, damage));
            return;
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
            yield return new WaitForSeconds(tic); 
            turret.healthPoints -= damage;
            // Debug.Log("Damaged");
        }
    }
    public IEnumerator DebuffTurretSpeed(float dTimer, float dAmount)
    {
        turret.fireRate = turret.fireRate / dAmount;
        // Debug.Log("I'm slowed - " + turret);
        yield return new WaitForSeconds(dTimer); 
		turret.fireRate = turret.startFireRate;
        turret.fireCountdown = 100f / turret.fireRate;
    }
    public IEnumerator DebuffTurretHealSpeed(float dTimer, float dAmount)
    {
        turret.healRate = turret.healRate / dAmount;
        // Debug.Log("My heal is slowed - " + turret);
        yield return new WaitForSeconds(dTimer);
		turret.healRate = turret.startHealRate;
    }
    public IEnumerator DebuffSilence(float sTimer, float sAmount)
    {
        StartCoroutine(DebuffTurretSpeed(sTimer, sAmount));
        StartCoroutine(DebuffTurretHealSpeed(sTimer, sAmount));
        yield return new WaitForSeconds(0f);
    }
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
