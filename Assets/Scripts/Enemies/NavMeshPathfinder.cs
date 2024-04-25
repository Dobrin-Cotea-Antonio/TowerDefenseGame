using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Class <c>NavMeshPathfinder</c> is an implementation of IPathFinder. It is used by the enemies.
/// </summary>
public class NavMeshPathfinder : MonoBehaviour, IPathFinder{

    NavMeshAgent agent;

    private void Start(){
        agent = GetComponent<NavMeshAgent>();
    }

    public bool MoveTowardsTarget(Vector3 pTargetPosition, float pAgentSpeed) {
        agent.speed = pAgentSpeed;
        agent.SetDestination(pTargetPosition);

        if ((pTargetPosition - transform.position).magnitude < 0.5f)
            return false;
        return true;
    }

    public void Stop(){
        agent.SetDestination(transform.position);
    }
}
