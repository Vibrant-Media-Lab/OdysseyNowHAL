using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class saveData : MonoBehaviour
{

    public Slider aiSlider1;
    public Slider aiSlider2;
    public Toggle aiToggle1;
    public Toggle aiToggle2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnDestroy is called when the object is destroyed
    // either manually or on a scene change
    void OnDestroy()
    {
        // saves ai values so the next scene knows which ai to use
        int val1 = -1;
        int val2 = -1;
        if(aiToggle1.isOn)
        {
            val1 = (int) aiSlider1.value;
        }
        if(aiToggle2.isOn)
        {
            val2 = (int) aiSlider2.value;
        }
        PlayerPrefs.SetInt("ai1", val1);
        PlayerPrefs.SetInt("ai2", val2);
    }
}
