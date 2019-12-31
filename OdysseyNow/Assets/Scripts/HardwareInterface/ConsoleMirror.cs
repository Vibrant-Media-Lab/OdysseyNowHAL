using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardwareInterface
{
    /// <summary>
    /// Handles communication with original console, through arduino.
    /// </summary>
    public class ConsoleMirror : MonoBehaviour
    {
        //Singleton instance
        public static ConsoleMirror instance;

        //transforms of visible game objects
        public Transform p1;
        public Transform p2;
        public Transform ball;
        public Transform line;

        //true if either player is controlled by console
        public bool p1Console;
        public bool p2Console;

        //true if console is plugged in
        public bool pluggedIn;

        // Calibration Parameters
        // -- Calibration params for Reading from Arduino
        public float _calib_votage_x_left = 617;
        public float _calib_votage_x_right = 300;
        public float _calib_votage_y_top = 711;
        public float _calib_votage_y_bottom = 379;
        // ---- These will be calculated
        public float _calib_x_mul = -1;
        public float _calib_x_offset = -1;
        public float _calib_y_mul = -1;
        public float _calib_y_offset = -1;
        // -- Calibration parameters for Writing to Arduino
        public float _calib_write_votage_x_left = 420;
        public float _calib_write_votage_x_right = 302;
        public float _calib_write_votage_y_top = 180;
        public float _calib_write_votage_y_bottom = 80;
        // ---- These will be calculated
        public float _calib_write_x_mul = -1;
        public float _calib_write_x_offset = -1;
        public float _calib_write_y_mul = -1;
        public float _calib_write_y_offset = -1;
        // -- Screen
        public float _calib_unity_x_left = -7.7f;
        public float _calib_unity_x_right = 7.7f;
        public float _calib_unity_y_top = 4.4f;
        public float _calib_unity_y_bottom = -4.4f;


        void _calib_calc_param_x()
        {
            _calib_x_mul = (_calib_unity_x_right - _calib_unity_x_left) / (_calib_votage_x_right - _calib_votage_x_left);
            _calib_x_offset = _calib_unity_x_left - _calib_votage_x_left * _calib_x_mul;
        }
        void _calib_calc_param_y()
        {
            _calib_y_mul = (_calib_unity_y_bottom - _calib_unity_y_top) / (_calib_votage_y_bottom - _calib_votage_y_top);
            _calib_y_offset = _calib_unity_y_top - _calib_votage_y_top * _calib_y_mul;
        }

        void _calib_write_calc_param_x()
        {
            _calib_write_x_mul = (_calib_unity_x_right - _calib_unity_x_left) / (_calib_write_votage_x_right - _calib_write_votage_x_left);
            _calib_write_x_offset = _calib_unity_x_left - _calib_write_votage_x_left * _calib_write_x_mul;
        }
        void _calib_write_calc_param_y()
        {
            _calib_write_y_mul = (_calib_unity_y_bottom - _calib_unity_y_top) / (_calib_write_votage_y_bottom - _calib_write_votage_y_top);
            _calib_write_y_offset = _calib_unity_y_top - _calib_write_votage_y_top * _calib_write_y_mul;
        }

        //Data from the console
        float p1X = 0f;
        float p1Y = 0f;

        float p2X = 0f;
        float p2Y = 0f;

        float ballX = 0f;
        float ballY = 0f;

        float wallX = 0f;

        //Ardity Serial Controller object for communication with the arduino -> console
        SerialController sc;

        //TODO: Handle sending messages back to the console. We want one to be able to play with one person playing through Unity and another through the console.

        public void p1Reset() { }

        /// <summary>
        /// On awake, make singleton and get SerialController instance.
        /// </summary>
        private void Awake()
        {
            if (ConsoleMirror.instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            pluggedIn = false;
            sc = gameObject.GetComponent<SerialController>();
            Debug.Log("ConsoleMirror awake");
        }

        private float lastSendTime = 0;
        private const float updatePeriod = 1.0f / 50.0f; // in second
        private ConsoleDataWrite cdw = new ConsoleDataWrite();

        /// <summary>
        /// On update, set locations to whatever the console sent to us, and send messages to the console
        /// </summary>
        private void LateUpdate()
        {
            if (pluggedIn)
            {
                cdw.P1W = p1Console == false ? 1 : 0;
                if (!p1Console)
                {
                    cdw.P1X = (int)xConvertToConsole(p1.position.x);
                    cdw.P1Y = (int)yConvertToConsole(p1.position.y);
                    //cdw.P1_X = (int)(p1.position.x);
                    //cdw.P1_Y = (int)(p1.position.y);
                } else {
                    p1.position = new Vector2(p1X, p1Y);
                }

                cdw.P2W = p2Console == false ? 1 : 0;
                if (!p2Console)
                {
                    cdw.P2X = (int)xConvertToConsole(p2.position.x);
                    cdw.P2Y = (int)yConvertToConsole(p2.position.y);
                    //cdw.P2_X = (int)(p2.position.x);
                    //cdw.P2_Y = (int)(p2.position.y);
                } else {
                    p2.position = new Vector2(p2X, p2Y);
                }

                if (Time.unscaledTime - lastSendTime >= updatePeriod)
                {
                    string _s = "<" + JsonUtility.ToJson(cdw) + ">";
                    // Debug.Log("[]MSG write: " + _s);
                    sc.SendSerialMessage(_s);
                    lastSendTime = Time.unscaledTime;
                }
                
                ball.position = new Vector2(ballX, ballY);
                line.position = new Vector2(wallX, line.position.y);
            }
        }

        /// <summary>
        /// Convert from TV pixel x to Unity x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float xConvertToUnity(float x)
        {
            return x * _calib_x_mul + _calib_x_offset;
        }

        /// <summary>
        /// Convert from unity x to TV pixel x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public float xConvertToConsole(float x)
        {
            return (x - _calib_write_x_offset) / _calib_write_x_mul;
        }

        /// <summary>
        /// Convert from TV pixel y to Unity y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public float yConvertToUnity(float y)
        {
            return y * _calib_y_mul + _calib_y_offset;
        }

        /// <summary>
        /// Convert from Unity y to TV pixel y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public float yConvertToConsole(float y)
        {
            return (y - _calib_write_y_offset) / _calib_write_y_mul;
        }

        // TODO: Make proper abstrations of console-read,
        //    since we have a calibration routine that want to consume the data
        private ConsoleData mLastConsoleData;
        public ConsoleData readControllerRawData()
        {
            return mLastConsoleData;
        }
        //public void writeController() {}

        /// <summary>
        /// Handles messages recieved from the arduino, setting the appropriate variables. Required by Ardity.
        /// </summary>
        /// <param name="msg"></param>
        void OnMessageArrived(string msg)
        {
            
            //Debug.Log("ConsoleMirror.OnMessageArrived(msg): " + msg);
            mLastConsoleData = JsonUtility.FromJson<ConsoleData>(msg);

            _calib_calc_param_x();
            _calib_calc_param_y();
            _calib_write_calc_param_x();
            _calib_write_calc_param_y();

            if ( ! mLastConsoleData.P1_IS_WRITING)
            {
                p1X = xConvertToUnity(mLastConsoleData.P1_X_READ);
                p1Y = yConvertToUnity(mLastConsoleData.P1_Y_READ);
            }

            if (! mLastConsoleData.P2_IS_WRITING)
            {
                p2X = xConvertToUnity(mLastConsoleData.P2_X_READ);
                p2Y = yConvertToUnity(mLastConsoleData.P2_Y_READ);
            }

            ballX = xConvertToUnity(mLastConsoleData.BALL_X_READ);
            ballY = yConvertToUnity(mLastConsoleData.BALL_Y_READ);
            wallX = xConvertToUnity(mLastConsoleData.WALL_X_READ);
        }

        /// <summary>
        /// Handles connection event. Required by ardity.
        /// </summary>
        /// <param name="success"></param>
        void OnConnectionEvent(bool success)
        {
            if (!success)
            {
                pluggedIn = false;
            }
        }


    }
}
