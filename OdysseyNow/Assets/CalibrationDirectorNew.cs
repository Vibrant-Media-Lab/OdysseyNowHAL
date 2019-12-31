using System.Collections;
using System.Collections.Generic;
using HardwareInterface;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CalibrationDirectorNew : MonoBehaviour
{
    // Expose to Unity Editor for easy assignment
    [Header("GUI related")]
    public UnityEngine.UI.Text mTextInstruction;
    public UnityEngine.UI.Button mBtnNext;
    public UnityEngine.UI.Button mBtnPrev;
    public TextMesh textDBP1;
    public TextMesh textDBP2;

    // 3d Scene related assignment
    [Header("2D/3D Scene related")]
    public float cameraAnimationSpeed = 30f;
    public float camLookRate = 0.3f;
    public AnimationCurve cameraTimeCurve;
    public Camera camera3DScene;
    public Camera cameraInGame;

    [Header("")]
    public GameObject mCalibTV_topLeft;
    public GameObject mCalibTV_bottomRight;
    public HardwareInterface.ConsoleMirror consoleMirror;
    public GameObject p1_target, p2_target;
    public UnityEvent afterCalibration;

    [Header("non-editor purposed")]
    public CalibrationOdysseySettings mCalibrationSettings;

    IStateStep mCurrentStep = null;
    List<IStateStep> mStepPipe = new List<IStateStep>();
    int mCurrentStepIndex = -1;
    Vector3 mCamLookRotation = Vector3.zero;
    //private Transform cam3dTargetTransform;

    // Start is called before the first frame update
    void Start()
    {
        //cam3dTargetTransform = camera3DScene.transform;

        // Construct StepState pipe based on calibration settings
        mStepPipe.Add(new S0_2D3DAnimation(this,
            cameraInGame, camera3DScene));
        mStepPipe.Add(new S1_Devices(this));

        if (mCalibrationSettings.useOverlay)
        {
            mStepPipe.Add(new S2_Overlay(this,
                mCalibrationSettings.overlay_extra_text));
        }

        // if we need read calibration
        if (mCalibrationSettings.p1_read || mCalibrationSettings.p2_read)
        {
            // find a player in read mode 
            string player_read_name = mCalibrationSettings.p1_read ? "P1" : "P2";
            Debug.Log("Read mode player: " + player_read_name);

            // if screen overlay is used
            if (mCalibrationSettings.useOverlay)
            {
                mStepPipe.Add(new S3_CalibRead(this,
                    mCalibrationSettings.overlay_topLeft_extra_text,
                    mCalibrationSettings.overlay_topLeft,
                    player_read_name, "top-left"));
                mStepPipe.Add(new S3_CalibRead(this,
                    mCalibrationSettings.overlay_bottomRight_extra_text,
                    mCalibrationSettings.overlay_bottomRight,
                    player_read_name, "bottom-right"));
            } else
            {
                // no screen overlay is used, calibrate to the top-left and bottom-right of the tv screen
                mStepPipe.Add(new S3_CalibRead(this,
                    "The top-left conner of the tv screen",
                    mCalibTV_topLeft, player_read_name, "top-left"));
                mStepPipe.Add(new S3_CalibRead(this,
                    "The bottom-right conner of the tv screen",
                    mCalibTV_bottomRight, player_read_name, "bottom-right"));
            }
        }

        // if we need write calibration
        if (!mCalibrationSettings.p1_read || !mCalibrationSettings.p2_read)
        {
            // find a player in write mode 
            string player_write_name = !mCalibrationSettings.p1_read ? "P1" : "P2";

            // if screen overlay is used
            if (mCalibrationSettings.useOverlay)
            {
                mStepPipe.Add(new S4_CalibWrite(this,
                    mCalibrationSettings.overlay_topLeft_extra_text,
                    mCalibrationSettings.overlay_topLeft,
                    player_write_name, "top-left"));
                mStepPipe.Add(new S4_CalibWrite(this,
                    mCalibrationSettings.overlay_bottomRight_extra_text,
                    mCalibrationSettings.overlay_bottomRight,
                    player_write_name, "bottom-right"));
            }
            else
            {
                // no screen overlay is used, calibrate to the top-left and bottom-right of the tv screen
                mStepPipe.Add(new S4_CalibWrite(this,
                    "the top-left conner of the tv screen",
                    mCalibTV_topLeft, player_write_name, "top-left"));
                mStepPipe.Add(new S4_CalibWrite(this,
                    "the bottom-right conner of the tv screen",
                    mCalibTV_bottomRight, player_write_name, "bottom-right"));
            }
        }

        // So far the camera3DScene.transform remains the "target" transform
        mStepPipe.Add(new S0_2D3DAnimation(this,
        camera3DScene, cameraInGame));

        // StateStep starts by calling:
        StepGoNext();
    }

    // Update is called once per frame
    void Update()
    {
        // State routine
        StateStep();

        // Handle user input
        if (Input.GetKeyUp(KeyCode.Return))
        {
            StepGoNext();
        }
    }

    void StateTransit(IStateStep targetState)
    {
        if (mCurrentStep != null)
            mCurrentStep.OnExitStep();

        mCurrentStep = targetState;
        targetState.OnEnterStep();
    }

    /// <summary>
    /// Call Step() at each frame
    /// </summary>
    void StateStep()
    {
        if (mCurrentStep != null)
            mCurrentStep.Step();
    }

    public string GetCurrentStepCount()
    {
        return "(" + (mCurrentStepIndex + 1) + "/" + mStepPipe.Count + ")";
    }

    /// <summary>
    /// Handler of user input
    /// </summary>
    public void StepGoPrevious()
    {
        mCurrentStepIndex--;
        if (mCurrentStepIndex == -1)
        {
            mBtnPrev.interactable = false; 
        } else
        {
            mBtnPrev.interactable = true;
            StateTransit(mStepPipe[mCurrentStepIndex]);
        }
    }

    /// <summary>
    /// Handler of user input
    /// </summary>
    public void StepGoNext()
    {
        mCurrentStepIndex++;
        if (mCurrentStepIndex >= mStepPipe.Count)
        {
            if (mCurrentStepIndex > 0)
            {
                mStepPipe[mCurrentStepIndex - 1].OnExitStep();
            }
            Debug.Log("StepGoNext(): mCurrentStepIndex >= mStepPipe.Count");
            mBtnNext.interactable = false;
            // end of calibration
            afterCalibration.Invoke();
        }
        else
        {
            mBtnNext.interactable = true;
            StateTransit(mStepPipe[mCurrentStepIndex]);
        }
    }

    /// <summary>
    /// Mouse-camera movement
    /// </summary>
    public void UpdateMouseCamera()
    {
        mCamLookRotation.y += Input.GetAxis("Mouse X");
        mCamLookRotation.x += -Input.GetAxis("Mouse Y");
        mCamLookRotation.y = Mathf.Clamp(mCamLookRotation.y, -1.5f, 1.5f);
        mCamLookRotation.x = Mathf.Clamp(mCamLookRotation.x, -2.5f, 2.5f);
        camera3DScene.transform.localEulerAngles = mCamLookRotation * camLookRate;
    }

}

