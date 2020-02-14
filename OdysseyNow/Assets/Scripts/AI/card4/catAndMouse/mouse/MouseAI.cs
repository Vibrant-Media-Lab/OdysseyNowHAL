using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseAI : MonoBehaviour
{

    public Transform target;
    private NavMeshAgent agent;
    public int level;

    // Start is called before the first frame update
    private void Start()
    {
        // Agent tend to rotate game object, that can be
        //      undesirable
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if ( ! agent.enabled) { return; }

        if ((transform.position - target.position).magnitude < 1)
        {
            Debug.Log("Mouse has the cheese");
        }
        switch(level){
            case 1:
            break;
            case 2:
            agent.SetDestination(target.position);
            break;
            case 3:
            break;
        }
    }
}
