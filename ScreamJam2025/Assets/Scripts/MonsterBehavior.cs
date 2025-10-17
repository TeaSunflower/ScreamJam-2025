using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MonsterBehavior : MonoBehaviour
{
    enum Phase
    {
        Inactive,
        Patrol,
        Chase,
        Stalking,
        Tracking
    }

    [SerializeField]
    GameObject player;

    Transform playTransform;

    PlayerBehavior playBehave;

    [SerializeField]
    Vector2 currentPosition;

    [SerializeField]
    Vector2 targetPoint;

    [SerializeField]
    Phase phase;

    NavMeshAgent agent;

    List<Vector2> nodeList;

    List<Vector2> spawnList;

    public int xBounds;

    public int yBounds;

    public float timeCounter;

    bool startChase;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playTransform = player.GetComponent<Transform>(); // Player components
        playBehave = player.GetComponent<PlayerBehavior>();

        agent = GetComponent<NavMeshAgent>(); // NavMesh shenanigans
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        timeCounter = 0;
        startChase = false;

        phase = Phase.Patrol; // Initial set up
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
        targetPoint = nodeList[Random.Range(0, nodeList.Count)];

        spawnList = new List<Vector2>(); // Sets up list of spawn locations
        spawnList.Add(new Vector2(27.5f, 17.5f));
        spawnList.Add(new Vector2(-28.5f, 16.5f));
        spawnList.Add(new Vector2(-27.5f, -18.5f));
        spawnList.Add(new Vector2(28.5f, -19.5f));

        agent.Warp(spawnList[Random.Range(0, 4)]);
        currentPosition = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if (startChase)
        {
            currentPosition = GetComponent<Transform>().position;

            if (phase == Phase.Chase || phase == Phase.Tracking) // Tracks player for Chase and Track
            {
                agent.SetDestination(playTransform.position);
            }

            if (phase == Phase.Patrol) // Tracks patrol points for Patrol phase
            {
                if (Vector2.Distance(currentPosition, targetPoint) <= 2) // Switches patrol points when within range
                {
                    targetPoint = nodeList[Random.Range(0, nodeList.Count)];
                }
                agent.SetDestination(targetPoint);
            }

            if (phase == Phase.Stalking)
            {
                if (Vector2.Distance(currentPosition, playTransform.position) > 4)
                {
                    agent.SetDestination(playTransform.position);
                }
                else
                {
                    agent.SetDestination(currentPosition);
                }
            }
        }
        else
        {
            if (timeCounter >= 3)
            {
                startChase = true;
            }
        }

            CheckPhase(); // Calls CheckPhase()
    }

    // Adjusts the phase based on distance to player
    private void CheckPhase()
    {
        PlayerBehavior.Movement pMove = playBehave.movePhase;
        int rng;
        if (timeCounter >= 5)
        {
            rng = Random.Range(0, 4);
            timeCounter = 0;
        }
        else
        {
            rng = 0;
            timeCounter += Time.deltaTime;
        }

        if (pMove == PlayerBehavior.Movement.Revealed)
        {
            phase = Phase.Tracking;
            agent.speed = 10;
            agent.angularSpeed = 120;
            agent.acceleration = 8;
        }

        switch (phase)
            {
                case Phase.Chase:
                if (Vector2.Distance(currentPosition, playTransform.position) >= 12 || pMove == PlayerBehavior.Movement.Hidden) // Distance to return to patrol (add hide condition later)
                    {
                        phase = Phase.Patrol;
                        agent.speed = 14;
                        agent.angularSpeed = 180;
                        agent.acceleration = 12;
                    }

                    break;

                case Phase.Patrol:
                if (Vector2.Distance(currentPosition, playTransform.position) <= 8 && pMove != PlayerBehavior.Movement.Hidden) // Distance to begin chase
                    {
                        phase = Phase.Chase;
                        agent.speed = 10;
                        agent.angularSpeed = 120;
                        agent.acceleration = 8;
                        break;
                    }
                    if (rng >= 2 && Vector2.Distance(currentPosition, playTransform.position) >= 15 && pMove != PlayerBehavior.Movement.Hidden)
                    {
                        phase = Phase.Stalking;
                        agent.speed = 8;
                        agent.angularSpeed = 120;
                        agent.acceleration = 6;
                        break;
                    }

                    break;

                case Phase.Stalking:
                    if (rng == 3 && pMove != PlayerBehavior.Movement.Hidden)
                    {
                        phase = Phase.Chase;
                        agent.speed = 10;
                        agent.angularSpeed = 120;
                        agent.acceleration = 8;
                    }
                    if (pMove == PlayerBehavior.Movement.Hidden)
                    {
                        phase = Phase.Patrol;
                        agent.speed = 14;
                        agent.angularSpeed = 180;
                        agent.acceleration = 12;
                    }
                    break;

                case Phase.Tracking:
                    if (Vector2.Distance(currentPosition, playTransform.position) <= 8 && pMove != PlayerBehavior.Movement.Hidden) // Distance to begin chase
                    {
                        phase = Phase.Chase;
                        agent.speed = 10;
                        agent.angularSpeed = 120;
                        agent.acceleration = 8;
                        break;
                    }
                    if (pMove != PlayerBehavior.Movement.Revealed)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                phase = Phase.Patrol;
                                agent.speed = 14;
                                agent.angularSpeed = 180;
                                agent.acceleration = 12;
                                break;

                            case 1:
                                phase = Phase.Stalking;
                                agent.speed = 8;
                                agent.angularSpeed = 120;
                                agent.acceleration = 6;
                                break;
                        }
                    }

                    break;
            }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("test3");
            PlayerPrefs.SetFloat("HighScore", Time.timeSinceLevelLoad);
            SceneManager.LoadScene("Assets/Scenes/GameOver.unity", LoadSceneMode.Single);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("test2");
            PlayerPrefs.SetFloat("HighScore", Time.timeSinceLevelLoad);
            SceneManager.LoadScene("Assets/Scenes/GameOver.unity", LoadSceneMode.Single);
        }
    }
}
