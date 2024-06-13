using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDots : MonoBehaviour
{
    private float timer, damage;
    private string spell;
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    public void DotEffect(string spell, float timer, float damage)
    {
        switch (spell)
        {
            case "Silence":
                StartCoroutine(Silence(timer));
            return;
            case "Slow":
                StartCoroutine(Slow(timer, damage));    
            return;
            case "Stop":
                StartCoroutine(Stop(timer));
            return;
            case "Fear":
                StartCoroutine(Fear(timer));
            return;
            case "DeBuffSlash":
                StartCoroutine(DeBuffSlash(timer, damage));
            return;
            case "DeBuffBlunt":
                StartCoroutine(DeBuffBlunt(timer, damage));
            return;
            case "DeBuffPierce":
                StartCoroutine(DeBuffPierce(timer, damage));
            return;
            case "DeBuffMag":
                StartCoroutine(DeBuffMag(timer, damage));
            return;
            case "Poison":
                StartCoroutine(Poison(timer, damage));
            return;
            case "Burn":
                StartCoroutine(Burn(timer, damage));
            return;
            // case "Shock":
            //     StartCoroutine(Shock(timer, damage));
            // return;

            // case "Suffocate":
            //     StartCoroutine(Suffocate(timer, damage));
            // return;

            // case "Drown":
            //     StartCoroutine(Drown(timer, damage));
            // return;
            
            // case "Freeze":
            //     StartCoroutine(Freeze(timer, damage));
            // return;

            // case "Scrape":
            //     StartCoroutine(Scrape(timer, damage));
            // return;


            default:

            return;
        }
    }
    public IEnumerator Silence(float sTimer)
    {
        // Debug.Log("sTimer is - " + sTimer);
        while(sTimer > 0)
        {
            for(int i = 0; i < sTimer; i++)
            {
                sTimer--;
            }
            enemy.silence = true;
            // Debug.Log("Silenced");
            yield return new WaitForSeconds(1f); 
        }
        enemy.silence = false;
    }
    public IEnumerator Slow(float sTimer, float sAmount)
    {
        // Debug.Log("sTimer is - " + sTimer);
        while(sTimer > 0)
        {
            if(enemy.stopEnemy)
            {
                yield break;
            }
            if(enemy.speedEnemy)
            {
                enemy.speed = enemy.startSpeed;
                enemy.speedEnemy = false;
                yield break;
            }
            enemy.slowEnemy = true;
            for(int i = 0; i < sTimer; i++)
            {
                sTimer--;
            }
            if(enemy.speed > (enemy.startSpeed * (1f - sAmount)))
            {
                enemy.speed = enemy.startSpeed * (1f - sAmount);
            }
            // Debug.Log("Slowed");
            yield return new WaitForSeconds(1f); 
        }
        enemy.slowEnemy = false;
        enemy.speed = enemy.startSpeed;
    }
    public IEnumerator Stop(float sTimer)
    {
        // Debug.Log("sTimer is - " + sTimer);
        Silence(sTimer);
        while(sTimer > 0)
        {
            if(enemy.speedEnemy)
            {
                enemy.speed = enemy.startSpeed;
                enemy.speedEnemy = false;
                yield break;
            }
            enemy.stopEnemy = true;
            for(int i = 0; i < sTimer; i++)
            {
                sTimer--;
            }
            if(enemy.speed > 0)
            {
                enemy.speed = 0f    ;
            }
            // Debug.Log("Stopped");
            yield return new WaitForSeconds(1f); 
        }
        enemy.stopEnemy = false;
        enemy.speed = enemy.startSpeed;
    }
    public IEnumerator Fear(float fTimer)
    {
        // Debug.Log("fTimer is - " + fTimer);
        Silence(fTimer);
        while(fTimer > 0)
        {
            if(enemy.speedEnemy)
            {
                enemy.speed = enemy.startSpeed;
                enemy.speedEnemy = false;
                yield break;
            }
            enemy.fearEnemy = true;
            for(int i = 0; i < fTimer; i++)
            {
                fTimer--;
            }
            if(enemy.speed > 0)
            {
                enemy.speed = 0f    ;
            }
            // Debug.Log("Feared");
            yield return new WaitForSeconds(1f); 
        }
        enemy.fearEnemy = false;
    }
    public IEnumerator DeBuffSlash(float sTimer, float sMinus)
    {
        // Debug.Log("sTimer is - " + sTimer);
        while(sTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.slashDef > (enemy.enemyStats.startSlashDef + sMinus))
            {
                enemy.slashDef = enemy.slashDef + sMinus;

            }
            // Debug.Log("Slash def = " + enemy.slashDef);
            yield return new WaitForSeconds(1f); 
        }
        enemy.slashDef = enemy.enemyStats.startSlashDef;
        // Debug.Log("debuff Slash over");
    }
    public IEnumerator DeBuffBlunt(float bTimer, float bMinus)
    {
        // Debug.Log("bTimer is - " + bTimer);
        while(bTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.bluntDef > (enemy.enemyStats.startBluntDef + bMinus))
            {
                enemy.bluntDef = enemy.bluntDef + bMinus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.bluntDef = enemy.enemyStats.startBluntDef;
        // Debug.Log("debuff Blunt over");
    }
    public IEnumerator DeBuffPierce(float pTimer, float pMinus)
    {
        // Debug.Log("pTimer is - " + pTimer);
        while(pTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.pierceDef > (enemy.enemyStats.startPierceDef + pMinus))
            {
                enemy.pierceDef = enemy.pierceDef + pMinus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.pierceDef = enemy.enemyStats.startPierceDef;
        // Debug.Log("debuff Pierce over");
    }
    public IEnumerator DeBuffMag(float mTimer, float mMinus)
    {
        // Debug.Log("mTimer is - " + mTimer);
        while(mTimer > 0)
        {
            if(timer > 0)
            {
                timer--;
            }
            if(enemy.magDef > (enemy.enemyStats.startMagDef + mMinus))
            {
                enemy.magDef = enemy.magDef + mMinus;
            }
            yield return new WaitForSeconds(1f); 
        }
        enemy.magDef = enemy.enemyStats.startMagDef;
        // Debug.Log("debuff Mag over");
    }
    public IEnumerator Poison(float pTimer, float pDamage)
    {
        // Debug.Log("pTimer is - " + pTimer);
        while(pTimer > 0)
        {
            for(int i = 0; i < pTimer; i++)
            {
                pTimer--;
            }
            enemy.TakeDamage(0f, 0f, 0f, pDamage);
            Debug.Log("Poison Damage");
            yield return new WaitForSeconds(1f); 
        }
    }
    public IEnumerator Burn(float bTimer, float bDamage)
    {
        // Debug.Log("bTimer is - " + bTimer);
        while(bTimer > 0)
        {
            for(int i = 0; i < bTimer; i++)
            {
                bTimer--;
            }
            enemy.TakeDamage(0f, 0f, 0f, bDamage);
            Debug.Log("Burn damage");
            yield return new WaitForSeconds(1f); 
        }
    }
    public IEnumerator Freeze(float fTimer, float fDamage)
    {
        // Debug.Log("fTimer is - " + fTimer);
        while(fTimer > 0)
        {
            for(int i = 0; i < fTimer; i++)
            {
                fTimer--;
            }
            enemy.TakeDamage(0f, 0f, 0f, fDamage);
            Debug.Log("Burn damage");
            yield return new WaitForSeconds(1f); 
        }
    }
}
