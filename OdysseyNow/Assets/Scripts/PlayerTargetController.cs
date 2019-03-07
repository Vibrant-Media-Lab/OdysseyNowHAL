using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class PlayerTargetController : MonoBehaviour {
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
        switch(gameObject.tag){
            case "Player1":
                player = 1;
                break;
            case "Player2":
                player = 2;
                break;
        }

        try
        {
            print("Inputs: " + InputManager.Devices.Count);
            print(InputManager.Devices[0].DeviceClass);
            con = InputManager.Devices[player - 1];
        }catch(Exception e){
            Debug.Log(e.Message);
            Debug.Log("Missing gamepad!");
        }
    }

    int GetControlScheme(){
        LocalInputManager.ControlScheme scheme = LocalInputManager.ControlScheme.Keyboard;

        if (player == 1){
            scheme = LocalInputManager.instance.p1Scheme;
        }
        else if (player == 1)
        {
            scheme = LocalInputManager.instance.p2Scheme;
        }

        switch(scheme){
            case LocalInputManager.ControlScheme.Keyboard:
                return 0;
            case LocalInputManager.ControlScheme.Traditional:
                return 1;
            case LocalInputManager.ControlScheme.OdysseyCon:
                return 2;
        }

        return 0;
    }

    // Update is called once per frame
    void Update () {
        if (!Director.instance.paused)
        {
            if (GetControlScheme() == 0)
            {
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

            else if (GetControlScheme() == 1){
                gameObject.transform.position = new Vector3(con.LeftStickX.RawValue, con.RightStickY.RawValue, transform.position.z);
            }
        }
	}

    private void ResetButtonDown(){
        GameObject.FindWithTag("Ball").GetComponent<BallController>().resetButton(gameObject.tag);
        if(resetExtinguish){
            extinguish();
        }
    }

    private void ResetButtonUp(){
        GameObject.FindWithTag("Ball").GetComponent<BallController>().resetButtonUp(gameObject.tag);
        unExtinguish();
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
