using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors;

namespace HardwareInterface
{
    public class OdysseyConDirector : MonoBehaviour
    {
        public static OdysseyConDirector instance;

        public Transform p1;
        public Transform p2;
        public Transform p1English;
        public Transform p2English;

        public BallController bc;

        public bool p1Con;
        public bool p2Con;
        public bool pluggedIn;

        float p1X = 0f;
        float p1Y = 0f;

        float p2X = 0f;
        float p2Y = 0f;

        float p1E = 0f;
        float p2E = 0f;

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

        float xConvertToUnity(float x)
        {
            float percent = (x - 140) / 560.0f;
            return -((percent*16f) - 8f);
        }

        float yConvertToUnity(float y)
        {
            float percent = (y-180) / 620.0f;
            return (percent * 12f) - 6f;
        }

        float englishConvertToUnity(float y)
        {
            float percent = (y - 90) / 840.0f;
            return (percent* 12f) - 6f;
        }

        float convertSpeed(float s){
            float percent = s / 255.0f;
            return (percent * (bc.maxMaxSpeed - bc.minMaxSpeed)) + bc.minMaxSpeed;
        }

        void OnMessageArrived(string msg)
        {
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

        void OnConnectionEvent(bool success)
        {
            if (!success)
            {
                pluggedIn = false;
            }
        }
    }
}
