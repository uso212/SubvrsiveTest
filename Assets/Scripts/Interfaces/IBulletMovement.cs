using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// This interface will communicate the speed, damage and target to a particular bullet.
    /// </summary>
    internal interface IBulletMovement
    {
        void SetBulletAttributes(float speed, int damage, Vector3 bulletInitialPosition);
        void SetTarget(Transform targetTransform);
    }
}
