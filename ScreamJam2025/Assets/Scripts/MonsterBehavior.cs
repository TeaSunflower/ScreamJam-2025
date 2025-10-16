using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MonsterBehavior : MonoBehaviour
{
    enum Phase
    {
        Inactive,
        Patrol,
        Chase
    }

    [SerializeField]
    GameObject player;

    Transform playTrans;

    [SerializeField]
    Vector2 patrolPoint;

    [SerializeField]
    Phase phase;

    NavMeshAgent agent;

    List<Vector2> nodeList;

    public int xBounds;

    public int yBounds;

    Vector2 currentPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPosition= GetComponent<Transform>().position;
        playTrans = player.GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>(); // NavMesh shenanigans
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        phase = Phase.Patrol;
        agent.speed = 14;
        agent.angularSpeed = 180;
        agent.acceleration = 12;

        nodeList = new List<Vector2>();

        // Creates patrol nodes

        for (int i = yBounds * -1; i < yBounds; i++)
        {
            for (int j = xBounds * -1; j < xBounds; j++)
            {
                nodeList.Add(new Vector2(j, i));
            }
        }
        patrolPoint = nodeList[Random.Range(0, nodeList.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = GetComponent<Transform>().position; // Sets current position

        if (phase == Phase.Chase) // Tracks player for Chase phase
        {
            agent.SetDestination(playTrans.position);
        }
        else if (phase == Phase.Patrol) // Tracks patrol points for Patrol phase
        {
            if (Vector2.Distance(currentPosition, patrolPoint) <= 1.5) // Switches patrol points when within range
            {
                patrolPoint = nodeList[Random.Range(0, nodeList.Count)];
            }
            agent.SetDestination(patrolPoint);
        }

        CheckPhase(); // Calls CheckPhase()
    }

    // Adjusts the phase based on distance to player
    private void CheckPhase()
    {
        if (phase == Phase.Chase)
        {
            if (Vector2.Distance(currentPosition, playTrans.position) >= 12) // Distance to return to patrol (add hide condition later)
            {
                phase = Phase.Patrol;
                agent.speed = 14;
                agent.angularSpeed = 180;
                agent.acceleration = 12;
            }
        }
        else if (phase == Phase.Patrol)
        {
            if (Vector2.Distance(currentPosition, playTrans.position) <= 8) // Distance to begin chase
            {
                phase = Phase.Chase;
                agent.speed = 10;
                agent.angularSpeed = 120;
                agent.acceleration = 8;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("test3");
        PlayerPrefs.SetFloat("HighScore", Time.timeSinceLevelLoad);
        SceneManager.LoadScene("Assets/Scenes/GameOver.unity", LoadSceneMode.Single);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("test2");
        PlayerPrefs.SetFloat("HighScore", Time.timeSinceLevelLoad);
        SceneManager.LoadScene("Assets/Scenes/GameOver.unity", LoadSceneMode.Single);
    }
}
