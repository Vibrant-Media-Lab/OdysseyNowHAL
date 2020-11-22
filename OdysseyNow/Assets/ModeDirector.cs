using CardDirection;
using UnityEngine;
using UnityEngine.AI;

public class ModeDirector : MonoBehaviour {
    public GameObject calibration;
    public GameObject AI;

    // Start is called before the first frame update
    private void Awake() {
        switch (PlayerPrefs.GetString("game")) {
            case "Cat and Mouse":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartCatAndMouse);
                break;
            case "SCAM-Stonehendge":
                calibration.GetComponent<CalibrationDirectorNew>().afterCalibration.AddListener(StartSuperCatAndMouse);
                break;
            case "Haunted House":
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

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));

        // if p1 is ai or keyboard, don't read calibration
        if (p1Input == LocalInputManager.ControlScheme.AI || p1Input == LocalInputManager.ControlScheme.Keyboard)
            calibration.GetComponent<CalibrationOdysseySettings>().p1_read = false;

        // if p2 is ai or keyboard, don't read calibration
        if (p2Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.Keyboard)
            calibration.GetComponent<CalibrationOdysseySettings>().p2_read = false;

        StartCalibration();
    }

    private void StartCatAndMouse() {
        calibration.SetActive(false);
        var p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        var p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");
        var game = AI.transform.Find("CatAndMouse").gameObject;

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
        
        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI) {
            game.SetActive(true);
            AI.SetActive(true);
        }
        
        if (p1Input == LocalInputManager.ControlScheme.AI) {
            p1.GetComponent<NavMeshAgent>().enabled = true;
            game.GetComponent<CatAI>().enabled = true;
            game.GetComponent<CatAI>().level = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            p2.GetComponent<NavMeshAgent>().enabled = true;
            game.GetComponent<MouseAI>().enabled = true;
            game.GetComponent<MouseAI>().level = PlayerPrefs.GetInt("ai2");
        }

    }

    private void StartSuperCatAndMouse() {
        calibration.SetActive(false);
        var p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        var p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");
        var game = AI.transform.Find("SCAM-Stonehenge").gameObject;

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
        
        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI) {
            game.SetActive(true);
            AI.SetActive(true);
        }
        
        if (p1Input == LocalInputManager.ControlScheme.AI) {
            p1.GetComponent<NavMeshAgent>().enabled = true;
            game.GetComponent<CatAI>().enabled = true;
            game.GetComponent<CatAI>().level = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            p2.GetComponent<NavMeshAgent>().enabled = true;
            game.GetComponent<MouseAI>().enabled = true;
            game.GetComponent<MouseAI>().level = PlayerPrefs.GetInt("ai2");
        }
    }

    private void StartTennis() {
        calibration.SetActive(false);
        var p1 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player1");
        var p2 = ElementSettings.FindFromNameAndTag("PlayerTarget", "Player2");

        var p1Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P1Input"));
        var p2Input = (LocalInputManager.ControlScheme) System.Enum.Parse(typeof(LocalInputManager.ControlScheme),
                                                                          PlayerPrefs.GetString("P2Input"));
        
        if (p1Input == LocalInputManager.ControlScheme.AI || p2Input == LocalInputManager.ControlScheme.AI)
            AI.SetActive(true);
        
        if (p1Input == LocalInputManager.ControlScheme.AI) {
            p1.GetComponent<NavMeshAgent>().enabled = true;
            p1.GetComponent<CatAI>().enabled = true;
            p1.GetComponent<CatAI>().level = PlayerPrefs.GetInt("ai1");
        }

        if (p2Input == LocalInputManager.ControlScheme.AI) {
            p2.GetComponent<NavMeshAgent>().enabled = true;
            p2.GetComponent<MouseAI>().enabled = true;
            p2.GetComponent<MouseAI>().level = PlayerPrefs.GetInt("ai2");
        }
    }

    private void StartCalibration() {
        AI.SetActive(false);
        calibration.SetActive(true);
    }
}
