using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class BulletPool : MonoBehaviour
    {
        public List<GameObject> pooledBullets = new List<GameObject>();
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