// --------------------------------------------------------------------

public interface IStateStep
{ 
    /// <summary>
    /// This get called on every update
    /// </summary>
    void Step();
    
    void OnEnterStep();
    void OnExitStep();
}

class S0_2D3DAnimation : IStateStep
{
    CalibrationDirectorNew mDirc;
    float mAnimStartTime;
    float mAnimJourneyLength;
    Vector3 mPosFrom, mPosTo;
    Quaternion mRotFrom, mRotTo;
    Camera mCamFrom, mCamTo;

    void IStateStep.OnEnterStep()
    {
        mAnimStartTime = Time.time;
        mPosFrom = mCamFrom.transform.position;
        mPosTo = mCamTo.transform.position;
        mRotFrom = mCamFrom.transform.rotation;
        mRotTo = mCamTo.transform.rotation;

        mAnimJourneyLength = Vector3.Distance(mPosFrom, mPosTo);
        mCamFrom.enabled = false;
        // But always override (show) the 3d camera 
        mDirc.camera3DScene.enabled = true;
    }

    void IStateStep.OnExitStep()
    {
        // switch camera
        mCamFrom.enabled = false;
        mCamTo.enabled = true;
    }

    void IStateStep.Step()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - mAnimStartTime) * mDirc.cameraAnimationSpeed;
        float progress = mDirc.cameraTimeCurve.Evaluate(distCovered / mAnimJourneyLength);

        // Set our position as a fraction of the distance between the start-end.
        mDirc.camera3DScene.transform.position = Vector3.Slerp(
            mPosFrom, mPosTo, progress);
        mDirc.camera3DScene.transform.rotation = Quaternion.Slerp(
            mRotFrom, mRotTo, progress);

        if (progress >= 1)
        {
            mDirc.StepGoNext();
        }
    }

    public S0_2D3DAnimation(CalibrationDirectorNew calibrationDirector,
        Camera fromCamera, Camera toCamera
        //Transform transFrom, Transform transTo
        )
    {
        mDirc = calibrationDirector;
        mCamFrom = fromCamera;
        mCamTo = toCamera;
        //mTFrom = transFrom;
        //mTTo = transTo;
    }
}

