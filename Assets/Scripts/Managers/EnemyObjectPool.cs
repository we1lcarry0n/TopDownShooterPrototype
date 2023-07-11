using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    public static EnemyObjectPool Instance;
    private List<GameObject> poolingList;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesToInstantiate;

    private void Awake()
    {
        Instance = this;

        poolingList = new List<GameObject>();
        GameObject enemy;

        for (int i = 0; i < enemiesToInstantiate; i++)
        {
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity, this.transform);
            enemy.SetActive(false);
            poolingList.Add(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        foreach (GameObject enemy in poolingList)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }
        return null;
    }
}
