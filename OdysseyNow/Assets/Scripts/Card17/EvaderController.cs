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
    private bool hasFired = false;
    public Actors.BallController ballController;
    private bool justReset = false;
    private int framesSinceReset;

    void FixedUpdate()
    {
        // fire projectile
        // Player1 Reset
        if ((int)float.Parse(roundTimer.text) % 10 == 0 && !hasFired)
        {
            Debug.Log("DBG Left Serve");
            consoleMirror._sendP1Reset = true;
            ballController.resetButton("Player1");
            hasFired = true;
            justReset = true;
        }
        // Player2 Reset
        else if ((int)float.Parse(roundTimer.text) % 5 == 0 && !hasFired)
        {
            Debug.Log("DBG Right Serve");
            consoleMirror._sendP2Reset = true;
            ballController.resetButton("Player2");
            hasFired = true;
            justReset = true;
        }

        // Wait 5 frames, then signal reset button release
        else if (justReset)
        {
            framesSinceReset++;
            Debug.Log("DBG" + framesSinceReset);
            if (framesSinceReset >= 5)
            {
                ballController.resetButtonUp("Player1");
                justReset = false;
                framesSinceReset = 0;
            }
        }

        // Primes the AI to fire again when possible
        else if ((int)float.Parse(roundTimer.text) % 10 != 0 && (int)float.Parse(roundTimer.text) % 5 != 0)
        {
            hasFired = false;
        }



        // this is ugly, but the basic idea is to move the target between pre-determined destinations,
        // based on the time left on the round timer
        if (float.Parse(roundTimer.text) > 45) // Part 1: Sequences 0, 1. 60-45 seconds
        {
            if (sequence != 0 && sequence != 1)
                sequence = 0;

            if (target.transform.position != destinations[sequence].transform.position)
                MoveTarget(destinations[sequence]);

            if (CheckCloseEnough(destinations[0].transform.position))
            {
                sequence = 1;
            }
            else if (CheckCloseEnough(destinations[1].transform.position))
            {
                sequence = 0;
            }
        }
        else if (float.Parse(roundTimer.text) <= 45 && float.Parse(roundTimer.text) > 25) // Part 2: Sequences 2, 3. 45-25 seconds
        {
            if (sequence != 2 && sequence != 3)
                sequence = 2;

            if (target.transform.position != destinations[sequence].transform.position)
                MoveTarget(destinations[sequence]);

            if (CheckCloseEnough(destinations[2].transform.position))
            {
                sequence = 3;
            }
            else if (CheckCloseEnough(destinations[3].transform.position))
            {
                sequence = 2;
            }
        }
        else // Part 3: Sequences 0, 1, 2, 3. 25-0 seconds
        {
            if (target.transform.position != destinations[sequence].transform.position)
                MoveTarget(destinations[sequence]);

            if (CheckCloseEnough(destinations[0].transform.position))
            {
                sequence = 1;
            }
            else if (CheckCloseEnough(destinations[1].transform.position))
            {
                sequence = 2;
            }
            else if (CheckCloseEnough(destinations[2].transform.position))
            {
                sequence = 3;
            }
            else if (CheckCloseEnough(destinations[3].transform.position))
            {
                sequence = 0;
            }
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
        if (distance.magnitude < .1)
            return true;
        else
            return false;
    }


}
