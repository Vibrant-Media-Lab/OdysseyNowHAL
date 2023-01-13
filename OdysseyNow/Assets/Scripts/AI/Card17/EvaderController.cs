using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaderController : MonoBehaviour
{
    // only level 1 has been implemented so far
    public int aiLevel;
    // AI Level 1 changes behavior based on time
    public Text roundTimer;
    public GameObject target;
    // how many units the target moves towards the destination per physics update
    public float targetSpeed = .1f;
    // destination positions for AI Level 1
    public GameObject[] destinations;
    // keeps track of which destination to visit next
    public int sequence;

    public HardwareInterface.ConsoleMirror consoleMirror = HardwareInterface.ConsoleMirror.Instance;
    public Actors.BallController ballController;
    private bool justReset = false;
    private int framesSinceReset;

    public bool lastServedLeft = true;

    public float leftServeTimerLength = 4f;
    public float elapsedTimeLeftServe;

    public float rightServeTimerLength = 3.5f;
    public float elapsedTimeRightServe;

    void FixedUpdate()
    {

        // Fire Projectile
        if (roundTimer.GetComponent<RoundTimer>().activelyTiming)
        {   
            // Player1 Reset (Left Serve)
            if (!lastServedLeft)
            {
                elapsedTimeLeftServe += Time.deltaTime;
                if (elapsedTimeLeftServe >= leftServeTimerLength)
                {
                    consoleMirror._sendP1Reset = true;
                    ballController.resetButton("Player1");
                    elapsedTimeLeftServe = 0f;
                    lastServedLeft = true;
                    // Pick a new timing between 3-5 seconds
                    leftServeTimerLength = Random.Range(3, 6);
                }
            }
            //Player2 Reset (Right Serve)
            else if (lastServedLeft)
            {
                elapsedTimeRightServe += Time.deltaTime;
                if (elapsedTimeRightServe >= rightServeTimerLength)
                {
                    consoleMirror._sendP2Reset = true;
                    ballController.resetButton("Player2");
                    elapsedTimeRightServe = 0f;
                    lastServedLeft = false;
                }
            }
        }
        else
        {
            elapsedTimeLeftServe = 0f;
            elapsedTimeRightServe = 0f;
        }

        // Wait 5 frames, then signal reset button release
        //else if (justReset)
        //{
        //    framesSinceReset++;
        //    Debug.Log("DBG" + framesSinceReset);
        //    if (framesSinceReset >= 5)
        //    {
        //        ballController.resetButtonUp("Player1");
        //        justReset = false;
        //        framesSinceReset = 0;
        //    }
        //}


        // Movement
        if (float.Parse(roundTimer.text) > 40) // Phase 1: 60-40 Seconds
        {
            // resets the sequencing (if necessary)
            if (sequence != 0 && sequence != 1 && sequence != 2 && sequence != 3)
                sequence = 0;

            // if the target hasn't reached the destination, move it closer
            if (target.transform.position != destinations[sequence].transform.position)
                MoveTarget(destinations[sequence]);

            // if the body is close enough to the destination, pick a new one
            if (CheckCloseEnough(destinations[sequence].transform.position))
                sequence = Random.Range(0, 4);
        }
        else if (float.Parse(roundTimer.text) < 40 && float.Parse(roundTimer.text) > 20) // Phase 2: 40-20 Seconds
        {
            if (target.transform.position != destinations[sequence].transform.position)
                MoveTarget(destinations[sequence]);

            if (CheckCloseEnough(destinations[sequence].transform.position))
                sequence = Random.Range(0, 10);
        }
        else // Phase 3: 20-0 Seconds
        {
            if (target.transform.position != destinations[sequence].transform.position)
                MoveTarget(destinations[sequence]);

            if (CheckCloseEnough(destinations[sequence].transform.position))
                sequence = Random.Range(0, 17);
        }
    }

    private void MoveTarget(GameObject destination)
    {
        Vector2 distance = new Vector2(target.transform.position.x - destination.transform.position.x, target.transform.position.y - destination.transform.position.y);
        if (distance.x > 0)
            target.transform.position = new Vector2(target.transform.position.x - targetSpeed, target.transform.position.y);
        if (distance.y > 0)
            target.transform.position = new Vector2(target.transform.position.x, target.transform.position.y - targetSpeed);
        if (distance.x < 0)
            target.transform.position = new Vector2(target.transform.position.x + targetSpeed, target.transform.position.y);
        if (distance.y < 0)
            target.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + targetSpeed);
    }

    private bool CheckCloseEnough(Vector2 destinationPos)
    {
        Vector2 distance = new Vector2(destinationPos.x - this.gameObject.transform.position.x, destinationPos.y - this.gameObject.transform.position.y);
        if (distance.magnitude < .2)
            return true;
        else
            return false;
    }


}
