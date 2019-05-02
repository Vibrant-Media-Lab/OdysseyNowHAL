using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;
using CardDirection;
using HardwareInterface;

namespace Actors
{
    public class PlayerTargetController : MonoBehaviour
    {
        public GameObject playerCube;
        public float startx, starty;
        public float speed = 5f;
        public KeyCode up, down, left, right, reset;
        public bool resetExtinguish;
        int player;

        InputDevice con;

        private void Start()
        {
            player = 1;
            switch (gameObject.tag)
            {
                case "Player1":
                    player = 1;
                    break;
                case "Player2":
                    player = 2;
                    break;
                case "Player1English":
                    player = 3;
                    break;
                case "Player2English":
                    player = 4;
                    break;
            }

            ConnectController();
        }

        void ConnectController()
        {
            try
            {
                con = InputManager.ActiveDevices[(player - 1) % 2];
            }
            catch (Exception e)
            {
                //Debug.Log(e.Message);
                //Debug.Log("Missing gamepad: " + ((player - 1) % 2));
            }
        }

        int GetControlScheme()
        {
            LocalInputManager.ControlScheme scheme = LocalInputManager.ControlScheme.Keyboard;

            if (LocalInputManager.instance == null) {
                scheme = LocalInputManager.ControlScheme.Keyboard;
            } else if (player == 1 || player == 3){
                scheme = LocalInputManager.instance.p1Scheme;
            }
            else if (player == 2 || player == 4)
            {
                scheme = LocalInputManager.instance.p2Scheme;
            }

            switch (scheme)
            {
                case LocalInputManager.ControlScheme.Keyboard:
                    return 0;
                case LocalInputManager.ControlScheme.Traditional:
                    return 1;
                case LocalInputManager.ControlScheme.OdysseyCon:
                    return 2;
                case LocalInputManager.ControlScheme.OriginalConsole:
                    return 3;
                case LocalInputManager.ControlScheme.AI:
                    return 4;
                case LocalInputManager.ControlScheme.OdysseyConLegacy:
                    return 5;
            }

            return 0;
        }

        // Update is called once per frame
        void Update()
        {

            if (InputManager.ActiveDevices != null && InputManager.ActiveDevices.Count > 0 && con == null)
            {
                ConnectController();
            }

            if (!Director.instance.paused)
            {
                if (GetControlScheme() == 0)
                {
                    //player target & english control for keyboard controls
                    float x = gameObject.transform.position.x;
                    float y = gameObject.transform.position.y;

                    float adjustedspeed = speed / 10;

                    if (Input.GetKey(up) && y < 6)
                    {
                        y += adjustedspeed;
                    }
                    if (Input.GetKey(down) && y > -6)
                    {
                        y -= adjustedspeed;
                    }
                    if (Input.GetKey(left) && x > -8)
                    {
                        x -= adjustedspeed;
                    }
                    if (Input.GetKey(right) && x < 8)
                    {
                        x += adjustedspeed;
                    }

                    gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);

                    if (Input.GetKeyDown(reset))
                    {
                        ResetButtonDown();
                    }

                    if (Input.GetKeyUp(reset))
                    {
                        ResetButtonUp();
                    }
                }
                else if (GetControlScheme() == 1 && player < 3)
                {
                    //Player target controll for traditional gamepad
                    try
                    {
                        if (Math.Abs(con.LeftStickX.RawValue) < 0.05f || Math.Abs(con.LeftStickY.RawValue) < 0.05f || Math.Abs(con.RightStickX.RawValue) < 0.05f)
                        {
                            gameObject.transform.position = new Vector3(10 * con.LeftStickX.RawValue, 10 * con.LeftStickY.RawValue, transform.position.z);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector3(10 * con.ReadRawAnalogValue(0), -10 * con.ReadRawAnalogValue(1), transform.position.z);
                        }

                        if (con.Action1.WasPressed)
                        {
                            print("Blah!!!");
                            ResetButtonDown();
                        }

                        if (con.Action2.WasReleased)
                        {
                            print("Up!!!");
                            ResetButtonUp();
                        }
                    }
                    catch (Exception e) { }
                }
                else if (GetControlScheme() == 1)
                {
                    //Control for english with gamepad
                    try
                    {
                        if (con.RightStickX.RawValue != 0)
                        {
                            gameObject.transform.position = new Vector3(transform.position.x, 6 * con.RightStickX.RawValue, transform.position.z);
                        }
                        else
                        {
                            gameObject.transform.position = new Vector3(transform.position.x, -6 * con.ReadRawAnalogValue(3), transform.position.z);
                        }
                    }
                    catch (Exception e) { }
                }
                else if (GetControlScheme() == 2 && player < 3)
                {
                    //control for player target with OdysseyCon
                    OdysseyConDirector.instance.pluggedIn = true;
                    if (player == 1)
                    {
                        OdysseyConDirector.instance.p1Con = true;
                    }
                    if (player == 2)
                    {
                        OdysseyConDirector.instance.p2Con = true;
                    }
                }
                else if (GetControlScheme() == 3 && player < 3)
                {
                    ConsoleMirror.instance.pluggedIn = true;
                    if (player == 1)
                    {
                        ConsoleMirror.instance.p1Console = true;
                    }
                    if (player == 2)
                    {
                        ConsoleMirror.instance.p2Console = true;
                    }
                }
                else if (GetControlScheme() == 5 && player < 3)
                {
                    //control for player target with OdysseyCon
                    OdysseyConLegacyDirector.instance.pluggedIn = true;
                    if (player == 1)
                    {
                        OdysseyConLegacyDirector.instance.p1Con = true;
                    }
                    if (player == 2)
                    {
                        OdysseyConLegacyDirector.instance.p2Con = true;
                    }
                }
            }
        }

        private void ResetButtonDown()
        {
            if (resetExtinguish)
            {
                extinguish();
            }
            GameObject.FindWithTag("Ball").GetComponent<BallController>().resetButton(gameObject.tag);
        }

        private void ResetButtonUp()
        {
            GameObject.FindWithTag("Ball").GetComponent<BallController>().resetButtonUp(gameObject.tag);
            unExtinguish();
            if (gameObject.CompareTag("Player1"))
                ConsoleMirror.instance.p1Reset();
        }

        void unExtinguish()
        {
            playerCube.GetComponent<MeshRenderer>().enabled = true;
            playerCube.GetComponent<BoxCollider>().enabled = true;
        }

        void extinguish()
        {
            playerCube.GetComponent<MeshRenderer>().enabled = false;
            playerCube.GetComponent<BoxCollider>().enabled = false;
        }
    }
}