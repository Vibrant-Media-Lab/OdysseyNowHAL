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

    public bool wallBounce = false;
    public bool wallExtinguish = false;
    public bool playerExtinguish = false;

    public bool resetExtinguishBall = false;

    // Use this for initialization
    void Start ()
    {
        full_speed /= 10;
        if (full_speed > maxMaxSpeed) full_speed = maxMaxSpeed;
        if (full_speed < minMaxSpeed) full_speed = minMaxSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Director.instance.paused)
        {
            // Speed is a function of the current position on the screen, running from the full speed value to some fraction of full speed at min
            // Makes a modifier as a fraction of the screen, and ensures that 1 <= modifier <= 0.5 (so we never stop)
            // Do it as an offset from the left so that the coordinates are always positive
            float coordinate_offset_from_left = gameObject.transform.position.x + Screen.width;
            float speed_modifier = (coordinate_offset_from_left / Screen.width) / 2 + 0.5f;
            // If player 2 has possession of the ball, we have to flip it
            if (ballpossesssion == possession.PLAYER2) speed_modifier = speed_modifier * -1;
            // If BOTH players have possession, need to set it so that it will go to the center
            // Do this by just enabling both behaviours
            if (ballpossesssion == possession.BOTH)
            {
                if (gameObject.transform.position.x > 0) speed_modifier = speed_modifier * -1;
                else if (Mathf.Abs(gameObject.transform.position.x) < 0.05) speed_modifier = 0;
            }

            float speed = full_speed * speed_modifier;
            float newx = gameObject.transform.position.x + speed;
            if (newx > 7) newx = 7;
            if (newx < -7) newx = -7;
            // Also need to implement it going to the English of P1 and P2
            float tgty = 0f;
            if (ballpossesssion == possession.PLAYER1) tgty = player1EnglishTarget.transform.position.y;
            if (ballpossesssion == possession.PLAYER2) tgty = player2EnglishTarget.transform.position.y;
            if (ballpossesssion == possession.BOTH) tgty = (player1EnglishTarget.transform.position.y + player2EnglishTarget.transform.position.y) / 2;
            float oldy = gameObject.transform.position.y;

            // Closes distance in the same way the playerspots do
            // Though tbh this doesn't match console behaviour very well, so we'll have to fix it up a little
            float newy = (tgty - oldy) / ball_english_lag + gameObject.transform.position.y;
            if (Mathf.Abs(newy - oldy) < 0.05) newy = oldy;

            // Now scoot along, little ball
            gameObject.transform.position = new Vector3(newx, newy, gameObject.transform.position.z);

            //Debug.Log(ballpossesssion);
        }
    }

    void swapPossession(){
        if(ballpossesssion == possession.PLAYER1){
            ballpossesssion = possession.PLAYER2;
        } else if (ballpossesssion == possession.PLAYER2){
            ballpossesssion = possession.PLAYER1;
        }
    }

    //called resetButton instead of reset as to not mess with Reset function built into unity
    public void resetButton(string player){
        if (!((ballpossesssion == possession.PLAYER1 && player.Equals("Player1")) || (ballpossesssion == possession.PLAYER2 && player.Equals("Player2")))) {
            unExtinguish();
            swapPossession();
        }
        if(resetExtinguishBall){
            extinguish();
        }
    }

    public void resetButtonUp(string player){
        unExtinguish();
    }

    void unExtinguish(){
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void extinguish(){
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Basically when you run into the player, you turn around and the player takes possession
        if (other.gameObject.tag == "Player1")
        {
            ballpossesssion = possession.PLAYER1;
        }
        else if (other.gameObject.tag == "Player2") {
            if (playerExtinguish){
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            } else {
                ballpossesssion = possession.PLAYER2;
            }
        }
        else if(other.gameObject.tag == "Wall"){
            if(wallBounce){
                swapPossession();
            } else if (wallExtinguish){
                extinguish();
            }
        }
    }
}
