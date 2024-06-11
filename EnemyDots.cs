using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDots : MonoBehaviour
{
    private float timer, damage, pTimer, pDamage;
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
            case "Poison":
                pTimer = timer;
                pDamage = damage;
                StartCoroutine(Poison(pTimer, pDamage));
                Debug.Log("I've chosen poison > d = " + pDamage + ".  t = " + pTimer);
            return;

            default:

            return;
        }
    }

    public IEnumerator Poison(float pTimer, float pDamage)
    {
        Debug.Log("pTimer is - " + pTimer);
        while(pTimer > 0)
        {
            for(int i = 0; i < pTimer; i++)
            {
                pTimer--;
            }
            enemy.TakeDamage(0f, 0f, 0f, pDamage);
            Debug.Log("Damaged enemy");
            yield return new WaitForSeconds(1f); 
        }
    }
}
