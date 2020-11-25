using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour {
    private NavMeshAgent agent;
    public Transform detective;
    public GameObject ghost;
    public int level;
    public bool ghost_has_hidden, detective_is_close, ghost_has_been_touched, ghost_has_finished_tasks;

    public Vector3 treasure_location = new Vector3((float)3.68, (float)2.3, (float)0);

    public double[] x_locations = { -4.63, -3.95, -3.36, -0.85, -0.45,  0.74,
                                                  -4.13,  1.62,  3.75,  4.31,  5.24,  1.48,
                                                   1.91,  2.56,  2.42,  1.40,  0.58, -0.68,
                                                  -1.87, -2.80, -2.86, -3.61, -4.36, -4.69,
                                                  -4.61, -3.53, -1.90,  0.00,  1.41,  2.29 };

    public double[] y_locations = { -2.24, -2.62, -2.98, -2.89, -2.28, -3.97,
                                                  -3.97, -3.55, -3.55, -2.47, -3.05, -2.35,
                                                  -1.93, -0.91, -0.45, -0.60, -1.37, -0.80,
                                                  -1.14, -1.22, -0.47, -1.19, -0.11,  0.31,
                                                   1.48,  0.76,  0.40,  1.35,  0.91,  2.82 };

    

    // Start is called before the first frame updates
    private void Start() {
        agent = ghost.GetComponent<NavMeshAgent>();
        // The agent tends to rotate the game object, that can be undesirable
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        GoAndHideAt(UnityEngine.Random.Range(0, x_locations.Length));
    }

    // Update is called once per frame
    private void Update() {
        if (!agent.enabled) {
            return;
        }

        switch (level) {
            case 1:
                Debug.Log((transform.position - detective.position).magnitude);

                if ((transform.position - detective.position).magnitude < 1)
                {
                    Debug.Log("Detective is close.");
                    detective_is_close = true;
                    //ExtinguishToggle();
                    Debug.Log("BOO!");
                }

                //if (!ghost_has_hidden)
                //{
                //    GoAndHideAt(UnityEngine.Random.Range(0, x_locations.Length));
                //    ghost_has_hidden = true;
                //}

                //else if (!detective_is_close)
                //{
                //    if ((transform.position - detective.position).magnitude < 2)
                //    {
                //        Debug.Log("Detective is close.");
                //        detective_is_close = true;
                //        ExtinguishToggle();
                //        Debug.Log("BOO!");
                //    }
                //}

                //else if (!ghost_has_been_touched)
                //{
                //    if ((transform.position - detective.position).magnitude < 1)
                //    {
                //        Debug.Log("Detective touched Ghost.");
                //        ghost_has_been_touched = true;
                //        ExtinguishToggle();
                //    }
                //}

                //if (!ghost_has_finished_tasks)
                //{
                //    if (TheDetectiveIsAtTheTreasure())
                //    {
                //        ghost_has_finished_tasks = true;
                //        Debug.Log("Detective foud the treasure!");
                //        Debug.Log("Restart game to play again.");
                //    }
                //}

                break;
        }
    }

    private bool TheDetectiveIsAtTheTreasure()
    {
        if ((treasure_location - detective.position).magnitude < 1) return true;
        return false;
    }

    private void GoAndHideAt(int i) {
        agent.SetDestination(new Vector3((float)x_locations[i], (float)y_locations[i], 0));
    }

    private void ExtinguishToggle() {
        GameObject playercube = ghost.GetComponent<Actors.PlayerTargetController>().playerCube;
        Debug.Log(playercube.ToString());
        if (playercube.GetComponent<MeshRenderer>().enabled) {
            playercube.GetComponent<MeshRenderer>().enabled = false;
            playercube.GetComponent<BoxCollider>().enabled = false;
        }
        else {
            playercube.GetComponent<MeshRenderer>().enabled = true;
            playercube.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
