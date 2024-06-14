using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshHandler
{
    private static SkinnedMeshRenderer[] enemyMeshes;
    private static SkinnedMeshRenderer enemyMesh;
    // private Material origEnemyMaterial;
    private static Color newColor;
    private static float newAlpha = 0.0f;
    private static Color origColor;
    private static float origAlpha = 1.0f;
    public static List<MaterialPropertyBlock> origPropertyBlocks = new List<MaterialPropertyBlock>();
    public static MaterialPropertyBlock origPropertyBlock;
    private static MaterialPropertyBlock propertyBlock;

    public static void GetEnemy(Enemy enemy)
    {
        enemyMeshes = enemy.GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    public static void GetMeshes(Enemy enemy)
    {
        GetEnemy(enemy);
        for (int i = 0; i < enemyMeshes.Length; i++)
        {
            origPropertyBlock = new MaterialPropertyBlock();
            enemyMesh = enemyMeshes[i];
            enemyMesh.GetPropertyBlock(origPropertyBlock);
            origPropertyBlocks.Add(origPropertyBlock);
            Debug.Log("Length = " + enemyMeshes.Length + ".  Property - " + origPropertyBlocks[i]);
        }
    }
    public static void SetMeshes(Enemy enemy)
    {
        GetEnemy(enemy);
        for (int i = 0; i < enemyMeshes.Length; i++)
        {
            enemyMesh = enemyMeshes[i];
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", newColor);
            enemyMesh.SetPropertyBlock(propertyBlock);
            enemyMesh.material.SetFloat("_Mode", 3);
            HideMe(enemyMesh, propertyBlock);
            enemy.gameObject.tag = "Fallen";
        }
    }
    public static void UnSetMeshes(Enemy enemy)
    {
        GetEnemy(enemy);
        for (int i = 0; i < enemyMeshes.Length; i++)
        {
            enemyMesh = enemyMeshes[i];
            origPropertyBlock = origPropertyBlocks[i];
            Debug.Log("Unhiding");
            enemy.gameObject.tag = "Enemy";
            enemyMesh.material.SetFloat("_Mode", 0);
            HideMe(enemyMesh, origPropertyBlock);
        }
    }
    public static void HideMe (SkinnedMeshRenderer enemyMesh, MaterialPropertyBlock materialProperty)
    {
        enemyMesh.SetPropertyBlock(materialProperty);
    }
}
