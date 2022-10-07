using System;
using MultiplayerWithBindingsExample;
using UnityEngine;
using UnityEngine.UI;

namespace CardDirection {
    /// <summary>
    /// Manages the configuration of input methods.
    /// </summary>
    public class LocalInputManager : MonoBehaviour {
        //The possible control schemes
        public enum ControlScheme {
            Keyboard,
            Traditional,
            OdysseyCon,
            OriginalConsole,
            AI,
            OdysseyConLegacy
        }

        // Dropdowns for the two options
        public Dropdown p1Option;
        public Dropdown p2Option;

        // slider to choose P1 AI difficulty
        public Slider difficultySliderP1;
        public Text sliderTextP1;

        // slider to choose P2 AI difficulty
        public Slider difficultySliderP2;
        public Text sliderTextP2;

        //singleton instance
        //public static LocalInputManager instance;


        private void Awake() {
            //PlayerPrefs.SetString("P1Input", ControlScheme.Keyboard.ToString());
            //PlayerPrefs.SetString("P2Input", ControlScheme.Keyboard.ToString());
            
            //get the current input selection for player 1 and adjust the dropdown to match the value
            //this stores the input type
            if (PlayerPrefs.GetString("P1Input").ToString() == ControlScheme.Keyboard.ToString()) { p1Option.value = 0; }
            else if (PlayerPrefs.GetString("P1Input").ToString() == ControlScheme.Traditional.ToString()) { p1Option.value = 1; }
            else if (PlayerPrefs.GetString("P1Input").ToString() == ControlScheme.OdysseyCon.ToString()) { p1Option.value = 2; }
            else if (PlayerPrefs.GetString("P1Input").ToString() == ControlScheme.OriginalConsole.ToString()) { p1Option.value = 3; }
            else if (PlayerPrefs.GetString("P1Input").ToString() == ControlScheme.AI.ToString()) { p1Option.value = 4; }
            else if (PlayerPrefs.GetString("P1Input").ToString() == ControlScheme.OdysseyConLegacy.ToString()) { p1Option.value=5; }
            else { p1Option.value = 0; }
            
            //get the current input selection for player 2 and adjust the dropdown to match the value
            //this stores the input type
            if (PlayerPrefs.GetString("P2Input").ToString() == ControlScheme.Keyboard.ToString()) { p2Option.value = 0 ; }
            else if (PlayerPrefs.GetString("P2Input").ToString() == ControlScheme.Traditional.ToString()) { p2Option.value = 1 ; }
            else if (PlayerPrefs.GetString("P2Input").ToString() == ControlScheme.OdysseyCon.ToString()) { p2Option.value = 2 ; }
            else if (PlayerPrefs.GetString("P2Input").ToString() == ControlScheme.OriginalConsole.ToString()) { p2Option.value = 3 ; }
            else if (PlayerPrefs.GetString("P2Input").ToString() == ControlScheme.AI.ToString()) { p2Option.value = 4 ; }
            else if (PlayerPrefs.GetString("P2Input").ToString() == ControlScheme.OdysseyConLegacy.ToString()) { p2Option.value = 5 ; }
            else { p2Option.value = 0 ; }


            PlayerPrefs.SetInt("ai1", 1);
            PlayerPrefs.SetInt("ai2", 1);
        }

        /// <summary>
        /// On start, set listeners.
        /// </summary>
        private void Start() {
            p1Option.onValueChanged.AddListener(value => P1ControlChanged());
            p2Option.onValueChanged.AddListener(value => P2ControlChanged());
            difficultySliderP1.onValueChanged.AddListener(value => P1SliderChanged());
            difficultySliderP2.onValueChanged.AddListener(value => P2SliderChanged());
            // hide the sliders' parent by default
            difficultySliderP1.transform.parent.gameObject.SetActive(false);
            difficultySliderP2.transform.parent.gameObject.SetActive(false);

            /*DontDestroyOnLoad(gameObject);

            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }*/
        }

        private void P1SliderChanged() {
            var value = (int) difficultySliderP1.value;
            sliderTextP1.text = "Difficulty: " + value;
            PlayerPrefs.SetInt("ai1", value);
        }

        private void P2SliderChanged() {
            var value = (int) difficultySliderP2.value;
            sliderTextP2.text = "Difficulty: " + value;
            PlayerPrefs.SetInt("ai2", value);
        }


        private void P1ControlChanged() {
            switch (p1Option.value) {
                case 0:
                    SetP1UserOptions(ControlScheme.Keyboard.ToString());
                    break;
                case 1:
                    SetP1UserOptions(ControlScheme.Traditional.ToString());
                    break;
                case 2:
                    SetP1UserOptions(ControlScheme.OdysseyCon.ToString());
                    break;
                case 3:
                    SetP1UserOptions(ControlScheme.OriginalConsole.ToString());
                    break;
                case 4:
                    PlayerPrefs.SetString("P1Input", ControlScheme.AI.ToString());
                    difficultySliderP1.transform.parent.gameObject.SetActive(true);
                    break;
                case 5:
                    SetP1UserOptions(ControlScheme.OdysseyConLegacy.ToString());
                    break;
            }
        }

        private void P2ControlChanged()
        {
            Debug.Log("DEBUG: In control change method");
            switch (p2Option.value) {
                case 0:
                    SetP2UserOptions(ControlScheme.Keyboard.ToString());
                    break;
                case 1:
                    SetP2UserOptions(ControlScheme.Traditional.ToString());
                    break;
                case 2:
                    SetP2UserOptions(ControlScheme.OdysseyCon.ToString());
                    break;
                case 3:
                    SetP2UserOptions(ControlScheme.OriginalConsole.ToString());
                    break;
                case 4:
                    PlayerPrefs.SetString("P2Input", ControlScheme.AI.ToString());
                    difficultySliderP2.transform.parent.gameObject.SetActive(true);
                    break;
                case 5:
                    SetP2UserOptions(ControlScheme.OdysseyConLegacy.ToString());
                    break;
            }
        }

        private void SetP1UserOptions(string controlOption) {
            PlayerPrefs.SetString("P1Input", controlOption);
            difficultySliderP1.transform.parent.gameObject.SetActive(false);
        }

        private void SetP2UserOptions(string controlOption) {
            PlayerPrefs.SetString("P2Input", controlOption);
            difficultySliderP2.transform.parent.gameObject.SetActive(false);
        }
    }
}
