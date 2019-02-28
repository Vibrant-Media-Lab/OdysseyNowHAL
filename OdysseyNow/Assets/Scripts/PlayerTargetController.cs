using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetController : MonoBehaviour {
    public GameObject playerCube;
    public float startx, starty;
    public float speed = 5f;
    public KeyCode up, down, left, right, reset;
    public bool resetExtinguish;

	// Update is called once per frame
	void Update () {
        if (!Director.instance.paused)
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
