using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    public RoundTimer roundtimer;
    public P2Card17 player2;
    public int p2CollisionsToWin = 40;

    public int p1Score = 0;
    public int p2Score = 0;
    public Text p1ScoreText;
    public Text p2ScoreText;

    public bool p2Exploded = false;

    private bool roundNotScored = false;

    public AudioSource Escape;
    public AudioSource Intro;
    public AudioSource Success;

    void Update()
    {
        CheckScoring();

        // Reset timer, start new round
        // May want to allow other inputs to reset the round timer
        if (Input.GetKeyDown("space") && !roundtimer.activelyTiming)
        {
            roundtimer.elapsedRoundTime = 0f;
            roundtimer.activelyTiming = true;

            player2.ResetCollisions();

            roundNotScored = true;

            player2.canScore = true;
        }
    }

    private void CheckScoring()
    {
        if (roundNotScored)
        {
            // p2 win-condition (collisions)
            if (roundtimer.activelyTiming && player2.currentScore == p2CollisionsToWin)
            {
                p2Score++;
                p2ScoreText.text = p2Score.ToString();

                roundtimer.activelyTiming = false;

                roundNotScored = false;
                Success.Play();
            }

            // p1 win-condition (collision)
            else if (p2Exploded)
            {
                p1Score++;
                p1ScoreText.text = p1Score.ToString();

                roundtimer.activelyTiming = false;

                roundNotScored = false;
                p2Exploded = false;
            }
            // p1 win-condition (time)
            else if (!roundtimer.activelyTiming && player2.currentScore < p2CollisionsToWin && !Intro.isPlaying)
            {
                p1Score++;
                p1ScoreText.text = p1Score.ToString();

                roundNotScored = false;

                Escape.Play();
            }
        }
    }
}
