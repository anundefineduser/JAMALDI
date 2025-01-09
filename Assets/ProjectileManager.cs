using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public bool isStatic;
    public static ProjectileManager instance;
    private void Awake()
    {
        if (isStatic)
            instance = this;
    }

    public List<GameObject> projectiles;

    public GameObject GetProjectile()
    {
        return projectiles[Random.Range(0, projectiles.Count)];
    }
}
