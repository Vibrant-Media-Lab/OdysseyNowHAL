using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2Card17 : MonoBehaviour
{
    // UI that displays P2's collisions with P1
    public Text collisionsCounter;
    // number of collisions P2 has scored
    public int currentScore = 0;
    // if P2 can score- cannot if currentScore == 10, 20, 30...
    public bool canScore = true;
    // true if P2 is collising with RechargeStation
    public bool isRecharging;

    public float rechargeTimerLength = 2f;
    public float elapsedTimeRecharging;

    public RoundTimer roundTimer;
    public ScoreTracker scoreTracker;
    public GameObject wall;

    public AudioSource P1P2Collision;
    public AudioSource Explosion;
    public AudioSource RechargeComplete;
    public AudioSource RechargeNecessary;

    private void Update()
    {
        // logic for RechargeStation timer
        if (isRecharging)
        {
            elapsedTimeRecharging += Time.deltaTime;
            if (elapsedTimeRecharging >= rechargeTimerLength)
            {
                elapsedTimeRecharging = 0f;
                canScore = true;
                RechargeComplete.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // P1-P2 Collision Enter
        if (other.gameObject.tag == "Player1" && canScore && roundTimer.activelyTiming)
        {
            currentScore++;
            collisionsCounter.text = currentScore.ToString();
            CheckCanScore();
            P1P2Collision.Play();
        }
        // P2-Ball Collision Enter
        if (other.gameObject.tag == "Ball" && (wall.transform.position.x - this.transform.position.x > 0))
        {
            scoreTracker.p2Exploded = true;
            Explosion.Play();
        }
        // P2-RechargeStation Collision Enter
        else if (other.gameObject.name == "RechargeStation")
        {
            isRecharging = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // P2-RechargeStation Collision Exit
        if (other.gameObject.name == "RechargeStation")
        {
            isRecharging = false;
            elapsedTimeRecharging = 0f;
        }
    }

    private void CheckCanScore()
    {
        if (currentScore != 0 && currentScore % 10 == 0)
        {
            canScore = false;
            RechargeNecessary.Play();
        }

        else
            canScore = true;
    }

    public void ResetCollisions()
    {
        currentScore = 0;
        collisionsCounter.text = currentScore.ToString();
    }
}
