using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationProcess : MonoBehaviour
{

    #region default-calibrations
    // Calibrate read from arduino
    public bool pCalibrateRead = true;
    // Calibrate write to arduino
    public bool pCalibrateWrite = true;
    // Calibration process with screen overlay
    public bool pCalibrateWithOverlay;
    #endregion default-calibrations

    List<string> mStepTexts = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        if (pCalibrateWithOverlay)
        {
            mStepTexts.Add("Please put on the card {0} screen overlay");
        }

        if (pCalibrateRead)
        {
            mStepTexts.Add("Odyssey controller calibration");
        }

        if (pCalibrateWrite)
        {
            mStepTexts.Add("OdysseyNow HAL calibration");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// State machine routine
    /// </summary>
    void StateUpdate()
    {

    }


}
