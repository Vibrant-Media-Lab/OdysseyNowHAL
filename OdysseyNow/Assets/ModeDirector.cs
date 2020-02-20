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

        if (LocalInputManager.instance == null) {
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
            // fixme
            
            
            calibration.GetComponent<CalibrationOdysseySettings>().useOverlay = false;
            
            // if p1 is ai, don't read calibration
            if (PlayerPrefs.GetInt("ai1") != -1)
                calibration.GetComponent<CalibrationOdysseySettings>().p1_read = false;
            // if p2 is ai, don't read calibration
            if (PlayerPrefs.GetInt("ai2") != -1)
                calibration.GetComponent<CalibrationOdysseySettings>().p2_read = false;
            StartCalibration();

            return;
        }

        switch (LocalInputManager.instance.p1Scheme) {
            case LocalInputManager.ControlScheme.AI:
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartCatAndMouse);
                break;
            case LocalInputManager.ControlScheme.OriginalConsole:
                calibration.GetComponent<CalibrationOdysseySettings>().p1_read = true;
                break;
            case LocalInputManager.ControlScheme.Keyboard:
                break;
            case LocalInputManager.ControlScheme.Traditional:
                break;
            case LocalInputManager.ControlScheme.OdysseyCon:
                break;
            case LocalInputManager.ControlScheme.OdysseyConLegacy:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (LocalInputManager.instance.p2Scheme == LocalInputManager.ControlScheme.OriginalConsole) {
            calibration.GetComponent<CalibrationOdysseySettings>().p2_read = true;
        }

        StartCalibration();
    }

    private void StartCatAndMouse() {
        calibration.SetActive(false);
        var p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        var p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");
        var player1AI = PlayerPrefs.GetInt("ai1");
        var player2AI = PlayerPrefs.GetInt("ai2");
        if (player1AI != -1) {
            p1.GetComponent<NavMeshAgent>().enabled = true;
            p1.GetComponent<CatAI>().enabled = true;
            p1.GetComponent<CatAI>().level = player1AI;
        }

        if (player2AI != -1) {
            p2.GetComponent<NavMeshAgent>().enabled = true;
            p2.GetComponent<MouseAI>().enabled = true;
            p2.GetComponent<MouseAI>().level = player2AI;
        }

        AI.SetActive(true);
    }

    private void StartCalibration() {
        AI.SetActive(false);
        calibration.SetActive(true);
    }
}
