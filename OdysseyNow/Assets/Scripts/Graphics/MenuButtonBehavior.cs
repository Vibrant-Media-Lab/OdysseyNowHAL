using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Graphics
{
    /// <summary>
    /// Behavior of all menu buttons in the main menu (and pause menus).
    /// </summary>
    public class MenuButtonBehavior : MonoBehaviour
    {
        // another UI object that will replace this element on screen, if clicked
        public GameObject otherMenu;
        // another UI object that will replace this element on screen, if clicked
        public GameObject gameSelect;
        // toggle that determines menu control flow
        public Toggle aiToggle;
        // toggle that determines menu control flow
        public Toggle connectToggle;

        /// <summary>
        /// On start, set button click listener.
        /// </summary>
        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(ButtonClicked);
        }

        /// <summary>
        /// On button click, either load a scene with the same name as this object or move around menu objects.
        /// </summary>
        void ButtonClicked()
        {
            // if gameSelect is null then this is just a normal button
            if(gameSelect != null) {
                // if the user wants to play with AI then they must be taken to the game
                // select irrespective of connecting an odyssey console.
                if(aiToggle.isOn) {
                    gameSelect.SetActive(true);
                    transform.parent.gameObject.SetActive(false);
                }
                // if an odyssey is connected but we don't want to play with AI
                else if(connectToggle.isOn)
                {
                    // pulls up the calibrations scene
                    SceneManager.LoadScene("Calibration");
                }
                // if an odyssey is not connected and we don't want to play with AI
                else
                {
                    // pulls up the card based on the button's name
                    SceneManager.LoadScene(gameObject.name.Replace(" ", ""));
                }
            }
            else if (otherMenu != null)
            {
                otherMenu.SetActive(true);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
