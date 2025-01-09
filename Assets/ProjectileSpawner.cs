using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameControllerScript gc;

    public ProjectileManager projectileOverride;

    public GameObject[] AIPoints;

    public float spawnCooldown;

    public float maxObjects;

    public float objects;

    public bool changeCooldown;

    private void Update()
    {
        ProjectileManager manager = projectileOverride != null ? projectileOverride : ProjectileManager.instance;
        if (spawnCooldown > 0f)
        {
            spawnCooldown -= Time.deltaTime;
        }
        if (spawnCooldown <= 0f)
        {
            if (objects < maxObjects)
            {
                GameObject spawnPoint = AIPoints[Random.Range(0, AIPoints.Length - 1)];

                GameObject projectileSpawn = Object.Instantiate(manager.GetProjectile(), spawnPoint.transform.position, spawnPoint.transform.rotation);
                projectileSpawn.transform.position += Vector3.up * 4f;
                projectileSpawn.GetComponent<ProjectileScript>().spawner = this;

                objects += 1f;
            }
            if (changeCooldown)
            {
                spawnCooldown = Random.Range(2f, 5f);
            }
        }
    }
}