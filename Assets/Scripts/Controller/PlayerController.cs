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

        [SerializeField] private float bulletSpeed;
        [SerializeField] private int bulletDamage;
        
        private string _characterName;
        private GameObject _bullet; //The bullet being shot.

        /*
         * We put this part in the Start() method since we want to perform this chunk of code
         * after we instantiated everything from GameController's Awake() function.
         */ 
        private void Start()
        {
            SetCharacterAttributes();
            
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
            
            SetCharacterDirection(characterToAttack);
            ShootBullet(characterToAttack);
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

        /// <summary>
        /// In this function we set the initial values for our character.
        /// </summary>
        private void SetCharacterAttributes()
        {
            /*
             * We always want to have the same health and attack range for all the players. But we set the
             * bullet speed and damage from here since we have one single pool of bullets for all the players
             * and each player has it's own attack damage and speed.
             */
            health = 100;
            moveSpeed = Random.Range(1, 6);
            attackSpeed = Random.Range(1, 6);
            attackRange = 10;
            bulletSpeed = Random.Range(1, 10);
            bulletDamage = Random.Range(2, 8);
        }

        /// <summary>
        /// Set the direction where the character will attack and rotate it at it's move speed.
        /// </summary>
        /// <param name="characterToAttack"></param>
        private void SetCharacterDirection(GameObject characterToAttack)
        {
            const int offset = -90;
            var position = transform.position;
            var direction = characterToAttack.transform.position - position;
            direction.Normalize();
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset) * moveSpeed * Time.deltaTime);
        }
        
        /// <summary>
        /// We get the next available bullet from the pool and set the target to where it'll attack,
        /// then we'll activate the bullet.
        /// </summary>
        /// <param name="characterToAttack"></param>
        private void ShootBullet(GameObject characterToAttack)
        {
            _bullet = BulletPoolManager.instance.GetPooledBullet();
            _bullet.transform.position = new Vector2(transform.position.x, transform.position.y + .45f);

            var bulletMovement = _bullet.GetComponent<BulletMovement>();
            bulletMovement.bulletSpeed = bulletSpeed;
            bulletMovement.bulletDamage = bulletDamage;
            bulletMovement.SetTarget(characterToAttack.transform);

            if (_bullet == null) return;
            _bullet.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            /*
             * If a bullet entered the character, and that bullet it's not the one I shot,
             * take damage and return the bullet to the pool.
             * (We use Unity's CompareTag which is 5x faster than C# .Equals and the == expression,
             * but we use if for comparing GameObjects.).
             */ 
            if (other.CompareTag("Bullet") && !other.gameObject.Equals(_bullet))
            {
                TakeDamage(other.GetComponent<BulletMovement>().DamageCharacter());
                other.gameObject.SetActive(false);;
            }
        }
    }
}
