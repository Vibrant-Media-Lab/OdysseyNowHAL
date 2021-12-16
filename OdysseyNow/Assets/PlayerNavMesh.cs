using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;

    private void Awake() 
    {
       navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() 
    {
        //moves navMesh to to where player moves
        navMeshAgent.destination = movePositionTransform.position;
    }

}
