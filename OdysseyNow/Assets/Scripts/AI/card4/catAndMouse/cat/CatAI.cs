using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{

    private float nextActionTime = 0.0f;
    private float period = 2f;
    public GameObject target;
    private NavMeshAgent agent;
    public int level;

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

        if ((transform.position - target.transform.position).magnitude < 1)
        {
            Debug.Log("Cat has the mouse.");
        }
        switch(level){
            case 1:
            if (Time.time > nextActionTime ) {
                nextActionTime += Time.time + period;
                agent.SetDestination(new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0));
            }
            break;
            case 2:
            break;
            case 3:
            agent.SetDestination(target.transform.position);
            break;
        }
    }
}
