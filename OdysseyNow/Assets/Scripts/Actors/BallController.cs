using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardDirection;

namespace Actors
{
    /// <summary>
    /// This controls the ball. Since the player does not directly move the ball, this should be called 'BallBehavior'.
    /// </summary>
    public class BallController : MonoBehaviour
    {
        //The three possible possession states. This may need to change with added players.
        enum possession { PLAYER1, PLAYER2, BOTH };
        //By default, ball begins in P1's possession
        possession ballpossesssion = possession.PLAYER1;

        //The two english targets for the two players
        //TODO: Add behavior for p3 and p4
        public GameObject player1EnglishTarget;
        public GameObject player2EnglishTarget;

        // TODO: Implement quirk where both resets pressed forces ball to middle of screen

        //The minimum set ball speed, adjusted by the player to at minimum hit this value
        public float minMaxSpeed = 0.1f;
        //The maximum set ball speed, adjusted by the player to at maximum hit this value
        public float maxMaxSpeed = 0.5f;
        //the practical 'full' speed of the ball
        public float full_speed = 5f;
        //The lag moving to the english target
        public float ball_english_lag = 10;

        //true if the ball should bounce off the wall
        public bool wallBounce = false;
        //true if the ball should extinguish if it hits the wall
        public bool wallExtinguish = false;
        //true if the blayer should extinguish the ball on collision
        public bool playerExtinguish = false;
        //true if hitting reset extinguishes the ball
        public bool resetExtinguishBall = false;

        /// <summary>
        /// On start, set the speed of the ball
        /// </summary>
        void Start()
        {
            full_speed /= 10;
            if (full_speed > maxMaxSpeed) full_speed = maxMaxSpeed;
            if (full_speed < minMaxSpeed) full_speed = minMaxSpeed;
        }

        /// <summary>
        /// On fixed update, if the game isn't paused, move the ball
        /// </summary>
        void FixedUpdate()
        {
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

        /// <summary>
        /// Swap possession of the ball from p1 to p2 or the inverse.
        /// </summary>
        void swapPossession()
        {
            if (ballpossesssion == possession.PLAYER1)
            {
                ballpossesssion = possession.PLAYER2;
            }
            else if (ballpossesssion == possession.PLAYER2)
            {
                ballpossesssion = possession.PLAYER1;
            }
        }

        //called resetButton instead of reset as to not mess with Reset function built into unity
        /// <summary>
        /// Handles reset button behavior.
        /// </summary>
        /// <param name="player"></param>
        public void resetButton(string player)
        {
            if (!((ballpossesssion == possession.PLAYER1 && player.Equals("Player1")) || (ballpossesssion == possession.PLAYER2 && player.Equals("Player2"))))
            {
                unExtinguish();
                swapPossession();
            }
            if (resetExtinguishBall)
            {
                extinguish();
            }
        }

        /// <summary>
        /// Handles what happens when the reset button is lifted
        /// </summary>
        /// <param name="player"></param>
        public void resetButtonUp(string player)
        {
            unExtinguish();
        }

        /// <summary>
        /// Make the ball visible and interactable
        /// </summary>
        void unExtinguish()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        /// <summary>
        /// Make the ball invisible, uninteractable, and play the crowbar noise.
        /// </summary>
        void extinguish()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            CardDirection.SoundFXManager.instance.playSound("Crowbar");
        }

        /// <summary>
        /// On trigger enter, handle that 'collision'
        /// If the ball hit p1 or p2, swap possession and play the bounce noise
        /// If the ball hit the wall, do what it's supposed to do (bounce, extinguish, or nothing) with associated sound effects.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            // Basically when you run into the player, you turn around and the player takes possession
            if (other.gameObject.tag == "Player1")
            {
                if (ballpossesssion != possession.PLAYER1)
                {
                    ballpossesssion = possession.PLAYER1;
                    CardDirection.SoundFXManager.instance.playSound("Bounce");
                }
            }
            else if (other.gameObject.tag == "Player2")
            {
                if (playerExtinguish)
                {
                    other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                else if (ballpossesssion != possession.PLAYER2)
                {
                    ballpossesssion = possession.PLAYER2;
                    CardDirection.SoundFXManager.instance.playSound("Bounce");
                }
            }
            else if (other.gameObject.tag == "Wall")
            {
                if (wallBounce)
                {
                    CardDirection.SoundFXManager.instance.playSound("Bounce");
                    swapPossession();
                }
                else if (wallExtinguish)
                {
                    extinguish();
                    CardDirection.SoundFXManager.instance.playSound("Crowbar");
                }
            }
        }
    }
}