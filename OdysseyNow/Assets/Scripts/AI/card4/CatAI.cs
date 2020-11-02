using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{

    public GameObject target;
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

        if ((transform.position - target.transform.position).magnitude < 1 && !stop)
        {
            //stop = true;
            //target.GetComponent<Navigate>().enabled = false;
            //Canvas canvas = FindObjectOfType<Canvas>();
            //var gameOver = canvas.transform.Find("Panel");
            //gameOver.gameObject.SetActive(true);
            //var score = canvas.transform.Find("Score");
            //score.SendMessage("Stop", true);

            Debug.Log("Cat has the mouse.");
        }
        agent.SetDestination(target.transform.position);
        //Navigate.DebugDrawPath(agent.path.corners);
    }
}
