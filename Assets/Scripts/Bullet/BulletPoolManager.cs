using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class BulletPoolManager : MonoBehaviour
    {
        public static BulletPoolManager Instance;
        
        [HideInInspector] public List<GameObject> pooledBullets = new List<GameObject>();

        public GameObject objectToPool;

        [SerializeField] private int amountToPool;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GeneratePool();
        }

        private void GeneratePool()
        {
            for (var i = 0; i < amountToPool; i++)
            {
                var bullet = Instantiate(objectToPool);
                bullet.SetActive(false);
                pooledBullets.Add(bullet);
            }
        }

        public GameObject GetPooledBullet()
        {
            for (var i = 0; i < pooledBullets.Count; i++)
            {
                if (!pooledBullets[i].activeInHierarchy)
                {
                    return pooledBullets[i];
                }
            }

            return null;
        }
    }
}