/// <summary>
/// Step 1 - Prompt to let user connect/disconnect controllers
/// </summary>
class S1_Devices : IStateStep
{
    private CalibrationDirectorNew mDirc;

    void IStateStep.OnEnterStep()
    {
        if (mDirc.mCalibrationSettings.p1_read
            && mDirc.mCalibrationSettings.p2_read)
        {
            mDirc.mTextInstruction.text =
                "Please connect both <color=brown>P1</color> and <color=brown>P2</color> " +
                "controllers";
        }
        else if (mDirc.mCalibrationSettings.p1_read)
        {
            mDirc.mTextInstruction.text =
                "Please setup your Oddyssey with as follows: \n" +
                "connect <color=brown>P1</color>, " +
                "disconnect <color=brown>P2</color>\n";
        }
        else if (mDirc.mCalibrationSettings.p2_read)
        {
            mDirc.mTextInstruction.text =
                "Please setup your Oddyssey with as follows: \n" +
                "disconnect <color=brown>P1</color>, " +
                "connect <color=brown>P2</color>\n";
        }
        else
        {
            mDirc.mTextInstruction.text =
                "Please disconnect both <color=brown>P1</color> and <color=brown>P2</color> " +
                "controllers";
        }

        mDirc.mTextInstruction.text += mDirc.GetCurrentStepCount();
        mDirc.UpdateMouseCamera();
    }

    void IStateStep.OnExitStep()
    {
        return;
    }

    void IStateStep.Step()
    {
        return;
    }

    public S1_Devices(CalibrationDirectorNew calibrationDirector)
    {
        mDirc = calibrationDirector;
    }

}

/// <summary>
/// Step 2 - Screen overlay calibration
/// </summary>
class S2_Overlay : IStateStep
{
    private CalibrationDirectorNew mDirc;
    private GameObject mOverlaySpot;
    private string mOverlayExtraText = "";
    private string mAvailablePlayerName = "";

    void IStateStep.OnEnterStep()
    {
        if (mOverlaySpot == null)
        {
            mDirc.mTextInstruction.text =
                "Please put the screen overlay on the TV screen.";

        } else
        {   
            mDirc.mTextInstruction.text =
                "Please move " + mAvailablePlayerName + " to the spot.";
            // Show the top-left or bottom-right spot 
            mOverlaySpot.SetActive(true);
        }

        // Append extra text
        if (mOverlayExtraText.TrimStart().TrimEnd() != "")
        {
            mDirc.mTextInstruction.text += "\n" +
                "<size=18>" + mOverlayExtraText + "</size>";
        }

        // mDirc.mCalibrationSettings.screenOverlay.enabled = true;
        mDirc.mTextInstruction.text += mDirc.GetCurrentStepCount();
        mDirc.UpdateMouseCamera();
    }

    void IStateStep.OnExitStep()
    {
        if (mOverlaySpot != null)
        {
            mOverlaySpot.SetActive(false);
        }
    }

    void IStateStep.Step()
    {
        return;
    }

    public S2_Overlay(CalibrationDirectorNew calibrationDirector,
        string extraText)
    {
        mDirc = calibrationDirector;
        mOverlayExtraText = extraText;
    }

    public S2_Overlay(CalibrationDirectorNew calibrationDirector,
        string extraText,
        GameObject overlaySpot,
        string playerName)
    {
        mDirc = calibrationDirector;
        mOverlaySpot = overlaySpot;
        mOverlayExtraText = extraText;
        mAvailablePlayerName = playerName;
    }

}


class S3_CalibRead : IStateStep
{
    private CalibrationDirectorNew mDirc;
    private GameObject mOverlaySpot;
    private string mPositionText = "";
    private string mOverlayExtraText = "";
    private string mAvailablePlayerName = "";
    private string mMode = ""; // "top-left" or "bottom-right"
    HardwareInterface.ConsoleData mHWData;

