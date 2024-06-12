using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffs : MonoBehaviour
{
    private float timer, bonus, sBonus, bBonus, pBonus;
    private string spell;
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    public void BuffEffect(string spell, float timer, int bonus)
    {
        switch (spell)
        {
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
    public IEnumerator BuffSlash(float sTimer, int sBonus)
    {
        Debug.Log("sTimer is - " + sTimer);
        while(sTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.slashDef < (enemy.enemyStats.startSlashDef + sBonus))
            {
                enemy.slashDef = enemy.slashDef + sBonus;

            }
            Debug.Log("Slash def = " + enemy.slashDef);
            yield return new WaitForSeconds(1f); 
        }
        enemy.slashDef = enemy.enemyStats.startSlashDef;
        Debug.Log("Buff Slash over");
    }
    public IEnumerator BuffBlunt(float bTimer, int bBonus)
    {
        Debug.Log("bTimer is - " + bTimer);
        while(bTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.bluntDef < (enemy.enemyStats.startBluntDef + bBonus))
            {
                enemy.bluntDef = enemy.bluntDef + bBonus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.bluntDef = enemy.enemyStats.startBluntDef;
        Debug.Log("Buff Blunt over");
    }
    public IEnumerator BuffPierce(float pTimer, int pBonus)
    {
        Debug.Log("pTimer is - " + pTimer);
        while(pTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.pierceDef < (enemy.enemyStats.startPierceDef + pBonus))
            {
                enemy.pierceDef = enemy.pierceDef + pBonus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.pierceDef = enemy.enemyStats.startPierceDef;
        Debug.Log("Buff Pierce over");
    }
    public IEnumerator BuffMag(float mTimer, int mBonus)
    {
        Debug.Log("mTimer is - " + mTimer);
        while(mTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.magDef < (enemy.enemyStats.startMagDef + mBonus))
            {
                enemy.magDef = enemy.magDef + mBonus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.magDef = enemy.enemyStats.startMagDef;
        Debug.Log("Buff Mag over");
    }
    
}
