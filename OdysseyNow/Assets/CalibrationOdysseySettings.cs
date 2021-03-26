using System.Collections.Generic;
using UnityEngine;

public class CalibrationOdysseySettings : MonoBehaviour
{

    [Header("Calibration Step 1:")]
    public bool p1_read;
    public bool p2_read;

    [Header("Calibration Step 2:")]
    public bool useOverlay;

    public List<string> games;
    public List<GameObject> calibdefs;
    
    public List<string> spot_left;
    
    public List<string> spot_right;

    public Dictionary<string, List<object>> game_data;

    public string overlay_extra_text;

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

    void Awake()
    {
        game_data = new Dictionary<string, List<object>>();
        for(int i = 0; i < games.Count; i++) {
            game_data.Add(games[i], new List<object>());
            game_data[games[i]].Add(calibdefs[i]);
            game_data[games[i]].Add(spot_left[i]);
            game_data[games[i]].Add(spot_right[i]);
        }
    }
}