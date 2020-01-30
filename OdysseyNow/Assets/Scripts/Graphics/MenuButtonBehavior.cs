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
        /// On start, set button click lisstener.
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
                    // get Game selects child based on card name
                    gameSelect.transform.Find(transform.name).gameObject.SetActive(true);
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
                // if we are the back button in game select, hide all of game
                // selects children before moving back to card select
                if (gameObject.transform.parent.name == "Game Select") {
                    foreach(Transform child in gameObject.transform.parent) {
                        // don't hide the back button
                        if(child.transform.name != "Back Button") {
                            child.gameObject.SetActive(false);
                        }
                    }
                }
                otherMenu.SetActive(true);
                transform.parent.gameObject.SetActive(false);
            }
            else {
                // pulls up the card based on the button's parent's name
                SceneManager.LoadScene(transform.parent.name.Replace(" ", ""));
            }
        }
    }
}
