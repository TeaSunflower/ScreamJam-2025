using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    Transform playerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPosition.position;
    }
}
