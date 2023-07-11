using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyObjectPool enemyPool;
    private Transform player;
    private Bounds bounds;

    private bool mustBeSpawned;

    [SerializeField] private float minimumDistanceToPlayer;

    [SerializeField] private float minTimeSpawn;
    [SerializeField] private float maxTimeSpawn;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyPool = EnemyObjectPool.Instance;
        bounds = GetComponent<Collider2D>().bounds;
        mustBeSpawned = true;
    }

    private void Update()
    {
        if (!mustBeSpawned)
        {
            return;
        }
        StartCoroutine(NextSpawnTimerCoroutine(Random.Range(minTimeSpawn, maxTimeSpawn)));
    }

    private Vector3 DetermineRandomPosition()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0);
        }
        while (Vector3.Distance(player.position, position) < minimumDistanceToPlayer);
        return position;
    }

    private void SpawnEnemy()
    {
        GameObject enemy = enemyPool.GetEnemy();
        enemy.transform.position = DetermineRandomPosition();
        enemy.transform.rotation = Quaternion.identity;
        enemy.SetActive(true);
    }

    private IEnumerator NextSpawnTimerCoroutine(float spawnCooldown)
    {
        mustBeSpawned = false;
        yield return new WaitForSeconds(spawnCooldown);
        mustBeSpawned = true;
        SpawnEnemy();
    }
}
