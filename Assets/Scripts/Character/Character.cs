using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerAttributes
{
    public class Character : MonoBehaviour
    {
        public int health;
        public float moveSpeed;
        public Weapon weapon;
        public Vector2 spawnPoint;

        private void Start()
        {
            transform.position = spawnPoint;
            GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), 
                Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
}
