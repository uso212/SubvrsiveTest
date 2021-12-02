using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Character", menuName = "Inventory/Character", order = 3)]
    public class Character : ScriptableObject
    {
        public int health;
        public float moveSpeed;
        public Sprite characterSprite;
        public Weapon weapon;
        public Vector2 spawnPoint;
    }
}
