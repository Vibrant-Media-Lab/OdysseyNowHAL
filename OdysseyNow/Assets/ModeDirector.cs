using CardDirection;
using UnityEngine;
using UnityEngine.AI;


public class ModeDirector : MonoBehaviour {
    public GameObject calibration;
    public GameObject AI;
    public GameObject p1;
    public GameObject p2;

    private void Awake() {
        switch (PlayerPrefs.GetString("game")) {
            case "Cat and Mouse":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartCatAndMouse);
                break;
            case "SCAM-Stonehendge":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartSuperCatAndMouse);
                break;
            case "Haunted House":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartHauntedHouse);
                break;
            case "Zoo Breakout":
                break;
            case "Football: Running":
                break;
            case "Tennis":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartTennis);
                break;
            case "Table Tennis":
                calibration.GetComponent<CalibrationOdysseySettings>().useOverlay = false;
                break;
        }

        var p1_target = p1.transform.Find("PlayerTarget").gameObject;
        var p2_target = p2.transform.Find("PlayerTarget").gameObject;

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));

        // if p1 is ai or keyboard, don't read calibration
        if (p1Input == LocalInputManager.ControlScheme.AI || p1Input == LocalInputManager.ControlScheme.Keyboard)
        {
            calibration.GetComponent<CalibrationOdysseySettings>().p1_read = false;
            p1_target.GetComponent<NavMeshAgent>().enabled                 = true;
        }

        // if p2 is ai or keyboard, don't read calibration
        if (p2Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.Keyboard)
        {
            calibration.GetComponent<CalibrationOdysseySettings>().p2_read = false;
            p2_target.GetComponent<NavMeshAgent>().enabled                 = true;
        }

        StartCalibration();
    }

    private void StartCatAndMouse() {
        calibration.SetActive(false);
        var game = AI.transform.Find("CatAndMouse").gameObject;


        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));

        setupStartPositions(game);

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI) {
            game.SetActive(true);
            AI.SetActive(true);
        }

        if (p1Input == LocalInputManager.ControlScheme.AI || p1Input == LocalInputManager.ControlScheme.Keyboard) {

            game.GetComponent<CatAI>().enabled      = true;
            game.GetComponent<CatAI>().level        = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            game.GetComponent<MouseAI>().enabled    = true;
            game.GetComponent<MouseAI>().level      = PlayerPrefs.GetInt("ai2");
        }

    }

    private void StartSuperCatAndMouse() {
        calibration.SetActive(false);
        var game = AI.transform.Find("SCAM-Stonehenge").gameObject;

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));

        setupStartPositions(game);

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI) {
            game.SetActive(true);
            AI.SetActive(true);
        }

        if (p1Input == LocalInputManager.ControlScheme.AI) {
            game.GetComponent<CatAI>().enabled = true;
            game.GetComponent<CatAI>().level   = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            game.GetComponent<MouseAI>().enabled = true;
            game.GetComponent<MouseAI>().level   = PlayerPrefs.GetInt("ai2");
        }
    }

    private void StartHauntedHouse()
    {
        calibration.SetActive(false);
        var p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");

        var game = AI.transform.Find("HauntedHouse").gameObject;

        var p1Input = (LocalInputManager.ControlScheme)System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme)System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI) {
            PlayerPrefs.SetString("P1Input", LocalInputManager.ControlScheme.Keyboard.ToString());
            p2.GetComponent<NavMeshAgent>().enabled = true;
            game.GetComponent<GhostAI>().enabled = true;
            game.GetComponent<GhostAI>().level = 1;
            game.SetActive(true);
            AI.SetActive(true);
        }
    }

    private void StartTennis() {
        calibration.SetActive(false);
        var p1_target = p1.transform.Find("PlayerTarget").gameObject;
        var p2_target = p2.transform.Find("PlayerTarget").gameObject;

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI)
            AI.SetActive(true);

        if (p1Input == LocalInputManager.ControlScheme.AI)
        {
            p1_target.GetComponent<NavMeshAgent>().enabled = true;
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            p2_target.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    private void setupStartPositions(GameObject game)
    {
        p1.transform.Find("PlayerBody").position   = game.transform.Find("p1_start").position;
        p2.transform.Find("PlayerBody").position   = game.transform.Find("p2_start").position;
        p1.transform.Find("PlayerTarget").position = game.transform.Find("p1_start").position;
        p2.transform.Find("PlayerTarget").position = game.transform.Find("p2_start").position;
    }

    private void StartCalibration() {
        AI.SetActive(false);
        calibration.SetActive(true);
    }
}
