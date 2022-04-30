using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardDirection
{
    /// <summary>
    /// The primary director of card scenes. This manages a lot of things.
    /// </summary>
    public class Director : MonoBehaviour
    {
        //instance for a singleton
        public static Director instance;
        //keeps track of whether the game is paused; referenced by other classes
        public bool paused = false;
        //references to the two pause menus; one for playground, another for all cards
        public GameObject menu;
        public GameObject plainMenu;
        //the card number; if it's not a valid number, the director will assume this is the playground.
        public int cardNumber;

        //Makes this a singleton
        private void Awake()
        {
            if (Director.instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        /* For reference:
         * public void UpdateAll(
         *      bool _secondPlayer, 
         *      bool _includeBall, 
         *      bool _includeWall, 
         *      int _wallPosition: 0, 1, 2
         *      int _wallHeight: 0, 1
         *      string _wallBallCollision : Pass, Extinguish, None
         *      string _p1P2Collision : None, Extinguish
         *      string _p2BallCollision : Bounce, Extinguish
         *      bool _p2Inertia,
         *      string _onResetExtinguish : None, P1 & P2, P2. Ball
         * )
         */
        
        /// <summary>
        /// On start, set parameters based on card number, set settings based on main menu configuration, and choose the right pause menu
        /// </summary>
        private void Start()
        {
            switch (cardNumber)
            {
                case 1:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, true, 1, 1, "Pass", "None", "Bounce", false, "None");
                    break;
                case 2:
                    menu.GetComponent<MenuDirector>().UpdateAll(false, false, false, 1, 1, "Pass", "None", "Bounce", false, "None");
                    break;
                case 3:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, false, 1, 1, "Pass", "None", "Bounce", false, "None");
                    break;
                case 4:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, false, false, 1, 1, "Pass", "Extinguish", "Bounce", false, "P2");
                    break;
                case 5:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, false, 1, 1, "Pass", "None", "Extinguish", false, "P2");
                    break;
                case 6:
                    menu.GetComponent<MenuDirector>().UpdateAll(false, false, false, 1, 1, "Pass", "None", "Bounce", true, "None");
                    break;
                case 7:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, true, 1, 0, "Extinguish", "None", "Bounce", false, "Ball");
                    break;
                case 8:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, true, 0, 1, "Bounce", "None", "Bounce", false, "None");
                    break;
                case 9:
                    menu.GetComponent<MenuDirector>().UpdateAll(false, false, false, 1, 1, "Pass", "None", "Bounce", false, "P2");
                    break;
                case 10:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, false, 1, 1, "Pass", "None", "Bounce", false, "Ball");
                    break;
                case 11:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, true, 0, 1, "Bounce", "None", "Bounce", false, "None");
                    break;
                case 12:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, false, false, 1, 1, "Pass", "Extinguish", "Bounce", true, "P2");
                    break;
                case 13:
                    //TODO - implement the weird stuff this card does
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, true, 0, 1, "Bounce", "None", "Bounce", false, "None");
                    break;
                case 14:
                    //TODO - implement the weird stuff this card does
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, true, 1, 1, "Pass", "None", "Bounce", false, "None");
                    break;
                case 15:
                    //TODO - implement the weird stuff this card does
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, false, 1, 1, "Pass", "None", "Bounce", true, "None");
                    break;
                case 16:
                    menu.GetComponent<MenuDirector>().UpdateAll(true, false, false, 1, 1, "Pass", "None", "Bounce", false, "P1 & P2");
                    break;
                case 17: // P1-P2 and P2-Ball collisions are handled independently
                    menu.GetComponent<MenuDirector>().UpdateAll(true, true, true, 0, 1, "Extinguish", "None", "None", true, "None");
                    break;
            }
            menu.SetActive(false);
            if (cardNumber > 0)
            {
                menu = plainMenu;
            }

            if (ElementSettings.instance != null)
            {
                ElementSettings.instance.updateAllSizes();
            }
        }

        /// <summary>
        /// Loads neighboring scene; used by '[]' controls to dynamically iterate through cards
        /// </summary>
        /// <param name="num"></param>
        void LoadCard(int num){
            if (num > 17)
                num = 1;
            else if (num <= 0)
                num = 17;

            SceneManager.LoadScene("Card" + num);
        }

        /// <summary>
        /// Handles all player input related to scene navigation.
        /// If the player presses escape, pause the game.
        /// If the player presses [ or ], iterate to previous or next scene.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                accessMenu();
            }

            if (Input.GetKeyDown(KeyCode.LeftBracket)){
                LoadCard(cardNumber - 1);
            }

            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                LoadCard(cardNumber + 1);
            }
        }

        /// <summary>
        /// Pause or unpause the game
        /// </summary>
        public void accessMenu()
        {
            paused = !paused;
            menu.SetActive(paused);
        }

        /// <summary>
        /// Return to main menu scene.
        /// </summary>
        public void MainMenu()
        {
            SceneManager.LoadScene("_MainMenu");
        }

        /// <summary>
        /// Redundant method; TODO: review code to check dependancies then remove
        /// </summary>
        /// <param name="player"></param>
        /// <param name="scheme"></param>
        public void UpdateControls(int player, string scheme)
        {

        }

    }
}
