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
            x = x - 115.0f;
            x = (x / 47.0f) * -6.75f;
            return x;
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
            y = y - 126.0f;
            y = (y / 52.0f) * 5.0f;
            return y;
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

        /// <summary>
        /// Handles messages recieved from the arduino, setting the appropriate variables. Required by Ardity.
        /// </summary>
        /// <param name="msg"></param>
        void OnMessageArrived(string msg)
        {
            msg = msg.Substring(0, msg.Length - 3) + "}";
            ConsoleData data = JsonUtility.FromJson<ConsoleData>(msg);
            p1X = xConvertToUnity(data.P1_X_READ);
            p1Y = yConvertToUnity(data.P1_Y_READ);
            p2X = xConvertToUnity(data.P2_X_READ);
            p2Y = yConvertToUnity(data.P2_Y_READ);
            ballX = xConvertToUnity(data.BALL_X_READ);
            ballY = yConvertToUnity(data.BALL_Y_READ);
            wallX = xConvertToUnity(data.WALL_X_READ);
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
