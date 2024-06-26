using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshHandler
{
    private static SkinnedMeshRenderer[] enemyMeshes;
    private static SkinnedMeshRenderer enemyMesh;
    private static SkinnedMeshRenderer[] turretMeshes;
    private static SkinnedMeshRenderer turretMesh;
    private static Color newColor;
    public static List<MaterialPropertyBlock> origPropertyBlocks = new List<MaterialPropertyBlock>();
    public static MaterialPropertyBlock origPropertyBlock;
    private static MaterialPropertyBlock propertyBlock;

    public static void GetEnemy(Enemy enemy)
    {
        enemyMeshes = enemy.GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    public static void GetEnemyMeshes(Enemy enemy)
    {
        GetEnemy(enemy);
        for (int i = 0; i < enemyMeshes.Length; i++)
        {
            origPropertyBlock = new MaterialPropertyBlock();
            enemyMesh = enemyMeshes[i];
            enemyMesh.GetPropertyBlock(origPropertyBlock);
            origPropertyBlocks.Add(origPropertyBlock);
        }
        UnSetEnemyMeshes(enemy);
    }
    public static void SetEnemyMeshes(Enemy enemy)
    {
        GetEnemy(enemy);
        for (int i = 0; i < enemyMeshes.Length; i++)
        {
            enemyMesh = enemyMeshes[i];
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", newColor);
            enemyMesh.SetPropertyBlock(propertyBlock);
            // enemyMesh.material.SetFloat("_Mode", 3);
            ToFadeMode(enemyMesh.material);
            HideEneMe(enemyMesh, propertyBlock);
            enemy.gameObject.tag = "Fallen";
        }
    }
    public static void UnSetEnemyMeshes(Enemy enemy)
    {
        GetEnemy(enemy);
        for (int i = 0; i < enemyMeshes.Length; i++)
        {
            enemyMesh = enemyMeshes[i];
            origPropertyBlock = origPropertyBlocks[i];
            ToOpaqueMode(enemyMesh.material);
            // enemyMesh.material.SetFloat("_Mode", 0);
            HideEneMe(enemyMesh, origPropertyBlock);
            enemy.gameObject.tag = "Enemy";
        }
    }

    public static void ToOpaqueMode(Material material)
    {
        material.SetOverrideTag("RenderType", "");
        // material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
   
    public static void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        // material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
    }

    public static void HideEneMe (SkinnedMeshRenderer enemyMesh, MaterialPropertyBlock materialProperty)
    {
        enemyMesh.SetPropertyBlock(materialProperty);
    }

    public static void GetTurret(Turret turret)
    {
        turretMeshes = turret.GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    public static void GetTurretMeshes(Turret turret)
    {
        GetTurret(turret);
        for (int i = 0; i < turretMeshes.Length; i++)
        {
            origPropertyBlock = new MaterialPropertyBlock();
            turretMesh = turretMeshes[i];
            turretMesh.GetPropertyBlock(origPropertyBlock);
            origPropertyBlocks.Add(origPropertyBlock);
        }
        UnSetTurretMeshes(turret);
    }
    public static void SetTurretMeshes(Turret turret)
    {
        GetTurret(turret);
        for (int i = 0; i < turretMeshes.Length; i++)
        {
            turretMesh = turretMeshes[i];
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", newColor);
            turretMesh.SetPropertyBlock(propertyBlock);
            ToFadeMode(turretMesh.material);
            HideTurretMe(turretMesh, propertyBlock);
            // turretMesh.material.SetFloat("_Mode", 3);
            turret.gameObject.tag = "Fallen";
        }
    }
    public static void UnSetTurretMeshes(Turret turret)
    {
        GetTurret(turret);
        for (int i = 0; i < turretMeshes.Length; i++)
        {
            turretMesh = turretMeshes[i];
            origPropertyBlock = origPropertyBlocks[i];
            ToOpaqueMode(turretMesh.material);
            HideTurretMe(turretMesh, origPropertyBlock);
            // turretMesh.material.SetFloat("_Mode", 0);
            turret.gameObject.tag = "Turret";
        }
    }
    public static void HideTurretMe (SkinnedMeshRenderer turretMesh, MaterialPropertyBlock materialProperty)
    {
        turretMesh.SetPropertyBlock(materialProperty);
    }

}
