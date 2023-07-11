using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : MonoBehaviour
{
    public static BulletObjectPool Instance;

    private List<GameObject> poolingList;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletToInstantiate;

    private void Awake()
    {
        Instance = this;

        poolingList = new List<GameObject>();
        GameObject bullet;

        for (int i = 0; i< bulletToInstantiate; i++)
        {
            bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, this.transform);
            bullet.SetActive(false);
            poolingList.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in poolingList)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }
}
