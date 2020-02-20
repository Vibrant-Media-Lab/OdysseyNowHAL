using CardDirection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Graphics {
    /// <summary>
    /// Behavior of all menu buttons in the main menu (and pause menus).
    /// </summary>
    public class MenuButtonBehavior : MonoBehaviour {
        // another UI object that will replace this element on screen, if clicked
        public GameObject otherMenu;

        // another UI object that will replace this element on screen, if clicked
        public GameObject gameSelect;

        // toggle that determines menu control flow
        public Toggle connectToggle;

        private Transform _parent;

        /// <summary>
        /// On start, set button click listener.
        /// </summary>
        private void Start() {
            gameObject.GetComponent<Button>().onClick.AddListener(ButtonClicked);
            _parent = transform.parent;
        }

        /// <summary>
        /// On button click, either load a scene with the same name as this object or move around menu objects.
        /// </summary>
        private void ButtonClicked() {
            var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                              PlayerPrefs.GetString("P1Input"));
            var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                              PlayerPrefs.GetString("P2Input"));
            var isAI = p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI;
            // if gameSelect is null then this is just a normal button
            if (gameSelect != null) {
                // if the user wants to play with AI then they must be taken to the game
                // select irrespective of connecting an odyssey console.
                if (isAI) {
                    gameSelect.SetActive(true);
                    // get Game selects child based on card name
                    gameSelect.transform.Find(transform.name).gameObject.SetActive(true);
                    _parent.gameObject.SetActive(false);
                }
                // if an odyssey is connected but we don't want to play with AI
                else if (connectToggle.isOn) {
                    // pulls up the calibrations scene
                    SceneManager.LoadScene("Calibration");
                }
                // if an odyssey is not connected and we don't want to play with AI
                else {
                    // pulls up the card based on the button's name
                    SceneManager.LoadScene(gameObject.name.Replace(" ", ""));
                }
            }
            else if (otherMenu != null) {
                switch (_parent.parent.name) {
                    // if we are the Play button in game select, determine if AI has been selected
                    case "Game Select" when transform.name == "Play Button": {
                        SceneManager.LoadScene(_parent.name.Replace(" ", ""));
                        // tells the scene configurer which game to use
                        PlayerPrefs.SetString("game",
                                              _parent.Find("Game Drop Down").Find("Label").GetComponent<Text>().text);
                        return;
                    }
                    // if we are the AI select button in game select, move to AI select
                    case "Game Select" when transform.name == "AI Select Button":
                        // Change AI Select's back button to return to the correct card after selection
                        otherMenu.transform.Find("Back Button").GetComponent<MenuButtonBehavior>().otherMenu =
                            _parent.gameObject;
                        _parent.parent.gameObject.SetActive(false);
                        break;
                }

                // if we are the Back button in game select, make sure to disable other game selections
                if (_parent.name == "Game Select" && transform.name == "Back Button") {
                    foreach (Transform child in transform.parent) {
                        // don't hide the back button or the odyssey now hal logo
                        if (child.name != "Back Button" && child.name != "Inverted_OdysseyNowLogo") {
                            child.gameObject.SetActive(false);
                        }
                    }
                }

                otherMenu.SetActive(true);
                _parent.gameObject.SetActive(false);
            }
            else {
                // pulls up the card based on the button's parent's name
                SceneManager.LoadScene(_parent.name.Replace(" ", ""));
            }
        }
    }
}
