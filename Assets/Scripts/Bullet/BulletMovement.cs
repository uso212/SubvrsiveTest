using Interfaces;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// This will handle the movement of the bullet, and to hold the damage the bullet will make.
    /// </summary>
    public class BulletMovement : MonoBehaviour
    {
        /*
         * This is serialized only to be visible in the inspector, and for purposes of the test.
         * Normally these fields would not have the SerializeField since it's not necessary.
         */
        [SerializeField] private float bulletSpeed;
        [SerializeField] private int bulletDamage;
        
        public Transform targetTransform;
        
        private Rigidbody2D _bulletRigidBody;
        private Vector3 _initialPosition;

        /// <summary>
        /// Each time we enable the bullet we'll reset it to the initial position.
        /// </summary>
        private void OnEnable()
        {
            transform.position = _initialPosition;
        }

        private void Start()
        {
            _bulletRigidBody = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// In this Update function we'll make the bullet travel to it's target to make damage.
        /// And if the target has already been taken out, we put the bullet back into the pool.
        /// </summary>
        private void Update()
        {
            _bulletRigidBody.velocity = 
                (targetTransform.position - transform.position).normalized * bulletSpeed;
            
            if (!targetTransform.gameObject.activeInHierarchy)
                gameObject.SetActive(false);
        }

        /// <summary>
        /// We'll inflict the amount of damaged defined in the bullet Scriptable Object. 
        /// </summary>
        /// <returns></returns>
        public int DamageCharacter()
        {
            return bulletDamage;
        }

        public void SetTarget(Transform target)
        {
            targetTransform = target;
        }
    }
}
