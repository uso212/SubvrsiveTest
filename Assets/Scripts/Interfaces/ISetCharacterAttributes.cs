using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// This interface set the scriptable object attributes into the character.
    /// </summary>
    internal interface ISetCharacterAttributes
    {
        void SetCharacterAttributes(int health, float moveSpeed, float attackSpeed, 
            float attackRange);
    }
}
