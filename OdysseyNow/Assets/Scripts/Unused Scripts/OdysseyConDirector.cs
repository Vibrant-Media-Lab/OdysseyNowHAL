using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors;

namespace HardwareInterface
{
    /// <summary>
    /// Handles all communication with the OdysseyCon arduino controller
    /// </summary>
    public class OdysseyConDirector : MonoBehaviour
    {
        //Singleton instance
        public static OdysseyConDirector instance;

        //transforms of both targets and english targets for players
        public Transform p1;
        public Transform p2;
        public Transform p1English;
        public Transform p2English;

        //The ball controller script
        public BallController bc;

        //true if p1 or p2 are using this controller
        public bool p1Con;
        public bool p2Con;

        //true if the odysseyCon is connected
        public bool pluggedIn;

        //Inputs from the controller
        float p1X = 0f;
        float p1Y = 0f;

        float p2X = 0f;
        float p2Y = 0f;

        float p1E = 0f;
        float p2E = 0f;

        //inputs from controller on previous frame; this is important for these since they create events based on change 
        //(ie, we only care about the frame a button is pressed, not all the time a button is down.)
        float prev_crowbar = 0;
        float prev_crowbar_reset = 0;
        float prev_enter = 0;
        float prev_select = 0;
        float prev_encoder = 0;
        float prev_p1_reset = 0;
        float prev_p2_reset = 0;

        SerialController sc;


        void p1Reset()
        {
            if(p1Con){

            }
        }

        void p2Reset()
        {
            if (p2Con)
            {

            }
        }

        void p1ResetUp()
        {
            if (p1Con)
            {

            }
        }

        void p2ResetUp()
        {
            if (p2Con)
            {

            }
        }

        void enterButton(){
            print("Enter!");
        }

        void selectButton(){
            print("Select!");
        }

        void encoderUp(){
            print("encoderUp!");
        }

        void encoderDown(){
            print("EncoderDown!");
        }

        void crowbar(){
            print("Crowbar!");
        }

        void crowbarReset(){
            print("Crowbar reset!");
        }

        //end TODO

        /// <summary>
        /// On awake, make a singleton and get the serialcontroller (Ardity object).
        /// </summary>
        private void Awake()
        {
            if (OdysseyConDirector.instance != null)
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
        /// If using this controller, set locations based on player input.
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
        /// Convert controller given to Unity X
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        float xConvertToUnity(float x)
        {
            float percent = (x - 140) / 560.0f;
            return -((percent*16f) - 8f);
        }

        /// <summary>
        /// Convert controller given to Unity Y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        float yConvertToUnity(float y)
        {
            float percent = (y-180) / 620.0f;
            return (percent * 12f) - 6f;
        }

        /// <summary>
        /// Convert english value from controller to usable Unity Y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        float englishConvertToUnity(float y)
        {
            float percent = (y - 90) / 840.0f;
            return (percent* 12f) - 6f;
        }

        /// <summary>
        /// Converts controller's given ball speed to a value usable by unity.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        float convertSpeed(float s){
            float percent = s / 255.0f;
            return (percent * (bc.maxMaxSpeed - bc.minMaxSpeed)) + bc.minMaxSpeed;
        }

        /// <summary>
        /// Required method from Ardity. Handles received message from arduino.
        /// Takes in arduino input and set variables.
        /// </summary>
        /// <param name="msg"></param>
        void OnMessageArrived(string msg)
        {
            Debug.Log("OnMessageArrived(string msg): " + msg);
            OdysseyConData data = JsonUtility.FromJson<OdysseyConData>(msg);
            p1X = xConvertToUnity(data.P1_X_READ);
            p1Y = yConvertToUnity(data.P1_Y_READ);
            p2X = xConvertToUnity(data.P2_X_READ);
            p2Y = yConvertToUnity(data.P2_Y_READ);
            p1E = englishConvertToUnity(data.P1_ENG_READ);
            p2E = englishConvertToUnity(data.P2_ENG_READ);
            bc.full_speed = convertSpeed(data.BALL_SPEED_READ);

            if(prev_crowbar == 0 && data.CROWBAR_READ == 1){
                crowbar();
            }

            if (prev_crowbar_reset == 0 && data.CROWBAR_RESET_READ == 1){
                crowbarReset();
            }

            if(prev_encoder < data.ENCODER_READ){
                encoderUp();
            } else if(prev_encoder > data.ENCODER_READ){
                encoderDown();
            }

            if (prev_enter == 0 && data.ENTER_READ == 1){
                enterButton();
            }

            if (prev_select == 0 && data.SELECT_READ == 1){
                selectButton();
            }

            if (prev_p1_reset == 0 && data.P1_RESET_READ == 1){
                p1Reset();
            } else if (prev_p1_reset == 1 && data.P1_RESET_READ == 0){
                p1ResetUp();
            }

            if (prev_p2_reset == 0 && data.P2_RESET_READ == 1){
                p2Reset();
            } else if (prev_p2_reset == 1 && data.P2_RESET_READ == 0){
                p2ResetUp();
            }

            prev_select = data.SELECT_READ;
            prev_crowbar = data.CROWBAR_READ;
            prev_crowbar_reset = data.CROWBAR_RESET_READ;
            prev_encoder = data.ENCODER_READ;
            prev_enter = data.ENTER_READ;
            prev_p1_reset = data.P1_RESET_READ;
            prev_p2_reset = data.P2_RESET_READ;
        }

        /// <summary>
        /// Handles connection with arduino. Required by ardity.
        /// </summary>
        /// <param name="success">True if connected to controller</param>
        void OnConnectionEvent(bool success)
        {
            if (!success)
            {
                pluggedIn = false;
            }
        }
    }
}
