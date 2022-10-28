using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour {

    /* Field Initializers --------------------------------------------------------------------------------------------------*/
   
    public  int level;

    public  Transform  detective;
    public  Transform  ghost;
    public  GameObject ghost_Ai;
    public  GameObject hiding_places;
    public  GameObject treasure_location;
    public  GameObject detective_start_location;
    public  GameObject ghost_start_location;

    private NavMeshAgent agent;
    private GameObject playercube;

    private bool players_are_setup, ghost_has_hidden,
                 detective_is_close, ghost_has_been_touched,
                 ghost_has_finished_tasks;

    private Vector3 hiding_spot;

    /* Main Functions ------------------------------------------------------------------------------------------------------*/

    // Start is called before the first frame updates
    private void Start() {
        // Prepare the navmesh agent
        agent = ghost_Ai.GetComponent<NavMeshAgent>();
        
        playercube = ghost_Ai.GetComponent<Actors.PlayerTargetController>().playerCube;

        // The agent tends to rotate the game object, that can be undesirable
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Set a random hiding spot
        int i = UnityEngine.Random.Range(1, 31);

        hiding_spot = hiding_places.transform.Find("Clue (" + i + ")").transform.position;
    }

    // Update is called once per frame
    private void Update() {
        if (!agent.enabled) return;

        if (!players_are_setup) // Setup the players to the correct positions.
        {
            if (!((ghost.position - ghost_start_location.transform.position).magnitude < 1 &&
                (detective.position - detective_start_location.transform.position).magnitude < 1))
            {
                agent.SetDestination(ghost_start_location.transform.position);
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
        if ((treasure_location.transform.position - detective.position).magnitude < 1) return true;
        return false;
    }

    // Tell the ghost to go an hide
    private void GoAndHideAt(Vector3 spot) {
        agent.SetDestination(spot);
    }

    // Hide the ghost or unhide
    private void ExtinguishToggle() {
        if (playercube.GetComponent<MeshRenderer>().enabled)
        {
            playercube.GetComponent<MeshRenderer>().enabled = false;
            playercube.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            playercube.GetComponent<MeshRenderer>().enabled = true;
            playercube.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
