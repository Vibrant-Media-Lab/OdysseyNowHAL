using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{

    private float nextActionTime = 0.0f;
    public float period = 5f;
    public GameObject target;
    public int level;
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
            stop = true;
            //target.GetComponent<Navigate>().enabled = false;
            //Canvas canvas = FindObjectOfType<Canvas>();
            //var gameOver = canvas.transform.Find("Panel");
            //gameOver.gameObject.SetActive(true);
            //var score = canvas.transform.Find("Score");
            //score.SendMessage("Stop", true);

            Debug.Log("Cat has the mouse.");
        }
        switch(level){
            case 1:
            agent.SetDestination(target.transform.position);
            break;
            case 2:
            if (Time.time > nextActionTime ) {
                nextActionTime += Time.time + period;
                agent.SetDestination(new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0));
            }
            break;
        }
        //Navigate.DebugDrawPath(agent.path.corners);
    }
}
