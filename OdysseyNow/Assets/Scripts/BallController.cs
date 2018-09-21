using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    bool directionRight = true;
    float speed = 5f;
    float decelRate = 0.03f;
    float minMaxSpeed = 4.2f;
    float maxMaxSpeed = 9f;
    float maxSpeed = 7f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }
}