    void IStateStep.OnEnterStep()
    {
        if (mOverlaySpot.GetComponent<SpriteMask>() == null)
        {
            mDirc.mTextInstruction.text =
                "Please use the Odyssey controller to \n" +
                "move " +mAvailablePlayerName+ " to " + mPositionText;
        }
        else
        {
            mDirc.mTextInstruction.text =
                "Please use the Odyssey controller to \n" +
                "move " + mAvailablePlayerName + " to the spot.";
            // Show the top-left or bottom-right spot 
        }
        mOverlaySpot.SetActive(true);

        // Append extra text
        if (mOverlayExtraText.TrimStart().TrimEnd() != "")
        {
            mDirc.mTextInstruction.text += "\n" +
                "<size=18>" + mOverlayExtraText + "</size>";
        }

        // mDirc.mCalibrationSettings.screenOverlay.enabled = true;
        mDirc.mTextInstruction.text += mDirc.GetCurrentStepCount();

        if (mAvailablePlayerName == "P1")
            mDirc.consoleMirror.p1Console = true;
        else
            mDirc.consoleMirror.p2Console = true;
        mDirc.consoleMirror.pluggedIn = true;
    }

    void IStateStep.OnExitStep()
    {
        // Save the current calibration-read
        mHWData = mDirc.consoleMirror.readControllerRawData();
        if (mHWData == null)
        {
            Debug.LogWarning("Controller is probably not connected");
        }

        if (mOverlaySpot.GetComponent<SpriteMask>() != null)
        {
            // Bounds always give world position (and our player is in world position as well)
            Debug.Log(mOverlaySpot.GetComponent<SpriteMask>().bounds.center);
            //Debug.Log(mOverlaySpot.transform.TransformPoint(mOverlaySpot.transform.position));
            if (mMode == "top-left")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.y;
            }
            else if (mMode == "bottom-right")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.y;
            }

        } else
        {
            if (mMode == "top-left")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.transform.position.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.transform.position.y;
            }
            else if (mMode == "bottom-right")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.transform.position.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.transform.position.y;
            }
        }

        if (mMode == "top-left") {
            if (mAvailablePlayerName.ToUpper() == "P1")
            {
                mDirc.mCalibrationSettings._calib_p1_read_votage_x_left = mHWData.P1_X_READ;
                mDirc.mCalibrationSettings._calib_p1_read_votage_y_top = mHWData.P1_Y_READ;
            }
            else if (mAvailablePlayerName.ToUpper() == "P2")
            {
                mDirc.mCalibrationSettings._calib_p2_read_votage_x_left = mHWData.P2_X_READ;
                mDirc.mCalibrationSettings._calib_p2_read_votage_y_top = mHWData.P2_Y_READ;
            }
        } else if (mMode == "bottom-right")
        {
            // Save the current calibration-read
            if (mAvailablePlayerName.ToUpper() == "P1")
            {
                mDirc.mCalibrationSettings._calib_p1_read_votage_x_right = mHWData.P1_X_READ;
                mDirc.mCalibrationSettings._calib_p1_read_votage_y_bottom = mHWData.P1_Y_READ;
            }
            else if (mAvailablePlayerName.ToUpper() == "P2")
            {
                mDirc.mCalibrationSettings._calib_p2_read_votage_x_right = mHWData.P2_X_READ;
                mDirc.mCalibrationSettings._calib_p2_read_votage_y_bottom = mHWData.P2_Y_READ;
            }
        } else
        {
            Debug.LogError("Wrong mode");
        }

        mOverlaySpot.SetActive(false);
    }

    void IStateStep.Step()
    {
        // Read controller data
        mHWData = mDirc.consoleMirror.readControllerRawData();
        if (mHWData != null)
        {
            mDirc.textDBP1.text = string.Format("({0},{1})", mHWData.P1_X_READ, mHWData.P1_Y_READ);
            mDirc.textDBP2.text = string.Format("({0},{1})", mHWData.P2_X_READ, mHWData.P2_Y_READ);
        }

        mDirc.UpdateMouseCamera();
    }

    public S3_CalibRead(CalibrationDirectorNew calibrationDirector,
        string extraText,
        GameObject overlaySpot,
        string playerName,
        string mode)
    {
        mDirc = calibrationDirector;
        mOverlaySpot = overlaySpot;
        mOverlayExtraText = extraText;
        mAvailablePlayerName = playerName;
        mMode = mode;
    }

}

