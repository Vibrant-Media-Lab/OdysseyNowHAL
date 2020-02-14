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
            switch(PlayerPrefs.GetString("game")){
                case "Cat and Mouse":
                go_calibration.GetComponent<CalibrationDirectorNew>()
                              .afterCalibration
                              .AddListener(startCatAndMouse);
                break;
                case "Super Cat and Mouse":
                break;
                case "Haunted House":
                break;
                case "Zoo Breakout":
                break;
                case "Football: Running":
                break;
            }
            // fixme

            // if p1 is ai, don't read calibration
            if(PlayerPrefs.GetInt("ai1") != -1)
                go_calibration.GetComponent<CalibrationOdysseySettings>().p1_read = false;
            // if p2 is ai, don't read calibration
            if(PlayerPrefs.GetInt("ai2") != -1)
                go_calibration.GetComponent<CalibrationOdysseySettings>().p2_read = false;
            StartCalibration();

            return;
        }

        if (LocalInputManager.instance.p1Scheme == LocalInputManager.ControlScheme.AI)
        {
            go_calibration.GetComponent<CalibrationDirectorNew>()
                          .afterCalibration
                          .AddListener(startCatAndMouse);
        }
        if (LocalInputManager.instance.p1Scheme == LocalInputManager.ControlScheme.OriginalConsole)
        {
            go_calibration.GetComponent<CalibrationOdysseySettings>().p1_read = true;
        }
        if (LocalInputManager.instance.p2Scheme == LocalInputManager.ControlScheme.OriginalConsole)
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

    public void startCatAndMouse()
    {
        go_calibration.SetActive(false);
        GameObject p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        GameObject p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");
        int player1AI = PlayerPrefs.GetInt("ai1");
        int player2AI = PlayerPrefs.GetInt("ai2");
        if(player1AI != -1) {
            p1.GetComponent<NavMeshAgent>().enabled = true;
            p1.GetComponent<CatAI>().enabled = true;
            p1.GetComponent<CatAI>().level = player1AI;
        }
        if(player2AI != -1) {
            p2.GetComponent<NavMeshAgent>().enabled = true;
            p2.GetComponent<MouseAI>().enabled = true;
            p2.GetComponent<MouseAI>().level = player2AI;
        }
        go_AI.SetActive(true);
    }

    public void StartCalibration()
    {
        go_AI.SetActive(false);
        go_calibration.SetActive(true);
    }

    public void StartGame_2p()
    {

    }
}
