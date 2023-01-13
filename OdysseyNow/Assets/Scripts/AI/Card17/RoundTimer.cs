using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour
{
    public Text roundTimer;
    public float roundTimerLength = 60f;
    public float elapsedRoundTime = 0f;
    public bool activelyTiming = false;

    public AudioSource TimeOut;

    private void Update()
    {
        if (activelyTiming)
        {
            elapsedRoundTime += Time.deltaTime;
            if (elapsedRoundTime >= roundTimerLength)
            {
                activelyTiming = false;
                elapsedRoundTime = 0f;
                TimeOut.Play();
            }

            roundTimer.text = (roundTimerLength - elapsedRoundTime).ToString();
        }
    }
}
