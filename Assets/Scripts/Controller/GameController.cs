using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    /// <summary>
    /// This will be in charge of setting the characters, setting up the weapon and the bullet. 
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static List<GameObject> Characters;

        [SerializeField] private Vector2[] playerInitialPositions;

        #region Finish Game Variables

        [SerializeField] private GameObject finishGameUI;
        [SerializeField] private TextMeshProUGUI winnerText;

        [SerializeField] private int numberOfPlayers;
        [SerializeField] private GameObject characterPrefab;

        #endregion

        private void OnEnable()
        {
            FinishGameEvent.OnGameEnded += ShowGameWinner;
        }
        
        public void OnDisable()
        {
            FinishGameEvent.OnGameEnded -= ShowGameWinner;
        }

        private void Awake()
        {
            Characters = new List<GameObject>();
            Time.timeScale = 0;
        }

        /// <summary>
        /// Starts the game by un pausing the game.
        /// </summary>
        public void StartGame()
        {
            for (var i = 0; i < numberOfPlayers; i++)
            {
                var character = Instantiate(characterPrefab);
                character.name = $"Character {i + 1}";
                character.transform.position = playerInitialPositions[i];
                Characters.Add(character);
            }
            
            Time.timeScale = 1;
        }

        /// <summary>
        /// Restarts the game by reloading the scene.
        /// </summary>
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Deactivate the Start button after it is clicked.
        /// </summary>
        public void DeactivateStartButton(GameObject startButton)
        {
            startButton.SetActive(false);
        }

        /// <summary>
        /// When we have a winner, we show it in the UI.
        /// </summary>
        /// <param name="winnerName">Name to the character that won.</param>
        private void ShowGameWinner(string winnerName)
        {
            finishGameUI.SetActive(true);
            winnerText.text = $"Winner is: {winnerName}";
        }
    }
}
