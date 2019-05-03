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
        //if true, will load a scene if clicked
        public bool load;
        //another UI object that will replace this element on screen, if clicked
        public GameObject otherMenu;

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
            if (load)
            {
                SceneManager.LoadScene(gameObject.name.Replace(" ", ""));
            }
            else if (otherMenu != null)
            {
                otherMenu.SetActive(true);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
