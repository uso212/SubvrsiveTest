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
        //[SerializeField] private List<Character> characterList;

        public static List<GameObject> Characters;

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
            // Characters = new List<GameObject>();
            //
            // for (var i = 0; i < characterList.Count; i++)
            // {
            //     var character = new GameObject($"Character {i + 1}");
            //
            //     //Add the visual component and the character controller to our character.
            //     character.AddComponent<PlayerController>();
            //     character.AddComponent<SpriteRenderer>().sprite = characterList[i].characterSprite;
            //     character.AddComponent<CircleCollider2D>();
            //     
            //     //Getting the component references and assigning the color to the character.
            //     character.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), 
            //         Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            //     var setCharacterAttributes = character.GetComponent<ISetCharacterAttributes>();
            //     var charWeapon = characterList[i].weapon;
            //     
            //     //Sending the data from the scriptable object to the newly created GameObject.
            //     setCharacterAttributes.SetCharacterAttributes(characterList[i].health, characterList[i].moveSpeed,
            //         charWeapon.attackSpeed, charWeapon.attackRange);
            //
            //     //Setting the position and scale of the character into the world.
            //     character.transform.position = characterList[i].spawnPoint;
            //     character.transform.localScale = new Vector2(3f, 3f);
            //     
            //     //Instantiate the bullets as a child of the character and add the bullet pool controller.
            //     character.AddComponent<BulletPoolManager>();
            //     
            //     for (var j = 0; j < 10; j++)
            //     {
            //         //We create a bullet and add the components that it will need.
            //         var bullet = new GameObject("Bullet");
            //         bullet.AddComponent<BulletMovement>();
            //         bullet.AddComponent<Rigidbody2D>();
            //         bullet.AddComponent<SpriteRenderer>().sprite = charWeapon.bullet.bulletSprite;
            //         bullet.AddComponent<CapsuleCollider2D>();
            //
            //         /*
            //          * We set the bullet as trigger so they won't collide with each other
            //          * and set the tag so the player can detect the collision with it.
            //          */
            //         var bulletCapsuleCollider = bullet.GetComponent<CapsuleCollider2D>();
            //         bulletCapsuleCollider.isTrigger = true;
            //         bulletCapsuleCollider.tag = "Bullet";
            //
            //         bullet.transform.localScale = new Vector2(.1f, .1f);
            //
            //         bullet.GetComponent<Rigidbody2D>().gravityScale = 0f;
            //         bullet.transform.parent = character.transform;
            //         var characterPosition = character.transform.position;
            //         bullet.transform.position = new Vector2(characterPosition.x,
            //             characterPosition.y + .45f);
            //         
            //         //We set the speed and damage of the bullet.
            //         var bulletMovement  = bullet.GetComponent<IBulletMovement>();
            //         bulletMovement.SetBulletAttributes(charWeapon.bullet.speed, charWeapon.bullet.damage,
            //             bullet.transform.position);
            //         
            //         bullet.SetActive(false);
            //         
            //         //Add bullet to the pool.
            //         character.GetComponent<BulletPoolManager>().pooledBullets.Add(bullet);
            //     }
            //
            //     //Add character to the list.
            //     Characters.Add(character);
            // }
            
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
