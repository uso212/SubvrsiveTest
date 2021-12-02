using System;
using Interfaces;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// This will handle the movement of the bullet, and to hold the damage the bullet will make.
    /// </summary>
    public class BulletMovement : MonoBehaviour, IBulletMovement
    {
        /*
         * This is serialized only to be visible in the inspector, and for purposes of the test.
         * Normally these fields would not have the SerializeField since it's not necessary.
         */
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private int _bulletDamage;
        [SerializeField] private Transform _targetTransform;
        
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
                (_targetTransform.position - transform.position).normalized * _bulletSpeed;
            
            if (!_targetTransform.gameObject.activeInHierarchy)
                gameObject.SetActive(false);
        }

        /// <summary>
        /// We set the speed and damage that the bullet will have.
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="bulletInitialPosition"></param>
        public void SetBulletAttributes(float speed, int damage, Vector3 bulletInitialPosition)
        {
            _bulletSpeed = speed;
            _bulletDamage = damage;
            _initialPosition = bulletInitialPosition;
        }

        /// <summary>
        /// We'll inflict the amount of damaged defined in the bullet Scriptable Object. 
        /// </summary>
        /// <returns></returns>
        public int DamageCharacter()
        {
            return _bulletDamage;
        }

        /// <summary>
        /// We set the target that this bullet will inflict damage to.
        /// </summary>
        /// <param name="targetTransform"></param>
        public void SetTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }
    }
}
