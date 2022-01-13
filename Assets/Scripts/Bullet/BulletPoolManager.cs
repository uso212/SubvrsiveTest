using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class BulletPoolManager : MonoBehaviour
    {
        public static BulletPoolManager instance;
        
        [HideInInspector] public List<GameObject> pooledBullets = new List<GameObject>();

        public GameObject objectToPool;

        [SerializeField] private int amountToPool;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GeneratePool();
        }

        /// <summary>
        /// We generate a pool of bullets to be used by all the character in-game.
        /// </summary>
        private void GeneratePool()
        {
            for (var i = 0; i < amountToPool; i++)
            {
                var bullet = Instantiate(objectToPool);
                bullet.SetActive(false);
                pooledBullets.Add(bullet);
            }
        }

        /// <summary>
        /// We look for an available bullet in the pool and return it to the player to be shot.
        /// </summary>
        /// <returns></returns>
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
