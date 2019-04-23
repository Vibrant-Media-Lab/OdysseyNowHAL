using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardDirection
{
    public class Director : MonoBehaviour
    {
        public static Director instance;
        public bool paused = false;
        public GameObject menu;
        public GameObject plainMenu;
        public int cardNumber;

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

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                accessMenu();
            }
        }

        public void accessMenu()
        {
            paused = !paused;
            menu.SetActive(paused);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void UpdateControls(int player, string scheme)
        {

        }
    }
}
