using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationDirector : MonoBehaviour
{
    public Transform p1;
    public Transform p2;

    public float cameraAnimationSpeed = 30f;
    public AnimationCurve cameraTimeCurve;
    public Camera camera3DScene;
    public Camera cameraInGame;
    public bool enableCalibrationScene = false;

    public TextMesh textLeft;
    public TextMesh textRight;
    public TextMesh textDBP1;
    public TextMesh textDBP2;
    public GameObject textInstruction;
    public GameObject playerBlink;
    public GameObject SimScreenActors;
    public GameObject SimScreenOverlay;

    public float CamLookRate = 0.3f;
    public HardwareInterface.ConsoleMirror consoleMirror;

    private UnityEngine.UI.Text mTextInstruction;
    private Animation mPlayerBlinkAnimation;
    private Vector3 m3DSceneCameraPosition;
    private Quaternion m3DSceneCameraRotation;
    private Vector3 mCamLookRotation;

    // Calibration Parameters
    // -- Calibration params for Reading from Arduino
    //public float _calib_votage_x_left = 617;
    //public float _calib_votage_x_right = 300;
    //public float _calib_votage_y_top = 711;
    //public float _calib_votage_y_bottom = 379;
    // -- Calibration parameters for Writing to Arduino
    //public float _calib_write_votage_x_left = 420;
    //public float _calib_write_votage_x_right = 302;
    //public float _calib_write_votage_y_top = 180;
    //public float _calib_write_votage_y_bottom = 80;

    // -- Calibration params for Reading from Arduino
    public float _calib_votage_x_left = 561;
    public float _calib_votage_x_right = 194;
    public float _calib_votage_y_top = 623;
    public float _calib_votage_y_bottom = 277;

    // ---- These will be calculated
    public float _calib_x_mul = -1;
    public float _calib_x_offset = -1;
    public float _calib_y_mul = -1;
    public float _calib_y_offset = -1;

    // -- Calibration parameters for Writing to Arduino
    //public float _calib_write_votage_x_left = 420;
    //public float _calib_write_votage_x_right = 302;
    //public float _calib_write_votage_y_top = 180;
    //public float _calib_write_votage_y_bottom = 80;

    public float _calib_write_votage_x_left = 177.0158f;
    public float _calib_write_votage_x_right = -12.86436f;
    public float _calib_write_votage_y_top = 103.1317f;
    public float _calib_write_votage_y_bottom = 204.0835f;

    // Think of the process of calibration is a state-machine
    private enum CalibrationStates
    {
        NOT_STARTED = 0,
        ANIMMOTION_START = 1,

        PRE_CALIB = 2,
        CALIB_LEFT_TOP = 3,
        CALIB_RIGHT_BOTTOM = 4,

        PRE_CALIB_WRITE = 5,
        CALIB_WRITE_LEFT_TOP = 6,
        CALIB_WRITE_RIGHT_BOTTOM = 7,

        CALIB_FINISH = 8,
        ANIMATION_FINISH = 9,
    };

    private CalibrationStates mCalibStates = CalibrationStates.NOT_STARTED;

    private float mAnimStartTime;
    private float mAnimJourneyLength;

    private float mOriginalPlayerTargetSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Preserve the target camera position
        m3DSceneCameraPosition = camera3DScene.transform.position;
        m3DSceneCameraRotation = camera3DScene.transform.rotation;

        // Set the init look direction before we ruin the camera angle
        mCamLookRotation = Vector3.zero; // camera3DScene.transform.eulerAngles;

        // Initialize the calibration cam transform from the GameCamera,
        //      For the animation init state
        camera3DScene.transform.position = cameraInGame.transform.position;
        camera3DScene.transform.rotation = cameraInGame.transform.rotation;

        // Calculate the journey length.
        mAnimJourneyLength = Vector3.Distance(camera3DScene.transform.position, m3DSceneCameraPosition);

        cameraInGame.enabled = true;
        camera3DScene.enabled = false;

        mTextInstruction = textInstruction.GetComponent<UnityEngine.UI.Text>();
        mTextInstruction.supportRichText = true;
        mTextInstruction.text = "";

        mPlayerBlinkAnimation = playerBlink.GetComponent<Animation>();

        SimScreenOverlay.SetActive(false);

        mOriginalPlayerTargetSpeed = p1.GetComponent<Actors.PlayerTargetController>().speed;
        // Set the speed of move for calibration only;
        p1.GetComponent<Actors.PlayerTargetController>().speed = 0.75f;
    }

    float distCovered, progress;

    // Update is called once per frame
    void Update()
    {
        calibration_state_update(false, false);
    }

    void calibration_state_update(bool extra_btn_prev, bool extra_btn_next)
    {

        bool cancel_calibration = (enableCalibrationScene == false);
        HardwareInterface.ConsoleData cData = consoleMirror.readControllerRawData();
        if (cData != null)
        {
            textDBP1.text = string.Format("({0},{1})", cData.P1_X_READ, cData.P1_Y_READ);
            textDBP2.text = string.Format("({0},{1})", cData.P2_X_READ, cData.P2_Y_READ);
        }

        // Again, the process of calibration is a state-machine
        switch (mCalibStates)
        {
            case CalibrationStates.NOT_STARTED:
                if (enableCalibrationScene)
                {
                    cameraInGame.enabled = false;
                    camera3DScene.enabled = true;
                    // Keep a note of the time the movement started.
                    mAnimStartTime = Time.time;

                    textLeft.text = "→";
                    textRight.text = "←";

                    mCalibStates += 1;
                }
                break;
            case CalibrationStates.ANIMMOTION_START:
                mTextInstruction.text = "Calibrate your Odyssey Input";
                // Running the camera animation (game scene -> 3d scene)

                // Distance moved equals elapsed time times speed..
                distCovered = (Time.time - mAnimStartTime) * cameraAnimationSpeed;
                progress = cameraTimeCurve.Evaluate(distCovered / mAnimJourneyLength);

                // Set our position as a fraction of the distance between the start-end.
                camera3DScene.transform.position = Vector3.Slerp(
                    cameraInGame.transform.position, m3DSceneCameraPosition, progress);
                camera3DScene.transform.rotation = Quaternion.Slerp(
                    cameraInGame.transform.rotation, m3DSceneCameraRotation, progress);

                if (progress >= 1)
                {
                    mCalibStates += 1;
                    mPlayerBlinkAnimation.enabled = true;
                }

                break;
            case CalibrationStates.PRE_CALIB:
                // Set P1 to read mode
                consoleMirror.p1Console = true;
                consoleMirror.pluggedIn = false;
                // Show "Screen" elements
                //SimScreenActors.SetActive(true);
                SimScreenOverlay.SetActive(false);
                mCalibStates = CalibrationStates.CALIB_LEFT_TOP;
                break;
            case CalibrationStates.CALIB_LEFT_TOP:

                mTextInstruction.text = "Please use the controller on Odessey, \n" +
                    "move player 1 to the upper-left corner\n" +
                    "<size=18>Hit <color=brown>RESET</color> on the controller to continue...</size>\n" +
                    "(1/4)";

                if (cancel_calibration)
                {
                    // Allow go back to the game scene any time
                    // Keep a note of the time the movement started.
                    mAnimStartTime = Time.time;
                    mCalibStates = CalibrationStates.ANIMATION_FINISH;
                    break;
                }

                // Move the p1 to top-left corner
                p1.position = new Vector2(consoleMirror._calib_unity_x_left, consoleMirror._calib_unity_y_top);

                if (Input.GetKeyUp(KeyCode.Return) || extra_btn_next) {
                    if (cData == null)
                    {
                        Debug.Log("No cdata");
                    }
                    // Save the calibration value
                    _calib_votage_x_left = cData.P1_X_READ;
                    _calib_votage_y_top = cData.P1_Y_READ;

                    mCalibStates++;
                } else if (extra_btn_prev) {
                    exit_scene();
                }

                update_camera_look();

                break;
            case CalibrationStates.CALIB_RIGHT_BOTTOM:

                mTextInstruction.text = "Now, move player 1 to the LOWER-RIGHT corner\n" +
                    "<size=18>Hit <color=brown>RESET</color> on the controller to continue...</size>\n" +
                    "(2/4)";

                if (cancel_calibration)
                {
                    // Keep a note of the time the movement started.
                    mAnimStartTime = Time.time;
                    mCalibStates = CalibrationStates.ANIMATION_FINISH;
                    break;
                }

                p1.position = new Vector2(consoleMirror._calib_unity_x_right, consoleMirror._calib_unity_y_bottom);

                if (Input.GetKeyUp(KeyCode.Return) || extra_btn_next)
                {
                    // Save the calibration value
                    _calib_votage_x_right = cData.P1_X_READ;
                    _calib_votage_y_bottom = cData.P1_Y_READ;

                    mCalibStates++;
                    textLeft.text = textRight.text = "";
                }
                else if (extra_btn_prev)
                {
                    mCalibStates--;
                }

                update_camera_look();
                break;

            case CalibrationStates.PRE_CALIB_WRITE:
                // Set P1 to write mode
                consoleMirror.p1Console = false;
                consoleMirror.pluggedIn = true;
                // Hide "Screen" elements
                //SimScreenActors.SetActive(false);
                SimScreenOverlay.SetActive(true);
                mCalibStates = CalibrationStates.CALIB_WRITE_LEFT_TOP;

                // Set the p1 to an initial position (so that has the visibility on the screen)
                p1.position = new Vector2(
                    (consoleMirror._calib_unity_x_right + consoleMirror._calib_unity_x_left) / 2,
                    (consoleMirror._calib_unity_y_bottom + consoleMirror._calib_unity_y_top) / 2
                );


                break;

            case CalibrationStates.CALIB_WRITE_LEFT_TOP:
                mTextInstruction.text = "Use the controller on HAL," +
                    "move the player to the upper-left corner\n" +
                    "<size=18>Hit <color=brown>RESET</color> on the controller to continue...</size>\n" +
                    "(3/4)";

                if (cancel_calibration)
                {
                    // Keep a note of the time the movement started.
                    mAnimStartTime = Time.time;
                    mCalibStates = CalibrationStates.ANIMATION_FINISH;
                    break;
                }

                if (Input.GetKeyUp(KeyCode.Return) || extra_btn_next)
                {
                    // Save the calibration value
                    _calib_write_votage_x_left = consoleMirror.xConvertToConsole(p1.position.x);
                    _calib_write_votage_y_top = consoleMirror.xConvertToConsole(p1.position.y);

                    mCalibStates++;
                    textLeft.text = textRight.text = "";

                    // Again, set the p1 to an initial position (so that has the visibility on the screen)
                    p1.position = new Vector2(
                        (consoleMirror._calib_unity_x_right + consoleMirror._calib_unity_x_left) / 2,
                        (consoleMirror._calib_unity_y_bottom + consoleMirror._calib_unity_y_top) / 2
                    );
                }
                else if (extra_btn_prev)
                {
                    mCalibStates = CalibrationStates.PRE_CALIB;
                }

                update_camera_look();
                break;

            case CalibrationStates.CALIB_WRITE_RIGHT_BOTTOM:
                mTextInstruction.text = "Now, use the controller on HAL," +
                    "move the player to the lower-right corner\n" +
                    "<size=18>Hit <color=brown>RESET</color> on the controller to continue...</size>\n" +
                    "(4/4)";

                if (cancel_calibration)
                {
                    // Keep a note of the time the movement started.
                    mAnimStartTime = Time.time;
                    mCalibStates = CalibrationStates.ANIMATION_FINISH;
                    break;
                }

                if (Input.GetKeyUp(KeyCode.Return) || extra_btn_next)
                {
                    // Save the calibration value
                    _calib_write_votage_x_right = consoleMirror.xConvertToConsole(p1.position.x);
                    _calib_write_votage_y_bottom = consoleMirror.xConvertToConsole(p1.position.y);

                    mCalibStates++;
                    textLeft.text = textRight.text = "";
                }
                else if (extra_btn_prev)
                {
                    mCalibStates--;
                }

                update_camera_look();
                break;

            case CalibrationStates.CALIB_FINISH:

                mTextInstruction.text = "How well does the blocks on the TV follow?\n" +
                    "";

                if (cancel_calibration || extra_btn_next)
                {
                    // Keep a note of the time the movement started.
                    mTextInstruction.text = "";
                    mAnimStartTime = Time.time;
                    mCalibStates = CalibrationStates.ANIMATION_FINISH;
                }

                if (extra_btn_next)
                {
                    // On confirm, write the calibration data
                    consoleMirror._calib_votage_x_left = _calib_votage_x_left;
                    consoleMirror._calib_votage_x_right = _calib_votage_x_right;
                    consoleMirror._calib_votage_y_top = _calib_votage_y_top;
                    consoleMirror._calib_votage_y_bottom = _calib_votage_y_bottom;
                    consoleMirror._calib_write_votage_x_left = _calib_write_votage_x_left;
                    consoleMirror._calib_write_votage_x_right = _calib_write_votage_x_right;
                    consoleMirror._calib_write_votage_y_top = _calib_write_votage_y_top;
                    consoleMirror._calib_write_votage_y_bottom = _calib_write_votage_y_bottom;

                    // restore the player target speed
                    p1.GetComponent<Actors.PlayerTargetController>().speed = mOriginalPlayerTargetSpeed;
                }
                else if (extra_btn_prev)
                {
                    mCalibStates--;
                }

                update_camera_look();
                break;

            case CalibrationStates.ANIMATION_FINISH:
                // Distance moved equals elapsed time times speed..
                distCovered = (Time.time - mAnimStartTime) * cameraAnimationSpeed;
                progress = cameraTimeCurve.Evaluate(distCovered / mAnimJourneyLength);

                // Set our position as a fraction of the distance between the start-end.
                camera3DScene.transform.position = Vector3.Slerp(
                    m3DSceneCameraPosition, cameraInGame.transform.position, progress);
                camera3DScene.transform.rotation = Quaternion.Slerp(
                    m3DSceneCameraRotation, cameraInGame.transform.rotation, progress);

                if (progress >= 1)
                {
                    cameraInGame.enabled = true;
                    camera3DScene.enabled = false;

                    mCalibStates = CalibrationStates.NOT_STARTED;
                    mPlayerBlinkAnimation.enabled = false;
                    enableCalibrationScene = false;

                    exit_scene();
                }

                break;
        }
    }

    void update_camera_look()
    {
        mCamLookRotation.y += Input.GetAxis("Mouse X");
        mCamLookRotation.x += -Input.GetAxis("Mouse Y");
        mCamLookRotation.y = Mathf.Clamp(mCamLookRotation.y, -1.5f, 1.5f);
        mCamLookRotation.x = Mathf.Clamp(mCamLookRotation.x, -2.5f, 2.5f);
        camera3DScene.transform.localEulerAngles = mCamLookRotation * CamLookRate;
    }

    public void on_prev_button()
    {
        calibration_state_update(true, false);
    }
    
    public void on_next_button()
    {
        calibration_state_update(false, true);
    }

    /// <summary>
    /// Exit the calibration scene
    /// </summary>
    void exit_scene()
    {
        // Go back to the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
