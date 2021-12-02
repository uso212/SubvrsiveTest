using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "Inventory/Bullet", order = 1)]
    public class Bullet : ScriptableObject
    {
        public Sprite bulletSprite;
        public int damage;
        public float speed;
    }
}
