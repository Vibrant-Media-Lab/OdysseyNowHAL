using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static Director instance;
    public bool paused = false;
    public GameObject menu;

    private void Awake()
    {
        if (Director.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            accessMenu();
        }
    }

    public void accessMenu()
    {
        paused = !paused;
        menu.SetActive(paused);
    }
}