class S4_CalibWrite : IStateStep
{
    private CalibrationDirectorNew mDirc;
    private GameObject mOverlaySpot;
    private string mPositionText = "";
    private string mOverlayExtraText = "";
    private string mAvailablePlayerName = "";
    private string mMode = ""; // "top-left" or "bottom-right"

    void IStateStep.OnEnterStep()
    {
        if (mOverlaySpot.GetComponent<SpriteMask>() == null)
        {
            mDirc.mTextInstruction.text =
                "Please use the Keyboard to \n" +
                "move " + mAvailablePlayerName + " to " + mPositionText;
        }
        else
        {
            mDirc.mTextInstruction.text =
                "Please use the Keyboard to \n" +
                "move " + mAvailablePlayerName + " to the spot.";
            // Show the top-left or bottom-right spot 
        }
        mOverlaySpot.SetActive(true);

        // Append extra text
        if (mOverlayExtraText.TrimStart().TrimEnd() != "")
        {
            mDirc.mTextInstruction.text += "\n" +
                "<size=18>" + mOverlayExtraText + "</size>";
        }

        // mDirc.mCalibrationSettings.screenOverlay.enabled = true;
        mDirc.mTextInstruction.text += mDirc.GetCurrentStepCount();
    }

    void IStateStep.OnExitStep()
    {
        // Save the current calibration-write

        if (mOverlaySpot.GetComponent<SpriteMask>() != null)
        {
            // Bounds always give world position (and our player is in world position as well)
            Debug.Log(mOverlaySpot.GetComponent<SpriteMask>().bounds.center);
            //Debug.Log(mOverlaySpot.transform.TransformPoint(mOverlaySpot.transform.position));
            if (mMode == "top-left")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.y;
            }
            else if (mMode == "bottom-right")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.GetComponent<SpriteMask>().bounds.center.y;
            }

        }
        else
        {
            // Calibration without overlay
            if (mMode == "top-left")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.transform.position.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.transform.position.y;
            }
            else if (mMode == "bottom-right")
            {
                mDirc.mCalibrationSettings._calib_unity_x_left
                    = mOverlaySpot.transform.position.x;
                mDirc.mCalibrationSettings._calib_unity_y_top
                    = mOverlaySpot.transform.position.y;
            }
        }

        if (mMode == "top-left")
        {
            var p = mAvailablePlayerName == "P1" ? mDirc.p1_target : mDirc.p2_target;
            mDirc.mCalibrationSettings._calib_p2_write_votage_x_left
                = mDirc.consoleMirror.xConvertToConsole(p.transform.position.x);
            mDirc.mCalibrationSettings._calib_p2_write_votage_y_top
                = mDirc.consoleMirror.yConvertToConsole(p.transform.position.y);
        }
        else if (mMode == "bottom-right")
        {
            var p = mAvailablePlayerName == "P1" ? mDirc.p1_target : mDirc.p2_target;
            mDirc.mCalibrationSettings._calib_p2_write_votage_x_right
                = mDirc.consoleMirror.xConvertToConsole(p.transform.position.x);
            mDirc.mCalibrationSettings._calib_p2_write_votage_y_bottom
                = mDirc.consoleMirror.yConvertToConsole(p.transform.position.y);
        }
        else
        {
            Debug.LogError("Wrong mode");
        }

        mOverlaySpot.SetActive(false);
    }

    void IStateStep.Step()
    {
        //// Read controller data
        //mHWData = mDirc.consoleMirror.readControllerRawData();
        //if (mHWData != null)
        //{
        //    mDirc.textDBP1.text = string.Format("({0},{1})", mHWData.P1_X_READ, mHWData.P1_Y_READ);
        //    mDirc.textDBP2.text = string.Format("({0},{1})", mHWData.P2_X_READ, mHWData.P2_Y_READ);
        //}
        mDirc.UpdateMouseCamera();
    }

    public S4_CalibWrite(CalibrationDirectorNew calibrationDirector,
        string extraText,
        GameObject overlaySpot,
        string playerName,
        string mode)
    {
        mDirc = calibrationDirector;
        mOverlaySpot = overlaySpot;
        mOverlayExtraText = extraText;
        mAvailablePlayerName = playerName;
        mMode = mode;
    }

}
