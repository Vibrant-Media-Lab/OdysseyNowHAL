using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseAI : MonoBehaviour
{

    public Transform target;
    private NavMeshAgent agent;
    public bool stop;


    // Start is called before the first frame update
    void Start()
    {
        // Agent tend to rotate game object, that can be
        //      undesirable
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( ! agent.enabled) { return; }

        if ((transform.position - target.position).magnitude < 1 && !stop)
        {
            stop = true;
            //target.GetComponent<Navigate>().enabled = false;
            //Canvas canvas = FindObjectOfType<Canvas>();
            //var gameOver = canvas.transform.Find("Panel");
            //gameOver.gameObject.SetActive(true);
            //var score = canvas.transform.Find("Score");
            //score.SendMessage("Stop", true);

            Debug.Log("Mouse has the cheese");
        }
        agent.SetDestination(target.position);
        //Navigate.DebugDrawPath(agent.path.corners);
    }
}
