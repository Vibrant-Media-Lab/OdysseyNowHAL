using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Graphics
{
    /// <summary>
    /// Manages the scene label in card pause menus.
    /// </summary>
    public class SceneLabelBehavior : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<Text>().text = SceneManager.GetActiveScene().name.Replace("Card", "Card ");
        }
    }
}

