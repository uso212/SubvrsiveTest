using System;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 2)]
    public class Weapon : ScriptableObject
    {
        public float attackSpeed;
        public float attackRange;
        public Bullet bullet;
    }
}
