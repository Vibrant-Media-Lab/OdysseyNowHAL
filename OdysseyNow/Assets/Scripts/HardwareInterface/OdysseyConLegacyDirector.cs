using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardwareInterface
{
    public class OdysseyConLegacyDirector : MonoBehaviour
    {
        public static OdysseyConLegacyDirector instance;

        public Transform p1;
        public Transform p2;
        public Transform p1English;
        public Transform p2English;

        public bool p1Con;
        public bool p2Con;
        public bool pluggedIn;

        float p1X = 0f;
        float p1Y = 0f;

        float p2X = 0f;
        float p2Y = 0f;

        float p1E = 0f;
        float p2E = 0f;

        SerialController sc;

        public void p1Reset()
        {
            //if(pluggedIn)
            //sc.SendSerialMessage("p1Reset");
        }

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

        float xConvertToUnity(string x)
        {
            float percent = float.Parse(x) / 1023.0f;
            return -((percent*16f) - 8f);
        }

        float yConvertToUnity(string y)
        {
            float percent = float.Parse(y) / 1023.0f;
            return (percent * 12f) - 6f;
        }

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

        void OnConnectionEvent(bool success)
        {
            if (!success)
            {
                pluggedIn = false;
            }
        }
    }
}
