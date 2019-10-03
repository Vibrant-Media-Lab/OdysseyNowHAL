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

        // for calibration
        public float _calib_votage_x_min = 617;
        public float _calib_votage_x_max = 300;
        public float _calib_votage_y_min = 711;
        public float _calib_votage_y_max = 379;
        public float _calib_unity_x_min = -7.7f;
        public float _calib_unity_x_max = 7.7f;
        public float _calib_unity_y_min = 4.4f;
        public float _calib_unity_y_max = -4.4f;

        //public float calib_votage_x_min
        //{
        //    get { return _calib_votage_x_min; }
        //    set { _calib_votage_x_min = value; _calib_calc_param_x(); }
        //}
        //public float calib_votage_x_max
        //{
        //    get { return _calib_votage_x_max; }
        //    set { _calib_votage_x_max = value; _calib_calc_param_x(); }
        //}
        //public float calib_votage_y_min
        //{
        //    get { return _calib_votage_y_min; }
        //    set { _calib_votage_y_min = value; _calib_calc_param_y(); }
        //}
        //public float calib_votage_y_max
        //{
        //    get { return _calib_votage_y_max; }
        //    set { _calib_votage_y_max = value; _calib_calc_param_y(); }
        //}

        public float _calib_x_mul = -1;
        public float _calib_x_offset = -1;
        public float _calib_y_mul = -1;
        public float _calib_y_offset = -1;

        void _calib_calc_param_x() {
            _calib_x_mul = (_calib_unity_x_max - _calib_unity_x_min) / (_calib_votage_x_max - _calib_votage_x_min);
            _calib_x_offset = _calib_unity_x_min - _calib_votage_x_min * _calib_x_mul;
        }
        void _calib_calc_param_y() {
            _calib_y_mul = (_calib_unity_y_max - _calib_unity_y_min) / (_calib_votage_y_max - _calib_votage_y_min);
            _calib_y_offset = _calib_unity_y_min - _calib_votage_y_min * _calib_y_mul;
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
        }

        /// <summary>
        /// On update, set locations to whatever the console sent to us, and (TODO) send messages to the console
        /// </summary>
        private void LateUpdate()
        {
            if (pluggedIn)
            {
                if (!p1Console)
                {
                    //sc.SendSerialMessage("setP1 " + xConvertToConsole(p1.position.x) + " " + yConvertToConsole(p1.position.y) + " ");
                }

                if (!p2Console)
                {

                }

                p1.position = new Vector2(p1X, p1Y);
                p2.position = new Vector2(p2X, p2Y);
                ball.position = new Vector2(ballX, ballY);
                line.position = new Vector2(wallX, line.position.y);
            }
        }

        /// <summary>
        /// Convert from TV pixel x to Unity x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        float xConvertToUnity(float x)
        {
            return x * _calib_x_mul + _calib_x_offset;
        }

        /// <summary>
        /// Convert from unity x to TV pixel x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        float xConvertToConsole(float x)
        {
            x = (x / -6.75f) * 47.0f;
            x = x + 115.0f;
            return x;
        }

        /// <summary>
        /// Convert from TV pixel y to Unity y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        float yConvertToUnity(float y)
        {
            return y * _calib_y_mul + _calib_y_offset;
        }

        /// <summary>
        /// Convert from Unity y to TV pixel y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        float yConvertToConsole(float y)
        {
            y = (y / 5.0f) * 52.0f;
            y = y + 126.0f;
            return y;
        }

        // TODO: Make proper abstrations of console-read,
        //    since we have a calibration routine that want to consume the data
        private ConsoleData mLastConsoleData;
        public ConsoleData getControllerRawData()
        {
            return mLastConsoleData;
        }

        /// <summary>
        /// Handles messages recieved from the arduino, setting the appropriate variables. Required by Ardity.
        /// </summary>
        /// <param name="msg"></param>
        void OnMessageArrived(string msg)
        {
            //msg = msg.Substring(0, msg.Length - 3) + "}";
            Debug.Log("ConsoleMirror.OnMessageArrived(msg): " + msg);
            mLastConsoleData = JsonUtility.FromJson<ConsoleData>(msg);
            _calib_calc_param_x();
            _calib_calc_param_y();
            p1X = xConvertToUnity(mLastConsoleData.P1_X_READ);
            p1Y = yConvertToUnity(mLastConsoleData.P1_Y_READ);
            p2X = xConvertToUnity(mLastConsoleData.P2_X_READ);
            p2Y = yConvertToUnity(mLastConsoleData.P2_Y_READ);
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

        /// <summary>
        /// Start calibration to map votage level from Arduino to unity value
        /// </summary>
        public void StartCalibration()
        {


            p1.position = new Vector2(_calib_unity_x_min, _calib_unity_y_min);

            // Please move the player 1 to the upper-left coner, as shown on the HAL screen

            p1.position = new Vector2(_calib_unity_x_max, _calib_unity_y_max);

            // Please move the player 1 to the upper-left coner, as shown on the HAL screen

            //ball.position = new Vector2(ballX, ballY);
            //line.position = new Vector2(wallX, line.position.y);

            // todo Prompt to ask user to tune the knobs and move the p1/p2 position



        }

    }
}
