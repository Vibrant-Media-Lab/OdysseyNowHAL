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
                // if we are the back button in AI select, enable or disable play based on whether or not AI was selected
                // this works because AI select back button knows which game selection menu to return to
                if (transform.parent.name == "AI Select" && transform.name == "Back Button") {
                    if(transform.parent.Find("P1 AI Toggle").GetComponent<Toggle>().isOn || transform.parent.Find("P2 AI Toggle").GetComponent<Toggle>().isOn) {
                        otherMenu.transform.Find("Play Button").gameObject.SetActive(true);
                    }
                    else {
                        otherMenu.transform.Find("Play Button").gameObject.SetActive(false);
                    }
                    otherMenu.transform.parent.gameObject.SetActive(true);
                }
                // if we are the Play button in game select, determine if AI has been selected
                if(transform.parent.parent.name == "Game Select" && transform.name == "Play Button") {
                    Transform aiSelect = transform.root.Find("AI Select");
                    // if AI has been selected, load the scene cooresponding to the card of the game
                    // otherwise, show a message to the player that they must select AI
                    if(aiSelect.Find("P1 AI Toggle").GetComponent<Toggle>().isOn || aiSelect.Find("P2 AI Toggle").GetComponent<Toggle>().isOn) {
                        Debug.Log(transform.parent.name.Replace(" ", ""));
                        SceneManager.LoadScene(transform.parent.name.Replace(" ", ""));
                        // tells the scene configurer which game to use
                        PlayerPrefs.SetString("game", transform.parent.Find("Game Drop Down").Find("Label").GetComponent<Text>().text);
                    }
                    else {
                        Debug.Log("Must pick at least 1 AI");
                    }
                    return;
                }
                // if we are the AI select button in game select, move to AI select
                if (transform.parent.parent.name == "Game Select" && transform.name == "AI Select Button") {
                    // Change AI Select's back button to return to the correct card after selection
                    otherMenu.transform.Find("Back Button").GetComponent<MenuButtonBehavior>().otherMenu = transform.parent.gameObject;
                    transform.parent.parent.gameObject.SetActive(false);
                }
                // if we are the Back button in game select, make sure to disable other game selections
                if (transform.parent.name == "Game Select" && transform.name == "Back Button") {
                    foreach(Transform child in transform.parent) {
                        // don't hide the back button or the odyssey now hal logo
                        if(child.name != "Back Button" && child.name != "Inverted_OdysseyNowLogo") {
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
