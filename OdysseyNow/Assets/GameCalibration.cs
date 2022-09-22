using CardDirection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCalibration : MonoBehaviour
{
    //singleton instance
    public static GameCalibration instance;

    public float _calib_votage_x_left = 617;
    public float _calib_votage_x_right = 300;
    public float _calib_votage_y_top = 711;
    public float _calib_votage_y_bottom = 379;


    public float _calib_write_votage_x_left = 419;
    public float _calib_write_votage_x_right = 302;
    public float _calib_write_votage_y_top = 180;
    public float _calib_write_votage_y_bottom = 80;

    //this stored the type on imput selected
    public LocalInputManager.ControlScheme p1Input = (LocalInputManager.ControlScheme)System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                      PlayerPrefs.GetString("P1Input"));
    public LocalInputManager.ControlScheme p2Input = (LocalInputManager.ControlScheme)System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));

    // Start is called before the first frame update
    void Start()
    {


        DontDestroyOnLoad(gameObject);

        if (instance != null)
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
        
    }

    public static GameCalibration Instance
    {
        get { return instance; }
    }
}
