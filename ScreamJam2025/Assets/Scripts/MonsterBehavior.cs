using UnityEngine;
using UnityEngine.AI;

public class MonsterBehavior : MonoBehaviour
{
    enum Phase
    {
        Inactive,
        Patrol,
        Chase
    }

    [SerializeField]
    Transform player;

    [SerializeField]
    Transform patrolPoint;

    NavMeshAgent agent;

    Phase phase;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        phase = Phase.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == Phase.Chase)
        {
            agent.SetDestination(player.position);
        }

        if (phase == Phase.Patrol)
        {
            agent.SetDestination(player.position);
        }
    }
}
