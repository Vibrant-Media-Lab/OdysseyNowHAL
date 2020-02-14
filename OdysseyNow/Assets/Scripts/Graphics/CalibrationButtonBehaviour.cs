using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalibrationButtonBehaviour : MonoBehaviour {
    public void LoadCalibrationScene() {
        SceneManager.LoadScene("Calibration");
    }
}
