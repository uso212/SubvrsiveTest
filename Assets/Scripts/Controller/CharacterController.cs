using Events;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    /// <summary>
    /// This class will handle the attacking, receiving damage and movement.
    /// </summary>
    public class CharacterController : MonoBehaviour, ISetCharacterAttributes
    {
        /*
         * This is serialized only to be visible in the inspector, and for purposes of the test.
         * Normally these fields would not have the SerializeField since it's not necessary.
         */
        [SerializeField] private int _health;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _attackRange;
        
        private BulletPool _bulletPool;
        private string _characterName;

        /*
         * We put this part in the Start() method since we want to perform this chunk of code
         * after we instantiated everything from GameController's Awake() function.
         */ 
        private void Start()
        {
            _characterName = gameObject.name;
            _bulletPool = GetComponent<BulletPool>();

            //We'll perform an attack at the speed it can attack 0.5 seconds after we start the game. 
            InvokeRepeating(nameof(Attack), 0.5f, _attackSpeed);
        }

        /// <summary>
        /// Set the attributes from the GameController scriptable objects into the character.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="attackSpeed"></param>
        /// <param name="attackRange"></param>
        public void SetCharacterAttributes(int health, float moveSpeed, float attackSpeed, float attackRange)
        {
            _health = health;
            _moveSpeed = moveSpeed;
            _attackSpeed = attackSpeed;
            _attackRange = attackRange;
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
                > _attackRange || characterToAttack.name.Equals(_characterName))
            {
                return;
            }

            //We'll set the direction where the character will attack and rotate it at it's move speed.
            const int offset = -90;
            var direction = characterToAttack.transform.position - transform.position;
            direction.Normalize();
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset) * _moveSpeed * Time.deltaTime);

            /*
             * We get the next available bullet from the pool and set the target to where it'll attack,
             * then we'll activate the bullet.
             */
            var bullet = _bulletPool.GetPooledBullet();

            var bulletMovement = bullet.GetComponent<IBulletMovement>();
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
            _health -= damageTaken;

            if (_health <= 0)
            {
                CancelInvoke(nameof(Attack));
                GameController.Characters.Remove(gameObject);
                gameObject.SetActive(false);
                
                //When the game is finished we invoke the event that will show the winners name.
                if (GameController.Characters.Count == 1)
                    FinishGameEvent.OnGameEnded?.Invoke(GameController.Characters[0].name);
            }
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
