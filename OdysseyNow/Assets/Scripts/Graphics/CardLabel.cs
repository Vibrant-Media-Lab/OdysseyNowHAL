using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Graphics {
    /// <summary>
    /// This creates and animates the red card title label that appears when a card scene first opens.
    /// </summary>
    public class CardLabel : MonoBehaviour {
        //name of the scene
        string sceneName;
        //the text object that appears on screen
        Text thisThing;

        //representation of the speed; serializable
        public float fadeSpeed;

        /// <summary>
        /// On enable, if this scene is a card scene, save the scene name and start the FadeSlide coroutine
        /// </summary>
        private void OnEnable() {
            sceneName = SceneManager.GetActiveScene().name;
            thisThing = gameObject.GetComponent<Text>();
            if (sceneName.Contains("Card")) {
                StartCoroutine(FadeSlide());
            }
        }

        /// <summary>
        /// Sets the text of the Text object to the card name, fades it to transparent while moving it upwards.
        /// </summary>
        /// <returns>An Ienumerator (not really a return value; look this up if you don't know what it is).</returns>
        IEnumerator FadeSlide() {
            thisThing.text = "Card " + sceneName.Substring(4);
            int k = 0;
            while (k < 40) {
                thisThing.color = Color.Lerp(thisThing.color, Color.clear, fadeSpeed * Time.deltaTime);
                transform.Translate(new Vector3(0f, 0.007f, 0f));
                yield return new WaitForFixedUpdate();
            }
            gameObject.SetActive(false);
        }
    }
}
