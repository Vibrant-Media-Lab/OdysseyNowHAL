using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationOdysseySettings : MonoBehaviour
{

    [Header("Calibration Step 1:")]
    public bool p1_read;
    public bool p2_read;

    [Header("Calibration Step 2:")]
    public bool useOverlay;
    public SpriteRenderer screenOverlay;
    public string overlay_extra_text;

    public GameObject overlay_topLeft;
    public string overlay_topLeft_extra_text;

    public GameObject overlay_bottomRight;
    public string overlay_bottomRight_extra_text;

    [Header("Calibration Step 3:")]
    public float _calib_p1_read_votage_x_left = 617;
    public float _calib_p1_read_votage_y_top = 711;
    public float _calib_p1_read_votage_x_right = 300;
    public float _calib_p1_read_votage_y_bottom = 379;
                       
    public float _calib_p1_write_votage_x_left = 420;
    public float _calib_p1_write_votage_y_top = 180;
    public float _calib_p1_write_votage_x_right = 302;
    public float _calib_p1_write_votage_y_bottom = 80;

    [Header("Calibration Step 4:")]
    public float _calib_p2_read_votage_x_left = 617;
    public float _calib_p2_read_votage_y_top = 711;
    public float _calib_p2_read_votage_x_right = 300;
    public float _calib_p2_read_votage_y_bottom = 379;

    public float _calib_p2_write_votage_x_left = 420;
    public float _calib_p2_write_votage_y_top = 180;
    public float _calib_p2_write_votage_x_right = 302;
    public float _calib_p2_write_votage_y_bottom = 80;

    [Header("Calibration Step 5:")]
    public bool show_validation;

    [Header("")]
    public float _calib_unity_x_left = -7.7f;
    public float _calib_unity_x_right = 7.7f;
    public float _calib_unity_y_top = 4.4f;
    public float _calib_unity_y_bottom = -4.4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
