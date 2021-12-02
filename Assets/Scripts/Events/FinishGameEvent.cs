using UnityEngine;

namespace Events
{
    /// <summary>
    /// Class that holds the End of Game event.
    /// </summary>
    public class FinishGameEvent : MonoBehaviour
    {
        public delegate void GameEvent(string winnerName);

        /// <summary>
        /// This event will be triggered when the game has ended and
        /// a winner has been decided.
        /// </summary>
        public static GameEvent OnGameEnded;
    }
}
