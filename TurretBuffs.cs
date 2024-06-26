using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuffs : MonoBehaviour
{
    // Everything that effects HP runs on 'tic' (currently set at 3s)
    
    private float timer, damage;
    private float tic = 3f;
    private string spell;
    private Turret turret;
    void Start()
    {
        turret = GetComponent<Turret>();
        MeshHandler.GetTurretMeshes(turret);
        MeshHandler.SetTurretMeshes(turret);
        MeshHandler.UnSetTurretMeshes(turret);
    }

    public void BuffEffect(string spell, float timer, float bonus)
    {
        switch (spell)
        {
            case "buffImmune":
                StartCoroutine(Immune(timer));
            return;
            case "Regen":
                StartCoroutine(Regen(timer, bonus));
            return;
            case "BuffHide":
                StartCoroutine(BuffHide(timer));
            return;
            case "SpeedBuff":
                StartCoroutine(SpeedBuff(timer, bonus));
            return;

            default:

            return;
        }
    }

    public IEnumerator Immune(float iTimer)
    {
		// turret.doom = false;
		// turret.slowEnemy = false;
		// turret.stopEnemy = false;
		// turret.fearEnemy = false;

        while (iTimer > 0)
        {
            if(iTimer > 0)
            {
                iTimer--;
            }
            turret.immune = true;
            Debug.Log("turret is immune");
            yield return new WaitForSeconds(1f);
        }
        turret.immune = false;
    }
    public IEnumerator Regen(float hTimer, float hBonus)
    {
        // Debug.Log("hTimer is - " + hTimer);
        while(hTimer > 0)
        {
            if(hTimer > 0)
            {
                hTimer--;
            }
            turret.Healing(hBonus);
            yield return new WaitForSeconds(tic); 
        }
    }
    public IEnumerator SpeedBuff(float dTimer, float dBonus)
    {
        turret.fireRate = turret.fireRate / dBonus;
        // Debug.Log("I'm slowed - " + turret);
        yield return new WaitForSeconds(dTimer); 
		turret.fireRate = turret.startFireRate;
    }
    public IEnumerator BuffHide(float hTimer)
    {
        MeshHandler.SetTurretMeshes(turret);
        
        while(hTimer > 0)
        {
            if(hTimer > 0)
            {
                hTimer--;
            }
        yield return new WaitForSeconds(1f); 
        }
        MeshHandler.UnSetTurretMeshes(turret);
        
    }
    

}
