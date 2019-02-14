using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetController : MonoBehaviour {
    public float startx, starty;
    public float speed = 5f;
    public KeyCode up, down, left, right;
	
	// Update is called once per frame
	void Update () {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;

        float adjustedspeed = speed / 10;

        if (Input.GetKey(up) && y < 6) {
            y += adjustedspeed;
        }
        if (Input.GetKey(down) && y > -6) {
            y -= adjustedspeed;
        }
        if (Input.GetKey(left) && x > -8) {
            x -= adjustedspeed;
        }
        if (Input.GetKey(right) && x < 8) {
            x += adjustedspeed;
        }

        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
	}
}
