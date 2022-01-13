using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    /// <summary>
    /// This class will handle the attacking, receiving damage and movement.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /*
         * This is serialized only to be visible in the inspector, and for purposes of the test.
         * Normally these fields would not have the SerializeField since it's not necessary.
         */
        [SerializeField] private int health;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float attackRange;
        [SerializeField] private Vector2 spawnPoint;
        
        private string _characterName;

        /*
         * We put this part in the Start() method since we want to perform this chunk of code
         * after we instantiated everything from GameController's Awake() function.
         */ 
        private void Start()
        {
            SetCharacterAttributes();
            transform.position = spawnPoint;
            GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), 
                Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            
            _characterName = gameObject.name;

            //We'll perform an attack at the speed it can attack 0.5 seconds after we start the game. 
            InvokeRepeating(nameof(Attack), 0.5f, attackSpeed);
        }

        private void Attack()
        {
            //Get the character you are going to attack.
            var range = Random.Range(0, GameController.Characters.Count);
            var characterToAttack = GameController.Characters[range];

            /*
             * If the character to attack is not in range or we're targeting ourselves
             * we try to attack again.
             */

            if (Vector2.Distance(characterToAttack.transform.position, transform.position)
                > attackRange || characterToAttack.name.Equals(_characterName))
            {
                return;
            }

            //We'll set the direction where the character will attack and rotate it at it's move speed.
            const int offset = -90;
            var direction = characterToAttack.transform.position - transform.position;
            direction.Normalize();
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset) * moveSpeed * Time.deltaTime);

            /*
             * We get the next available bullet from the pool and set the target to where it'll attack,
             * then we'll activate the bullet.
             */
            var bullet = BulletPoolManager.Instance.GetPooledBullet();
            bullet.transform.position = new Vector2(spawnPoint.x, spawnPoint.y + .45f);

            var bulletMovement = bullet.GetComponent<BulletMovement>();
            bulletMovement.SetTarget(characterToAttack.transform);

            if (bullet == null) return;
            bullet.SetActive(true);
        }

        /// <summary>
        /// We subtract the damage received from the health.
        /// If we're the last one standing. We won the game.
        /// </summary>
        /// <param name="damageTaken"></param>
        private void TakeDamage(int damageTaken)
        {
            health -= damageTaken;

            if (health <= 0)
            {
                CancelInvoke(nameof(Attack));
                GameController.Characters.Remove(gameObject);
                gameObject.SetActive(false);
                
                //When the game is finished we invoke the event that will show the winners name.
                if (GameController.Characters.Count == 1)
                    FinishGameEvent.OnGameEnded?.Invoke(GameController.Characters[0].name);
            }
        }

        private void SetCharacterAttributes()
        {
            health = 100;
            moveSpeed = Random.Range(1, 6);
            //attackSpeed =
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            /*
             * If a bullet entered the character, take damage and return the bullet to the pool.
             * (We use Unity's CompareTag which is 5x faster than C# .Equals and the == expression).
             */ 
            if (other.CompareTag("Bullet"))
            {
                TakeDamage(other.GetComponent<BulletMovement>().DamageCharacter());
                other.gameObject.SetActive(false);;
            }
        }
    }
}
