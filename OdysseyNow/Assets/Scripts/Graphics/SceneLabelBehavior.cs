using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Graphics
{
    public class SceneLabelBehavior : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<Text>().text = SceneManager.GetActiveScene().name.Replace("Card", "Card ");
        }
    }
}

