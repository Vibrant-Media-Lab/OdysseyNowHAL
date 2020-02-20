using System;
using System.Collections;
using System.Collections.Generic;
using CardDirection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ModeDirector : MonoBehaviour {
    public GameObject calibration;
    public GameObject AI;

    private void Awake() {
        //Debug.Log(PlayerPrefs.GetInt("ai1"));
        //Debug.Log(PlayerPrefs.GetInt("ai2"));
        //Debug.Log(PlayerPrefs.GetString("game"));
    }

    // Start is called before the first frame update
    private void Start() {
        //if (localInputManager.isdirty)
        //{
        // Enable calibration

        switch (PlayerPrefs.GetString("game")) {
            case "Cat and Mouse":
                // load cat and mouse after calibration
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartCatAndMouse);
                break;
            case "Super Cat and Mouse":
                break;
            case "Haunted House":
                break;
            case "Zoo Breakout":
                break;
            case "Football: Running":
                break;
        }

        // if both input schemes are keyboard, then no need for overlay
        // calibration.GetComponent<CalibrationOdysseySettings>().useOverlay = false;

        calibration.GetComponent<CalibrationOdysseySettings>().useOverlay = true;

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
        // if p1 is ai, don't read calibration
        if (p1Input == LocalInputManager.ControlScheme.AI)
            calibration.GetComponent<CalibrationOdysseySettings>().p1_read = false;
        // if p2 is ai, don't read calibration
        if (p2Input == LocalInputManager.ControlScheme.AI)
            calibration.GetComponent<CalibrationOdysseySettings>().p2_read = false;
        StartCalibration();
    }

    private void StartCatAndMouse() {
        calibration.SetActive(false);
        var p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        var p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");
        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
        if (p1Input == LocalInputManager.ControlScheme.AI) {
            p1.GetComponent<NavMeshAgent>().enabled = true;
            p1.GetComponent<CatAI>().enabled = true;
            p1.GetComponent<CatAI>().level = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            p2.GetComponent<NavMeshAgent>().enabled = true;
            p2.GetComponent<MouseAI>().enabled = true;
            p2.GetComponent<MouseAI>().level = PlayerPrefs.GetInt("ai2");
        }

        AI.SetActive(true);
    }

    private void StartCalibration() {
        AI.SetActive(false);
        calibration.SetActive(true);
    }
}
