﻿using CardDirection;
using UnityEngine;
using UnityEngine.AI;

public class ModeDirector : MonoBehaviour {
    public GameObject calibration;
    public GameObject AI;

    // Start is called before the first frame update
    private void Awake() {
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
            case "Tennis":
                Debug.Log("Case: Tennis");
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartTennis);
                break;
        }

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
        // if both input schemes are keyboard, then no need for overlay
        if ((p1Input == LocalInputManager.ControlScheme.Keyboard &&
             p2Input == LocalInputManager.ControlScheme.Keyboard) ||
            (p1Input == LocalInputManager.ControlScheme.AI && p2Input == LocalInputManager.ControlScheme.AI))
            calibration.GetComponent<CalibrationOdysseySettings>().useOverlay = false;
        else
            calibration.GetComponent<CalibrationOdysseySettings>().useOverlay = true;

        // if p1 is ai or keyboard, don't read calibration
        if (p1Input == LocalInputManager.ControlScheme.AI || p1Input == LocalInputManager.ControlScheme.Keyboard)
            calibration.GetComponent<CalibrationOdysseySettings>().p1_read = false;

        // if p2 is ai or keyboard, don't read calibration
        if (p2Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.Keyboard)
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

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI)
            AI.SetActive(true);
    }

    private void StartTennis() {
        calibration.SetActive(false);
        var p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        var p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
        if (p1Input == LocalInputManager.ControlScheme.AI) {
            Debug.Log("p1 input AI");
            p1.GetComponent<NavMeshAgent>().enabled = true;
            p1.GetComponent<CatAI>().enabled = true;
            p1.GetComponent<CatAI>().level = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            Debug.Log("p2 input AI");
            p2.GetComponent<NavMeshAgent>().enabled = true;
            p2.GetComponent<MouseAI>().enabled = true;
            p2.GetComponent<MouseAI>().level = PlayerPrefs.GetInt("ai2");
        }

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI)
            AI.SetActive(true);
    }

    private void StartCalibration() {
        AI.SetActive(false);
        //calibration.SetActive(true);
    }
}
