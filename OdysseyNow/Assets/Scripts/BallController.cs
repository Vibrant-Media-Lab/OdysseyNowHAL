using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    enum possession {PLAYER1, PLAYER2, BOTH};
    possession ballpossesssion = possession.PLAYER1;

    public GameObject player1EnglishTarget;
    public GameObject player2EnglishTarget;

    // TODO: Implement quirk where both resets pressed forces ball to middle of screen
    
    const float minMaxSpeed = 0.1f;
    const float maxMaxSpeed = 0.5f;
    public float full_speed = 5f;

    public float ball_english_lag = 10;

    // Use this for initialization
    void Start ()
    {
        full_speed /= 10;
        if (full_speed > maxMaxSpeed) full_speed = maxMaxSpeed;
        if (full_speed < minMaxSpeed) full_speed = minMaxSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Speed is a function of the current position on the screen, running from the full speed value to some fraction of full speed at min
        // Makes a modifier as a fraction of the screen, and ensures that 1 <= modifier <= 0.5 (so we never stop)
        float speed_modifier = (gameObject.transform.position.x / Screen.width) / 2 + 0.5f;
        // If player 2 has possession of the ball, we have to flip it
        if (ballpossesssion == possession.PLAYER2) speed_modifier = 1 - speed_modifier;
        // If BOTH players have possession, need to set it so that it will go to the center
        // TO BE IMPLEMENTED LATER

        float speed = full_speed * speed_modifier;
        float newx = gameObject.transform.position.x + speed;
        if (Mathf.Abs(newx) > 7) newx = 7;
        // Also need to implement it going to the English of P1 and P2
        float tgty = 0f;
        if (ballpossesssion == possession.PLAYER1) tgty = player1EnglishTarget.transform.position.y;
        if (ballpossesssion == possession.PLAYER2) tgty = player2EnglishTarget.transform.position.y;
        float oldy = gameObject.transform.position.y;
        float newy = (tgty - oldy) / ball_english_lag + gameObject.transform.position.y;
        if (Mathf.Abs(newy - oldy) < 0.05) newy = oldy;
        // Now scoot along, little ball

        gameObject.transform.position = new Vector3(newx, newy, gameObject.transform.position.z);

    }
}
