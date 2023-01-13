using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardwareInterface
{
    /// <summary>
    /// Handles all interface with the OdysseyConLegacy controller
    /// This is the old arduino controller from the original Java version
    /// </summary>
    public class OdysseyConLegacyDirector : MonoBehaviour
    {
        //Singleton instance
        public static OdysseyConLegacyDirector instance;

        //transforms of the players targets and english targets
        public Transform p1;
        public Transform p2;
        public Transform p1English;
        public Transform p2English;

        //true if each of these players are using this controller
        public bool p1Con;
        public bool p2Con;

        //true if this controller is plugged in and being used at all
        public bool pluggedIn;

        //These variables hold the controller inputs
        float p1X = 0f;
        float p1Y = 0f;

        float p2X = 0f;
        float p2Y = 0f;

        float p1E = 0f;
        float p2E = 0f;

        //Ardity serial controller
        SerialController sc;

        /// <summary>
        /// On awake, make singleton, get serialController
        /// </summary>
        private void Awake()
        {
            if (OdysseyConLegacyDirector.instance != null)
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
        /// If using this controller, update position to match controller input
        /// </summary>
        private void LateUpdate()
        {
            if (pluggedIn)
            {
                if (p1Con)
                {
                    p1.position = new Vector2(p1X, p1Y);
                    p1English.position = new Vector2(p1English.position.x, p1E);
                }

                if (p2Con)
                {
                    p2.position = new Vector2(p2X, p2Y);
                    p2English.position = new Vector2(p2English.position.x, p2E);
                }
            }
        }

        /// <summary>
        /// Converts TV pixel to Unity X coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        float xConvertToUnity(string x)
        {
            float percent = float.Parse(x) / 1023.0f;
            return -((percent*16f) - 8f);
        }

        /// <summary>
        /// Converts TV pixel to Unity Y coordinate
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        float yConvertToUnity(string y)
        {
            float percent = float.Parse(y) / 1023.0f;
            return (percent * 12f) - 6f;
        }

        /// <summary>
        /// Required method by Ardity for arduino communication. This handles message receiving.
        /// </summary>
        /// <param name="msg">Serial message sent by the arduino</param>
        void OnMessageArrived(string msg)
        {
            print(msg);
            string[] splitMsg = msg.Split(',');
            p1E = yConvertToUnity(splitMsg[0]);
            p1X = xConvertToUnity(splitMsg[1]);
            p1Y = yConvertToUnity(splitMsg[2]);
            p2E = yConvertToUnity(splitMsg[3]);
            p2X = xConvertToUnity(splitMsg[4]);
            p2Y = yConvertToUnity(splitMsg[5]);
        }

        /// <summary>
        /// Required method by Ardity for arduino communication. This handles the connection event. On success, set pluggedIn=true.
        /// </summary>
        /// <param name="success">Whether connection was successful</param>
        void OnConnectionEvent(bool success)
        {
            if (!success)
            {
                pluggedIn = false;
            }
        }
    }
}
