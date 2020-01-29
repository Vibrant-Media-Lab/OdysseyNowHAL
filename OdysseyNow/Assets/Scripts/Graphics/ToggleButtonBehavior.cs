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
            // hide the sliders parent by default
            difficultySlider.transform.parent.gameObject.SetActive(false);
        }

        /// <summary>
        /// On toggle, hide or show the slider
        /// </summary>
        void ToggleClicked(bool on)
        {
            if(difficultySlider != null && on) {
                difficultySlider.transform.parent.gameObject.SetActive(true);
            } else if(difficultySlider != null && !on) {
                difficultySlider.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
