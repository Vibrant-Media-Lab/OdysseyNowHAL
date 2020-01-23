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
    public class ToggleButtonBehavior : MonoBehaviour
    {
        // slider to choose AI difficulty
        public Slider difficultySlider;

        /// <summary>
        /// On start, set button click listener.
        /// </summary>
        void Start()
        {
            gameObject.GetComponent<Toggle>().onValueChanged.AddListener(ToggleClicked);
        }

        /// <summary>
        /// On toggle, hide or show the slider
        /// </summary>
        void ToggleClicked(float num)
        {
            if(difficultySlider != null && difficultySlider.gameObject.active) {
                difficultySlider.gameObject.SetActive(false);
            } else if(difficultySlider != null && !difficultySlider.gameObject.active) {
                difficultySlider.gameObject.SetActive(true);
            }
        }
    }
}
