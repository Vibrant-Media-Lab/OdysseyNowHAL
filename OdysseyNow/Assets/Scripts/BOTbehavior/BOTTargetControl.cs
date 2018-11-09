using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOTTargetControl : MonoBehaviour {
    public float startx, starty;
    public GameObject ball;
    
    void Start () {
        gameObject.transform.position = new Vector3(startx, starty,0);
    }
	
	void Update () {
        //Always follows the ball(LagFactor reduced to allow the AI catch up to the ball)
        if (ball.transform.position.x>0)
        {
            float y = ball.transform.position.y;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        }
	}
}
