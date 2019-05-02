using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Graphics {
    public class CardLabel : MonoBehaviour {
        string sceneName;
        Text thisThing;

        public float fadeSpeed;

        private void OnEnable() {
            sceneName = SceneManager.GetActiveScene().name;
            thisThing = gameObject.GetComponent<Text>();
            if (name.Contains("Card")) {
                StartCoroutine(FadeSlide());
            }
        }

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
