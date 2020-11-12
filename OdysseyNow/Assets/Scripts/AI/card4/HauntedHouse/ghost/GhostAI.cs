using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour {
    private float nextActionTime = 0.0f;
    private float period = 2f;
    private NavMeshAgent agent;

    public Transform target;
    public GameObject ghost;
    public int level;

    // Start is called before the first frame update
    private void Start() {
        // Agent tend to rotate game object, that can be
        //      undesirable
        agent = ghost.GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void Update() {
        if (!agent.enabled) {
            return;
        }

        if ((transform.position - target.position).magnitude < 1) {
            Debug.Log("Ghost has the detective.");
        }

        switch (level) {
            case 1:
                agent.SetDestination(target.position);
                // if (Time.time > nextActionTime) {
                //     nextActionTime += Time.time + period;
                //     agent.SetDestination(new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0));
                // }

                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
