using System.Collections;
using System.Collections.Generic;
using CardDirection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ModeDirector : MonoBehaviour
{
    public GameObject go_calibration;
    public GameObject go_AI;

    void Awake()
    {
        Debug.Log(PlayerPrefs.GetInt("ai1"));
        Debug.Log(PlayerPrefs.GetInt("ai2"));
        Debug.Log(PlayerPrefs.GetString("game"));
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (localInputManager.isdirty)
        //{
        // Enable calibration

        if (LocalInputManager.instance == null)
        {
            // fixme
            go_calibration.GetComponent<CalibrationDirectorNew>()
                .afterCalibration.AddListener(StartGame_CatAI);
            go_calibration.GetComponent<CalibrationOdysseySettings>().p2_read = true;
            StartCalibration();

            return;
        }

        if (LocalInputManager.instance.p1Scheme
            == LocalInputManager.ControlScheme.AI)
        {
            go_calibration.GetComponent<CalibrationDirectorNew>()
                .afterCalibration.AddListener(StartGame_CatAI);
        }
        if (LocalInputManager.instance.p1Scheme
            == LocalInputManager.ControlScheme.OriginalConsole)
        {
            go_calibration.GetComponent<CalibrationOdysseySettings>().p1_read = true;
        }
        if (LocalInputManager.instance.p2Scheme
            == LocalInputManager.ControlScheme.OriginalConsole)
        {
            go_calibration.GetComponent<CalibrationOdysseySettings>().p2_read = true;
        }

        StartCalibration();

        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame_CatAI()
    {
        Debug.Log("in StartGame_CatAI");
        go_calibration.SetActive(false);
        GameObject p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        p1.GetComponent<NavMeshAgent>().enabled = true;
        go_AI.SetActive(true);
    }

    public void StartCalibration()
    {
        // Turn Off Cat AI
        go_AI.SetActive(false);
        GameObject p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        p1.GetComponent<NavMeshAgent>().enabled = false;

        go_calibration.SetActive(true);
    }

    public void StartGame_2p()
    {

    }
}
