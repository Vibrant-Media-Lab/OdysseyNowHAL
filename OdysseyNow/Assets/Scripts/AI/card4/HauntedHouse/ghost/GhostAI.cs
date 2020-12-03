using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour {
    /* Field Initializers --------------------------------------------------------------------------------------------------*/

    private NavMeshAgent agent;
    public int level;
    public Transform detective;
    public Transform ghost;
    public GameObject ghost_Ai;
    public bool players_are_setup, ghost_has_hidden, detective_is_close, ghost_has_been_touched, ghost_has_finished_tasks;
    public Vector3 treasure_location = new Vector3((float)3.68, (float)2.3, 0);
    public Vector3 detective_start_location = new Vector3((float)-6.06, (float)-3.29, 0);
    public Vector3 ghost_start_location = new Vector3((float)-6.06, (float)-1.91, 0);
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
    private Vector3 hiding_spot;
    /* Main Functions ------------------------------------------------------------------------------------------------------*/

    // Start is called before the first frame updates
    private void Start() {
        agent = ghost_Ai.GetComponent<NavMeshAgent>();

        // The agent tends to rotate the game object, that can be undesirable
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Set a random hiding spot
        int i = UnityEngine.Random.Range(0, x_locations.Length);
        hiding_spot = new Vector3((float)x_locations[i], (float)y_locations[i], 0);

        //Set Corrected Size For Game
        ghost.parent.Find("PlayerTarget").transform.localScale = new Vector3((float)0.8, (float)0.8, (float)0.8);
        detective.parent.Find("PlayerTarget").transform.localScale = new Vector3((float)0.8, (float)0.8, (float)0.8);
    }

    // Update is called once per frame
    private void Update() {
        if (!agent.enabled) return;

        // For debbuging
        // Debug.Log("Closeness: " + (ghost.position - detective.position).magnitude);

        if (!players_are_setup) // Setup the players to the correct positions.
        {
            if (!((ghost.position - ghost_start_location).magnitude < 1 && (detective.position - detective_start_location).magnitude < 1))
            {
                agent.SetDestination(ghost_start_location);
                Debug.Log("waiting for detective to get to start position!");
            }
            else
            {
                Debug.Log("Time to hide!");
                players_are_setup = true;
                ExtinguishToggle();
            }
        }
        else // Now that players are in position let the ghost go and hide.
        {
            if (!((ghost.position - hiding_spot).magnitude < 1))
            {
                GoAndHideAt(hiding_spot);
                Debug.Log("Hiding...");
            }
            else
            {
                Debug.Log("Ghost has hidden!");
                ghost_has_hidden = true;
            }
        }

        if (ghost_has_hidden && !ghost_has_finished_tasks)
        {
            if (TheDetectiveIsAtTheTreasure()) // If the detective arrived at the treasure game is over!
            {
                ghost_has_finished_tasks = true;
                Debug.Log("Detective foud the treasure!");
                Debug.Log("Restart game to play again.");
            }
            else if (!detective_is_close) // Otherwise wait for the detective to get close.
            {
                if ((ghost.position - detective.position).magnitude < 1.3)
                {
                    Debug.Log("Detective is too close!");
                    detective_is_close = true;
                    ExtinguishToggle();
                    Debug.Log("BOO!");
                }
            }
            else if (!ghost_has_been_touched)
            {
                if ((ghost.position - detective.position).magnitude < 1)
                {
                    Debug.Log("Detective touched Ghost. They lose half of their cards.");
                    ghost_has_been_touched = true;
                    ExtinguishToggle();
                }
            }
        }
    }

    /* Helper Functions ----------------------------------------------------------------------------------------------------*/

    // Check if the detective has reached the treasure
    private bool TheDetectiveIsAtTheTreasure()
    {
        if ((treasure_location - detective.position).magnitude < 1) return true;
        return false;
    }

    // Tell the ghost to go an hide
    private void GoAndHideAt(Vector3 spot) {
        agent.SetDestination(spot);
    }

    // Hide the ghost or unhide
    private void ExtinguishToggle() {
        GameObject playercube = ghost_Ai.GetComponent<Actors.PlayerTargetController>().playerCube;
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
