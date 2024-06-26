using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffs : MonoBehaviour
{
    // Everything that effects HP runs on 'tic' (currently set at 3s)

    private float timer, bonus, sBonus, bBonus, pBonus;
    private float tic = 3f;
    private string spell;
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        MeshHandler.GetEnemyMeshes(enemy);
        MeshHandler.SetEnemyMeshes(enemy);
        MeshHandler.UnSetEnemyMeshes(enemy);
    }
    public void BuffEffect(string spell, float timer, float bonus)
    {
        switch (spell)
        {
            case "buffImmune":
                StartCoroutine(Immune(timer));
            return;
            case "Healing":
                StartCoroutine(Healing(timer, bonus));
            return;
            case "BuffHide":
                StartCoroutine(BuffHide(timer));
            return;
            case "SpeedBuff":
                StartCoroutine(SpeedBuff(timer, bonus));
            return;
            case "BuffSlash":
                StartCoroutine(BuffSlash(timer, bonus));
            return;
            case "BuffBlunt":
                StartCoroutine(BuffBlunt(timer, bonus));
            return;
            case "BuffPierce":
                StartCoroutine(BuffPierce(timer, bonus));
            return;
            case "BuffMag":
                StartCoroutine(BuffMag(timer, bonus));
            return;

            default:

            return;
        }
    }
    public IEnumerator Immune(float iTimer)
    {
		enemy.doom = false;
		enemy.slowEnemy = false;
		enemy.stopEnemy = false;
		enemy.fearEnemy = false;

        while (iTimer > 0)
        {
            if(iTimer > 0)
            {
                iTimer--;
            }
            enemy.immune = true;
            Debug.Log("Enemy is immune");
            yield return new WaitForSeconds(1f);
        }
        enemy.immune = false;
    }
    public IEnumerator Healing(float hTimer, float hBonus)
    {
        // Debug.Log("hTimer is - " + hTimer);
        while(hTimer > 0)
        {
            if(hTimer > 0)
            {
                hTimer--;
            }
            enemy.Healing(hBonus);
            yield return new WaitForSeconds(tic); 
        }
    }
    public IEnumerator SpeedBuff(float sTimer, float sBonus)
    {
        // Debug.Log("sTimer is - " + sTimer);
        while(sTimer > 0)
        {
            if(enemy.stopEnemy || enemy.slowEnemy)
            {
                // Debug.Log("The enemy was slower, now returned to normal");
                enemy.speed = enemy.startSpeed;
                enemy.stopEnemy = false;
                enemy.slowEnemy = false;
                yield break;
            }
            enemy.speedEnemy = true;
            if(sTimer > 0)
            {
                sTimer--;
            }
            enemy.speed = enemy.startSpeed + sBonus;

            // Debug.Log("Speed = " + enemy.speed);
            yield return new WaitForSeconds(1f); 
        }
        enemy.speed = enemy.startSpeed;
        enemy.speedEnemy = false;
        // Debug.Log("Buff Speed over");
    }
    public IEnumerator BuffHide(float hTimer)
    {
        MeshHandler.SetEnemyMeshes(enemy);
        
        while(hTimer > 0)
        {
            if(hTimer > 0)
            {
                hTimer--;
            }
        yield return new WaitForSeconds(1f); 
        }
        MeshHandler.UnSetEnemyMeshes(enemy);
        
    }
    public IEnumerator BuffSlash(float sTimer, float sBonus)
    {
        // Debug.Log("sTimer is - " + sTimer);
        while(sTimer > 0)
        {
            if(sTimer > 0)
            {
                sTimer--;
            }
            if(enemy.slashDef < (enemy.enemyStats.startSlashDef + sBonus))
            {
                enemy.slashDef = enemy.slashDef + sBonus;

            }
            // Debug.Log("Slash def = " + enemy.slashDef);
            yield return new WaitForSeconds(1f); 
        }
        enemy.slashDef = enemy.enemyStats.startSlashDef;
        // Debug.Log("Buff Slash over - " + enemy.slashDef);
    }
    public IEnumerator BuffBlunt(float bTimer, float bBonus)
    {
        // Debug.Log("bTimer is - " + bTimer);
        while(bTimer > 0)
        {
            if(bTimer > 0)
            {
                bTimer--;
            }
            if(enemy.bluntDef < (enemy.enemyStats.startBluntDef + bBonus))
            {
                enemy.bluntDef = enemy.bluntDef + bBonus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.bluntDef = enemy.enemyStats.startBluntDef;
        // Debug.Log("Buff Blunt over");
    }
    public IEnumerator BuffPierce(float pTimer, float pBonus)
    {
        // Debug.Log("pTimer is - " + pTimer);
        while(pTimer > 0)
        {
            if(pTimer > 0)
            {
                pTimer--;
            }
            if(enemy.pierceDef < (enemy.enemyStats.startPierceDef + pBonus))
            {
                enemy.pierceDef = enemy.pierceDef + pBonus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.pierceDef = enemy.enemyStats.startPierceDef;
        // Debug.Log("Buff Pierce over");
    }
    public IEnumerator BuffMag(float mTimer, float mBonus)
    {
        // Debug.Log("mTimer is - " + mTimer);
        while(mTimer > 0)
        {
            if(mTimer > 0)
            {
                mTimer--;
            }
            if(enemy.magDef < (enemy.enemyStats.startMagDef + mBonus))
            {
                enemy.magDef = enemy.magDef + mBonus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.magDef = enemy.enemyStats.startMagDef;
        // Debug.Log("Buff Mag over");
    }
    
}

