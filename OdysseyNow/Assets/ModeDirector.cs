using CardDirection;
using UnityEngine;
using UnityEngine.AI;


public class ModeDirector : MonoBehaviour {
    public GameObject calibration;
    public GameObject AI;
    public GameObject p1;
    public GameObject p2;
    private NavMeshAgent p1_nav_mesh_agent;
    private NavMeshAgent p2_nav_mesh_agent;
    private LocalInputManager.ControlScheme p1Input;
    private LocalInputManager.ControlScheme p2Input;

    private void Awake() {

        p1_nav_mesh_agent = p1.transform.Find("PlayerTarget").gameObject.GetComponent<NavMeshAgent>();
        p2_nav_mesh_agent = p2.transform.Find("PlayerTarget").gameObject.GetComponent<NavMeshAgent>();

        switch (PlayerPrefs.GetString("game")) {
            case "Cat and Mouse":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartCatAndMouse);
                break;
            case "SCAM-Stonehenge":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartSuperCatAndMouse);
                break;
            case "Haunted House":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartHauntedHouse);
                break;
            case "Deadly Haunted House":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartDeadlyHauntedHouse);
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

        // get p1Input and p2Input
        GetPlayerPrefs();

        // if p1 is ai or keyboard, don't read calibration
        if (p1Input == LocalInputManager.ControlScheme.AI || p1Input == LocalInputManager.ControlScheme.Keyboard)
        {
            calibration.GetComponent<CalibrationOdysseySettings>().p1_read = false;
            p1_nav_mesh_agent.enabled = true;
        }

        // if p2 is ai or keyboard, don't read calibration
        if (p2Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.Keyboard)
        {
            calibration.GetComponent<CalibrationOdysseySettings>().p2_read = false;
            p2_nav_mesh_agent.enabled = true;
        }

        StartCalibration();
    }

    private void StartCatAndMouse() {
        calibration.SetActive(false);
        var game = AI.transform.Find("CatAndMouse").gameObject;

        GetPlayerPrefs();

        setupStartPositions(game);

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI) {
            game.SetActive(true);
            AI.SetActive(true);
        }

        if (p1Input == LocalInputManager.ControlScheme.AI) {
            game.GetComponent<CatAI>().enabled = true;
            game.GetComponent<CatAI>().level = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            game.GetComponent<MouseAI>().enabled    = true;
            game.GetComponent<MouseAI>().level      = PlayerPrefs.GetInt("ai2");
        }

    }

    private void StartSuperCatAndMouse() {
        calibration.SetActive(false);
        var game = AI.transform.Find("SCAM-Stonehenge").gameObject;

        GetPlayerPrefs();

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

    private void StartHauntedHouse() {
        calibration.SetActive(false);
        var game = AI.transform.Find("HauntedHouse").gameObject;

        GetPlayerPrefs();

        setupStartPositions(game);

        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI) {
            PlayerPrefs.SetString("P1Input", LocalInputManager.ControlScheme.Keyboard.ToString());
            PlayerPrefs.SetString("P2Input", LocalInputManager.ControlScheme.AI.ToString());
            game.SetActive(true);
            AI.SetActive(true);
            game.GetComponent<GhostAI>().enabled = true;
            game.GetComponent<GhostAI>().level = 1;
        }
    }

    private void StartDeadlyHauntedHouse()
    {
        calibration.SetActive(false);
        var game = AI.transform.Find("DeadlyHauntedHouse").gameObject;

        GetPlayerPrefs();

        setupStartPositions(game);


        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI)
        {
            game.SetActive(true);
            AI.SetActive(true);
        }

        if (p1Input == LocalInputManager.ControlScheme.AI)
        {
            game.GetComponent<HeirAI>().enabled = true;
            game.GetComponent<HeirAI>().level = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI)
        {
            game.GetComponent<GhostAI>().enabled = true;
            game.GetComponent<GhostAI>().level = PlayerPrefs.GetInt("ai2");
        }
    }

    private void StartTennis() {
        calibration.SetActive(false);
        var p1_target = p1.transform.Find("PlayerTarget").gameObject;
        var p2_target = p2.transform.Find("PlayerTarget").gameObject;

        GetPlayerPrefs();

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
        p1.transform.Find("PlayerBody").position   = game.transform.Find("p1-start").position;
        p2.transform.Find("PlayerBody").position   = game.transform.Find("p2-start").position;
        p1.transform.Find("PlayerTarget").position = game.transform.Find("p1-start").position;
        p2.transform.Find("PlayerTarget").position = game.transform.Find("p2-start").position;
    }

    private void StartCalibration() {
        AI.SetActive(false);
        calibration.SetActive(true);
    }

    private void GetPlayerPrefs()
    {
        p1Input = (LocalInputManager.ControlScheme)System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        p2Input = (LocalInputManager.ControlScheme)System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
    }
}
