using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Graphics
{
    /// <summary>
    /// Behavior of AI toggle buttons and their sliders
    /// </summary>
    public class ToggleButtonBehavior : MonoBehaviour
    {
        // slider to choose AI difficulty
        public Slider difficultySlider;
        public Text sliderText;

        /// <summary>
        /// On start, set button click listener.
        /// </summary>
        void Start()
        {
            gameObject.GetComponent<Toggle>().onValueChanged.AddListener(ToggleClicked);
            difficultySlider.onValueChanged.AddListener(value => SliderChanged());
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

        /// <summary>
        /// On slider value change, update text to reflect difficulty
        /// </summary>
        void SliderChanged()
        {
            sliderText.text = "Difficulty: " + difficultySlider.value;
        }
    }
}
