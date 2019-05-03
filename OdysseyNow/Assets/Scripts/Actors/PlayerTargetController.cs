using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;
using CardDirection;
using HardwareInterface;

namespace Actors
{
    /// <summary>
    /// This controls the player target: where the player box is trying to move to. It has to account for most possible input methods.
    /// This also controls the english target, though we may want to change that as this script continues to expand in complexity.
    /// </summary>
    public class PlayerTargetController : MonoBehaviour
    {
        //This is the visible player block.
        public GameObject playerCube;
        //The start location of the target.
        public float startx, starty;
        //The initial speed... might be redundant at this point...
        public float speed = 5f;
        //Key configurations for keyboard controls... this is not the best way to do this.
        public KeyCode up, down, left, right, reset;
        //bool of whether hitting reset will extinguish the player block
        public bool resetExtinguish;
        //player number (most likely player 1 or player 2)
        int player;

        //The input device assigned to this player, for traditional gamepad input.
        InputDevice con;

        ///On start, set the player number and connect the controller.
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

        /// <summary>
        /// If possible, get a traditional gamepad for this player.
        /// </summary>
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

        /// <summary>
        /// Gets the control scheme based on what was selected in the LocalInputManager. Default is keyboard controls.
        /// </summary>
        /// <returns> A number corresponding to the control scheme </returns>
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

        /// <summary>
        /// On update, try to connect the controller if it isn't already connected & move if the game isn't paused.
        /// </summary>
        void Update()
        {
            //If you aren't connected and you can connect, try to connect.
            if (InputManager.ActiveDevices != null && InputManager.ActiveDevices.Count > 0 && con == null)
            {
                ConnectController();
            }
            
            //If the game isn't paused, move.
            if (!Director.instance.paused)
            {
                //If you have keyboard controls...
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
                //if you have traditional gamepad controls
                else if (GetControlScheme() == 1 && player < 3)
                {
                    //Player target controller for traditional gamepad
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

                        //TODO: handle reset button for all gamepads. One can probably do this with con, an InControl object.

                        if (con.Action1.WasPressed)
                        {
                            ResetButtonDown();
                        }

                        if (con.Action2.WasReleased)
                        {
                            ResetButtonUp();
                        }
                    }
                    catch (Exception e) { }
                }
                //If you have traditional gamepad controls & this is attached to the english target
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
                //If you have a new OdysseyCon (the arduino one)
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
                    //TODO: Handle reset buttons
                }
                //If you're controlling with an original console.
                //This basically just lets the console mirror handle everything. The Unity simulation is a slave.
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
                //If you have an older legacy OdysseyCon (the older arduino controller)
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

        /// <summary>
        /// Handles if the reset button is pressed down.
        /// If you extinguish on reset down, call extinguish.
        /// Call the reset action on the ball.
        /// </summary>
        private void ResetButtonDown()
        {
            if (resetExtinguish)
            {
                extinguish();
            }
            GameObject.FindWithTag("Ball").GetComponent<BallController>().resetButton(gameObject.tag);
        }

        /// <summary>
        /// Handles when the reset button is let back up
        /// </summary>
        private void ResetButtonUp()
        {
            GameObject.FindWithTag("Ball").GetComponent<BallController>().resetButtonUp(gameObject.tag);
            unExtinguish();
            if (gameObject.CompareTag("Player1"))
                ConsoleMirror.instance.p1Reset();
        }

        /// <summary>
        /// On unextinguish, make the player visible and collidable again.
        /// </summary>
        void unExtinguish()
        {
            playerCube.GetComponent<MeshRenderer>().enabled = true;
            playerCube.GetComponent<BoxCollider>().enabled = true;
        }

        /// <summary>
        /// On extinguish, make the ball invisible and uncollidable.
        /// </summary>
        void extinguish()
        {
            playerCube.GetComponent<MeshRenderer>().enabled = false;
            playerCube.GetComponent<BoxCollider>().enabled = false;
        }
    }
}